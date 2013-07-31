<%@ Import Namespace="System.Diagnostics"%>
<%@ Import Namespace="LiveCasinoLibrary.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Business" %>
<%@ Import Namespace="Sunplus.Accounting.Business.BootStrappers" %>
<%@ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Logging" %>
<%@ Application Language="C#" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Security.Principal"%>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Core.LoggingLib" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Microsoft.Practices.Unity" %>
<%@ Import Namespace="Microsoft.Practices.Unity.Configuration" %>
<%@ Import Namespace = "LVCasinoSetting" %>


<%@ Import Namespace="Fanex.Tarus.Web.Controllers.Areas" %>
<%@ Import Namespace="Fanex.Tarus.Web.Controllers.Filters" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        
        // Code that runs on application startup    
        BootStrapperManager.Initialize(new UnityBootStrapper());
        //For Data Replication System
        //Sunplus.Data.DataAbstract.BootStrappers.BootStrapperManager.Initialize(new Sunplus.Data.DataAbstract.BootStrappers.UnityBootStrapper());
        
        var logger = new CasinoAPILogger();
        var manager = new Sunplus.Libraries.APILogger.APILogManager(logger);
        string pattern = "AutoInsert*|Update*|Insert*|Move*|Reset*|FullSettlementResetMaxTransfer|Rollback*";
        manager.Register(typeof(CasinoLibrary.SBAgent.Customer), pattern);
        manager.Register(typeof(CasinoLibrary.SBAgent.BasicInfo), pattern);

        
        Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
       
        RegisterGlobalFilters(GlobalFilters.Filters);
        
        RegisterRoutes(RouteTable.Routes);
        // ViewEngines.Engines.Add(new CustomView());
        AreaRegistration.RegisterAllAreas();
                
        SettingMgr.GetInstance().Initialize(SiteEnum.AgentSite);
    }


    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {

        //filters.Add(new LogExceptionFilter());
        filters.Add(new HandleErrorAttribute());

    }

    static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
        routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
        routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
        //routes.IgnoreRoute("{*staticfile}", new { staticfile = @".*\.(css|js|gif|jpg|zip)(/.*)?" });
        routes.MapRoute(
            "Default",
            // Route name
            "{controller}/{action}/{id}",
            // URL with parameters
            new { controller = "Home", action = "Index", id = "" } // Parameter defaults
            );

        routes.CreateArea(
            "accounting",
            "Fanex.Tarus.Web.Controllers",
            routes.MapRoute(
                "accmvc-defaultroute",
                "{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = "" }));

        routes.IgnoreRoute("defaultwebpage", new { action = "Index", controller = "accounting"});

    }


    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        //Phuong.Nguyen, handler system error
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError().GetBaseException();
        //Lewin
        //(new Sunplus.Core.LoggingLib.EntLibLogger()).Error(ex.Message, ex);

        if (ex.GetType() != typeof(HttpException))
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        

        #region MyRegion
        //string message = ex.Message +
        //                "\n SOURCE: " + ex.Source +
        //                "\n FORM: " + Request.Form.ToString() +
        //                "\n QUERYSTRING: " + Request.QueryString.ToString() +
        //                "\n TARGETSITE: " + ex.TargetSite +
        //                "\n STACKTRACE: " + ex.StackTrace;

        //SendMailBugSystem.NotificationBugSystem(message);
        //Logger.Write(message);
        //END 
        #endregion
       
    }   
    
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Application.Lock();
        if (Application["UserOnlineNo"] == null)
            Application["UserOnlineNo"] = 1;
        else
            Application["UserOnlineNo"] = ((int)Application["UserOnlineNo"]) + 1;
        Application.UnLock();
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Application.Lock();
        if (Application["UserOnlineNo"] != null)
            Application["UserOnlineNo"] = (int)Application["UserOnlineNo"] - 1;
        Application.UnLock();
        Response.Redirect("~/SignIn.aspx");
        
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        
        string url = Request.RawUrl.ToLower();
       
        var isDowntime = ConfigurationManager.AppSettings["UM"] == "1";
        try
        {
            if (isDowntime && url.Contains(".aspx"))
            {
                var downTimeInfo = new Fanex.Tarus.Business.Implement.CommonService().GetDowntimeInfo();
                if (downTimeInfo.IsDowntime)
                {
                    if (!url.Contains(Constants.Page.UMPage.ToLower().Replace("~", "")))
                        Response.Redirect(Constants.Page.UMPage);
                }
                else
                {

                    if (!url.Contains(Constants.Page.SignIn.ToLower().Replace("~", "")) && !Request.IsAuthenticated)
                    {
                        Response.Redirect(Constants.Page.SignIn);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

    }
    
 

    private void OnRemoveRolePermissionCache(String k, Object v, CacheItemRemovedReason r)
    {
        //InitRolePermission();
    }
    
    

   
    #region Kill session and write log
    public void InsertSessionCacheItem(string onUserId, string username, string roleId, string serverId)
    {
        int mnSessionSeconds = 60;
        string msSessionCacheName = onUserId + "_" + username + "_" + roleId;

        if (!string.IsNullOrEmpty(this.GetSessionCacheItem(onUserId, username, roleId)))
        {
            return;
        }
        CacheItemRemovedCallback oRemove = new CacheItemRemovedCallback(this.KickOut);
        System.Web.HttpContext.Current.Cache.Insert(msSessionCacheName, serverId, null, DateTime.MaxValue, TimeSpan.FromSeconds(mnSessionSeconds), CacheItemPriority.High, oRemove);
    }
    public void RemoveSessionCacheItem(string onUserId, string username, string roleId)
    {
        string msSessionCacheName = onUserId + "_" + username + "_" + roleId;
        if (!string.IsNullOrEmpty(this.GetSessionCacheItem(onUserId, username, roleId)))
        {
            System.Web.HttpContext.Current.Cache.Remove(msSessionCacheName);
        }
    }
    public string GetSessionCacheItem(string onUserId, string username, string roleId)
    {
        string msSessionCacheName = onUserId + "_" + username + "_" + roleId;
        string sRet = string.Empty;

        sRet = (string)System.Web.HttpContext.Current.Cache[msSessionCacheName];
        if (string.IsNullOrEmpty(sRet))
        {
            sRet = string.Empty;
        }

        return sRet;
    }
    public void KickOut(string key, object val, CacheItemRemovedReason r)
    {
        if (CacheItemRemovedReason.Expired == r)
        {
            string[] temp = key.Split('_');
            int onUserId = Convert.ToInt32(temp[0]);
            string username = Convert.ToString(temp[1]);
            byte roleId = Convert.ToByte(temp[2]);
            Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLogOut(onUserId, username, Sunplus.Accounting.Core.Configurations.WebSettings.ServerId);
        }
    }
    #endregion
    //Temporarily deny U3 account from access un-limited pages
    bool U3Limit(string url)
    {
        string UnLimitPage=";Others/Default.aspx;Settle/Index.aspx;Customer/default.aspx";
        UnLimitPage=UnLimitPage.ToLower();
        return (AppSession.Username.IndexOf("U3") == 0 && UnLimitPage.Contains(";" + url.ToLower()));
    }
</script>
