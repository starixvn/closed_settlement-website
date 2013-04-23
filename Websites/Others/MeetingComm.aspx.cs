
using System;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Core;
using Resources;

public partial class UserControl_Menu : PageBase
{

    public string SystemPath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SystemPath = Sunplus.Accounting.Core.Web.WebUtils.GetTemplateFolder().Replace("\\","/");
    } 

    private string GenHRMeetingList()
    {
        string strResult = "";

        DataTable dtMeetingList = CompanyReport.GetHRMeetingList();
        if(dtMeetingList != null && dtMeetingList.Rows.Count > 0)
        {
            string tplContent = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplateBySystem("Others/MeetingComm.htm", true);
            string tplRow = TemplateHandler.CopyBlock(tplContent, "row");
            //Asign Header
            TemplateHandler.Assign(ref tplContent, "{lblMeetingEn}", Resources.Language.lblMeetingEn);            
            TemplateHandler.Assign(ref tplContent, "{lblCommHR}", Resources.Language.Comm + " (%)");
            TemplateHandler.Assign(ref tplContent, "{lblWin}", Resources.Language.lblWin);
            TemplateHandler.Assign(ref tplContent, "{lblPlace}", Resources.Language.lblPlace);
            TemplateHandler.Assign(ref tplContent, "{lblWinPlace}", Resources.Language.lblWinPlace);
            TemplateHandler.Assign(ref tplContent, "{lblCommType}", Resources.Language.lblCommType);
            //Gen data
            int i = 1;
            string rowContent = "";

            DataRowCollection dtRow = dtMeetingList.Rows;
            foreach (DataRow row in dtRow)
            {
                string sRow = tplRow;
                string rowBgClass = (i % 2 == 0) ? "evenrow" : "oddrow";
                double commWin = CastAide.AsDouble(row["commwin"], 0) * 100;
                double commPlace = CastAide.AsDouble(row["commplace"], 0) * 100;
                double commWinPlace = ( commWin + commPlace) / 2;
                string commType = (CastAide.AsInt32(row["commtype"], 0) == 1) ? Language.lblUKMeeting : Language.lblSaMeeting;
                TemplateHandler.Assign(ref sRow, "{bgrow}", rowBgClass);
                TemplateHandler.Assign(ref sRow, "{no}", i.ToString());
                TemplateHandler.Assign(ref sRow, "{meetingEn}", RemotingRef.RefData.GetLeagueName(CastAide.AsString(row["meetingid"]), "EN"));
                TemplateHandler.Assign(ref sRow, "{commWin}", commWin.ToString());

                TemplateHandler.Assign(ref sRow, "{ProgramID}", CastAide.AsString(row["ProgramID"]));
                TemplateHandler.Assign(ref sRow, "{commPlace}", commPlace.ToString());
                TemplateHandler.Assign(ref sRow, "{commWinPlace}", commWinPlace.ToString());
                TemplateHandler.Assign(ref sRow, "{commtype}", commType);
                rowContent += sRow;
                i++;
            }
            TemplateHandler.ReplaceBlock(ref tplContent, "row", rowContent);
            strResult = tplContent;

        }
        else
        {
            string tplNoInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Others/NoMeetingComm.htm");
            TemplateHandler.Assign(ref tplNoInfo, "{lblMeetingEn}", Resources.Language.lblMeetingEn);
            TemplateHandler.Assign(ref tplNoInfo, "{lblMeetingTC}", Resources.Language.lblMeetingTC);
            TemplateHandler.Assign(ref tplNoInfo, "{lblMeetingSC}", Resources.Language.lblMeetingSC);
            TemplateHandler.Assign(ref tplNoInfo, "{lblCommHR}", Resources.Language.Comm + " (%)");
            TemplateHandler.Assign(ref tplNoInfo, "{lblWin}", Resources.Language.lblWin);
            TemplateHandler.Assign(ref tplNoInfo, "{lblPlace}", Resources.Language.lblPlace);
            TemplateHandler.Assign(ref tplNoInfo, "{lblWinPlace}", Resources.Language.lblWinPlace);
            TemplateHandler.Assign(ref tplNoInfo, "{lblCommType}", Resources.Language.lblCommType);
            TemplateHandler.Assign(ref tplNoInfo, "{noinfo}", Resources.Language.thereisnodata);

            strResult = tplNoInfo;
        }
        return strResult;
    }
    private DataTable GenSampleData()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add("meetingid", typeof(int));
        tbl.Columns.Add("ProgramID", typeof(string));
        tbl.Columns.Add("commwin", typeof(int));
        tbl.Columns.Add("commplace", typeof(int));
        tbl.Columns.Add("commtype", typeof(string));

        for (int i = 1; i < 2000; i++)
        {
            tbl.Rows.Add(i, "ID_" + i, i, i + 1, "Type_" + i);
        }
        return tbl;
    }
}
