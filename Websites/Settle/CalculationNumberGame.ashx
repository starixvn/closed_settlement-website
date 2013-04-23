<%@ WebHandler Language="C#" Class="CalculationNumberGame" %>

using System;
using System.Web;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using DNAide.Data;


public class CalculationNumberGame : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string _s = "false";
        string msg = "";
        string action = string.Empty;
        int userId = 0;
        string permissions = string.Empty;
        int matchId = DNAide.Core.CastAide.AsInt32(context.Request["matchid"], 0);
        //context.Request.Form["scoreDirect"];
        try
        {
            if(0 == matchId)
            {
                context.Response.Write("no|Invalid match!");
                return;
            }
            userId = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (userId == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(userId);

            action = context.Request["action"];
            var isMessage = DNAide.Core.CastAide.AsInt32(context.Request["isMsg"], 0);
            

            var numberGameSettlement = new NumberGame();
            var result = new ErrorDetail();
            bool isSuccess = false;
            switch (action.ToLowerInvariant())
            {

                case "refund":
                    if (permissions.IndexOf("[C]") > -1)
                    {
                        result = numberGameSettlement.Refund(matchId, AppSession.AccId); //DoSettle.RefundOrCancel(custID, matchID, IsExcludeBetdaq);
                    }
                    break;
                case "refundft":
                    if (permissions.IndexOf("[C]") > -1)
                    {
                        result = numberGameSettlement.RefundFt(matchId, AppSession.AccId); //DoSettle.RefundOrCancel(custID, matchID, IsExcludeBetdaq);
                    }
                    break;
                default:
                    break;
            }

            if (0 != result.ErrorCode) { _s = "no|" + result.ErrorMessage; }
            else 
            {
                _s = "success";
                if (1 == isMessage) 
                {
                    var messageSend = new System.Collections.Generic.Dictionary<string, string>();
                    messageSend.Add("EN", CastAide.AsString(context.Request["msgen"], string.Empty));
                    messageSend.Add("CH", CastAide.AsString(context.Request["msgch"], string.Empty));
                    messageSend.Add("CS", CastAide.AsString(context.Request["msgcs"], string.Empty));

                    var refundAction = (action == "refund") ? "Refund" : "Refund-FT";
                    var resultMesssage = numberGameSettlement.PrivateMessage(matchId, refundAction, AppSession.AccId, messageSend);
                    
                    _s += "|msg:1{err:" + resultMesssage.ErrorCode + "}";
                }
                // close and refesh
            }
            
        }
        catch (AccException ex)
        {
            _s = "<br />" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            _s = "<br />" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            //throw ex;
        }
        context.Response.Write(_s);
    }



    public bool IsReusable {
        get {
            return false;
        }
    }
}
