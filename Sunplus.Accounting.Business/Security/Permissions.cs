using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Bingo.Core;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Security
{
    public class Permissions
    {
        public static bool IsDisabledCS(int custId)
        {
            return CasinoManager.IsDisabledCasino(custId);
        }

        public static bool IsAllowEditCasino(int parentId)
        {
            if (parentId == 0)
                return true;
            var cust = CasinoManager.GetCasinoStatus(parentId);
            //if (WebSettings.disableCS || !cust.CasinoSync)
            //return false;
            //return true;

            return cust.CasinoSync;
        }

        public static CSCustomerDetail SelStatus3rdParty(int parentId)
        {
            return CasinoManager.GetCasinoStatus(parentId);
        }

        public static void InitAccessPermission(Enumerations.AccountingSystem systemType)
        {
            var pages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                DataTable dtb = DataAccess.IBCDLL.Accounting.UserControl.FunctionInfoSelect(systemType);
                foreach (DataRow dr in dtb.Rows)
                {
                    string strFilesName = dr["FileNames"].ToString();
                    string strFuncName = dr["FuncName"].ToString();
                    string[] strListFiles = strFilesName.Split(';');
                    for (int i = 0; i < strListFiles.Length; i++)
                    {
                        if (!pages.ContainsKey(strListFiles[i].Trim()))
                        {
                            pages.Add(strListFiles[i].Trim(), strFuncName.Trim() + "-1");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            AppCache.AccessPermission = pages;
        }

        public static void InitAccessPermission()
        {
            var pages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                DataTable dtb = DataAccess.IBCDLL.Accounting.UserControl.FunctionInfoSelect();
                foreach (DataRow dr in dtb.Rows)
                {
                    //if (CastAide.AsInt32(dr["SubFuncCount"]) == 0)
                    //{
                    string strFilesName = dr["FileNames"].ToString();
                    string strFuncName = dr["FuncName"].ToString();
                    string[] strListFiles = strFilesName.Split(';');
                    for (int i = 0; i < strListFiles.Length; i++)
                    {
                        if (!pages.ContainsKey(strListFiles[i].Trim()))
                        {
                            pages.Add(strListFiles[i].Trim(), strFuncName.Trim() + "-1");
                        }
                        //try
                        //{

                        //    pages.Add(strListFiles[i].Trim(), strFuncName.Trim() + "-1");
                        //}
                        //catch
                        //{

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
            }
            //Application["AccessPermission"] = pages;
            AppCache.AccessPermission = pages;
        }

        public static Dictionary<string, string> GetAccessPermission(Enumerations.AccountingSystem systemType)
        {
            var pages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                DataTable dtb = DataAccess.IBCDLL.Accounting.UserControl.FunctionInfoSelect(systemType);
                foreach (DataRow dr in dtb.Rows)
                {
                    string strFilesName = dr["FileNames"].ToString();
                    string strFuncName = dr["FuncName"].ToString();
                    string[] strListFiles = strFilesName.Split(';');
                    for (int i = 0; i < strListFiles.Length; i++)
                    {
                        if (!pages.ContainsKey(strListFiles[i].Trim()))
                        {
                            pages.Add(strListFiles[i].Trim(), strFuncName.Trim() + "-1");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return pages;
        }

        /// <summary>
        /// Finance Transfer: allow current login admin can transfer money from Finance account to Betting account or not 
        /// </summary>
        /// <param name="fromcustid"></param>
        /// <param name="touser"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static bool IsFiTransfer(int fromcustid, string touser, ref string errmsg)
        {
            bool IsCrossTransfer = false;
            var hash = new Hashtable();
            hash.Add("fromcustid", fromcustid);
            hash.Add("touser", touser);
            hash.Add("iscrosstransfer", 0);
            hash.Add("errcode", 0);
            hash.Add("errmsg", string.Empty);
            DbResult rs = DbHelper.Execute("Acc_CustomerSelIsCrossTransfer", hash, DBType.WAREHOUSE);

            int errcode = CastAide.AsInt32(rs.Outputs["errcode"]);
            errmsg = CastAide.AsString(rs.Outputs["errmsg"]);

            if (errcode > 0)
                return false;
            IsCrossTransfer = CastAide.AsBoolean(rs.Outputs["iscrosstransfer"]);

            if (IsCrossTransfer)
            {
                if (Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.FinanceTransfer))
                {
                    return IsFiTransferAccStatus(fromcustid, touser, ref errmsg);
                }
                else
                {
                    errmsg = "You are not authorized to transfer cross user";
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check: Just allow transfer when 2 accounts are open (Fi Tranfer)
        /// </summary>
        /// <param name="fromcustid"></param>
        /// <param name="touser"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public static bool IsFiTransferAccStatus(int fromcustid, string touser, ref string errmsg)
        {
            bool IsTransfer = false;
            var hash = new Hashtable();
            hash.Add("fromcustid", fromcustid);
            hash.Add("touser", touser);
            hash.Add("istransfer", 0);
            hash.Add("errcode", 0);
            hash.Add("errmsg", string.Empty);
            DbResult rs = DbHelper.Execute("Acc_CustomerSelFITransferStatus", hash, DBType.WAREHOUSE);

            int errcode = CastAide.AsInt32(rs.Outputs["errcode"]);
            errmsg = CastAide.AsString(rs.Outputs["errmsg"]);

            if (errcode > 0)
                return false;
            IsTransfer = CastAide.AsBoolean(rs.Outputs["istransfer"]);
            return IsTransfer;
        }

        public static bool AllowSaTote(string currencyName)
        {
            return true;
        }

        public static int CheckPermission<T>(int custId, T function, ref string errmsg)
        {
            int errcode = 0;
            try
            {
                var functionName = "[" + Enumerations.Functions.GetName(function) + "]";
                DbResult rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.CheckHostPermission(custId, functionName);
                errcode = CastAide.AsInt32(rs.Outputs["errcode"]);
                errmsg = CastAide.AsString(rs.Outputs["errmsg"]);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                errmsg = ex.Message;

            }
            return errcode;
        }



        public static bool IsDenyView2HSoccer
        {
            get { return !HasPermission(Enumerations.Functions.Reports.Report2HS); }
        }

        public static bool IsDenyView2HOther
        {
            get { return !HasPermission(Enumerations.Functions.Reports.Report2HO); }
        }

        public static bool IsAllowEditMG()
        {
            if (WebSettings.OpenMiniGame)
                return true;
            return false;
        }

        /// <summary>
        /// T: Enumeration.Functions enum (Customer, Setting...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="custId"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static bool GetHostPermission<T>(T function)
        {
            try
            {
                string functionName = "[" + Enumerations.Functions.GetName(function) + "]";

                if (AppSession.RoleAccess != null)
                {
                    if (AppSession.RoleAccess.ContainsKey(functionName.ToLower()))
                    {
                        if (AppSession.RoleAccess[functionName.ToLower()] == 3)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }



        public static bool HasPermission<T>(T function)
        {
            if (AppSession.RoleAccess == null)
                return false;
            var functionName = "[" + Enumerations.Functions.GetName(function) + "]";
            return AppSession.RoleAccess.ContainsKey(functionName.ToLower());
        }

        public static bool HasHostPermission<T>(T function)
        {
            if (AppSession.RoleAccess == null)
            {
                return false;
            }
            var functionName = "[" + Enumerations.Functions.GetName(function) + "]";
            if (!AppSession.RoleAccess.ContainsKey(functionName.ToLower()))
            {
                return false;
            }
            return AppSession.RoleAccess[functionName.ToLower()] == 3;           
        }

        public static bool HasPermission(string funcName)
        {
            if (AppSession.RoleAccess == null)
                return false;
            if (string.IsNullOrEmpty(funcName))
                return false;
            funcName = "[" + funcName.ToLower() + "]";

            return AppSession.RoleAccess.ContainsKey(funcName.ToLower());
        }

        public static bool IsAllowCasino()
        {
            if (WebSettings.disableCS || !AppSession.IsSyncCSCurrentCust)
                return false;
            return true;
        }



        public static bool IsAllowEditCasino(int parentId, bool syncCasino)
        {
            if (parentId == 0)
                return true;
            if (WebSettings.disableCS || !syncCasino)
                return false;
            return true;
        }

        public static bool IsAllowEditBingo(int parentId, bool syncBingo)
        {
            if (parentId == 0)
                return true;
            if (!BingoSettings.BingoGame.Enabled || !syncBingo)
                return false;
            return true;
        }


        public static bool IsAllowEditFI()
        {
            return (!WebSettings.disabledFI);
        }

        public static bool IsEnabledAutoMaster(bool isAllowedAutoMaster)
        {
            if (WebSettings.disableAutoM)
                return false;
            if (!isAllowedAutoMaster)
                return false;
            return true;
        }

        public static bool ViewMainFunction(int mainFuncId, DataRowCollection lsFunc)
        {
            try
            {
                foreach (DataRow func in lsFunc)
                {
                    if (CastAide.AsInt16(func["parentid"], 0) == mainFuncId && CastAide.AsInt16(func["funcid"], 0) != mainFuncId)
                    {
                        if (DataUtils.IsExistKey(func, "actual_policy"))
                        {
                            if (CastAide.AsInt16(func["actual_policy"], 0) > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }

        public static bool IsAllowEditLiveCasino(int parentId, bool syncLiveCasino)
        {
            if (parentId == 0)
                return true;
            if (!WebSettings.EnableLiveCasino || !syncLiveCasino)
                return false;
            return true;
        }

        #region Settlement
        public static string CheckHostPermissionSettle(int custID)
        {
            string result = "";
            if (GetHostPermission(Enumerations.Functions.Settle.KeyScore))
                result += "[K]"; //Host permission on keyscore
            if (GetHostPermission(Enumerations.Functions.Settle.Unsettle))
                result += "[U]"; //Host permission on unsettle
            if (GetHostPermission(Enumerations.Functions.Settle.Calculation))
                result += "[C]"; //Host permission on Calculate/Refund
            if (GetHostPermission(Enumerations.Functions.Settle.UndecidedSettleMatch))
                result += "[D]"; //Host permission on Undecided Settle Match
            return result;
        }
        #endregion


        #region Nested type: FinancePermission

        public class FinancePermission
        {
            /// <summary>
            /// Check Admin can Add Transaction for FI Account or not
            /// </summary>
            public static bool IsFiAddTransaction
            {
                get { return Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.FinanceTransfer); }
            }

            /// <summary>
            /// Create Finance Super Account: 
            /// - Only user admin that is granted hot permission on this function can create a new finance super account 
            /// </summary>
            public static bool IsCreateFiAcc
            {
                get { return Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.CreateFinanceAccount); }
            }

            /// <summary>
            /// Open/Close Finance Account: 
            /// - Allow a certain admin can Close or Open back a Finance Account 
            /// </summary>
            public static bool IsOpenCloseFiAcc
            {
                get { return Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.OpenCloseFinanceAccount); }
            }

            /// <summary>
            /// Verify Transaction
            /// - Allow to Verify  transaction on Daily Transactions and Statement or not 
            /// </summary>
            public static bool IsVerifyTrans
            {
                get { return Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.ManageTransaction); }
            }

            /// <summary>
            /// Verify Transaction
            /// Allow to view Daily Tnx or not
            /// </summary>
            public static bool IsViewFiTrans
            {
                get { return Permissions.HasPermission(Enumerations.Functions.FinancialManagement.ManageTransaction); }
            }



            /// <summary>
            /// View Finance Account:
            /// - Allow to search Finance account in the left panel
            /// - Allow to view the List of Finance Account 
            /// </summary>
            /// <param name="IsInternal"></param>
            /// <returns></returns>
            public static bool IsViewFiAcc(bool IsInternal)
            {
                if (IsInternal)
                    return HasPermission(Enumerations.Functions.FinancialManagement.ViewFinanceAccount);
                else
                    return true;
            }
        }

        #endregion

    }
}
