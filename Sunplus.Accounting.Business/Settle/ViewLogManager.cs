using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using IBCUtils;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Settle;
using System.Threading;
using System.Text;

namespace Sunplus.Accounting.Business.Settle
{
    public class ViewLogManager:IViewLogManager
    {
        public SettleLogResult SelSettleLogByMachCode(string matchCode, string fDate, string tDate)
        {
            var result = new SettleLogResult();

            fDate = CastAide.AsDateTime(fDate, DateTime.Today.AddDays(-2)).ToShortDateString();
            tDate = CastAide.AsDateTime(tDate).ToShortDateString();
            matchCode = CastAide.AsString(matchCode).Replace("^", "&");

            var param = new Hashtable();
            param.Add("matchcode", matchCode);
            param.Add("fromdate", fDate);
            param.Add("todate", tDate);
            param.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            var rs = IBCProvider.Settle.SelMatchInfo(param);

            if(rs!= null && rs.ResultSet.Tables.Count > 0)
            {
                result.Rows = new List<SettleLog>();
                foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                {
                    var machId = CastAide.AsString(dr["matchid"]);
                    var sportType = CastAide.AsInt32(dr["sporttype"]);
                    var settleLog = new SettleLog()
                                        {
                                            SportType = sportType,
                                            ShowTime = CastAide.AsString(dr["showtime"]),
                                            LeagueName = CacheRef.GetLeagueName(dr["leagueid"]),
                                            HomeName = string.Format("{0}{1}", CacheRef.GetTeamName(dr["homeid"]), dr.Table.Columns.Contains("isneutral") ? (CastAide.AsBoolean(dr["isneutral"]) ? " (N)" : "") : ""),
                                            AwayName = CacheRef.GetTeamName(dr["awayid"]),
                                            EventDate = CastAide.AsDateTime(dr["EventDate"]).ToShortDateString(),
                                            MatchId = string.Format("dv{0}", machId),
                                            SettleLogDetails = SelSettleLogDetail(machId, fDate, tDate, sportType, CastAide.AsInt32(dr["homeid2"]))
                                        };
                    if(settleLog.SettleLogDetails != null && settleLog.SettleLogDetails.Count > 0)
                        result.Rows.Add(settleLog);
                }

                
            }

            return result;
        }

        private List<SettleLogDetail> SelSettleLogDetail(string matchId, string fDate, string tDate, int sporttype, int bettypeId)
        {
            var result = new List<SettleLogDetail>();

            fDate = CastAide.AsDateTime(fDate, DateTime.Today.AddDays(-2)).ToShortDateString();
            tDate = CastAide.AsDateTime(tDate).ToShortDateString();

            DbResult rs = null;
            var param = new Hashtable();
            param.Add("matchid", matchId);
            param.Add("fromdate", FormatHandler.ResetStartDate(CastAide.AsDateTime(fDate)));
            param.Add("todate", FormatHandler.ResetEndDate(CastAide.AsDateTime(tDate)));
            rs = IBCProvider.Settle.SelMatchSettleLog(param);
            
            var viewIpVn = Permissions.HasPermission(Enumerations.Functions.ViewIPVN);
            var iplocation = WebSettings.ipLocation;

            if(rs!= null && rs.ResultSet.Tables.Count >0)
            {
                var rows = rs.ResultSet.Tables[0].Select();
                var index = 0;
                foreach (DataRow dr in rows)
                {
                    index++;

                    var resultString = CastAide.AsString(dr["betstatus"]).Length == 0 ? "-" : CastAide.AsString(dr["betstatus"]);
                    if (sporttype == 162)
                    {
                        resultString = ParseResultLiveCasino(resultString, bettypeId);
                    }
                    var ftScore = string.Format("{0} - {1}", CastAide.AsString(dr["finalhomescore"]), CastAide.AsString(dr["finalawayscore"]));
                    if (bettypeId == 1102)
                    {                        
                        ftScore = string.IsNullOrEmpty(CastAide.AsString(dr["finalhomescore"])) ? "-" : CastAide.AsString(dr["finalhomescore"]);
                        resultString = ftScore == "0" ? "-" : resultString;
                    }
                    var htScore = string.Format("{0} - {1}", CastAide.AsString(dr["hthomescore"]), CastAide.AsString(dr["htawayscore"]));

                    var settleLogDetail = new SettleLogDetail()
                                              {
                                                  No =index,
                                                  Date = CastAide.AsString(dr["logdate"]),
                                                  UserName = CastAide.AsString(dr["username"]),
                                                  Action = CastAide.AsString(dr["ActionNameEN"]),
                                                  FTScore = ftScore,
                                                  HTScore = htScore,
                                                  GoalRecord = CastAide.AsString(dr["GoalRecord"]).Length == 0 ? "-" : CastAide.AsString(dr["GoalRecord"]),
                                                  ResultString = resultString,
                                                  Remark = CastAide.AsString(dr["Remark"]).Length == 0 ? "-" : CastAide.AsString(dr["Remark"]),
                                                  Ip = GetIP(CastAide.AsString(dr["ip"]), viewIpVn, iplocation)
                                              };
                    result.Add(settleLogDetail);
                    
                }
            }

            return result;
        }

        private string ParseResultLiveCasino(string BetStatus, int bettypeId)
        {
            if (string.IsNullOrEmpty(BetStatus)) return string.Empty;
            string[] arr = BetStatus.Split(',');
            var rs = new StringBuilder();
            var index = 0;
            foreach (string s in arr)
            {
                rs.Append(string.Format("{0}{1}", 
                    CacheRef.GetLiveCasinoResult(CastAide.AsInt32(s.Trim()), bettypeId), 
                    (index == 2 && bettypeId == 1102 ? "; <br>" : "; ")));
                index++;
            }
            var result = rs.ToString();
            return result.Remove(result.Length - 2, 2);
        }

        private string GetIP(string ip, bool viewVnIp, string iplocation)
        {
            var link = "";

            if (!viewVnIp) ip = IpManagement.ReplaceVNIP(ip);
            if (ip.Length > 3)
            {
                link = "<a href=\"" + iplocation.Replace("{ip}", ip) + "\" target=\"_blank\">" + ip + "</a>";
            }
            return link;
        }
    }
}
