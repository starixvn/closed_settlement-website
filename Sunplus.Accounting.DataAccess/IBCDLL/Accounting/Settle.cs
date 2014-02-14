using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public static class Settle
    {
        private static System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;

        public static DbResult GetSettlePermission(int _custID)
        {
            Hashtable p = new Hashtable();
            p.Add("custid", _custID);
            p.Add("funcid", 1); // default function: settlement
            p.Add("language", "E");
            p.Add("errcode", 0);
            p.Add("errmsg", string.Empty);

            return DbHelper.Execute("Acc_Con_CheckPermission", p, DBType.DB03, 120);
        }

        /// <summary>
        /// get all sport
        /// store procedure: Acc_SelSports
        /// @viet hoang
        /// Using DNAide's Library 
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <returns></returns>
        public static IDataReader SettleSelectSport()
        {
            var spElement = new GetSpConfig().GetStoreElement(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "SelSports");
            return DbHelper.ExecuteReader(spElement.Name, spElement.DbName);

        }
        /// <summary>
        /// get league for settlement
        /// store procedure: Acc_Set_LeagueSel_1
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// PHuong.Nguyen updated: 2009-01-14
        /// </summary>        
        public static IDataReader SettleSelectLeague(DateTime fromDate, DateTime toDate, int sporttype)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("fromdate", fromDate);
                param.Add("todate", FormatHandler.ResetEndDate(toDate));
                param.Add("sporttype", sporttype);

                //var spElement = new GetSpConfig().GetStoreElement(StoreProcedureSection.StoreCollection.Settlement, "SetLeagueSel");
                var spElement = new GetSpConfig().GetStoreElement(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "SetLeagueSel");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select match for settlement
        /// store procedure: Acc_Set_MatchSel_1
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// Phuong.Nguyen updated: 20090206
        /// </summary>        
        public static DataSet SettleSelectMatch(int sporttype, DateTime fromDate, DateTime toDate, int leagueid, bool uncal, bool uncalmul)
        {
            try
            {                 
                using (ObjectDb objDb = new ObjectDb("Acc_Set_MatchSel", 800))
                {
                    Hashtable param = new Hashtable();
                    param.Add("sporttype", sporttype);
                    param.Add("fromdate", fromDate);
                    param.Add("todate", FormatHandler.ResetEndDate(toDate));
                    param.Add("leagueid", leagueid);
                    param.Add("uncalonly", uncal);
                    param.Add("uncalmultionly", uncalmul);

                    objDb.AssignParamValues(param);
                    return objDb.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new DataSet();
            }
        }

        ///<summary>
        /// Select Competed match for settlement
        /// SP: Acc_Set_MatchSelCompleted_1
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>       
        public static DataSet SettleSelectCompleteMatch(int sporttype, DateTime eventdate)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_Set_MatchSelCompleted", 1800))
                {
                    Hashtable param = new Hashtable();
                    param.Add("fromdate", eventdate);
                    param.Add("sporttype", sporttype);

                    objDb.AssignParamValues(param);
                    return objDb.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {    
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new DataSet();
            }
        }

        ///<summary>
        /// Select Competed match for settlement
        /// SP: Acc_Set_MatchSelCompletedHT_Soccer
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>       
        public static DataSet SettleSelectCompleteHTMatch_soccer(DateTime eventdate, int sportID)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_Set_MatchSelCompletedHT", 1800))
                {
                    Hashtable param = new Hashtable();
                    param.Add("fromdate", eventdate);
                    param.Add("sporttype", sportID);

                    objDb.AssignParamValues(param);
                    return objDb.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new DataSet();
            }
        }


        /// <summary>
        /// Select match for bill in settlement
        /// store procedure: Acc_Set_MatchSelForBill_1
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>        
        public static DataSet SelectMatchForBill(int matchID, ref Hashtable outputPrams)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_Set_MatchSelForBill", 120))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchID);
                    p.Add("isexistfglg", false);//is exist First goal / Last goal
                    p.Add("ishtonly", false); //First half only
                    p.Add("errcode", 0);
                    p.Add("isexistsng", 0);
                    p.Add("errMsg", string.Empty);

                    objDb.AssignParamValues(p);
                    var dsResult = objDb.ExecuteDataSet();

                    outputPrams = new Hashtable(objDb.GetOuputParams());                    

                    return dsResult;

                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        /// <summary>
        /// Select match for Key Score in settlement
        /// store procedure: Acc_Set_MatchSelScore_1
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>        
        public static DataSet SelectMatchForKeyScore(int matchID, ref Hashtable outputPrams)
        {               
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_Set_MatchSelScore", 120))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchID);
                    p.Add("isexistfglg", true);//is exist First goal / Last goal
                    p.Add("isexistlg", false);//is exist Last goal only
                    p.Add("errcode", 0);
                    p.Add("errMsg", string.Empty);

                    objDb.AssignParamValues(p);
                    
                   
                    var dsResult = objDb.ExecuteDataSet();
                    outputPrams = new Hashtable(objDb.GetOuputParams());                      

                    return dsResult;
                }
            }
            catch (Exception ex)
            {  
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        /// <summary>
        /// Update Settlement
        /// store procedure: Admin_Settlement_1
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>        
        public static void Settlement(int custID, int matchID, int homeHT, int awayHT, int homeFT, int awayFT, string betStatus, string action, bool isHTCal, string goalrecord, ref int errorCode, ref string errMsg)
        {
            try
            {                   
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_1", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custID);
                    p.Add("matchid", matchID);
                    p.Add("hth", homeHT);
                    p.Add("hta", awayHT);
                    if (!isHTCal)
                    {
                        p.Add("fth", homeFT);
                        p.Add("fta", awayFT);
                    }
                    p.Add("goalrecord", goalrecord);
                    p.Add("betstatus", betStatus);
                    p.Add("action", action);

                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    errMsg = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                }

            }
            catch (Exception ex)
            {
                
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custID"></param>
        /// <param name="matchID"></param>
        /// <param name="betStatus"></param>
        /// <param name="includeLG"></param>
        public static void SettlementSoccerFGLG(int custID, int matchID, string betStatus, bool includeLG, string GR, ref int errCode, ref string errMsg)
        {
            try
            {                   
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_FGLG", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custID);
                    p.Add("matchid", matchID);
                    p.Add("betstatus", betStatus);
                    p.Add("includelg", includeLG);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    p.Add("goalrecord", GR);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    errCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    errMsg = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
        }


        /// <summary>
        /// Settlement Match Update Score
        /// store procedure: Acc_Set_MatchUpdScore_1
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>        
        public static void KeyScore(int custID, int matchID, int homeHT, int awayHT, int homeFT, int awayFT, string betStatus)
        {
            try
            {
                Hashtable p = new Hashtable();
                p.Add("userid", custID);
                p.Add("matchid", matchID);
                p.Add("hthomescore", homeHT);
                p.Add("htawayscore", awayHT);
                p.Add("finalhomescore", homeFT);
                p.Add("finalawayscore", awayFT);
                p.Add("betstatus", betStatus);
                var spElement = new GetSpConfig().GetStoreElement(Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "SetMatchUpdScore");
                DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Refund settlement
        /// store procedure: refund and updateMatch_remark
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>        
        public static bool Refund(int custid, int matchid, bool IsExcludeBetdaq, bool IsSettleDeterminedTickets, ref string msg
            , string homeHScore, string awayHScore, string homeFScore, string awayFScore, string scoreConfirm_Betstatus, bool IsExcludeBetfair = false, bool IsSettleDeterminedBetFairTickets = false)
        {
            try
            {
                // ------------ refund---- 
                Hashtable pRefund = new Hashtable();
                pRefund.Add("creator", custid);
                pRefund.Add("matchid", matchid);
                pRefund.Add("isexcludebetdaq", IsExcludeBetdaq);
                pRefund.Add("issettleabandonedmatch", IsSettleDeterminedTickets);
                pRefund.Add("isexcludebetfair", IsExcludeBetfair);
                pRefund.Add("issettleabandonedmatchbetfair", IsSettleDeterminedBetFairTickets);
                pRefund.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                if (homeHScore != null) pRefund.Add("hth", DNAide.Core.CastAide.AsInt32(homeHScore));
                else pRefund.Add("hth", 0);

                if (awayHScore != null) pRefund.Add("hta", DNAide.Core.CastAide.AsInt32(awayHScore));
                else pRefund.Add("hta", 0);

                if (homeFScore != null) pRefund.Add("fth", DNAide.Core.CastAide.AsInt32(homeFScore));
                else pRefund.Add("fth", 0);

                if (awayFScore != null) pRefund.Add("fta", DNAide.Core.CastAide.AsInt32(awayFScore));
                else pRefund.Add("fta", 0);

                if (scoreConfirm_Betstatus != null) pRefund.Add("betstatus", scoreConfirm_Betstatus);

                using (ObjectDb objDb = new ObjectDb("Admin_Settlement_Refund_All", 1800))
                {
                    objDb.AssignParamValues(pRefund);
                    objDb.ExecuteNonQuery();

                    var errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                    msg = CastAide.AsString(objDb.GetParameterValue("errmsg"));

                    #region update match_remark
                    Hashtable pUpdate = new Hashtable();
                    pUpdate.Add("matchid", matchid);
                    pUpdate.Add("command", "F");
                    using (ObjectDb objDbUpdateRemark = new ObjectDb("updateMatch_remark", 1800))
                    {
                        objDbUpdateRemark.AssignParamValues(pUpdate);
                        objDbUpdateRemark.ExecuteNonQuery();
                    }
                    #endregion

                    #region refund logging
                    Hashtable pLog = new Hashtable();
                    pLog.Add("userid", custid);
                    pLog.Add("doing", "Refund for matchid:" + matchid);

                    using (ObjectDb objDbRefundLogin = new ObjectDb("WriteLog_UserLog", 180))
                    {
                        objDbRefundLogin.AssignParamValues(pLog);
                        objDbRefundLogin.ExecuteNonQuery();
                    }
                    #endregion

                    if (errCode > 0) return false;
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Settlement refund fulltime
        /// store procedure: refund_fulltime and updateMatch_remark
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>                      
        public static bool RefundFullTime(int custid, int matchid, bool IsExcludeBetdaq, bool IsSettleDeterminedTickets, ref string msg
            , string homeFScore, string awayFScore, string scoreConfirm_Betstatus, bool IsExcludeBetfair = false, bool IsSettleDeterminedBetFairTickets = false)
        {
            try
            {                   
                using (ObjectDb objDb = new ObjectDb("Admin_Settlement_RefundFT", 1800))
                {
                    // ------------ refund---- 
                    Hashtable pRefund = new Hashtable();
                    pRefund.Add("adminid", custid);
                    pRefund.Add("matchid", matchid);
                    pRefund.Add("isexcludebetdaq", IsExcludeBetdaq);
                    pRefund.Add("issettleabandonedmatch", IsSettleDeterminedTickets);
                    pRefund.Add("isexcludebetfair", IsExcludeBetfair);
                    pRefund.Add("issettleabandonedmatchbetfair", IsSettleDeterminedBetFairTickets);
                    pRefund.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                    if (homeFScore != null) pRefund.Add("fth", DNAide.Core.CastAide.AsInt32(homeFScore));
                    else pRefund.Add("fth", 0);

                    if (awayFScore != null) pRefund.Add("fta", DNAide.Core.CastAide.AsInt32(awayFScore));
                    else pRefund.Add("fta", 0);

                    if (scoreConfirm_Betstatus != null) pRefund.Add("betstatus", scoreConfirm_Betstatus);

                    objDb.AssignParamValues(pRefund);
                    objDb.ExecuteNonQuery();

                    var errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                    if (errCode > 0)
                    {
                        msg = CastAide.AsString(objDb.GetParameterValue("errmsg"));
                        return false;
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Settlement refund halftime
        /// HuyNguyen updated: 7/21/2009
        /// </summary>       
        public static bool RefundHalfTime(int custid, int matchid, bool IsExcludeBetdaq, bool IsSettleDeterminedTickets, ref string msg
             , string homeHScore, string awayHScore, bool IsExcludeBetfair = false, bool IsSettleDeterminedBetfairTickets = false)
        {
            try
            {                   
                using (ObjectDb objDb = new ObjectDb("Admin_Settlement_RefundHT", 1800))
                {
                    // ------------ refund---- 
                    Hashtable pRefund = new Hashtable();
                    pRefund.Add("adminid", custid);
                    pRefund.Add("matchid", matchid);
                    pRefund.Add("isexcludebetdaq", IsExcludeBetdaq);
                    pRefund.Add("issettleabandonedmatch", IsSettleDeterminedTickets);
                    pRefund.Add("isexcludebetfair", IsExcludeBetfair);
                    pRefund.Add("issettleabandonedmatchbetfair", IsSettleDeterminedBetfairTickets);
                    pRefund.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                    if (homeHScore != null) pRefund.Add("hth", DNAide.Core.CastAide.AsInt32(homeHScore));
                    else pRefund.Add("hth", 0);

                    if (awayHScore != null) pRefund.Add("hta", DNAide.Core.CastAide.AsInt32(awayHScore));
                    else pRefund.Add("hta", 0);

                    objDb.AssignParamValues(pRefund);
                    objDb.ExecuteNonQuery();

                    var errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                    if (errCode > 0)
                    {
                        msg = CastAide.AsString(objDb.GetParameterValue("errmsg"));
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custID"></param> 
        /// <param name="matchID"></param>
        /// <param name="betStatus"></param>
        /// <param name="includeLG"></param>
        public static bool RefundSoccerFGLG(int custid, int matchid, bool IsExcludeBetdaq, ref string errMsg)
        {
            try
            {                
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Refund_FGLG", 1800))
                {
                    Hashtable pRefund = new Hashtable();
                    pRefund.Add("adminid", custid);
                    pRefund.Add("matchid", matchid);
                    pRefund.Add("lgonly", false);
                    pRefund.Add("ip", System.Web.HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(pRefund);
                    objDB.ExecuteNonQuery();

                    var errCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    errMsg = string.Format("{0}", objDB.GetParameterValue("errmsg"));

                    return errCode > 0 ? false : true;
                }

            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                errMsg = ex.Message;
                return false;
            }
        }

        public static bool RefundSoccerAllLastGoal(int custid, int matchid, bool isExcludeBetdaq, ref string errMsg)
        {
            try
            {                
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Refund_FGLG", 1800))
                {
                    Hashtable pRefund = new Hashtable();
                    pRefund.Add("adminid", custid);
                    pRefund.Add("matchid", matchid);
                    pRefund.Add("lgonly", true);
                    pRefund.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(pRefund);
                    objDB.ExecuteNonQuery();

                    var errCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    errMsg = string.Format("{0}", objDB.GetParameterValue("errmsg"));

                    return errCode > 0 ? false : true;
                }

            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                errMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Settlement Update for Mixparlay
        /// store procedure: Admin_Settlement_Parlay, @action='CAL'
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>        
        public static void Multiple(int custid, int matchid)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Parlay", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custid);
                    p.Add("matchids", matchid.ToString());
                    p.Add("action", "CAL");
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
        }

        public static ErrorDetail Multiple1(int custid, string matchid)
        {
            try
            {   
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Parlay", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custid);
                    p.Add("matchids", matchid);
                    p.Add("action", "CAL");
                    p.Add("errmsg", "");
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    return new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt16(objDB.GetParameterValue("errcode")) ,
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {    
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };

            }
        }


        /// <summary>
        /// Settlement update mixpalay cancel
        /// store procedure: Admin_Settlement_Parlay, @action='REFUND'
        /// @viet hoang
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>        
        public static ErrorDetail RefundMultiple(int custid, int matchid)
        {                    
            try
            {
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Parlay", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custid);
                    p.Add("matchids", matchid.ToString());
                    p.Add("action", "REFUND");
                    p.Add("ip", System.Web.HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    return new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt16(objDB.GetParameterValue("errcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }

            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }            
        }

        public static ErrorDetail RefundMultipleHT(int custid, int matchid)
        {
            try
            {

                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Parlay", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custid);
                    p.Add("matchids", matchid.ToString());
                    p.Add("action", "REFUND-HT");
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    return new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt16(objDB.GetParameterValue("errcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ErrorDetail RefundMultipleFT(int custid, int matchid)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Parlay", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("creator", custid);
                    p.Add("matchids", matchid.ToString());
                    p.Add("action", "REFUND-FT");
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    return new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt16(objDB.GetParameterValue("errcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }
        }




        /// <summary>
        /// HR Settle Get Horse by Race
        /// @huy nguyen
        /// Using DNAide's library
        /// Huy.Nguyen update: 3/13/2009
        /// </summary>        
        public static DbResult Acc_Set_HR_SelforBill(int leagueid, int raceno, string eventdate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceno);
                p.Add("eventdate", eventdate);

                //var spElement = new GetSpConfig().GetStoreElement(StoreProcedureSection.StoreCollection.Settlement, "SetHRSelForBill");
                var spElement = new GetSpConfig().GetStoreElement(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "SetHRSelForBill");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);

            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }


        /// <summary>
        /// Settlement Checking bettype Next Goal
        /// KhoaTran@20090215
        /// </summary>
        /// <param name="matchid"></param>
        public static ErrorDetail SettlementChecking(int matchid, string action)
        {
            ErrorDetail result = null;
            try
            {                                   
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_Checking", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchid);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("action", action);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    result = new ErrorDetail() {
                        ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
                Core.Extensions.LogExtensions.ExceptionHandler(ex);                
            }
            return result;
        }

        public static DbResult BetDaq_Checking(int matchid)
        {
            DbResult rs = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("matchid", matchid);
                p.Add("ismovebetdaq", 0);
                p.Add("isexcludebetdaq", 0);
                p.Add("issettleabandonedmatch", 0);
                var spElement = new GetSpConfig().GetStoreElement(Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "Acc_Set_BetDaq_Checking");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
                //rs = DbHelper.ExecuteCmd("Acc_Set_BetDaq_Checking", p, DBType.MAIN, 180);
            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return rs;
        }

        public static DbResult BetFair_Checking(int matchid)
        {
            DbResult rs = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("matchid", matchid);
                p.Add("ismovebetfair", 0);
                p.Add("isexcludebetfair", 0);
                p.Add("issettleabandonedmatchbetfair", 0);
                var spElement = new GetSpConfig().GetStoreElement(Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "Acc_Set_BetFair_Checking");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return rs;
        }

        public static ErrorDetail Settlement_BetDaq_Move(int matchid)
        {
            ErrorDetail result = null; 
            try
            {   
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_BetDaq_Move", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchid);
                    p.Add("creator", AppSession.AccId);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("newmatchcode", string.Empty);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    result = new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode")),
                        ErrCode = CastAide.AsString(objDB.GetParameterValue("newmatchcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrCode = string.Empty,
                    ErrorMessage = ex.Message
                };
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static ErrorDetail Settlement_BetFair_Move(int matchid)
        {
            ErrorDetail result = null;
            try
            {
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_BetFair_Move", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchid);
                    p.Add("creator", AppSession.AccId);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("newmatchcode", string.Empty);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    result = new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode")),
                        ErrCode = CastAide.AsString(objDB.GetParameterValue("newmatchcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrCode = string.Empty,
                    ErrorMessage = ex.Message
                };
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static ErrorDetail Settlement_BetDaq_Move(int matchid, int newMatchId)
        {
            ErrorDetail result = null;
            try
            {                 
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_BetDaq_Move", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchid);
                    p.Add("newmatchid", newMatchId);
                    p.Add("creator", AppSession.AccId);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("newmatchcode", string.Empty);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    result = new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode")),
                        ErrCode = CastAide.AsString(objDB.GetParameterValue("newmatchcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrCode = string.Empty,
                    ErrorMessage = ex.Message
                };
                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static ErrorDetail Settlement_BetFair_Move(int matchid, int newMatchId)
        {
            ErrorDetail result = null;
            try
            {
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_BetFair_Move", 1800))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchid);
                    p.Add("newmatchid", newMatchId);
                    p.Add("creator", AppSession.AccId);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);
                    p.Add("newmatchcode", string.Empty);
                    p.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    objDB.AssignParamValues(p);
                    objDB.ExecuteNonQuery();

                    result = new ErrorDetail()
                    {
                        ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode")),
                        ErrCode = CastAide.AsString(objDB.GetParameterValue("newmatchcode")),
                        ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"))
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrCode = string.Empty,
                    ErrorMessage = ex.Message
                };
                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DataTable Settlement_BetDaq_SelectMatches(int matchid)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable { { "matchid", matchid } };

                //var spElement = new GetSpConfig().GetStoreElement(StoreProcedureSection.StoreCollection.Settlement, "SetBetDaqMatchSel");
                var spElement = new GetSpConfig().GetStoreElement(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "SetBetDaqMatchSel");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            var dtMatches = new DataTable();
            if (rs != null && rs.ResultSet != null && rs.ResultSet.Tables.Count > 0)
            {
                dtMatches = rs.ResultSet.Tables[0];
            }
            return dtMatches;
        }        

        public static DataTable GetNumberGameResult(DateTime fromDate, int leagueId)
        {
            var dtResult = new DataTable();
            try
            {
                var p = new Hashtable { { "fromdate", fromDate }, { "leagueid", leagueId } };
                var spElement =
                    new GetSpConfig().GetStoreElement(
                        Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl,
                        "MatchSelNumberGameByLeague");
                var result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
                if (result != null && result.ResultSet.Tables.Count > 0)
                {
                    dtResult = result.ResultSet.Tables[0];
                }


            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return dtResult;

        }

        public static DataTable GetNumberGameSettlement(DateTime fromDate, DateTime toDate, int leagueId)
        {
            var dtResult = new DataTable();
            try
            {
                var p = new Hashtable { { "fromdate", fromDate }, { "todate", toDate }, { "leagueid", leagueId } };
                var spElement =
                    new GetSpConfig().GetStoreElement(
                        Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl,
                        "MatchSelNumberGameByLeague");
                var result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
                if (result != null && result.ResultSet.Tables.Count > 0)
                {
                    dtResult = result.ResultSet.Tables[0];
                }


            }
            catch (Exception ex)
            {

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return dtResult;

        }

        public static ErrorDetail RefundNumberGame(int matchId, int userId)
        {
            var result = new ErrorDetail();
            result.ErrorCode = 0;
            try
            {
                using (ObjectDb objDb = new ObjectDb("Admin_Settlement_s161", 180))
                {
                    var p = new Hashtable { { "matchid", matchId }, { "adminid", userId }, { "action", "Refund" }, { "ip", HttpContext.Current.Request.UserHostAddress } };

                    objDb.AssignParamValues(p);
                    objDb.ExecuteNonQuery();

                    result.ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                    result.ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"));
                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = CastAide.AsInt32(1);
                result.ErrorMessage = CastAide.AsString(ex.Message);
                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;

        }

        public static ErrorDetail RefundNumberGameFt(int matchId, int userId)
        {
            var result = new ErrorDetail();
            result.ErrorCode = 0;
            try
            {
                using (ObjectDb objDb = new ObjectDb("Admin_Settlement_s161", 180))
                {
                    var p = new Hashtable { { "matchid", matchId }, { "adminid", userId }, { "action", "Refund-FT" }, { "ip", HttpContext.Current.Request.UserHostAddress } };

                    objDb.AssignParamValues(p);
                    objDb.ExecuteNonQuery();

                    result.ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                    result.ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"));
                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = CastAide.AsInt32(1);
                result.ErrorMessage = CastAide.AsString(ex.Message);
                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;

        }
        /// <summary>
        /// Send private message
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="action"></param>
        /// <param name="userId"></param>
        /// <param name="messageSend"></param>
        /// <returns>ErrorDetail</returns>
        public static ErrorDetail PrivateMessageRefundNumberGame(int matchId, string action, int userId, Dictionary<string, string> messageSend)
        {
            var result = new ErrorDetail();
            result.ErrorCode = 0;
            try
            {                 
                using (ObjectDb objDb = new ObjectDb("Acc_Set_PrivateMessageForRefund", 180))
                {
                    var p = new Hashtable { 
                        { "matchid", matchId }, { "adminid", userId }, { "action", action }, { "ip", HttpContext.Current.Request.UserHostAddress } ,
                        { "content_en", messageSend["EN"] }, { "content_ch", messageSend["CH"] }, { "content_cs", messageSend["CS"] }
                    };

                    objDb.AssignParamValues(p);
                    objDb.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;

                Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;    
        }


        public class GetSpConfig : DataAccess
        {
            public StoreProcedureElement GetStoreElement(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}
