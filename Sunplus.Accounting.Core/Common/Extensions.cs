using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;

namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Summary description for Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert DataTable to StringCollection
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static StringCollection ToStringCollection(this DataTable dataTable)
        {
            var stringCollection = new StringCollection();
            foreach (DataRow row in dataTable.Rows)
                stringCollection.Add(CastAide.AsString(row[0]));
            return stringCollection;
        }

        /// <summary>
        /// Convert a list of interger to a string seperated by commas
        /// </summary>
        /// <param name="list">list of interger</param>
        /// <exception cref="Exception">throw exception if number of item = 0</exception>
        /// <returns>string seperated by commas</returns>
        public static string ToListString(this List<int> list)
        {
            if (list.Count <= 0)
                throw new Exception("Number of item in the list cannot be less than or equal 0");

            var idList = new StringBuilder();
            list.ForEach
                (
                    id => idList.Append(id.ToString() + ",")
                );

            return idList.ToString().TrimEnd(',');
        }

        public static bool TryParse<T>(this Enum theEnum, string strType,
                                       out T result)
        {
            string strTypeFixed = strType.Replace(' ', '_');
            if (Enum.IsDefined(typeof (T), strTypeFixed))
            {
                result = (T) Enum.Parse(typeof (T), strTypeFixed, true);
                return true;
            }

            string[] names = Enum.GetNames(typeof (T));
            foreach (
                string value in names.Where(value => value.Equals(strTypeFixed, StringComparison.OrdinalIgnoreCase)))
            {
                result = (T) Enum.Parse(typeof (T), value);
                return true;
            }

            result = default(T);
            return false;
        }
    }
}