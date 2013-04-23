using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for ObjectMapper
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.BusinessObject
{
    public class ObjectMapper
    {
        /// <summary>
        /// Loads the PropertyMappingInfo collection for the type specified by objType from the cache, or creates the
        /// collection and adds it to the cache if it does not exist.
        /// </summary>
        /// <param name="objType">Type to load the properties for.</param>
        /// <returns>A collection of PropertyMappingInfo objects that are associated with the Type.</returns>
        private static List<PropertyMappingInfo> LoadPropertyMappingInfo(Type objType)
        {
            var mapInfoList = new List<PropertyMappingInfo>();

            foreach (PropertyInfo info in objType.GetProperties())
            {
                var mapAttr =
                    (DataMappingAttribute) Attribute.GetCustomAttribute(info, typeof (DataMappingAttribute));

                if (mapAttr != null)
                {
                    var mapInfo =
                        new PropertyMappingInfo(mapAttr.DataFieldName, mapAttr.NullValue, info);
                    mapInfoList.Add(mapInfo);
                }
            }

            return mapInfoList;
        }

        /// <summary>
        /// Loads the PropertyMappingInfo collection for type specified.
        /// </summary>
        /// <param name="objType">Type that contains the properties to load.</param>
        /// <returns>A collection of PropertyMappingInfo objects that are associated with the Type.</returns>
        private static List<PropertyMappingInfo> GetProperties(Type objType)
        {
            List<PropertyMappingInfo> info = MappingInfoCache.GetCache(objType.Name);

            if (info == null)
            {
                info = LoadPropertyMappingInfo(objType);
                MappingInfoCache.SetCache(objType.Name, info);
            }
            return info;
        }

        /// <summary>
        /// Returns an array of integers that correspond to the index of the matching field in the PropertyInfoCollection.
        /// </summary>
        /// <param name="propMapList">PropertyMappingInfo Collection</param>
        /// <param name="dr">DataReader</param>
        /// <returns>Array of integers that correspond to the field's index position in the datareader for each one of the PropertyMappingInfo objects.</returns>
        private static int[] GetOrdinals(List<PropertyMappingInfo> propMapList, IDataReader dr)
        {
            var ordinals = new int[propMapList.Count];

            if (dr != null)
            {
                for (int i = 0; i <= propMapList.Count - 1; i++)
                {
                    ordinals[i] = -1;
                    try
                    {
                        ordinals[i] = dr.GetOrdinal(propMapList[i].DataFieldName);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        
                        {
                            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                        }
                    }
                }
            }

            return ordinals;
        }

        /// <summary>
        /// Returns an array of integers that correspond to the index of the matching field in the PropertyInfoCollection.
        /// </summary>
        /// <param name="propMapList">PropertyMappingInfo Collection</param>
        /// <param name="dr">DataRow</param>
        /// <returns>Array of integers that correspond to the field's index position in the datareader for each one of the PropertyMappingInfo objects.</returns>
        private static int[] GetOrdinals(List<PropertyMappingInfo> propMapList, DataRow dr)
        {
            var ordinals = new int[propMapList.Count];

            if (dr != null)
            {
                for (int i = 0; i <= propMapList.Count - 1; i++)
                {
                    ordinals[i] = -1;
                    try
                    {
                        ordinals[i] = dr.Table.Columns[propMapList[i].DataFieldName].Ordinal;
                            //dr.GetOrdinal(propMapList[i].DataFieldName);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        
                        {
                            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                        }
                    }
                }
            }

            return ordinals;
        }

        /// <summary>
        /// Iterates through the object type's properties and attempts to assign the value from the datareader to
        /// the matching property.
        /// </summary>
        /// <typeparam name="T">The type of object to populate.</typeparam>
        /// <param name="dr">The IDataReader that contains the data to populate the object with.</param>
        /// <param name="propInfoList">List of PropertyMappingInfo objects.</param>
        /// <param name="ordinals">Array of integers that indicate the index into the IDataReader to get the value from.</param>
        /// <returns>A populated instance of type T</returns>
        private static T CreateObject<T>(IDataReader dr,
                                         List<PropertyMappingInfo> propInfoList, int[] ordinals) where T : class, new()
        {
            var obj = new T();

            // iterate through the PropertyMappingInfo objects for this type.
            for (int i = 0; i <= propInfoList.Count - 1; i++)
            {
                if (propInfoList[i].PropertyInfo.CanWrite)
                {
                    Type type = propInfoList[i].PropertyInfo.PropertyType;
                    object value = propInfoList[i].DefaultValue;

                    if (ordinals[i] != -1 && dr.IsDBNull(ordinals[i]) == false)
                        value = dr.GetValue(ordinals[i]);

                    try
                    {
                        // try implicit conversion first
                        propInfoList[i].PropertyInfo.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // data types do not match

                        try
                        {
                            // need to handle enumeration types differently than other base types.
                            if (type.BaseType.Equals(typeof (Enum)))
                            {
                                propInfoList[i].PropertyInfo.SetValue(
                                    obj, Enum.ToObject(type, value), null);
                            }
                            else
                            {
                                // try explicit conversion
                                propInfoList[i].PropertyInfo.SetValue(
                                    obj, Convert.ChangeType(value, type), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            // error assigning the datareader value to a property
                            
                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Iterates through the object type's properties and attempts to assign the value from the datareader to
        /// the matching property.
        /// </summary>
        /// <typeparam name="T">The type of object to populate.</typeparam>
        /// <param name="dr">The DataRow that contains the data to populate the object with.</param>
        /// <param name="propInfoList">List of PropertyMappingInfo objects.</param>
        /// <param name="ordinals">Array of integers that indicate the index into the IDataReader to get the value from.</param>
        /// <returns>A populated instance of type T</returns>
        private static T CreateObject<T>(DataRow dr,
                                         List<PropertyMappingInfo> propInfoList, int[] ordinals) where T : class, new()
        {
            var obj = new T();

            // iterate through the PropertyMappingInfo objects for this type.
            for (int i = 0; i <= propInfoList.Count - 1; i++)
            {
                if (propInfoList[i].PropertyInfo.CanWrite)
                {
                    Type type = propInfoList[i].PropertyInfo.PropertyType;
                    object value = propInfoList[i].DefaultValue;

                    if (ordinals[i] != -1 && dr[ordinals[i]] != null)
                        value = dr[ordinals[i]];

                    try
                    {
                        // try implicit conversion first
                        propInfoList[i].PropertyInfo.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // data types do not match

                        try
                        {
                            // need to handle enumeration types differently than other base types.
                            if (type.BaseType.Equals(typeof (Enum)))
                            {
                                propInfoList[i].PropertyInfo.SetValue(
                                    obj, Enum.ToObject(type, value), null);
                            }
                            else
                            {
                                // try explicit conversion
                                propInfoList[i].PropertyInfo.SetValue(
                                    obj, Convert.ChangeType(value, type), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            // error assigning the datareader value to a property
                            
                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Creates and populates an instance of the objType Type.
        /// </summary>
        /// <typeparam name="T">Type of object to return.</typeparam>
        /// <param name="objType">Type of the object to instantiate.</param>
        /// <param name="dr">IDataReader with the data to populate the object instance with.</param>
        /// <returns>An instance of the objType type.</returns>
        public static T FillObject<T>(Type objType, IDataReader dr) where T : class, new()
        {
            T obj = null;

            //try
            //{
            List<PropertyMappingInfo> mapInfo = GetProperties(objType);
            int[] ordinals = GetOrdinals(mapInfo, dr);
            if (((SqlDataReader) dr).HasRows)
            {
                obj = CreateObject<T>(dr, mapInfo, ordinals);
            }
            //}
            //finally
            //{
            //    if (dr.IsClosed == false)
            //        dr.Close();
            //}

            return obj;
        }

        /// <summary>
        /// Creates and populates an instance of the objType Type.
        /// </summary>
        /// <typeparam name="T">Type of object to return.</typeparam>
        /// <param name="objType">Type of the object to instantiate.</param>
        /// <param name="dr">DataRow with the data to populate the object instance with.</param>
        /// <returns>An instance of the objType type.</returns>
        public static T FillObject<T>(Type objType, DataRow dr) where T : class, new()
        {
            T obj = null;

            //try
            //{
            List<PropertyMappingInfo> mapInfo = GetProperties(objType);
            int[] ordinals = GetOrdinals(mapInfo, dr);
            if (dr != null)
            {
                obj = CreateObject<T>(dr, mapInfo, ordinals);
            }
            //}
            //finally
            //{
            //    if (dr.IsClosed == false)
            //        dr.Close();
            //}

            return obj;
        }

        /// <summary>
        /// Creates and populates a collection with instances of the objType object passed in.
        /// </summary>
        /// <typeparam name="T">Type of object to add to the collection.</typeparam>
        /// <param name="objType">Type of object to add to the collection.</param>
        /// <param name="dr">IDataReader that contains the data for the collection.</param>
        /// <param name="collection">ICollection object to populate.</param>
        /// 
        public static C FillCollection<T, C>(Type objType, IDataReader dr)
            where T : class, new()
            where C : ICollection<T>, new()
        {
            var coll = new C();
            try
            {
                List<PropertyMappingInfo> mapInfo = GetProperties(objType);
                int[] ordinals = GetOrdinals(mapInfo, dr);

                while (dr.Read())
                {
                    var obj = CreateObject<T>(dr, mapInfo, ordinals);
                    coll.Add(obj);
                }
            }
            finally
            {
                if (dr.IsClosed == false)
                    dr.Close();
            }
            return coll;
        }
    }
}