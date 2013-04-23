using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;
using DNAide.Core;
using DNAide.Data;

namespace Sunplus.Accounting.Entities
{
    public class Result
    {
        public Result()
        {
            ErrorCode = 0;
            ErrorMessage = "Successful";
        }

        public Result(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public Dictionary<int, string> ErrorHandler { get; set; }

        public void HandleError(ref Result result, DbResult ds, string fileResource)
        {
            try
            {
                result.ErrorHandler = new Dictionary<int, string>();
                if (CastAide.AsString(ds.Outputs["errcode"]).Length > 0)
                {
                    var listErrorCode = ds.Outputs["errcode"].ToString().Split('');
                    var listErrorMsg = ds.Outputs["errmsg"].ToString().Split('');
                    var len = listErrorMsg.Length;
                    result.ErrorCode = 100;
                    result.ErrorMessage = string.Empty;
                    for (int ind = 0; ind < len; ind++)
                    {
                        var eCode = CastAide.AsInt32(listErrorCode[ind]);
                        if (eCode == 0) { continue; }
                        var eMsg = eCode > 0
                                        ? listErrorMsg[ind]
                                        : HttpContext.GetGlobalResourceObject(fileResource, eCode.ToString()).ToString();
                        result.ErrorHandler.Add(eCode, eMsg);
                    }
                    if (result.ErrorHandler.Count==0)
                    {
                        var msgSuccess = HttpContext.GetGlobalResourceObject(fileResource, "0").ToString();
                        result.ErrorCode = 0;
                        result.ErrorMessage = msgSuccess;
                        result.ErrorHandler.Add(0, msgSuccess);
                    }
                }
                else
                {
                    var msgSuccess = HttpContext.GetGlobalResourceObject(fileResource, "0").ToString();
                    result.ErrorCode = 0;
                    result.ErrorMessage = msgSuccess;
                    result.ErrorHandler.Add(0, msgSuccess);
                }
            }
            catch (NullReferenceException ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
                result.ErrorHandler = new Dictionary<int, string> { { 100, ex.Message } };
            }
        }
    }

}
