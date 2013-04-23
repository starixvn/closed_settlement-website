using System;
using System.Data;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using DNAide.Data;
using DNAide.Web;
using DNAide.Core;


public partial class CSSyncList : PageBase
{

    public string vlUserName;
    public int vlSearchBy;

    protected void Page_Load(object sender, EventArgs e)
    {
        vlUserName = CastAide.AsString(Request["UserName"]);
        vlSearchBy = CastAide.AsInt16(Request["SearchBy"]);
        if (vlUserName.Trim() != "")
            headerDatetime.Text = CasinoSyncList(vlUserName, vlSearchBy);
    }

    #region Casino Synchronised List By IBC DB
    public string CasinoSyncList(string vlUserName, int vlSearchBy)
    {
        StringBuilder str = new StringBuilder();
        try
        {
            DbResult dbr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CasinoSynchronise.Acc_Rpt_CasinoSynchronise(vlUserName, vlSearchBy);
            DataTable dtSync = dbr.ResultSet.Tables[0];

            string main = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/CasinoSyncList.htm");

            TemplateHandler.RemoveBlock(ref main, "LinkSyncUpd");
            if (vlSearchBy == 0)
            {
                TemplateHandler.RemoveBlock(ref main, "LinkSyncUpd");
                TemplateHandler.RemoveBlock(ref main, "LinkSync");
            }

            string sHeader = TemplateHandler.CopyBlock(main, "HeaderContent");
            string sRow = TemplateHandler.CopyBlock(main, "RowContent");
            str.Append("<table class='tbllayout' cellspacing='1' style='width:400px'>");
            string custid, username, _newheader, _newrow, _roleID = "0", parentSAg = "", parentMAg = "", parentAg = "", chkEnableCasino = "0";
            bool hasInsertData = false;

            if (dtSync.Rows.Count > 0)
            {
                DataTable dtCasino_SAg = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CasinoSynchronise.AP_Casino_Synchronise(vlUserName, 4);
                DataTable dtCasino_MAg = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CasinoSynchronise.AP_Casino_Synchronise(vlUserName, 3);
                DataTable dtCasino_Ag = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CasinoSynchronise.AP_Casino_Synchronise(vlUserName, 2);
                DataTable dtCasino_Mem = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CasinoSynchronise.AP_Casino_Synchronise(vlUserName, 1);

                bool bolInsert;
                foreach (DataRow row in dtSync.Rows)
                {
                    bolInsert = false;
                    _newrow = sRow;
                    _newheader = sHeader;
                    custid = row["custid"].ToString();
                    username = row["username"].ToString();
                    if (row["roleid"].ToString() == "4")
                    {
                        DataRow[] r = dtCasino_SAg.Select("custid=" + custid);
                        if (r.Length != 0)
                            chkEnableCasino = r[0].ItemArray[2].ToString();
                        else
                            chkEnableCasino = "-1"; // Not yet Synchronise!
                        if ((vlSearchBy == 1 && (chkEnableCasino == "-1" || row["disableCasino"].ToString() == chkEnableCasino)) || (vlSearchBy == 0 && chkEnableCasino != "-1"))
                        {
                            bolInsert = true;
                            TemplateHandler.Assign(ref _newheader, "{thColor}", "style='color:White'");
                            TemplateHandler.Assign(ref _newheader, "{FHeader_TD}", "");
                            TemplateHandler.Assign(ref _newrow, "{FRow_TD}", "");
                            TemplateHandler.Assign(ref _newheader, "{LHeader_TD}", "<td colspan=3 bgcolor='#536b81'>");
                            TemplateHandler.Assign(ref _newrow, "{LRow_TD}", "<td colspan=3></td>");
                            TemplateHandler.Assign(ref _newrow, "{role}", "Super");
                            TemplateHandler.Assign(ref _newheader, "{bgColor}", "class=tdtitle");
                            TemplateHandler.Assign(ref _newheader, "{parentUser}", "");
                            if (vlSearchBy != 0)
                                TemplateHandler.RemoveBlock(ref _newrow, "NoLinkSync");
                            // Check_Compare disableCasino
                            if (row["disableCasino"].ToString() == chkEnableCasino)
                                TemplateHandler.Assign(ref _newrow, "{bgDisableCas}", "class=tdtitle2");
                        }

                        parentSAg = username;
                    }
                    else if (row["roleid"].ToString() == "3")
                    {
                        DataRow[] r = dtCasino_MAg.Select("custid=" + custid);
                        if (r.Length != 0)
                            chkEnableCasino = r[0].ItemArray[2].ToString();
                        else
                            chkEnableCasino = "-1"; // Not yet Synchronise!
                        if ((vlSearchBy == 1 && (chkEnableCasino == "-1" || row["disableCasino"].ToString() == chkEnableCasino)) || (vlSearchBy == 0 && chkEnableCasino != "-1"))
                        {
                            bolInsert = true;
                            TemplateHandler.Assign(ref _newheader, "{FHeader_TD}", "<td></td>");
                            TemplateHandler.Assign(ref _newrow, "{FRow_TD}", "<td></td>");
                            TemplateHandler.Assign(ref _newheader, "{LHeader_TD}", "<td colspan=2 bgcolor='#88aaff'></td>");
                            TemplateHandler.Assign(ref _newrow, "{LRow_TD}", "<td colspan=2></td>");
                            TemplateHandler.Assign(ref _newrow, "{role}", "Master");
                            TemplateHandler.Assign(ref _newheader, "{bgColor}", "class=tdtitle");
                            TemplateHandler.Assign(ref _newheader, "{parentUser}", (parentSAg != "") ? " (" + parentSAg + ")" : "");
                            // Check UpLine to Sysnchronise             
                            if (vlSearchBy != 0)
                            {
                                if (dtCasino_SAg.Compute("count(custid)", "custid=" + row["srecommend"].ToString()).ToString() == "0")
                                    TemplateHandler.RemoveBlock(ref _newrow, "LinkSync");
                                else
                                    TemplateHandler.RemoveBlock(ref _newrow, "NoLinkSync");
                            }
                            // Check_Compare disableCasino
                            if (row["disableCasino"].ToString() == chkEnableCasino)
                                TemplateHandler.Assign(ref _newrow, "{bgDisableCas}", "class=tdtitle2");
                        }
                        parentMAg = username;
                    }
                    else if (row["roleid"].ToString() == "2")
                    {
                        DataRow[] r = dtCasino_Ag.Select("custid=" + custid);
                        if (r.Length != 0)
                            chkEnableCasino = r[0].ItemArray[2].ToString();
                        else
                            chkEnableCasino = "-1"; // Not yet Synchronise!
                        if ((vlSearchBy == 1 && (chkEnableCasino == "-1" || row["disableCasino"].ToString() == chkEnableCasino)) || (vlSearchBy == 0 && chkEnableCasino != "-1"))
                        {
                            bolInsert = true;
                            TemplateHandler.Assign(ref _newheader, "{FHeader_TD}", "<td colspan=2></td>");
                            TemplateHandler.Assign(ref _newrow, "{FRow_TD}", "<td colspan=2></td>");
                            TemplateHandler.Assign(ref _newheader, "{LHeader_TD}", "<td bgcolor='#a0a97a'></td>");
                            TemplateHandler.Assign(ref _newrow, "{LRow_TD}", "<td></td>");
                            TemplateHandler.Assign(ref _newrow, "{role}", "Agent");
                            TemplateHandler.Assign(ref _newheader, "{bgColor}", "class=tdtitle");
                            TemplateHandler.Assign(ref _newheader, "{parentUser}", (parentMAg != "") ? " (" + parentMAg + ")" : "");
                            // Check UpLine to Sysnchronise
                            if (vlSearchBy != 0)
                            {
                                if ((dtCasino_MAg.Compute("count(custid)", "custid=" + row["mrecommend"].ToString()).ToString() == "0")
                                    || (dtCasino_SAg.Compute("count(custid)", "custid=" + row["srecommend"].ToString()).ToString() == "0"))
                                    TemplateHandler.RemoveBlock(ref _newrow, "LinkSync");
                                else
                                    TemplateHandler.RemoveBlock(ref _newrow, "NoLinkSync");
                            }
                            // Check_Compare disableCasino
                            if (row["disableCasino"].ToString() == chkEnableCasino)
                                TemplateHandler.Assign(ref _newrow, "{bgDisableCas}", "class=tdtitle2");
                        }
                        parentAg = username;
                    }
                    else
                    {
                        DataRow[] r = dtCasino_Mem.Select("custid=" + custid);
                        if (r.Length != 0)
                            chkEnableCasino = r[0].ItemArray[2].ToString();
                        else
                            chkEnableCasino = "-1"; // Not yet Synchronise!
                        if ((vlSearchBy == 1 && (chkEnableCasino == "-1" || row["disableCasino"].ToString() == chkEnableCasino)) || (vlSearchBy == 0 && chkEnableCasino != "-1"))
                        {
                            bolInsert = true;
                            TemplateHandler.Assign(ref _newheader, "{FHeader_TD}", "<td colspan=3></td>");
                            TemplateHandler.Assign(ref _newrow, "{FRow_TD}", "<td colspan=3></td>");
                            TemplateHandler.Assign(ref _newheader, "{LHeader_TD}", "");
                            TemplateHandler.Assign(ref _newrow, "{LRow_TD}", "");
                            TemplateHandler.Assign(ref _newrow, "{role}", "Member");
                            TemplateHandler.Assign(ref _newheader, "{bgColor}", "class=tdtitle");
                            TemplateHandler.Assign(ref _newheader, "{parentUser}", (parentAg != "") ? " (" + parentAg + ")" : "");
                            // Check UpLine to Sysnchronise
                            if (vlSearchBy != 0)
                            {
                                if ((dtCasino_Ag.Compute("count(custid)", "custid=" + row["recommend"].ToString()).ToString() == "0")
                                    || (dtCasino_MAg.Compute("count(custid)", "custid=" + row["mrecommend"].ToString()).ToString() == "0")
                                    || (dtCasino_SAg.Compute("count(custid)", "custid=" + row["srecommend"].ToString()).ToString() == "0"))
                                    TemplateHandler.RemoveBlock(ref _newrow, "LinkSync");
                                else
                                    TemplateHandler.RemoveBlock(ref _newrow, "NoLinkSync");
                            }
                            // Check_Compare disableCasino
                            if (row["disableCasino"].ToString() == chkEnableCasino)
                                TemplateHandler.Assign(ref _newrow, "{bgDisableCas}", "class=tdtitle2");
                        }
                    }

                    if (bolInsert)
                    {
                        hasInsertData = true;
                        TemplateHandler.Assign(ref _newrow, "{custid}", custid);
                        TemplateHandler.Assign(ref _newrow, "{username}", username);

                        if (_roleID != row["roleid"].ToString())
                        {
                            _roleID = row["roleid"].ToString();
                            str.Append(_newheader + _newrow);

                        }
                        else
                        {
                            str.Append(_newrow);
                        }
                    }
                }
            }
            if (!hasInsertData)
            {
                TemplateHandler.Assign(ref sHeader, "{bgColor}", "class=tdtitle");
                TemplateHandler.Assign(ref sHeader, "{thColor}", "style='color:White'");
                TemplateHandler.Assign(ref sHeader, "{FHeader_TD}", "");
                TemplateHandler.Assign(ref sHeader, "{LHeader_TD}", "");
                TemplateHandler.Assign(ref sHeader, "{parentUser}", " Name");
                str.Append(sHeader + AccCommon.GenNoInformation("7"));
            }
            str.Append("</table>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return str.ToString();
    }
    #endregion

}
