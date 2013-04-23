using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using System.Collections;
using DNAide.Utils;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.DataAccess.SpecialDelete
{
    public class SpecialDeleteDAO : DataAccess, ISpecialDeleteDAO
    {
        public DbResult SelSpecialDeleteNumberGame(int matchId, long fromRefNo, long toRefNo, string live, string dead , string userName)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchId);
            hashtable.Add("fromrefno", fromRefNo);
            hashtable.Add("torefno", toRefNo);
            hashtable.Add("live", live);
            hashtable.Add("dead", dead);
            hashtable.Add("username", userName);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.NumberGame, "SelSpecialDel_NumberGame");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_BettransSelSpecialDel_NumberGame", hashtable, DBType.ARCHIVE, 600);
        }


        public DbResult SelMatchInfo(int matchId, long transid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchId);
            hashtable.Add("transid", transid);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "SelMatchInfo");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_MatchSelByID", hashtable, DBType.ARCHIVE, 600);
        }


        public DbResult DeleteSpecialTransactionNumberGame(int matchId, long frefno, long trefno, string live, string dead, string username, int custId, string userpwd, int DelOption, string transids, int DelOption_BD, string transids_BD, int DelOption_12EU, string transids_12EU, int DelOption_MHC, string transids_MHC, string ip)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchId);
            hashtable.Add("f_transid", frefno);
            hashtable.Add("t_transid", trefno);
            hashtable.Add("live", live);
            hashtable.Add("dead", dead);
            hashtable.Add("username", username);
            hashtable.Add("userpwd", userpwd);
            hashtable.Add("userid", custId);
            hashtable.Add("deloption", DelOption);
            hashtable.Add("transids", transids);
            hashtable.Add("deloption_bd", DelOption_BD);
            hashtable.Add("transids_bd", transids_BD);
            hashtable.Add("deloption_12eu", DelOption_12EU);
            hashtable.Add("transids_12eu", transids_12EU);
            hashtable.Add("deloption_mhc", DelOption_MHC);
            hashtable.Add("transids_mhc", transids_MHC);
            hashtable.Add("ip", ip);
            hashtable.Add("errcode", 0);
            hashtable.Add("errmsg", string.Empty);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.NumberGame, "DelSpecial_NumberGame");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_BettransDelSpecial_NumberGame", hashtable, DBType.MAIN, 600);
        }

        public DbResult DeleteSpecialTransaction(string matchId, string frefno, string trefno, string bettype,
                                                       string betteam1378, string betteam515, string hdp, string uname,
                                                       string userId, string pass, string DelOption, string transids,
                                                       string liveindicator, string DelOption_BD, string transids_BD,
                                                       string DelOption_12EU, string transids_12EU,
                                                       string DelOption_MHC, string transids_MHC, string betteam430,
                                                       string betteam6, string betteam14, string betteam16, string homescore, string awayscore, string fromtransdate, string totransdate, int SAFilter)
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
                hash.Add("hdp", DNAide.Core.CastAide.AsDecimal(hdp,999));
                hash.Add("username", uname);
                hash.Add("userpwd", Encryption.CfsEnCode(pass));
                hash.Add("userid", userId);
                hash.Add("errcode", 0);
                hash.Add("errmsg", string.Empty);

                hash.Add("deloption", DelOption);
                hash.Add("transids", transids);
                hash.Add("liveindicator", DNAide.Core.CastAide.AsInt32(liveindicator, 2));
                hash.Add("deloption_bd", DelOption_BD);
                hash.Add("transids_bd", transids_BD);
                hash.Add("deloption_12eu", DelOption_12EU);
                hash.Add("transids_12eu", transids_12EU);
                hash.Add("deloption_mhc", DelOption_MHC);
                hash.Add("transids_mhc", transids_MHC);
                hash.Add("betteam430", betteam430);
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                //adding filter option
                hash.Add("betteam6", betteam6);
                hash.Add("betteam14", betteam14);
                hash.Add("betteam16", betteam16);
                hash.Add("safilter", SAFilter);
                if (string.IsNullOrEmpty(homescore))
                    homescore = string.Empty;
                if (string.IsNullOrEmpty(awayscore))
                    awayscore = string.Empty;
                if (string.IsNullOrEmpty(fromtransdate))
                    fromtransdate = string.Empty;
                if (string.IsNullOrEmpty(totransdate))
                    totransdate = string.Empty;

                if (homescore.Length > 0 && awayscore.Length > 0 && fromtransdate.Length == 0 && totransdate.Length == 0)
                {
                    hash.Add("livehomescore", homescore);
                    hash.Add("liveawayscore", awayscore);
                }
                else if (fromtransdate.Length > 0 && totransdate.Length > 0 && homescore.Length == 0 && awayscore.Length == 0)
                {
                    hash.Add("fromtransdate", CastAide.AsDateTime( fromtransdate).AddHours(-12));
                    hash.Add("totransdate", CastAide.AsDateTime(totransdate).AddHours(-12));
                }
                else if(homescore.Length>0 && awayscore.Length>0 && fromtransdate.Length>0 && totransdate.Length > 0)
                {
                    hash.Add("livehomescore", homescore);
                    hash.Add("liveawayscore", awayscore);
                    hash.Add("fromtransdate", CastAide.AsDateTime(fromtransdate).AddHours(-12));
                    hash.Add("totransdate", CastAide.AsDateTime(totransdate).AddHours(-12));
                }

                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransDelSpecial");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
                //rs = DbHelper.Execute("Acc_BettransDelSpecial", hash, DBType.MAIN, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }




        public DbResult SelRecoverSpecialBets(string transid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("transid", transid);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransSelRecoverSpecialBets");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_BettransSelRecoverSpecialBets", hashtable, DBType.ARCHIVE, 600);
        }


        public DbResult RecoverSpecialBets(string transid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("transid", transid);
            hashtable.Add("userid", AppSession.AccId);
            hashtable.Add("ip", Utils.GetClientIP());
            hashtable.Add("errcode", 0);
            hashtable.Add("errmsg", string.Empty);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransRecoverSpecialBets");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName);
            //return DbHelper.Execute("Acc_BettransRecoverSpecialBets", hashtable, DBType.MAIN);

        }

        public DbResult RemovePrivateMsg(string transid, int UserID)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("transid", transid);
            hashtable.Add("userid", UserID);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "Msg_Remove");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName);
            //return DbHelper.Execute("Acc_Msg_Remove", hashtable, DBType.MESSAGE);
        }


        public DbResult BettransSelSpecialDel(SpecialDeleteParam deleteParam)
        {
            Hashtable param = new Hashtable();
            param.Add("matchid", deleteParam.MatchId);
            param.Add("fromrefno", deleteParam.FromRefNo);
            param.Add("torefno", deleteParam.ToRefNo);
            param.Add("hdp", deleteParam.Hdp);
            param.Add("liveindicator", deleteParam.Liveindicator);
            param.Add("bettypes", deleteParam.Bettypes);
            param.Add("betteam1378", deleteParam.Betteam1378);
            param.Add("betteam515", deleteParam.Betteam515);
            param.Add("betteam430", deleteParam.Betteam430);
            param.Add("username", AppSession.Username);
            //adding filter option
            param.Add("betteam6", deleteParam.Betteam6);
            param.Add("betteam14", deleteParam.Betteam14);
            param.Add("betteam16", deleteParam.Betteam16);
            param.Add("safilter", deleteParam.SAFilter);

            if (deleteParam.HomeScore.Length > 0 && deleteParam.AwayScore.Length > 0 && deleteParam.FromTransdate.Length == 0 && deleteParam.ToTransdate.Length == 0)
            {
                param.Add("livehomescore", deleteParam.HomeScore);
                param.Add("liveawayscore", deleteParam.AwayScore);
            }
            else if (deleteParam.FromTransdate.Length > 0 && deleteParam.ToTransdate.Length > 0 && deleteParam.HomeScore.Length == 0 && deleteParam.AwayScore.Length == 0)
            {
                param.Add("fromtransdate", CastAide.AsDateTime(deleteParam.FromTransdate).AddHours(-12));
                param.Add("totransdate", CastAide.AsDateTime(deleteParam.ToTransdate).AddHours(-12));
            }
            else if (deleteParam.HomeScore.Length > 0 && deleteParam.AwayScore.Length > 0 && deleteParam.FromTransdate.Length > 0 && deleteParam.ToTransdate.Length > 0)
            {
                param.Add("livehomescore", deleteParam.HomeScore);
                param.Add("liveawayscore", deleteParam.AwayScore);
                param.Add("fromtransdate", CastAide.AsDateTime(deleteParam.FromTransdate).AddHours(-12));
                param.Add("totransdate", CastAide.AsDateTime(deleteParam.ToTransdate).AddHours(-12));
            }

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransSelSpecialDel");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_BettransSelSpecialDel", param, DBType.ARCHIVE, 600);
        }

        public DbResult BettransDelSpecial_OutrightSel(int LeagueID)
        {
            var param = new Hashtable();
            param.Add("leagueid", LeagueID);
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "Special_OutrightSel");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_BettransDelSpecial_OutrightSel", param, DBType.ARCHIVE, 200);
        }

        public DbResult BettransSelDelSpecial_Outright(Hashtable param)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "Special_Outright");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            //return DbHelper.Execute("Acc_BettransSelDelSpecial_Outright", param, DBType.ARCHIVE, 600);
        }

        public DbResult BettransSelBet_BatchMsg(Hashtable param)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "BatchMsg");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);

            //return DbHelper.Execute("Acc_BettransSelBet_BatchMsg", param, DBType.REPORT, 600);
        }
        #region EarlyKOBet
        /// <summary>
        /// SelectEarlyKOBet
        /// </summary>
        /// <param name="deleteParam"></param>
        /// <returns></returns>
        public DbResult SelectEarlyKOBet(SpecialDeleteParamEarlyKO deleteParam)
        {
            DateTime koDate = DateTime.Parse(deleteParam.KichOffDate).AddHours(-12);

            Hashtable param = new Hashtable();
            param.Add("matchid", deleteParam.MatchId);
            //param.Add("kickoffdate", deleteParam.KichOffDate);
            param.Add("kickoffdate", koDate);
            param.Add("hdp", deleteParam.Hdp);
            param.Add("bettypes", deleteParam.Bettypes);
            param.Add("betteam1378", deleteParam.Betteam1378);
            param.Add("betteam515", deleteParam.Betteam515);
            param.Add("betteam430", deleteParam.Betteam430);
            param.Add("username", AppSession.Username);
            //add filter option
            param.Add("betteam6", deleteParam.Betteam6);
            param.Add("betteam14", deleteParam.Betteam14);
            param.Add("betteam16", deleteParam.Betteam16);
            param.Add("safilter", deleteParam.SAFilter);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.EarlyKO, "GetEarlyKOBet");
            
            //return DbHelper.Execute("Acc_BettransSelSpecialDel_EarlyKO", param, DBType.ARCHIVE, 600);
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
        }
        /// <summary>
        /// bobby.nguyen: SelectEarlyKOBet
        /// </summary>
        /// <param name="deleteParam"></param>
        /// <returns></returns>
        public DbResult SelectEarlyKOMixparlay(string refno, string transid, int curency)
        {
            Hashtable param = new Hashtable();
            param.Add("refno", refno);
            param.Add("transid", transid);
            param.Add("currencyid", curency);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.EarlyKO, "GetEarlyKOBet_MixParlay");

            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
        }
        public DbResult DeleteSpecialEarlyKO(string matchId, string bettype,
                                                       string betteam1378, string betteam515, string hdp, string uname,
                                                       string userId, string pass, string DelOption, string transids,
                                                       string kickoffdate, string DelOption_BD, string transids_BD,
                                                       string DelOption_12EU, string transids_12EU,
                                                       string DelOption_MHC, string transids_MHC, string betteam430, 
                                                       string betteam6, string betteam14, string betteam16, int SAFilter)
        {
            DbResult rs = null;
            var hash = new Hashtable();

            DateTime koDate = DateTime.Parse(kickoffdate).AddHours(-12);

            hash.Add("matchid", matchId);
            hash.Add("deloption", DelOption);
            hash.Add("kickoffdate", koDate);
            hash.Add("transids", transids);
            hash.Add("bettypes", bettype);
            hash.Add("betteam1378", betteam1378);
            hash.Add("betteam515", betteam515);
            hash.Add("betteam430", betteam430);
            hash.Add("hdp", DNAide.Core.CastAide.AsDecimal(hdp, 999));
            hash.Add("username", uname);
            hash.Add("userpwd", Encryption.CfsEnCode(pass));
            hash.Add("userid", userId);
            hash.Add("errcode", 0);
            hash.Add("errmsg", string.Empty);
            hash.Add("deloption_bd", DelOption_BD);
            hash.Add("transids_bd", transids_BD);
            hash.Add("deloption_12eu", DelOption_12EU);
            hash.Add("transids_12eu", transids_12EU);
            hash.Add("deloption_mhc", DelOption_MHC);
            hash.Add("transids_mhc", transids_MHC);
            hash.Add("ip", Utils.GetClientIP());
            //add filter option
            hash.Add("betteam6", betteam6);
            hash.Add("betteam14", betteam14);
            hash.Add("betteam16", betteam16);
            hash.Add("safilter", SAFilter);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.EarlyKO, "DelEarlyKOBet");

            rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 1000);
            return rs;
        }
        #endregion
        #region Mixparlay
        /// <summary>
        /// GetMPList
        /// </summary>
        /// <param name="deleteParam"></param>
        /// <returns></returns>
        public DbResult GetMPList(SpecialDeleteParam deleteParam)
        {
            Hashtable param = new Hashtable();
            param.Add("matchid", deleteParam.MatchId);
            param.Add("hdp", deleteParam.Hdp);
            param.Add("bettypes", deleteParam.Bettypes);
            param.Add("betteam1378", deleteParam.Betteam1378);
            param.Add("betteam515", deleteParam.Betteam515);
            param.Add("betteam430", deleteParam.Betteam430);
            param.Add("betteam6", deleteParam.Betteam6);
            param.Add("betteam14", deleteParam.Betteam14);
            param.Add("betteam16", deleteParam.Betteam16);
            param.Add("type", deleteParam.Type);
            param.Add("bettype", deleteParam.SelectBettype);

            if (deleteParam.HomeScore.Length > 0 && deleteParam.AwayScore.Length > 0 && deleteParam.FromTransdate.Length == 0 && deleteParam.ToTransdate.Length == 0)
            {
                param.Add("livehomescore", deleteParam.HomeScore);
                param.Add("liveawayscore", deleteParam.AwayScore);
            }
            else if (deleteParam.FromTransdate.Length > 0 && deleteParam.ToTransdate.Length > 0 && deleteParam.HomeScore.Length == 0 && deleteParam.AwayScore.Length == 0)
            {
                param.Add("fromtransdate", CastAide.AsDateTime(deleteParam.FromTransdate).AddHours(-12));
                param.Add("totransdate", CastAide.AsDateTime(deleteParam.ToTransdate).AddHours(-12));
            }
            else if (deleteParam.HomeScore.Length > 0 && deleteParam.AwayScore.Length > 0 && deleteParam.FromTransdate.Length > 0 && deleteParam.ToTransdate.Length > 0)
            {
                param.Add("livehomescore", deleteParam.HomeScore);
                param.Add("liveawayscore", deleteParam.AwayScore);
                param.Add("fromtransdate", CastAide.AsDateTime(deleteParam.FromTransdate).AddHours(-12));
                param.Add("totransdate", CastAide.AsDateTime(deleteParam.ToTransdate).AddHours(-12));
            }

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Mixparlay, "GetMPList");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
        }
        public DbResult GetMPTicket(string refno)
        {
            Hashtable param = new Hashtable();
            param.Add("refno", refno);
            param.Add("transid", 0);
            param.Add("currencyid", 0);
            param.Add("language", "E");

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Mixparlay, "GetMPTicket");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
        }

        public DbResult CheckRefno(string matchid, string fromRefno, string toRefno)
        {
            Hashtable param = new Hashtable();
            param.Add("matchid", matchid);
            param.Add("fromrefno", fromRefno);
            param.Add("torefno", toRefno);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "CheckRefno");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
        }
        public DbResult DeleteSpecialMixparlay(string matchId, int Type, string HomeScore, string AwayScore, string bettypes,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass, 
                                          string DelOption, string transids,
                                          string DelOption_D, string transids_D,      
                                          string DelOption_BD, string transids_BD, string DelOption_12EU,
                                          string transids_12EU, string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, string Fromtransdate, string Totransdate, string ParlayType)
        {
            var hash = new Hashtable();
            hash.Add("matchid", matchId);
            hash.Add("type", Type);

            if (HomeScore.Length > 0 && AwayScore.Length > 0 && Fromtransdate.Length == 0 && Totransdate.Length == 0)
            {
                hash.Add("livehomescore", HomeScore);
                hash.Add("liveawayscore", AwayScore);
            }
            else if (Fromtransdate.Length > 0 && Totransdate.Length > 0 && HomeScore.Length == 0 && AwayScore.Length == 0)
            {
                hash.Add("fromtransdate", CastAide.AsDateTime(Fromtransdate).AddHours(-12));
                hash.Add("totransdate", CastAide.AsDateTime(Totransdate).AddHours(-12));
            }
            else if (HomeScore.Length > 0 && AwayScore.Length > 0 && Fromtransdate.Length > 0 && Totransdate.Length > 0)
            {
                hash.Add("livehomescore", HomeScore);
                hash.Add("liveawayscore", AwayScore);
                hash.Add("fromtransdate", CastAide.AsDateTime(Fromtransdate).AddHours(-12));
                hash.Add("totransdate", CastAide.AsDateTime(Totransdate).AddHours(-12));
            }


            hash.Add("hdp", DNAide.Core.CastAide.AsDecimal(hdp, 999));
            hash.Add("bettypes", bettypes);
            hash.Add("betteam1378", betteam1378);
            hash.Add("betteam515", betteam515);
            hash.Add("betteam430", betteam430);
            hash.Add("betteam6", betteam6);
            hash.Add("betteam14", betteam14);
            hash.Add("betteam16", betteam16);
            hash.Add("deloption", DelOption);
            hash.Add("transids", transids);
            hash.Add("deloption_d", DelOption_D);
            hash.Add("transids_d", transids_D);
            hash.Add("deloption_bd", DelOption_BD);
            hash.Add("transids_bd", transids_BD);
            hash.Add("deloption_12eu", DelOption_12EU);
            hash.Add("transids_12eu", transids_12EU);
            hash.Add("deloption_mhc", DelOption_MHC);
            hash.Add("transids_mhc", transids_MHC);
            hash.Add("username", uname);
            hash.Add("userpwd", Encryption.CfsEnCode(pass));
            hash.Add("userid", userId);
            hash.Add("errcode", 0);
            hash.Add("errmsg", string.Empty);
            hash.Add("ip", Utils.GetClientIP());
            //hash.Add("parlaytype", ParlayType);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Mixparlay, "DelMPList");
            return DbHelper.Execute(spElement.Name, hash, spElement.DbName, 1000);
        }
        #endregion


        public DbResult SelBettype(int type)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("mode", type);

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "BetTypeSel");
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName);
        }


        public DataTable SelBetBatchMsg(string transId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_BettransSelBet_BatchMsg"))
                {
                    objDB.AddInParameter("transids", DbType.String, transId);
                    DataSet ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
    }

}
