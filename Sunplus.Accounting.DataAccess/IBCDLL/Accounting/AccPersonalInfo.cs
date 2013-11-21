using System;
using System.Collections;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Core.LoggingLib;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class AccPersonalInfo
    {

        private static StoreProcedureElement GetStoreProcedure(SportbookPtSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = SportbookPtSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }
        private static StoreProcedureElement GetUserStoreProcedure(UserControlSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = UserControlSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        public static DbResult AccPasswordVerify(string accName, int accId, string pass)
        {
            DbResult dr = null;
            try
            {
                var param = new Hashtable();
                param.Add("username", accName);
                param.Add("userpwd", pass);
                param.Add("userid", accId);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                
                using (ObjectDb db = new ObjectDb("Acc_Con_AdminSelPwdVerify"))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    dr = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return dr;
        }

        public static IDataReader AccInformationSelect(int custid)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                var spElement = GetUserStoreProcedure(UserControlSession.StoreCollection.AccountInfo, "CustomerSelUserInfo");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public static void AccInformationUpdate(Hashtable param)
        {
            try
            {
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                
                Hashtable outout = new Hashtable();
                using (ObjectDb db = new ObjectDb("Acc_CustomerUpdUserInfo"))
                {
                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();
                    outout = db.GetOuputParams();
                }

                if ((int)outout["errcode"] > 0) throw new AccException(outout["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}