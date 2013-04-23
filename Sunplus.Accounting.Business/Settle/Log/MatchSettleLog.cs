using System;
using System.Collections;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using IBCUtils;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.Core.Web.TplCore.App_Templates.Settle.App_Templates.Settle;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Core.Extensions;

/// <summary>
/// Summary description for MatchSettleLog
/// </summary>

namespace Sunplus.Accounting.Business.Settle.Log
{
    public class MatchSettleLog
    {
        #region LoadMenu

        public string LoadMenu()
        {
            var menu = new ViewLogMenu_Template();
            return menu.ToString();
        }

        #endregion

        #region LoadHeaderForm

        public string LoadHeaderForm(string matchcode, string fdate, string tdate)
        {
            fdate = CastAide.AsDateTime(fdate, DateTime.Today.AddDays(-2)).ToShortDateString();
            tdate = CastAide.AsDateTime(tdate).ToShortDateString();

            var form = new ViewLogHeaderForm_Template();
            matchcode = CastAide.AsString(matchcode).Replace("^", "&");
            form.MatchCode = matchcode;
            form.fromdate = Calendar.GenCalendarControl("fromdate", "../", "fromdateTrigger", fdate, false, false, 0, 0,
                                                        "");
            form.todate = Calendar.GenCalendarControl("todate", "../", "todateTrigger", tdate, true, false, 0, 0, "");
            return form.ToString();
        }

        #endregion

        #region LoadMatchInfo

        public string LoadMatchInfo(string matchcode, string fdate, string tdate)
        {
            fdate = CastAide.AsDateTime(fdate, DateTime.Today.AddDays(-2)).ToShortDateString();
            tdate = CastAide.AsDateTime(tdate).ToShortDateString();
            matchcode = CastAide.AsString(matchcode).Replace("^", "&");

            var html = new StringBuilder();

            html.Append("<div id='sData'>");

            var info = new ViewLogMatchInfo_Template();

            var param = new Hashtable();
            param.Add("matchcode", matchcode);
            param.Add("fromdate", fdate);
            param.Add("todate", tdate);
            param.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            DbResult rs = IBCProvider.Settle.SelMatchInfo(param);
            if (rs.ResultSet.Tables[0].Rows.Count == 0)
            {
                var noinfo = new ViewLogMatchDetailsNoInfo_Template();
                noinfo.NoInfo = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata") ;
                html.Append(noinfo.ToString());
            }
            else
            {
                int count = rs.ResultSet.Tables[0].Rows.Count;
                string matchId = "";
                int index = 0;
                bool isHasData = false;
                string matchInfo = "";
                foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                {
                    matchId = CastAide.AsString(dr["matchid"]);
                    matchInfo = LoadMatchInfoDetails(matchId, fdate, tdate, ref isHasData);
                    if (!isHasData) continue;
                    index++;

                    info = new ViewLogMatchInfo_Template();
                    info.LeagueName = CacheRef.GetLeagueName(dr["leagueid"]);
                    info.Home = CacheRef.GetTeamName(dr["homeid"]);
                    info.Away = CacheRef.GetTeamName(dr["awayid"]);
                    info.EventDate = CastAide.AsDateTime(dr["EventDate"]).ToShortDateString();

                    info.MatchInfoDetails = matchInfo;
                    info.matchid = "dv" + matchId;
                    info.matchinfoclass = index == 1 ? "" : "hide";
                    html.Append(info.ToString());
                }

                if (index == 0)
                {
                    var noinfo = new ViewLogMatchDetailsNoInfo_Template();
                    noinfo.NoInfo = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
                    html.Append(noinfo.ToString());
                }
            }

            html.Append("</div>");
            return html.ToString();
        }

        #endregion

        #region LoadMatchInfoDetails

        private string LoadMatchInfoDetails(string matchid, string fdate, string tdate, ref bool isHasData)
        {
            fdate = CastAide.AsDateTime(fdate, DateTime.Today.AddDays(-2)).ToShortDateString();
            tdate = CastAide.AsDateTime(tdate).ToShortDateString();

            DbResult rs = null;
            var param = new Hashtable();
            param.Add("matchid", matchid);
            param.Add("fromdate", FormatHandler.ResetStartDate(CastAide.AsDateTime(fdate)));
            param.Add("todate", FormatHandler.ResetEndDate(CastAide.AsDateTime(tdate)));
            rs = IBCProvider.Settle.SelMatchSettleLog(param);
            DataRow[] Rows = rs.ResultSet.Tables[0].Select();

            var details = new ViewLogMatchDetails_Template();
            details.ContentDetail.Clear();
            bool viewIpVn = Permissions.HasPermission(Enumerations.Functions.ViewIPVN);
            string iplocation = WebSettings.ipLocation;
            int index = 0;
            if (Rows.Length > 0)
            {
                isHasData = true;
                foreach (DataRow dr in Rows)
                {
                    index++;
                    var content = new ViewLogMatchDetails_ContentDetail_Block();
                    content.No = index.ToString();
                    content.LogDate = CastAide.AsString(dr["logdate"]);
                    content.UserName = CastAide.AsString(dr["username"]);
                    content.ActionName = CastAide.AsString(dr["ActionNameEN"]);
                    content.ActionName = CastAide.AsString(dr["ActionNameEN"]);
                    content.FTHomeScore = CastAide.AsString(dr["finalhomescore"]);
                    content.FTAwayScore = CastAide.AsString(dr["finalawayscore"]);
                    content.HTHomeScore = CastAide.AsString(dr["hthomescore"]);
                    content.HTAwayScore = CastAide.AsString(dr["htawayscore"]);

                    if (CastAide.AsString(dr["GoalRecord"]).Length == 0)
                    {
                        content.GoalRecord = "-";
                    }
                    else
                    {
                        content.GoalRecord = CastAide.AsString(dr["GoalRecord"]);
                    }

                    if (CastAide.AsString(dr["betstatus"]).Length == 0)
                    {
                        content.BetStatus = "-";
                    }
                    else
                    {
                        content.BetStatus = CastAide.AsString(dr["betstatus"]);
                    }

                    if (CastAide.AsString(dr["Remark"]).Length == 0)
                    {
                        content.ReMark = "-";
                        content.class_ReMark = "c";
                    }
                    else
                    {
                        content.ReMark = CastAide.AsString(dr["Remark"]);
                        content.class_ReMark = "";
                    }


                    content.IP = GetIP(CastAide.AsString(dr["ip"]), viewIpVn, iplocation);
                    details.ContentDetail.Append(content);
                }
            }
            else
            {
                isHasData = false;
                details.ContentDetail.Append(
                    "<tr><td align='center' bgcolor='white' colspan='13'>There is no infomation available</td></tr>");
            }

            return details.ToString();
        }

        #endregion

        #region GetIP

        private string GetIP(string ip, bool viewVnIp, string iplocation)
        {
            string link = "";

            if (!viewVnIp) ip = IpManagement.ReplaceVNIP(ip);
            if (ip.Length > 3)
            {
                link = "<a href=\"" + iplocation.Replace("{ip}", ip) + "\" target=\"_blank\">" + ip + "</a>";
            }
            return link;
        }

        #endregion

        public string ResouceExtension { get; set; }
    }
}