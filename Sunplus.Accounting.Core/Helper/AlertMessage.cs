using System;
using System.Web;
using DNAide.Core;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Constant;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Helper
{
    /// <summary>
    /// Summary description for AlertMessage
    /// </summary>
    public class AlertMessage
    {
        #region MsgType enum

        public enum MsgType
        {
            Success = 0,
            Error = 1,
            Warning = 2
        }

        #endregion

        /// <summary>
        /// Return list formated error messages and error code from output message and code
        /// </summary>
        /// <param name="ErrorCode"></param>
        /// <param name="ErrorMsg"></param>
        /// <param name="ErrMsgContent"></param>
        /// <param name="ErrMsgOutPut"></param>
        public static void ParseError(ref int ErrorCode, ref string ErrorMsg, string ErrMsgContent, string ErrMsgOutPut)
        {
            try
            {
                string strsplit = "";
                if (ErrMsgContent != "" && ErrMsgContent.IndexOf(strsplit) >= 0)
                {
                    string[] arrerrors;
                    arrerrors = ErrMsgContent.Split(Convert.ToChar(strsplit));
                    string[] arrmsgs;
                    arrmsgs = ErrMsgOutPut.Split(Convert.ToChar(strsplit));
                    if (arrerrors.Length > 1)
                    {
                        ErrorMsg = "";
                        for (int i = 0; i < arrerrors.Length - 1; i++)
                        {
                            if (CastAide.AsInt32(arrerrors[i]) < 0)
                            {
                                ErrorMsg += GetMsgUnboxed(GetErrMessage(Globals.ErrorResource, arrerrors[i]),
                                                          MsgType.Success);
                            }
                            else
                            {
                                if (arrmsgs[i] != "")
                                {
                                    ErrorCode = CastAide.AsInt32(arrerrors[i]);
                                    ErrorMsg += GetMsgUnboxed(arrmsgs[i], MsgType.Error);
                                }
                            }
                        }
                        ErrorMsg = "<div class=\"alertbox\">" + ErrorMsg + "</div>";
                    }
                }
                else
                {
                    ErrorCode = Globals.ErrorCodeInvalid;
                    ErrorMsg = Globals.ErrorMsgInvalid;
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string GetMsg(string content, MsgType type)
        {
            string result = "";
            content = content.Replace("'", "&rsquo;");
            switch (type)
            {
                case MsgType.Error:
                    result = "<div class=\"alertboxerr\"><div>" + content + "</div></div>";
                    break;
                case MsgType.Success:
                    result = "<div class=\"alertboxscc\"><div>" + content + "</div></div>";
                    break;
                case MsgType.Warning:
                    result = "<div class=\"alertboxwrn\"><div>" + content + "</div></div>";
                    ;
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public static string FormatErrorMsgLogin(string errmsg, string root)
        {
            string result = "";
            result = "<img src='" + root +
                     "App_Themes/Images/ico_warning.png' style='vertical-align:middle;' border='0' />&nbsp;" + errmsg;
            return result;
        }

        private static string GetMsgUnboxed(string content, MsgType type)
        {
            string result = "";
            content = content.Replace("'", "&rsquo;");
            switch (type)
            {
                case MsgType.Error:
                    result = "<div class=\"msgerror\">" + content + "</div>";
                    break;
                case MsgType.Success:
                    result = "<div class=\"msgcorrect\">" + content + "</div>";
                    break;
                case MsgType.Warning:
                    result = "<div class=\"msgwarning\">" + content + "</div>";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        private static string GetErrMessage(string ErrResourceName, string ErrorCode)
        {
            try
            {
                return HttpContext.GetGlobalResourceObject(ErrResourceName, ErrorCode).ToString();
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }
    }
}