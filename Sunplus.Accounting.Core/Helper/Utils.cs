using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;

using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Helper
{
    public class Utils
    {
        private static readonly SiteMapNodeCollection SystemSiteMap = SiteMap.RootNode.ChildNodes;
        //Huy test
        //Reset IBC setting

        public static string GetClientIP()
        {
            try
            {
                if (HttpContext.Current.Request.IsLocal)
                    return "127.0.0.1";
                return CastAide.AsString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"], "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ParseError(ref int ErrorCode, ref string ErrorMsg, string ErrMsgContent, string ErrMsgOutPut,
                                      string root)
        {
            try
            {
                ErrorMsg = "";
                string strsplit = "";

                if (ErrMsgContent != "" && ErrMsgContent.IndexOf(strsplit) > 0)
                {
                    string[] arrerrors;
                    arrerrors = ErrMsgContent.Split(Convert.ToChar(strsplit));
                    string[] arrmsgs;
                    arrmsgs = ErrMsgOutPut.Split(Convert.ToChar(strsplit));
                    if (arrerrors.Length > 1)
                    {
                        for (int i = 0; i < arrerrors.Length - 1; i++)
                        {
                            if (CastAide.AsInt32(arrerrors[i]) < 0)
                            {
                                ErrorMsg += "<img src='" + root +
                                            "App_Themes/SteelBlue/Images/t1.gif' style='vertical-align:middle;' width='18px' height='18px' border='0' />&nbsp;" +
                                            "<span style='color:#000080;'>" +
                                            GetErrMessage(Globals.ErrorResource, arrerrors[i]) + "</span><br>";
                            }
                            else
                            {
                                ErrorCode = CastAide.AsInt32(arrerrors[i]);
                                ErrorMsg += "<img src='" + root +
                                            "App_Themes/SteelBlue/Images/t1.gif' style='vertical-align:middle;' width='18px' height='18px' border='0' />&nbsp;" +
                                            "<span style='color:red;'>" + arrmsgs[i] + "</span><br>";
                            }
                        }
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
            }
        }


        public static void ParseError(ref int ErrorCode, ref string ErrorMsg, string ErrMsgContent, string ErrMsgOutPut)
        {
            try
            {
                string strsplit = "";
                if (ErrMsgContent != "" && ErrMsgContent.IndexOf(strsplit) > 0)
                {
                    string[] arrerrors;
                    arrerrors = ErrMsgContent.Split(Convert.ToChar(strsplit));
                    string[] arrmsgs;
                    arrmsgs = ErrMsgOutPut.Split(Convert.ToChar(strsplit));
                    if (arrerrors.Length > 1)
                    {
                        for (int i = 0; i < arrerrors.Length - 1; i++)
                        {
                            if (CastAide.AsInt32(arrerrors[i]) < 0)
                            {
                                ErrorMsg +=
                                    "<img src='../../App_Themes/SteelBlue/Images/t1.gif' style='vertical-align:middle;' width='18px' height='18px' border='0' />&nbsp;" +
                                    "<span style='color:#000080;'>" + GetErrMessage(Globals.ErrorResource, arrerrors[i]) +
                                    "</span><br>";
                            }
                            else
                            {
                                ErrorCode = CastAide.AsInt32(arrerrors[i]);
                                ErrorMsg +=
                                    "<img src='../../App_Themes/SteelBlue/Images/x5.gif' style='vertical-align:middle;' width='18px' height='18px' border='0' />&nbsp;<span style='color:red;'>" +
                                    arrmsgs[i] + "</span><br>";
                            }
                        }
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
            }
        }

        public static string GetErrMessage(string ErrResourceName, string ErrorCode)
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
                return string.Empty;
            }
        }


        public static bool IsBooleanType(object value)
        {
            try
            {
                if (value.GetType() != typeof(Boolean))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool IsStringType(object value)
        {
            try
            {
                if (value.GetType() != typeof(String))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static IEnumerable<SystemBO> GetSystemList()
        {
            const string noteName = "System";
            SiteMapNode systemNote =
                SystemSiteMap.Cast<SiteMapNode>().Where(note => note.Title.ToLower().Contains(noteName.ToLower())).
                    FirstOrDefault();

            return systemNote == null
                       ? null
                       : (from object system in systemNote.Roles select new SystemBO(1, system.ToString())).ToList();
        }

        public static string GetDecimalRoundDown(decimal num)
        {
            string[] chars = num.ToString().Split('.');
            if (chars[1].Length <= 2)
                return String.Format("{0:0,0}", CastAide.AsDouble(num));
            else
            {
                if(CastAide.AsDouble(chars[0]) >= 1000)
                   return String.Format("{0:0,0}", CastAide.AsDouble(chars[0])) + "." + chars[1].Substring(0, 2);
                else
                    return chars[0] + "." + chars[1].Substring(0, 2);
               
            }
        }
    }
}
