using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class Credit
    {
        /// <summary>
        /// Select Credit for customer
        /// SP: Acc_CustomerSelCredit
        /// @viet hoang
        /// </summary>
        /// <param name="_custId"></param>
        /// <returns></returns>
        public static IDataReader CustomerSelectCredit(int _custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", _custId);

                return DbHelper.ExecuteReader("CustomerSelCredit", param, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Update New Credit for customer
        /// SP: Acc_CustomerUpdCredit
        /// @viet hoang
        /// </summary>
        /// <param name="_accId"></param>
        /// <param name="_custId"></param>
        /// <param name="_newCredit"></param>
        /// <returns></returns>
        public static void CustomerUpdateCredit(int _accId, int _custId, decimal _newCredit)
        {
            try
            {
                var param = new Hashtable();
                param.Add("userid", _accId);
                param.Add("custid", _custId);
                param.Add("newcredit", _newCredit);
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdCredit", param, DBType.MAIN, 120);
                if ((int) dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
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

    public sealed class UserCredit
    {
        /// <summary>
        /// Select Agent Credit --
        /// store procedure: Acc_CustomerSelUsedCredit_Ag --
        /// @viet hoang
        /// </summary>        
        /// <param name="_agentId"></param>
        /// <returns></returns>
        public static IDataReader SelectAgentCredit(int _agentId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("agentid", _agentId);

                return DbHelper.ExecuteReader("Acc_CustomerSelUsedCredit_Ag", param, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select Master Credit --
        /// Store Procedure: Acc_CustomerSelUsedCredit_MAg --
        /// @viet hoang
        /// </summary>
        /// <param name="_masterId"></param>
        /// <returns></returns>
        public static IDataReader SelectMasterCredit(int _masterId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("masterid", _masterId);

                return DbHelper.ExecuteReader("Acc_CustomerSelUsedCredit_MAg", param, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select Super Credit Details --
        /// Store Procedure: Acc_CustomerSelUsedCredit_SAg --
        /// @viet hoang
        /// </summary>
        /// <param name="_superId"></param>
        /// <returns></returns>
        public static IDataReader SelectSuperCredit(int _superId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("superid", _superId);

                return DbHelper.ExecuteReader("Acc_CustomerSelUsedCredit_SAg", param, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Agent Reset Member Credit
        /// SP: Acc_CustomerUpdResetMemberCredit
        /// @viet hoang
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_agentId"></param>
        /// <param name="closedOnly"></param>
        /// <returns></returns>
        public static void ResetMemberCredit(int _userId, int _agentId, bool closedOnly)
        {
            try
            {
                var param = new Hashtable();
                param.Add("userid", _userId);
                param.Add("agentid", _agentId);
                param.Add("closedonly", closedOnly);
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdResetMemberCredit", param, DBType.MAIN, 120);
                if ((int) dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
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