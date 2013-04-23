using System;
using DNAide.Core;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;
using SCCACHE = SharedCache.WinServiceCommon.Provider.Cache.IndexusDistributionCache;
//using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for Report
/// </summary>
namespace Sunplus.Accounting.Business.CacheManager
{
    public class Report
    {
        public static string REPORT_TRADER
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("acctrader." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("acctrader." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_DIRECT_BAL
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accdirectbal." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accdirectbal." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }

        public static string REPORT_2A_MATCHWL
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreport2amatchwl." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreport2amatchwl." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_TRADER_DETAIL
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("acctraderdetail." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("acctraderdetail." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }

        public static string REPORT_SUPER_MOVEMENT_BASE
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accsupermovementbase." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accsupermovementbase." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_SUPER_MOVEMENT_ORIGINAL
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accsupermovementoriginal." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accsupermovementoriginal." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT_COMPANY
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportcomstatement." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportcomstatement." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportstatement." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportstatement." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT_DETAIL
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportstatementdetail." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportstatementdetail." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT_AUDIT
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportstatementaudit." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportstatementaudit." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }

        public static string REPORT_LEAGUE_WINLOSS_SPORT
        {
            get
            {
                try
                {
                    return
                        CastAide.AsString(SCCACHE.SharedCache.Get("accreportleaguewinlosssport." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportleaguewinlosssport." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_M88WINLOSS_DAILY
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportm88winlossdaily." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportm88winlossdaily." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_M88WINLOSS_MONTHLY
        {
            get
            {
                try
                {
                    return
                        CastAide.AsString(SCCACHE.SharedCache.Get("accreportm88winlossmonthly." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportm88winlossmonthly." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_M88WINLOSS_SPORT
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportm88winlosssport." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportm88winlosssport." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_M88WINLOSS_DETAIL
        {
            get
            {
                try
                {
                    return CastAide.AsString(SCCACHE.SharedCache.Get("accreportm88winlossdetail." + AppSession.Username));
                }
                catch (Exception ex)
                {

                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    return string.Empty;
                }
            }
            set
            {
                SCCACHE.SharedCache.Add("accreportm88winlossdetail." + AppSession.Username, value,
                                        DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
    }
}
