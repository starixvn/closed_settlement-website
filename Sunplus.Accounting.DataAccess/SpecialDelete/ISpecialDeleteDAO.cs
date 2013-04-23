using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Data;
using Sunplus.Accounting.Entities.SpecialDelete;

namespace Sunplus.Accounting.DataAccess.SpecialDelete
{
    public interface ISpecialDeleteDAO
    {
        
        DbResult SelSpecialDeleteNumberGame(int matchId, long fromRefNo, long toRefNo, string live, string dead , string userName);
        DbResult SelMatchInfo(int matchId, long transid);
        DbResult DeleteSpecialTransactionNumberGame(int matchId, long frefno, long trefno, string live, string dead,
                     string username, int custId, string userpwd, int DelOption, string transids,  
            int DelOption_BD, string transids_BD, int DelOption_12EU, string transids_12EU, int DelOption_MHC, string transids_MHC,
            string ip);

        DbResult DeleteSpecialTransaction(string matchId, string frefno, string trefno, string bettype,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass, string DelOption, string transids,
                                          string liveindicator, string DelOption_BD, string transids_BD,
                                          string DelOption_12EU, string transids_12EU,
                                          string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, string homescore, string awayscore, string fromtransdate, string totransdate, int SAFilter);

        DbResult SelRecoverSpecialBets(string transid);

        DbResult RecoverSpecialBets(string transid);
        DbResult RemovePrivateMsg(string transid, int UserID);

        DbResult BettransSelSpecialDel(SpecialDeleteParam param);

        DbResult BettransDelSpecial_OutrightSel(int LeagueID);


        DbResult BettransSelDelSpecial_Outright(Hashtable param);

        DbResult BettransSelBet_BatchMsg(Hashtable param);

        #region EarlyKOBet
        DbResult SelectEarlyKOBet(SpecialDeleteParamEarlyKO deleteParam);

        DbResult SelectEarlyKOMixparlay(string refno, string transid, int curency);

        DbResult DeleteSpecialEarlyKO(string matchId, string bettype,
                                                       string betteam1378, string betteam515, string hdp, string uname,
                                                       string userId, string pass, string DelOption, string transids,
                                                       string kickoffdate, string DelOption_BD, string transids_BD,
                                                       string DelOption_12EU, string transids_12EU,
                                                       string DelOption_MHC, string transids_MHC, string betteam430,
                                                       string betteam6, string betteam14, string betteam16, int SAFilter);
        #endregion
        #region Mixparlay
        DbResult GetMPList(SpecialDeleteParam deleteParam);
        DbResult GetMPTicket(string refno);
        DbResult DeleteSpecialMixparlay(string matchId, int Type, string HomeScore, string AwayScore, string bettypes,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass, 
                                          string DelOption, string transids,
                                          string DelOption_D, string transids_D,
                                          string DelOption_BD, string transids_BD, string DelOption_12EU, 
                                          string transids_12EU, string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, string Fromtransdate, string Totransdate, string ParlayType);
        #endregion

        DbResult CheckRefno(string matchid, string fromRefno, string toRefno);

        DbResult SelBettype(int type);

        DataTable SelBetBatchMsg(string transId);

    }
}
