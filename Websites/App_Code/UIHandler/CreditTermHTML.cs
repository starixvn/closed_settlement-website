using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

namespace Sunplus.Accounting.Presentation
{
    /// <summary>
    /// Summary description for CreditTermHTML
    /// </summary>
    public class CreditTermHTML
    {
        public static string html = "";
        public CreditTermHTML()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Credit Term
        public static string GetContentCreditTermFilter(HttpContext context)
        {
            html = WebUtils.LoadTemplate("CreditTeam/Creditterm_Group_Feed_Form.htm");
            string result = TemplateHandler.CopyBlock(html, "Filter");
            int groupid = 0;
            if (!string.IsNullOrEmpty(context.Request["groupid"]))
            {
                groupid = Convert.ToInt32(context.Request["groupid"]);
                AppSession.GroupID = groupid;
            }
            else if (AppSession.GroupID != null) groupid = AppSession.GroupID;
            TemplateHandler.Assign(ref result, "{groupid}", groupid.ToString());
            return result;
        }
        public static string GetContentCreditTerm(HttpContext context)
        {
            html = WebUtils.LoadTemplate("CreditTeam/Creditterm_Group_Feed_Form.htm");
            string from = html;
            try
            {
                TemplateHandler.RemoveBlock(ref from, "Filter");
                int groupid = 0;
                if (!string.IsNullOrEmpty(context.Request["groupid"]))
                {
                    groupid = Convert.ToInt32(context.Request["groupid"]);
                    AppSession.GroupID = groupid;
                }
                else if (AppSession.GroupID != null) groupid = AppSession.GroupID;

                string username = context.Request["username"];
                int userid = AppSession.AccId; // Session["AccId"]
                string listname = context.Request["lstlistname"];
                int type = CastAide.AsInt32(context.Request["type"]);
                //type:
                // 1 - Add this user to group >>
                // 2 - Set Selected User to Headline
                // 3 - Remove Selected Users from This Group
                string username_selected = "", custid_selected = "", msg = "";
                int count_normal_user_selected = 0;

                if (type == 1) // 1 - Add this user to group >>
                {
                    DbResult rs = AccountCredit.Acc_CreditTermUpdAddToGroup(username, groupid, userid);
                    if (Convert.ToInt32(rs.Outputs["errcode"]) != 0)
                        TemplateHandler.Assign(ref from, "{msg}",
                                               "<script>showAlertMessage('errMsg','" +
                                               Convert.ToString(rs.Outputs["errmsg"]) + "',1);</script>");
                    else
                        TemplateHandler.Assign(ref from, "{msg}",
                                               "<script>showAlertMessage('errMsg','Add " + username +
                                               " to group successfully.',0);</script>");
                }
                else if (type == 3) // 3 - Remove Selected Users from This Group
                {
                    var stringseparator = new[] {","};
                    foreach (string item in listname.Split(stringseparator, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item != "")
                        {
                            username_selected = item.Split('|')[1];
                            custid_selected = item.Split('|')[0];
                            DbResult rs = AccountCredit.Acc_CreditTermUpdRemoveFromGroup(custid_selected, groupid, userid);
                            if (rs != null)
                            {
                                if (CastAide.AsInt32(rs.Outputs["errcode"]) != 0)
                                {
                                    msg = "<script>showAlertMessage('errMsg','" + CastAide.AsString(rs.Outputs["errmsg"]) +
                                          "',1);</script>";
                                }
                                else
                                {
                                    msg += username_selected + ", ";
                                    count_normal_user_selected++;
                                }
                            }
                        }
                    }
                    if (msg.LastIndexOf(",") > 0)
                        msg = msg.Substring(0, msg.Length - 2);
                    if (count_normal_user_selected > 0)
                    {
                        msg += " has been removed.";
                        msg = "<script>showAlertMessage('errMsg','" + msg + "',0);</script>";
                    }

                    TemplateHandler.Assign(ref from, "{msg}", msg);
                }
                else if (type == 2) // 2 - Set Selected User to Headline
                {
                    custid_selected = listname;
                    DbResult rs = AccountCredit.Acc_CreditTermUpdSetGroupHeadline(Convert.ToInt32(custid_selected), groupid,
                                                                                  userid);
                    if (rs != null)
                    {
                        if (CastAide.AsInt32(rs.Outputs["errcode"]) != 0)
                        {
                            TemplateHandler.Assign(ref from, "{msg}",
                                                   "<script>showAlertMessage('errMsg','" +
                                                   CastAide.AsString(rs.Outputs["errmsg"]) + "',1);</script>");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref from, "{msg}",
                                                   "<script>showAlertMessage('errMsg','Group Headline has been changed.',0);</script>");
                        }
                    }
                }
                else TemplateHandler.Assign(ref from, "{msg}", "");

                if (type > 0)
                {
                    var newThread = new Thread(ThreadMethod);
                    newThread.Start();
                    Thread.Sleep(2000);
                }

                DataTable dt = AccountCredit.Acc_CreditTermSelByGroupID(groupid).ResultSet.Tables[0];
                string headline = "";
                var userrows = new StringBuilder();
                string userrow = TemplateHandler.CopyBlock(from, "UserRow");
                TemplateHandler.RemoveBlock(ref from, "UserRow");
                string _userrow = "";
                string headlineclass = "";
                string headlineclasstext = "";
                string removeitem = "";
                string jsheadline = "";
                string title = "";
                TemplateHandler.RemoveBlock(ref from, "UserRow");
                string roweven = "evenrow";
                foreach (DataRow dr in dt.Rows)
                {
                    _userrow = (string) userrow.Clone();
                    if (Convert.ToBoolean(dr["isgroupheadline"]))
                    {
                        jsheadline = "";
                        headline = "(HEAD LINE)";
                        headlineclass = "../App_Themes/Images/headlineH.gif";
                        headlineclasstext = "headlinetext";
                        removeitem = "";
                        title = "Headline user";
                    }
                    else
                    {
                        title = "Set this user to headline";
                        headlineclass = "../App_Themes/Images/headline.gif";
                        headlineclasstext = "";
                        headline = "";
                        removeitem = "<input type='checkbox' value='" + dr["custid"] + "|" + dr["username"] +
                                     "' name='chkuser' id='chkuser' />";
                        jsheadline = "ExecuteCreditTerm('2','" + dr["custid"] + "');";
                    }
                    if (roweven == "") roweven = "evenrow";
                    else roweven = "";
                    TemplateHandler.Assign(ref _userrow, "{row-even}", roweven);
                    TemplateHandler.Assign(ref _userrow, "{CustId}", dr["username"] + " " + headline);
                    TemplateHandler.Assign(ref _userrow, "{UserName}", dr["username"] + " " + headline);
                    TemplateHandler.Assign(ref _userrow, "{headline}", headlineclass);
                    TemplateHandler.Assign(ref _userrow, "{headlineclasstext}", headlineclasstext);
                    TemplateHandler.Assign(ref _userrow, "{removeitem}", removeitem);
                    TemplateHandler.Assign(ref _userrow, "{jsaddheadline}", jsheadline);
                    TemplateHandler.Assign(ref _userrow, "{title}", title);

                    userrows.Append(_userrow);
                }
                TemplateHandler.Assign(ref from, "{UserRowList}", userrows.ToString());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                TemplateHandler.Assign(ref from, "{UserRowList}",
                                       "<tr><td colspan='3' class='c'>" + ex.Message + "</td></tr>");
            }
            return from;
        }

        #endregion

        private static void ThreadMethod()
        {
            Thread.Sleep(10);
        }
    }
}
