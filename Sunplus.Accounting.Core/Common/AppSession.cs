using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;

using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Summary description for AppSession
    /// </summary>
    public class AppSession
    {
        public static bool IsSyncCSCurrentCust
        {
            get { return CastAide.AsBoolean(HttpContext.Current.Session["IsSyncCSCurrentCust"], false); }
            set { HttpContext.Current.Session["IsSyncCSCurrentCust"] = value; }
        }

        public static bool IsSyncLCSCurrentCust
        {
            get { return CastAide.AsBoolean(HttpContext.Current.Session["IsSyncLCSCurrentCust"], false); }
            set { HttpContext.Current.Session["IsSyncLCSCurrentCust"] = value; }
        }

        public static List<MenuBO> UserMenu
        {
            get
            {
                if (HttpContext.Current.Session["UserMenu"] != null)

                    return (List<MenuBO>) HttpContext.Current.Session["UserMenu"];
                else
                    return null;
            }
            set { HttpContext.Current.Session["UserMenu"] = value; }
        }
        public static List<MenuBO>BAMenu
        {
            get
            {
                if (HttpContext.Current.Session["BAMenu"] != null)

                    return (List<MenuBO>)HttpContext.Current.Session["BAMenu"];
                else
                    return null;
            }
            set { HttpContext.Current.Session["BAMenu"] = value; }
        }
        public static List<MenuBO> ReportMenu
        {
            get
            {
                if (HttpContext.Current.Session["ReportMenu"] != null)

                    return (List<MenuBO>)HttpContext.Current.Session["ReportMenu"];
                else
                    return null;
            }
            set { HttpContext.Current.Session["ReportMenu"] = value; }
        }

        public static bool IsSyncBingoCurrentCust
        {
            get { return CastAide.AsBoolean(HttpContext.Current.Session["IsSyncBingoCurrentCust"], false); }
            set { HttpContext.Current.Session["IsSyncBingoCurrentCust"] = value; }

        }
        public static int CustID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["custid"], 0); }
            set { HttpContext.Current.Session["custid"] = value; }
        }

        /// <summary>
        /// Admin RoleId
        /// </summary>
        public static int RoleID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["roleId"], 0); }
            set { HttpContext.Current.Session["roleId"] = value; }
        }


        public static int AccId
        {
            get
            {
                try
                {
                    return CastAide.AsInt32(HttpContext.Current.Session["AccId"], 0);
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set { HttpContext.Current.Session["AccId"] = value; }
        }

        


        public static int OnAccId
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["OnAccId"], 0); }
            set { HttpContext.Current.Session["OnAccId"] = value; }
        }

        public static string Username
        {
            get { return CastAide.AsString(HttpContext.Current.Session["Username"]); }
            set { HttpContext.Current.Session["Username"] = value; }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get { return (Dictionary<string, int>) HttpContext.Current.Session["RoleAccess"]; }
            set { HttpContext.Current.Session["RoleAccess"] = value; }
        }

        public static DataTable RoleAccessDL
        {
            get { return (DataTable) HttpContext.Current.Session["RoleAccessDL"]; }
            set { HttpContext.Current.Session["RoleAccessDL"] = value; }
        }

        public static string AccPwd
        {
            get { return CastAide.AsString(HttpContext.Current.Session["AccPwd"]); }
            set { HttpContext.Current.Session["AccPwd"] = value; }
        }

        public static Dictionary<string, int> PageAccess
        {
            get { return (Dictionary<string, int>) HttpContext.Current.Session["PageAccess"]; }
            set { HttpContext.Current.Session["PageAccess"] = value; }
        }

        public static int MemberID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["MemberID"]); }
            set { HttpContext.Current.Session["MemberID"] = value; }
        }

        public static int AgentID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["AgentID"]); }
            set { HttpContext.Current.Session["AgentID"] = value; }
        }

        public static int MasterID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["MasterID"]); }
            set { HttpContext.Current.Session["MasterID"] = value; }
        }

        public static int SuperID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["SuperID"]); }
            set { HttpContext.Current.Session["SuperID"] = value; }
        }

        public static string MemberUserName
        {
            get { return CastAide.AsString(HttpContext.Current.Session["MemberUserName"]); }
            set { HttpContext.Current.Session["MemberUserName"] = value; }
        }

        public static string AgentUserName
        {
            get { return CastAide.AsString(HttpContext.Current.Session["AgentUserName"]); }
            set { HttpContext.Current.Session["AgentUserName"] = value; }
        }

        public static string MasterUserName
        {
            get { return CastAide.AsString(HttpContext.Current.Session["MasterUserName"]); }
            set { HttpContext.Current.Session["MasterUserName"] = value; }
        }

        public static string SusperUserName
        {
            get { return CastAide.AsString(HttpContext.Current.Session["SusperUserName"]); }
            set { HttpContext.Current.Session["SusperUserName"] = value; }
        }
        public static object BalanceUpDay
        {
            get
            {
                return HttpContext.Current.Session["BalanceUpDay"];
            }
            set { HttpContext.Current.Session["BalanceUpDay"] = value; }
        }

        public static string CustUName
        {
            get { return CastAide.AsString(HttpContext.Current.Session["CustUname"]); }
            set { HttpContext.Current.Session["CustUname"] = value; }
        }

        public static int CustRoleId
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["CustRoleId"], 0); }
            set { HttpContext.Current.Session["CustRoleId"] = value; }
        }

        public static string AccUserName
        {
            get { return CastAide.AsString(HttpContext.Current.Session["AccUserName"], string.Empty); }
            set { HttpContext.Current.Session["AccUserName"] = value; }
        }

        /// <summary>
        /// credit term group feed
        /// </summary>
        public static int GroupID
        {
            get { return CastAide.AsInt32(HttpContext.Current.Session["GroupID"]); }
            set { HttpContext.Current.Session["GroupID"] = value; }
        }
        public static bool ViewIPVN
        {
            get { return CastAide.AsBoolean(HttpContext.Current.Session["ViewIPVN"], false); }
            set { HttpContext.Current.Session["ViewIPVN"] = value; }
        }

        public static bool IsInternal
        {
            get { return CastAide.AsBoolean(HttpContext.Current.Session["IsInternal"]); }
            set { HttpContext.Current.Session["IsInternal"] = value; }
        }

        /// <summary>
        /// Check status is open or close (custinfo.closed)
        /// </summary>
        public static bool IsClosed
        {
            get { return CastAide.AsBoolean(HttpContext.Current.Session["IsClosed"]); }
            set { HttpContext.Current.Session["IsClosed"] = value; }
        }

        public static Enumerations.AccountingSystem CurrentSystemNAME
        {
            get
            {
                Enumerations.AccountingSystem accountingSystem = Enumerations.AccountingSystem.Accounting;
                try
                {
                    accountingSystem =
                        (Enumerations.AccountingSystem) HttpContext.Current.Session["CurrentSystem"];
                    return accountingSystem;
                }
                catch
                {
                }
                return accountingSystem;
            }
            set { HttpContext.Current.Session["CurrentSystem"] = value; }
        }

        public static string CurrentSystemName
        {
            get { return Enumerations.GetSystemName(SYSTEM); }
            set { HttpContext.Current.Session["CurrentSystemName"] = value; }
        }

        public static string SYSTEM
        {
            get
            {
                try
                {
                    return CastAide.AsString(HttpContext.Current.Session["CurrentSystemName"]);
                }
                catch (Exception)
                {

                    return string.Empty;
                }
                
            }
            set { HttpContext.Current.Session["CurrentSystemName"] = value; }
        }
       
        
        public static object GetSession(HttpSessionState sessionState, string key)
        {

            return sessionState[key];

        }

        public static bool IsFinishUMTime
        {
            get { return DNAide.Core.CastAide.AsBoolean(HttpContext.Current.Session["IsFinishUMTime"] ); }
            set { HttpContext.Current.Session["IsFinishUMTime"] = value; }
        }
       

        public static void AddSession(HttpSessionState sessionState, string key, object value)
        {
            sessionState[key] = value;
        }
    }
}
