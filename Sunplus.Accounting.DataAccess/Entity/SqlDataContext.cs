using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sunplus.Accounting.DataAccess.Entity.BusinessObject;

/// <summary>
/// Summary description for SqlDataContext
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity
{
    public class SqlDataContext : DataAccess
    {
        /// <summary>
        /// Execute Selection SP
        /// </summary>
        /// <typeparam name="TO">Entity Class</typeparam>
        /// <param name="spName"></param>
        /// <param name="param"></param>
        /// <param name="dbtype"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static List<TO> ExecuteReader<TO>(string spName, Hashtable param, string dbtype, int timeout)
            where TO : class, new()
        {
            //ClearCachedMapperMethods();
            //TODO: Insert method into your data access utility class                  
            var dbConn = new SqlConnection(ConString(dbtype));
            //Open connection if it's closed
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            //TODO: Move constant declaration below directly into containing class          

            var cmd = new SqlCommand(spName, dbConn)
                          {
                              CommandType = CommandType.StoredProcedure,
                              CommandTimeout = timeout
                          };
            foreach (string key in param.Keys)
            {
#pragma warning disable 612,618
                cmd.Parameters.Add("@" + key, param[key]);
#pragma warning restore 612,618
                //p.Value = param[p.ParameterName.Replace("@", "").ToLowerInvariant()];
            }

            SqlDataReader rdr;
            var result = new List<TO>();

            try
            {
                rdr = cmd.ExecuteReader();

                try
                {
                    if (rdr != null)
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                //   result.Add(OjbectMapper
                                //MapToEntities<LeagueSportWLDetails>(rdr);
                                var entity = ObjectMapper.FillObject<TO>(typeof (TO), rdr);
                                result.Add(entity);
                            }
                        }
                }
                finally
                {
                    if (rdr != null) rdr.Close();
                }
            }
            catch (Exception ex)
            {
                //TODO: Handle Exception
                throw ex;
            }
            finally
            {
                cmd.Dispose();

                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }

            return result;
        }

        /// <summary>
        /// Execute Selection SP
        /// </summary>
        /// <typeparam name="TO">Entity Class</typeparam>
        /// <param name="spName"></param>
        /// <param name="param"></param>
        /// <param name="dbtype"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ArrayList ExecuteReaders<TO>(string spName, Hashtable param, string dbtype, int timeout)
            where TO : class, new()
        {
            //ClearCachedMapperMethods();
            //TODO: Insert method into your data access utility class                  
            var dbConn = new SqlConnection(ConString(dbtype));
            //Open connection if it's closed
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            //TODO: Move constant declaration below directly into containing class          

            var cmd = new SqlCommand(spName, dbConn)
                          {
                              CommandType = CommandType.StoredProcedure,
                              CommandTimeout = timeout
                          };
            foreach (string key in param.Keys)
            {
#pragma warning disable 612,618
                cmd.Parameters.Add("@" + key, param[key]);
#pragma warning restore 612,618
                //p.Value = param[p.ParameterName.Replace("@", "").ToLowerInvariant()];
            }

            //SqlDataReader rdr;
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            DataTableCollection dts = ds.Tables;
            var resultIteam = new List<TO>();
            var results = new ArrayList();


            try
            {
                //rdr = cmd.ExecuteReader();

                try
                {
                    foreach (DataTable dt in dts)
                    {
                        //resultIteam.Clear();
                        resultIteam = new List<TO>();
                        DataRow[] drs = dt.Select();
                        foreach (DataRow dr in drs)
                        {
                            //   result.Add(OjbectMapper
                            //MapToEntities<LeagueSportWLDetails>(rdr);
                            var entity = ObjectMapper.FillObject<TO>(typeof (TO), dr);
                            resultIteam.Add(entity);
                        }
                        results.Add(resultIteam);
                    }
                }
                finally
                {
                    da.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //TODO: Handle Exception
                throw ex;
            }
            finally
            {
                cmd.Dispose();

                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }

            return results;
        }

        public static List<TO> ExecuteReader<TO>(string spName, Hashtable param, string dbtype) where TO : class, new()
        {
            //ClearCachedMapperMethods();
            //TODO: Insert method into your data access utility class                  
            var dbConn = new SqlConnection(ConString(dbtype));
            //Open connection if it's closed
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            //TODO: Move constant declaration below directly into containing class          

            var cmd = new SqlCommand(spName, dbConn) {CommandType = CommandType.StoredProcedure};
            foreach (string key in param.Keys)
            {
#pragma warning disable 612,618
                cmd.Parameters.Add("@" + key, param[key]);
#pragma warning restore 612,618
                //p.Value = param[p.ParameterName.Replace("@", "").ToLowerInvariant()];
            }

            SqlDataReader rdr;
            var result = new List<TO>();

            try
            {
                rdr = cmd.ExecuteReader();

                try
                {
                    if (rdr != null)
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                //   result.Add(OjbectMapper
                                //MapToEntities<LeagueSportWLDetails>(rdr);
                                var entity = ObjectMapper.FillObject<TO>(typeof (TO), rdr);
                                result.Add(entity);
                            }
                        }
                }
                finally
                {
                    if (rdr != null) rdr.Close();
                }
            }
            catch (Exception ex)
            {
                //TODO: Handle Exception
                throw ex;
            }
            finally
            {
                cmd.Dispose();

                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }

            return result;
        }

        /// <summary>
        /// Return with output
        /// </summary>
        /// <typeparam name="TO"></typeparam>
        /// <param name="spName"></param>
        /// <param name="param"></param>
        /// <param name="dbtype"></param>
        /// <param name="timeout"></param>
        /// <param name="output">Output Params</param>
        /// <returns></returns>
        public static List<TO> ExecuteReader<TO>(string spName, Hashtable param, string dbtype, int timeout,
                                                 ref Hashtable output) where TO : class, new()
        {
            //ClearCachedMapperMethods();
            //TODO: Insert method into your data access utility class                  
            var dbConn = new SqlConnection(ConString(dbtype));
            //Open connection if it's closed
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            //TODO: Move constant declaration below directly into containing class          

            var cmd = new SqlCommand(spName, dbConn)
                          {
                              CommandType = CommandType.StoredProcedure,
                              CommandTimeout = timeout
                          };
            foreach (string key in param.Keys)
            {
#pragma warning disable 612,618
                cmd.Parameters.Add("@" + key, param[key]);
#pragma warning restore 612,618
                //p.Value = param[p.ParameterName.Replace("@", "").ToLowerInvariant()];
            }

            SqlDataReader rdr;
            var result = new List<TO>();

            try
            {
                rdr = cmd.ExecuteReader();

                try
                {
                    if (rdr != null)
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                //   result.Add(OjbectMapper
                                //MapToEntities<LeagueSportWLDetails>(rdr);
                                var entity = ObjectMapper.FillObject<TO>(typeof (TO), rdr);
                                result.Add(entity);
                            }
                        }
                }
                finally
                {
                    if (rdr != null) rdr.Close();
                }
            }
            catch (Exception ex)
            {
                //TODO: Handle Exception
                throw ex;
            }
            finally
            {
                cmd.Dispose();

                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TO"></typeparam>
        /// <param name="spName"></param>
        /// <param name="param"></param>
        /// <param name="dbtype"></param>
        /// <param name="output">Output Params</param>
        /// <returns></returns>
        public static List<TO> ExecuteReader<TO>(string spName, Hashtable param, string dbtype, ref Hashtable output)
            where TO : class, new()
        {
            //ClearCachedMapperMethods();
            //TODO: Insert method into your data access utility class                  
            var dbConn = new SqlConnection(ConString(dbtype));
            //Open connection if it's closed
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            //TODO: Move constant declaration below directly into containing class          

            var cmd = new SqlCommand(spName, dbConn) {CommandType = CommandType.StoredProcedure};
            foreach (string key in param.Keys)
            {
#pragma warning disable 612,618
                cmd.Parameters.Add("@" + key, param[key]);
#pragma warning restore 612,618
                //p.Value = param[p.ParameterName.Replace("@", "").ToLowerInvariant()];
            }

            foreach (object keyOut in output.Keys)
            {
                var outputParam = new SqlParameter("@" + keyOut, output[keyOut]) {Direction = ParameterDirection.Output};
                cmd.Parameters.Add(outputParam);
            }


            SqlDataReader rdr;
            var result = new List<TO>();

            try
            {
                rdr = cmd.ExecuteReader();

                try
                {
                    if (rdr != null)
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                //   result.Add(OjbectMapper
                                //MapToEntities<LeagueSportWLDetails>(rdr);
                                var entity = ObjectMapper.FillObject<TO>(typeof (TO), rdr);
                                result.Add(entity);
                            }
                        }
                }
                finally
                {
                    if (rdr != null) rdr.Close();
                }
                //output params
                var o = new Hashtable();
                foreach (object keyOut in output.Keys)
                {
                    o.Add(keyOut, cmd.Parameters["@" + keyOut].Value);
                }
                output = o;
            }
            catch (Exception ex)
            {
                //TODO: Handle Exception
                throw ex;
            }
            finally
            {
                cmd.Dispose();

                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();
            }

            return result;
        }
    }
}