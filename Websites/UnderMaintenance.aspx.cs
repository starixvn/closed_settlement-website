using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;

public partial class UnderMaintenance : System.Web.UI.Page
{
    protected string UMTime_From = string.Empty;
    protected string UMTime_To = string.Empty;

    protected override void OnLoad(EventArgs e)
    {
        Session.Clear();
        FormsAuthentication.SignOut();


        var downTimeInfo = new Fanex.Tarus.Business.Implement.CommonService().GetDowntimeInfo();
        UMTime_From = downTimeInfo.From;
        UMTime_To = downTimeInfo.To;

        if (string.IsNullOrEmpty(UMTime_From) || string.IsNullOrEmpty(UMTime_To))
        {
            UMTime_From = CastAide.AsString(Request.QueryString["from"], string.Empty);
            UMTime_To = CastAide.AsString(Request.QueryString["to"], string.Empty);
        }
          
    }
}
