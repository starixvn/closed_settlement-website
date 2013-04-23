using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CasinoLibrary.Model.BaseBO;
using CasinoLibrary.Model.Common;
using DNAide.Core;
using DNAide.Data;
using JSoft.Utility;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Extensions;


namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    public class SelectCustomer: DataAccess
    {
        private static Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;
            
        }

        private static Core.Configurations.StoreProcedureElement GetHelperStoreProcedure(string aliasName)
        {
            var sps = HelperSession.Instance[HelperSession.StoreCollection.HelperControl];
            return sps != null ? sps[aliasName] : null;

        }

        private static Core.Configurations.StoreProcedureElement GetExchangeStoreProcedure(string aliasName)
        {
            var sps = ExchangeRateSession.Instance[ExchangeRateSession.StoreCollection.ExchangeRateControl];

            return sps != null ? sps[aliasName] : null;

        }

        private static StoreProcedureElement GetUserControlStoreProcedure(string aliasName)
        {
            var sps = UserControlSession.Instance[UserControlSession.StoreCollection.AccountInfo];

            return sps != null ? sps[aliasName] : null;

        }

        private static StoreProcedureElement GetSportBookStoreProcedure(string aliasName)
        {
            var sps = SportbookPtSession.Instance[SportbookPtSession.StoreCollection.SportBooksPtControl];

            return sps != null ? sps[aliasName] : null;

        }

        

        /// <summary>
        /// call Store SiteSel to get all SiteName
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns>DataResult</returns>
        public static DbResult SiteSelRpt(string system)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable {{"system", system.ToString().ToUpper()}};
                var spElement = GetStoreProcedure("SiteSel");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call Store ExchangeSel to get all Currency
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns>DataResult</returns>
        public static DbResult ExchangeSelRpt(string accountingSystem)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable {{"system",accountingSystem.ToString().ToUpper()}};

                var spElement = GetExchangeStoreProcedure("ExchangeSel");

                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code
        /// Vu Thi Hong Hanh review 
        /// 09/11/2006
        /// call store procedure CustomerSelSAgList
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns></returns>
        public static DbResult CustomerSelSAgListRpt(string accountingSystem)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable {{"system", accountingSystem.ToString().ToUpper()}};



                var spElement = GetStoreProcedure("SelSAgList");
                 rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// execute SP CustomerSelectByUserName to get user information
        /// @PhuongDN
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="accountingSystem"></param>
        /// <returns>DataResult</returns>
        public static DbResult CustSelByUsername(string userName, string accountingSystem)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable
                            {
                                {"username", userName},
                                {"system", accountingSystem.ToString().ToUpper()}
                            };
                var spElement = GetStoreProcedure("CustomerSelByUserName");
                ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        public static DbResult CustSelById(int custId, string accountingSystem)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable
                            {
                                {"custid", custId},
                                {"system", accountingSystem.ToString().ToUpper()}
                            };
                var spElement = GetStoreProcedure("CustomerSelByUserName");
                ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        ///// <summary>
        ///// SP: Age_CustomerSelCheckBelonging_ByUsername
        ///// Get custid of account or subaccount
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="uplineID"></param>
        ///// <returns></returns>
        //public static DbResult CustSelByUsername(string username, int uplineID)
        //{
        //    DbResult ds = null;
        //    try
        //    {
        //        Hashtable p = new Hashtable();
        //        p.Add("username", username);
        //        p.Add("uplineid", uplineID);
        //        p.Add("custid", 0);
        //        p.Add("issubacc", false);
        //        ds = DbHelper.Execute("Age_CustomerSelCheckBelonging_ByUsername", p, DBType.ARCHIVE);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        //    }
        //    return ds;
        //}


        /// <summary>
        /// Nguyen Thi Minh Thu 
        /// 29/11/2006
        /// Execute SP CustomerSelAgentInfo to get agent information 
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid">agentId</param>
        /// <returns></returns>
        public static DbResult CustomerSelAgentInfoRpt(object custid)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custid);


                var spElement = GetStoreProcedure("CustomerSelAgentInfo");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Execute SP CustomerSelMAgentInfo to get master agent info
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static DbResult CustomerSelMAgentInfo(object custId)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custId);


                var spElement = GetStoreProcedure("CustomerSelMAgentInfo");

                ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// call SP CustomerSelSAgentInfo
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId">custId</param>
        /// <returns>DataResult</returns>
        public static DbResult CustomerSelSAgentInfoRpt(string custId)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);

                var spElement = GetStoreProcedure("CustomerSelSAgentInfo");


                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 
        /// 29/11/2006
        /// Execute SP CustomerSelCustomerInfo to get group for add new member
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult CustomerSelCustomerInfoRpt(object custid)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custid);
                var spElement = GetStoreProcedure("CustomerSelCustomerInfo");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// call SP CustomerSelAddOperator to get data for Operator list
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns>DataResult</returns>
        public static DbResult CustomerSelAddOperatorRpt(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement = GetStoreProcedure("OperatorSel");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// @Khoa.Tran
        /// exec Acc_OperatorSel_1
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        //public static DataResult CustomerSelAddOperatorRpt_1(Hashtable hash)
        public static DbResult CustomerSelAddOperatorRpt_1(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement = GetStoreProcedure("OperatorSel");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call SP CustomerSelEditOperator to get data for Edit Operator
        /// writer: Vu Hanh
        /// </summary>
        /// <param name="custId">custId</param>
        /// <returns>DataResult</returns>
        //public static DataResult CustomerSelEditOperatorRpt(int custId)
        public static DbResult CustomerSelEditOperatorRpt(int custId)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);
                var spElement = GetStoreProcedure("OperatorSelByID");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// @Khoa.Tran 
        /// exec sp Acc_CustomerSelSAg_1
        /// Using DNAide's library
        /// Tho Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static DbResult GetSuperList_1(Hashtable hash, string accountingSystem)
        {
            DbResult rs = null;
            try
            {
                hash.Add("system", accountingSystem.ToString().ToUpper());

                var spElement = GetStoreProcedure("SuperList");

                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call SP CustomerSelBySAg to get data for Master list
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 11/29/2008
        /// </summary>
        /// <returns>DataResult</returns>
        public static DbResult GetMasterList(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement = GetStoreProcedure("MasterList");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call SP CustomerSelByMAg to get data for Agent list
        /// writer: Vu Hanh
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <returns>DataResult</returns>
        public static DbResult GetAgentList(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement = GetStoreProcedure("AgentList");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// call SP CustomerSelByAg to get data for Member list
        /// writer: Vu Hanh
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <returns>DataResult</returns>
        public static DbResult GetMemberList(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                
                var spElement = GetStoreProcedure("MemberList");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static DbResult GetMasterByComList_1(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement = GetStoreProcedure("DirectMasterList");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu
        /// Get information of user
        /// execute sp Acc_CustomerSelUserInfo
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult CustomerSelUserInfoRpt(object custid)
        {
            var hash = new Hashtable();
            DbResult ds = null;
            hash.Add("custid", custid);
            try
            {
                var spElement = GetUserControlStoreProcedure("CustomerSelUserInfo");
                ds = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Execute SP CustomerSelComEditOverride_Ag, CustomerSelComEditOverride_MAg, or CustomerSelComEditOverride_SAg
        /// to get Min Max position taking
        /// @PhuongDN
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="agentType">s: Super;m: Master; a: Agent</param>
        /// <param name="custId"></param>
        /// <returns>DataResult containing Max, Min, Mid position taking of Dead ball soccer, Live soccer, Basket ball.
        /// 0.9>=Max>=Mid;Mid>=Min>=0</returns>
        public static DbResult GetMaxMinPositionTaking(string agentType, string custId)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custId);
                switch (agentType.ToLower())
                {
                    case "super":
                        var spElementS = GetSportBookStoreProcedure("CustomerSelComEditOverride_SAg");
                        ds = DbHelper.Execute(spElementS.Name, p, spElementS.DbName);
                        break;
                    case "master":
                        var spElementM = GetSportBookStoreProcedure("CustomerSelComEditOverride_MAg");
                        ds = DbHelper.Execute(spElementM.Name, p, spElementM.DbName);
                        break;
                    case "agent":
                        var spElementA = GetSportBookStoreProcedure("CustomerSelComEditOverride_Ag");
                        ds = DbHelper.Execute(spElementA.Name, p, spElementA.DbName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Execute SP CustomerSelComEditOverride_Ag, CustomerSelComEditOverride_MAg, or CustomerSelComEditOverride_SAg
        /// to get Min Max position taking
        /// @PhuongDN
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="agentType">s: Super;m: Master; a: Agent</param>
        /// <param name="custId"></param>
        /// <returns>DataResult containing Max, Min, Mid position taking of Dead ball soccer, Live soccer, Basket ball.
        /// 0.9>=Max>=Mid;Mid>=Min>=0</returns>
        public static DbResult GetMaxMinPositionTaking_acc(string agentType, string custId)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custId);

                var spElement = new SelectCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelComEditOverride_Ag");

                switch (agentType.ToLower())
                {
                    case "super":

                        spElement = new SelectCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelComEditOverride_SAg");
                        ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
                        break;
                    case "master":

                        spElement = new SelectCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelComEditOverride_MAg");
                        ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
                        break;
                    case "agent":
                        spElement = new SelectCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelComEditOverride_Ag");
                        ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public static bool IsDirectMaster(int custID)
        {
            var p = new Hashtable();
            p.Add("custid", custID);
            p.Add("isdirect", false);
            var spElement = GetStoreProcedure("CustomerSelIsDirectMaster");
            DbResult dr = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            return CastAide.AsBoolean(dr.Outputs["isdirect"], false);
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public static decimal GetMatchLimit(int custID)
        {
            try
            {
                var p = new Hashtable();
                p.Add("custid", custID);
                p.Add("matchlimit", 0);
                var spElement = GetStoreProcedure("CustomerSelMatchLimit");
                DbResult dr = DbHelper.Execute(spElement.Name, p, spElement.DbName);
                return CastAide.AsDecimal(dr.Outputs["matchlimit"], 0);
            }
            catch (InvalidOperationException invalidOperation)
            {
                
                    LogExtensions.ExceptionHandler(invalidOperation);
                throw;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns></returns>
        public static IDataReader GetPTSportType()
        {
            //var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(StoreProcedureSection.StoreCollection.ViewPt, "GetSportType");
            var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ViewPtSession.StoreCollection.ViewPtControl, "GetSportType");
            return DbHelper.ExecuteReader(spElement.Name, spElement.DbName);
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns></returns>
        public static IDataReader GetPTBetType(int sportID)
        {
            var p = new Hashtable();
            if (sportID != 0) p.Add("sporttype", sportID);

            //var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(StoreProcedureSection.StoreCollection.ViewPt, "GetBetType");
            var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ViewPtSession.StoreCollection.ViewPtControl, "GetBetType");
            return DbHelper.ExecuteReader(spElement.Name, p, spElement.DbName);
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns></returns>
        public static DbResult GetPT(string username, int sportID, int bettype, bool isLive, bool hasComm)
        {
            var p = new Hashtable();
            p.Add("username", username);
            p.Add("sporttype", sportID);
            p.Add("bettype", bettype);
            p.Add("islive", isLive);
            p.Add("hascomm", hasComm);
            p.Add("errcode", 0);
            p.Add("errmsg", string.Empty);

            //var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(StoreProcedureSection.StoreCollection.ViewPt, "GetPT");
            var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ViewPtSession.StoreCollection.ViewPtControl, "GetPT");
            return DbHelper.Execute(spElement.Name, p, spElement.DbName);
        }

        /// <summary>
        /// Using CasinoLibrary's Library
        /// Tuan.Anh Create: 2/4/2009
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCSPT(string username, int GameKind, int GameType)
        {
            DataTable dtb = null;
            try
            {
                Dictionary<string, string> objPara = null;
                objPara = new Dictionary<string, string>();
                objPara.Add("username", username);
                objPara.Add("GameKind", GameKind.ToString());
                objPara.Add("GameType", GameType.ToString());

                var nInParameters = new SqlParameter[3];
                nInParameters[0] = objPara.ContainsKey("username")
                                       ? new SqlParameter("@username", StringUtil.CStr(objPara["username"]))
                                       : new SqlParameter("@username", SqlDbType.VarChar);
                nInParameters[0].Direction = ParameterDirection.Input;
                nInParameters[1] = objPara.ContainsKey("GameKind")
                                       ? new SqlParameter("@GameKind", StringUtil.CInt(objPara["GameKind"]))
                                       : new SqlParameter("@GameKind", SqlDbType.Int);
                nInParameters[1].Direction = ParameterDirection.Input;
                nInParameters[2] = objPara.ContainsKey("GameType")
                                       ? new SqlParameter("@GameType", StringUtil.CInt(objPara["GameType"]))
                                       : new SqlParameter("@GameType", SqlDbType.Int);
                nInParameters[2].Direction = ParameterDirection.Input;

                PersistenceMgr objMgr = null;
                var bs = new CustGameTypeBaseBO();
                objMgr = bs.GetDBManager(DBType.CS_BackupDB);
                dtb = objMgr.ExecSPbyParamsWithDataTable("Acc_MT_GetCSPT", nInParameters);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dtb;
        }


        /// <summary>
        /// Using CasinoLibrary's Library
        /// Shifu Create: 13/04/2010
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMiniGamePt(string username)
        {
            DataTable dtb = null;
            try
            {
                Dictionary<string, string> objPara = null;
                objPara = new Dictionary<string, string> {{"username", username}};

                var nInParameters = new SqlParameter[1];
                nInParameters[0] = objPara.ContainsKey("username")
                                       ? new SqlParameter("@username", StringUtil.CStr(objPara["username"]))
                                       : new SqlParameter("@username", SqlDbType.VarChar);
                nInParameters[0].Direction = ParameterDirection.Input;

                PersistenceMgr objMgr = null;
                var bs = new CustGameTypeBaseBO();
                objMgr = bs.GetDBManager(DBType.MiniGame);
                dtb = objMgr.ExecSPbyParamsWithDataTable("Acc_MT_GetMiniGamePT", nInParameters);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dtb;
        }

        /// <summary>
        /// SP: Agw_SelCustomerGivenCredit
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static decimal GetCustomerGivenCredit(string custid /*, out string roleid*/)
        {
            var param = new Hashtable();
            param.Add("custid", custid);
            var spElement = GetStoreProcedure("SelCustomerGivenCredit");
            DbResult dr = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            //roleid = dr.ResultSet.Tables[0].Rows[0]["roleid"].ToString();
            return Math.Round(decimal.Parse(dr.ResultSet.Tables[0].Rows[0]["givencredit"].ToString()));
        }

        public static DbResult QueryUserName(int userid, string query, string accountingSystem, bool hasViewFinanceAccount)
        {
            

            var param = new Hashtable();
            param.Add("custid", userid);
            param.Add("username", query);
            param.Add("system", accountingSystem.ToString());
            param.Add("isallowfisearch", hasViewFinanceAccount);
            var spElement = GetStoreProcedure("CustomerSelUserSearch");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName);
        }

        public static DbResult GetLiveCasinoPt(string username)
        {
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("username", username);

                var spElement = new SelectCustomerAdapter().GetStoreProcedureInfo(ViewPtSession.StoreCollection.ViewPtControl, "GetLiveCSPT");
                dr = DbHelper.Execute(spElement.Name, p, spElement.DbName);
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }
       
    }

    public class SelectCustomerAdapter : DataAccess
    {
        public StoreProcedureElement GetStoreProcedureInfo(ViewPtSession.StoreCollection storeCollection, string aliasName)
        {
            return GetStoreProcedure(storeCollection, aliasName);
        }
    }
}
