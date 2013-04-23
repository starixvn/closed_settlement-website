using System;
using System.Collections.Generic;
using System.Data;

namespace Sunplus.Accounting.Core.Helper
{
    /// <summary>
    /// Summary description for DataUtils
    /// </summary>
    public class DataUtils
    {
        public static bool IsExistKey(DataRow dr, string key)
        {
            try
            {
                string value = dr[key].ToString();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsExistKey(Dictionary<string, double> dr, string key)
        {
            try
            {
                string value = dr[key].ToString();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}