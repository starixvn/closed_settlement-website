using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using IBCUtils.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.DAL;
using DNAide.Core;
using DNAide.Data;
using IBCUtils;
using DNAide.Web;

public partial class Settle_CalculationHR : System.Web.UI.Page
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
   
    //bool isBill = false;
    private int leagueID = -1, raceNo = -1;
    private string eventDate = "";
    protected string sportName = "Racing";
    /* Element
     *      type (bill, keyscore) 
     *      p (leagueid | raceno | eventdate  )
     *      calType 
     * */
    int custID = 0;
    string permissions = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string s = string.Empty;
       
        custID = Convert.ToInt32(Session["AccId"] as object ?? -1);
        if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
        permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(custID);

        if (Request["p"] == null || Request["type"] == null) throw new AccException("Sorry, Invalid session!");
        string type = Request["type"];
        string _params = Request["p"];
        leagueID = Convert.ToInt32(_params.Split('|')[0]);
        raceNo = Convert.ToInt32(_params.Split('|')[1]);
        eventDate = _params.Split('|')[2].ToString() ;
        if (string.Compare(type, "b", true) != 0 && string.Compare(type, "k", true) != 0 && string.Compare(type, "r", true) != 0)
            throw new AccException("Sorry, Invalid type!");
        //if (string.Compare(type, "b", true) == 0) isBill = true;
        DbResult dr = Settle.Acc_Set_HR_SelforBill(leagueID, raceNo, eventDate);
        s=LoadCalHTML(dr.ResultSet.Tables[0].Select(), permissions);

        this.sContent.Text = s.Replace(Environment.NewLine, string.Empty).Replace("  ", string.Empty);
    }

    private string LoadCalHTML(DataRow[] rows, string permissions)
    {
        string _sContent = "";
        _sContent=Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillHorseRacing.htm");
        string strrow = TemplateHandler.CopyBlock(_sContent, "row");
        TemplateHandler.RemoveBlock(ref _sContent, "row");
        TemplateHandler.Assign(ref _sContent, "{leagueName}", CacheRef.GetLeagueName(leagueID));
        TemplateHandler.Assign(ref _sContent, "{raceNumber}", raceNo.ToString());
        TemplateHandler.Assign(ref _sContent, "{eventStatus}", rows[0]["eventstatus"].ToString());
        TemplateHandler.Assign(ref _sContent, "{eventDate}", CastAide.AsDateTime(eventDate).ToShortDateString());
        if (permissions.IndexOf("[C]") > -1)
        {
            TemplateHandler.RemoveBlock(ref _sContent, "ReadPermission");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref _sContent, "HostPermission");
        }
        string _strrows = "";
        string _strrow = "";
        //rows = Calculate.OrderHorseList(rows);
        int i=1;

        foreach (DataRow row in rows)
        {
            _strrow = (string)strrow.Clone();

            string selectPlace = "<select id=\"Place_" + i.ToString() + "\" style=\"width: 50px;\" onchange=\"ChangeHorsePosition(this);\" >";

            selectPlace += "<option value=\"0\">---</option>";
            for (int j = 1; j <= rows.Length; j++)
            {
                if (CastAide.AsInt32(row["placing"]) == j && CastAide.AsInt32(row["placing"])<5)
                {
                    selectPlace += "<option value=\"" + j.ToString() + "\"  selected=\"selected\" >" + AccCommon.GetPlaceFormat(j) + "</option>";
                }
                else
                {
                    selectPlace += "<option value=\"" + j.ToString() + "\">" + AccCommon.GetPlaceFormat(j) + "</option>";
                }
            }
            selectPlace += "</select>";
            TemplateHandler.Assign(ref _strrow, "{placeNumber}", selectPlace);
            TemplateHandler.Assign(ref _strrow, "{lblHorseNo_Name}", row["HorseNo"].ToString() + "-" + CacheRef.GetTeamName(row["Horseid"]));
            string SCR = "";
            if (CastAide.AsInt32(row["IsSCR"]) == 1)
            {
                SCR = "checked";
            }
            TemplateHandler.Assign(ref _strrow, "{inputCheckSCR}", "<input id=\"Place_" + i + "_SCR\" " + SCR + " type=\"checkbox\"  style=\" width: 20px;{disabled}\"   />  ");
            if (CastAide.AsInt32(row["placing"]) < 5)
            {
                TemplateHandler.Assign(ref _strrow, "{disabled}", "display: none;");
            }
            else
            {
                TemplateHandler.Assign(ref _strrow, "{disabled}", "display: inline;");
            }

             if (CastAide.AsInt32(row["placing"]) < 4)
            {
                TemplateHandler.Assign(ref _strrow, "{rowEven}", "rowEven");
            }
            else
            {
                TemplateHandler.Assign(ref _strrow, "{rowEven}", "");
            }

            string TF = "";
            if (CastAide.AsInt32(row["IsFavor"]) == 1)
            {
                TF = "checked";
            }
            TemplateHandler.Assign(ref _strrow, "{inputCheckTF}", "<input id=\"Place_" + i + "_TF\" " + TF + " type=\"checkbox\"  style=\" width: 20px;\"  />  ");

            string oddwin = "";
            oddwin = FormatHandler.FormatNumber20(row["odds"]);
            TemplateHandler.Assign(ref _strrow, "{txtWin}", "<input id=\"Place_" + i + "_Win\" type=\"text\" value=\"" + oddwin + "\" style=\"text-align:center; width: 56px;{disabled}\" onkeypress=\"return OnKP(event, this);\" />");
            if (CastAide.AsInt32(row["placing"]) ==1)
            {
                TemplateHandler.Assign(ref _strrow, "{disabled}", "display: inline;");
            }
            else
            {
                TemplateHandler.Assign(ref _strrow, "{disabled}", "display: none;");
            }

            string oddplace = "";
            oddplace = FormatHandler.FormatNumber20(row["placeodds"]);
            TemplateHandler.Assign(ref _strrow, "{txtPlace}", "<input id=\"Place_" + i + "_Place\" value=\"" + oddplace + "\" type=\"text\" style=\"text-align:center; width: 56px;{disabled}\" onkeypress=\"return OnKP(event, this);\" />");
            if (CastAide.AsInt32(row["placing"]) < 4)
            {
                TemplateHandler.Assign(ref _strrow, "{disabled}", "display: inline;");
            }
            else
            {
                TemplateHandler.Assign(ref _strrow, "{disabled}", "display: none;");
            }
            TemplateHandler.Assign(ref _strrow, "{rowid}", "Place_" + i + "_Row");

            if (permissions.IndexOf("[C]") > -1)
            {
                TemplateHandler.Assign(ref _strrow, "{btnCalc}", "<input id=\"Place_" + i + "_BtnCalc\" value=\"Calculate\" type=\"Button\" style=\" width:60px;\" />");
            }
            else
            {
                TemplateHandler.Assign(ref _strrow, "{btnCalc}", "<input id=\"Place_" + i + "_BtnCalc\" value=\"Calculate\" disabled='disabled' type=\"Button\" style=\" width:60px;\" />");
            }
            if (permissions.IndexOf("[K]") > -1)
            {
                TemplateHandler.Assign(ref _strrow, "{btnKey}", "<input id=\"Place_" + i + "_BtnKey\" onclick=\"DoCalHR1(this);\" value=\"Update\" type=\"Button\" style=\" width:60px;\" />");
            }
            else
            {
                TemplateHandler.Assign(ref _strrow, "{btnKey}", "<input id=\"Place_" + i + "_BtnKey\" value=\"Update\" disabled='disabled' type=\"Button\" style=\" width:60px;\" />");
            }
                 _strrows += _strrow;
            i++;
        }
        TemplateHandler.Assign(ref _sContent, "{rows}", _strrows);
        return _sContent;
    }
}
