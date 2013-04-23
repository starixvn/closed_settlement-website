using System;
using System.Collections;
using System.Web;
using DNAide.Data;
using DNAide.Utils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class AccountTransaction: DataAccess
    {
        /// <summary>
        /// call store BettransIns_Admin, return transid
        /// @author: Vu Thi Hong Hanh
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId">custId</param>
        /// <param name="transDate">transDate</param>
        /// <param name="transType">transType</param>
        /// <param name="creditAmt">creditAmt</param>
        /// <param name="debitAmt">debitAmt</param>
        /// <param name="transDesc">transDesc</param>
        /// <param name="creator">creator</param>
        /// <param name="matchDate">matchDate</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>        
        public static string[] BettransInsTransaction(int custId, DateTime transDate, double amount, string transDesc,
                                                      int userId, string lang, string ip)
        {
            string[] res = {"", "", ""};
            try
            {
                var hash = new Hashtable();
                hash.Add("transid", 0);
                hash.Add("custid", custId);
                hash.Add("transdate", transDate);
                hash.Add("amount", amount);
                hash.Add("transdesc", transDesc);
                hash.Add("userid", userId);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("language", lang);
                hash.Add("ip", ip);
                var spElement =
                    new TransactionStoreProcedure().GetTransactionStoreProcedure(TransactionGroupSection.StoreCollection.Bettrans, "BettranInsTransaction");

                DbResult rs = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 300);
                if (rs != null)
                {
                    res[0] = rs.Outputs["transid"].ToString();
                    res[1] = rs.Outputs["errcode"].ToString();
                    res[2] = rs.Outputs["errmsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return res;
        }

        /// <summary>
        /// Execute BetTransSelSpecialDel SP
        /// @PhuongDN
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        //public static DataResult BetTransSelSpecialDel(int matchId, long fromRefNo, long toRefNo, int bettype,string betteam,string hdp, bool isLiveIndicator)        
        public static DbResult BetTransSelSpecialDel(int matchId, long fromRefNo, long toRefNo, string bettypes,
                                                     string betteam1378, string betteam515, string betteam4, string hdp,
                                                     int liveindicator, string username)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("matchid", matchId);
                hash.Add("fromrefno", fromRefNo);
                hash.Add("torefno", toRefNo);
                hash.Add("bettypes", bettypes);
                hash.Add("betteam1378", betteam1378);
                hash.Add("betteam515", betteam515);
                hash.Add("betteam4", betteam4);
                hash.Add("hdp", hdp);
                hash.Add("liveindicator", liveindicator);
                hash.Add("username", username);
                var spElement =
                    new SpecialDeleteStoreProcedure().GetSpecialDeleteStoreProcedure(
                        SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransSelSpecialDel");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        //Phuong.Nguyen modified <2008-12-02>
        //public static DataResult GetAllBetType() //IBCUtils
        public static DbResult GetAllBetType() //DNAide
        {
            DbResult rs = null;
            try
            {
                var spElement = new SportbookPtStoreProcedure().GetSportbookPtStoreProcedure(
                           SportbookPtSession.StoreCollection.SportBooksPtControl, "BetTypeSel");
                rs = DbHelper.Execute(spElement.Name, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        //Phuong.Nguyen modified <2008-12-02>
        //public static DataResult MatchSeById(int matchId)
        public static DbResult MatchSeById(int matchId)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("matchid", matchId);
                var spElement =
                    new SpecialDeleteStoreProcedure().GetSpecialDeleteStoreProcedure(
                        SpecialDeleteGroupSection.StoreCollection.Sportbook, "SelMatchInfo");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Delete transaction
        /// @PhuongDN
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="frefno"></param>
        /// <param name="trefno"></param>
        /// <param name="bettype"></param>
        /// <param name="uname"></param>
        /// <param name="pass"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>        
        public static DbResult DeleteSpecialTransaction(string matchId, string frefno, string trefno, string bettype,
                                                        string betteam1378, string betteam515, string hdp, string uname,
                                                        object userId, string pass, string DelOption, string transids,
                                                        int liveindicator, string DelOption_BD, string transids_BD,
                                                        string DelOption_12EU, string transids_12EU,
                                                        string DelOption_MHC, string transids_MHC, string betteam4)
        {
            //DataResult rs = null;
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("matchid", matchId);
                hash.Add("f_transid", frefno);
                hash.Add("t_transid", trefno);
                hash.Add("bettypes", bettype);
                hash.Add("betteam1378", betteam1378);
                hash.Add("betteam515", betteam515);
                hash.Add("hdp", hdp);
                hash.Add("username", uname);
                hash.Add("userpwd", Encryption.CfsEnCode(pass));
                hash.Add("userid", userId);
                hash.Add("errcode", 0);
                hash.Add("errmsg", string.Empty);

                hash.Add("deloption", DelOption);
                hash.Add("transids", transids);
                hash.Add("liveindicator", liveindicator);
                hash.Add("deloption_bd", DelOption_BD);
                hash.Add("transids_bd", transids_BD);
                hash.Add("deloption_12eu", DelOption_12EU);
                hash.Add("transids_12eu", transids_12EU);
                hash.Add("deloption_mhc", DelOption_MHC);
                hash.Add("transids_mhc", transids_MHC);
                hash.Add("betteam4", betteam4);
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                var spElement =
                    new SpecialDeleteStoreProcedure().GetSpecialDeleteStoreProcedure(
                        SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransDelSpecial");

                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }


        /// <summary>
        /// @KhoaTran
        /// </summary>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static DbResult BettransDelSpecial_LeagueSelOutright() //DNAide
        {
            DbResult rs = null;
            try
            {
                var spElement =
                    new SpecialDeleteStoreProcedure().GetSpecialDeleteStoreProcedure(
                        SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransDelSpecial_LeagueSelOutright");
                rs = DbHelper.Execute(spElement.Name, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="LeagueID"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static DbResult BettransDelSpecial_OutrightSel(int LeagueID)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("leagueid", LeagueID);
                var spElement =
                    new SpecialDeleteStoreProcedure().GetSpecialDeleteStoreProcedure(
                        SpecialDeleteGroupSection.StoreCollection.Sportbook, "Special_OutrightSel");
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
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static DbResult BettransSelDelSpecial_Outright(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement =
                    new SpecialDeleteStoreProcedure().GetSpecialDeleteStoreProcedure(
                        SpecialDeleteGroupSection.StoreCollection.Sportbook, "Special_Outright");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        private class SpecialDeleteStoreProcedure : DataAccess
        {
            public Core.Configurations.StoreProcedureElement GetSpecialDeleteStoreProcedure(SpecialDeleteGroupSection.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
        private class SportbookPtStoreProcedure : DataAccess
        {
            public Core.Configurations.StoreProcedureElement GetSportbookPtStoreProcedure(SportbookPtSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }

        private class TransactionStoreProcedure: DataAccess
        {
            public StoreProcedureElement GetTransactionStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.TransactionGroupSection.StoreCollection transType, string storeAlias)
            {
                var spElement = GetStoreProcedure(transType, storeAlias);
                return spElement;
            }
        }

        

    }
}