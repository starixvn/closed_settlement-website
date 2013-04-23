<%@ WebHandler Language="C#" Class="UpdateHedgeRecord" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;


public class UpdateHedgeRecord : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/xml";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        {                
            try
            {
                if (context.Session["AccId"] == null)
                {
                    context.Session.Clear();
                    context.Response.Write("no|" +IBCUtils.IbcError.SESSION_EXPIRED_MSG);    
                    context.Response.End();
                }
                else
                {
                    long TransID = DNAide.Core.CastAide.AsInt64(context.Request["TransID"]);
                    int Creator =  DNAide.Core.CastAide.AsInt32(context.Session["AccId"]);
                    decimal HomeScore = 0, AwayScore = 0, Price = 0, Amount = 0, hdp1 = 0, hdp2 = 0;
                    string Remark = "";
                    int Live = 0;
                    HomeScore = CastAide.AsDecimal(context.Request["HomeScore"],0);
                    AwayScore = CastAide.AsDecimal(context.Request["AwayScore"], 0);
                    Price = CastAide.AsDecimal(context.Request["Price"], 0);
                    Amount = CastAide.AsDecimal(context.Request["Amount"], 0);
                    Remark = context.Request["Remark"];
                    hdp1 = CastAide.AsDecimal(context.Request["hdp1"], 0);
                    hdp2 = CastAide.AsDecimal(context.Request["hdp2"], 0);
                    Live = CastAide.AsInt32(context.Request["Live"], 0);

                    Hashtable hash = new Hashtable();
                    hash.Add("transid", TransID);
                    hash.Add("odds", Price);
                    hash.Add("hdp1", hdp1);
                    hash.Add("hdp2", hdp2);
                    hash.Add("creator", Creator);
                    hash.Add("remark", Remark);
                    hash.Add("stake", Amount);
                    if (Live == 1)
                    {
                        hash.Add("livehomescore", HomeScore);
                        hash.Add("liveawayscore", AwayScore);
                    }
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_HedgeUpd(hash);
                    context.Response.Write("ok|updated");
                    //context.Response.Write(hdp1.ToString() + "|" + hdp2.ToString() + "|" + HomeScore.ToString() + "|" + AwayScore.ToString() + "|"  + Price.ToString() + "|" + Amount.ToString() + "|" + Remark + "|" + Live + "|" + TransID);
                    
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                context.Response.Write("no|" + StringUtils.ParseToJSMsg(ex.Message));
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
