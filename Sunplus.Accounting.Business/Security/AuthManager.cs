using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.DataAccess.IBCDLL;
using Sunplus.Core.LoggingLib;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Security
{
    /// <summary>
    /// Manage system authorization
    /// </summary>
    public class AuthManager
    {
        public AuthManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void DoLogout()
        {
            try
            {
                int accOnId = 0;
                if (HttpContext.Current.Session["OnAccId"] != null)
                {
                    accOnId = (int)HttpContext.Current.Session["OnAccId"];
                }
                UserController.AccLogOut(accOnId, AppSession.Username, WebSettings.ServerId);
                HttpContext.Current.Session.Clear();
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/SignIn.aspx");
            }
            catch (Exception ex)
            {
                //Logs.Write(ex.Message);
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        public static void AssignRoles(int custid)
        {
            try
            {
                var pageAcess = new Dictionary<string, int>();
                var roleAccess = new Dictionary<string, int>();
                char[] delimiterChars = { ';' };

                //DataResult ds = UserControl.SelectAdminPolicy(custid);
                DbResult ds = DataAccess.IBCDLL.Accounting.UserControl.Acc_Con_FileNameSelByCustID(custid);
                if (ds != null)
                {
                    string filenames = "";
                    foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                    {
                        //result.Add(row["pagenames"].ToString().ToLower());
                        filenames = CastAide.AsString(row["filenames"], string.Empty).ToLower();

                        string functionName = CastAide.AsString(row["funcname"], string.Empty).ToLower();
                        int actualPolicy = CastAide.AsInt32(row["actual_policy"], 0);
                        roleAccess.Add(functionName, actualPolicy);

                        if (string.IsNullOrEmpty(filenames))
                            continue;
                        string[] fileList = filenames.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string file in fileList)
                        {
                            var cleanFilePath = file.Trim();
                            if (!cleanFilePath.IndexOf("?").Equals(-1))
                            {
                                cleanFilePath = file.Substring(0, file.IndexOf('?'));
                                cleanFilePath = cleanFilePath.Trim();
                            }

                            if (!pageAcess.ContainsKey(cleanFilePath))
                                pageAcess.Add(cleanFilePath, actualPolicy);
                        }
                    }
                }
                AppSession.PageAccess = pageAcess;
                AppSession.RoleAccess = roleAccess;
                AppSession.ViewIPVN = Permissions.HasPermission(Enumerations.Functions.ViewIPVN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsValidRequestUrl(string requestUrl, string accountingSystem)
        {
            requestUrl = StringUtils.ExtractDomainNameFromURL(requestUrl);

            if (WebSettings.ExternalSite.ContainsKey(requestUrl) && accountingSystem.ToLower().Equals(Enumerations.AccountingSystem.Accounting.ToString().ToLower()))
                return false;
            return true;
        }

        public static bool IsRequestFromExternalSite(string requestUrl)
        {
            requestUrl = StringUtils.ExtractDomainNameFromURL(requestUrl);
            return WebSettings.ExternalSite.ContainsKey(requestUrl);
        }

        public static bool IsValidSystem(string userSystemName)
        {
            if (string.IsNullOrEmpty(userSystemName)) return false;
            return userSystemName.ToLower().Equals(WebSettings.SystemName.ToLower());
        }
    }
}
