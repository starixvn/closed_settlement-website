<%@ WebHandler Language="C#" Class="ReplicateData" %>

using System;
using System.Web;
using DNAide.Core;
using DNAide.Web;
using Fanex.Tarus.Settlement.Entity;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using DNAide.Data;
using System.Collections;
using Fanex.Data;

public class ReplicateData : IHttpHandler, IRequiresSessionState
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private int custID = -1, sportID = -1, matchID = -1;
    private string type = string.Empty;
    private string permissions = "";
    private string _matchIds_Hedging = string.Empty;
    public void ProcessRequest(HttpContext context)
    {
        //  [Louis.Nguyen][01072013]
        //  Call Dot_Repl_UnSettled after un-calulating the match
        string sParams = context.Request.Form["sParams"];
        var successMatchIds = context.Request.Form["successMatchIds"];
        var sportId = Convert.ToInt32(sParams.Split('|')[1]);
        var accId = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
        if (accId != -1 && !string.IsNullOrEmpty(successMatchIds))
        {
            var arrIds = successMatchIds.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string matchId in arrIds)
            {
                try
                {
                    //var innerThread = new System.Threading.Thread(() =>
                    //{
                        using (ObjectDb objDb = new ObjectDb("Admin_Settlement_ReplicateData_UnSettled", 1800))
                        {
                            Hashtable p = new Hashtable();
                            p.Add("matchid", matchId);
                            objDb.AssignParamValues(p);
                            objDb.ExecuteNonQuery();
                        }
                    //});

                    //innerThread.Start();
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
