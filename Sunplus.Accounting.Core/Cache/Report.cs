using System;
using DNAide.Core;
using SharedCache.WinServiceCommon.Provider.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;


namespace Sunplus.Accounting.Core.Cache
{
    /// <summary>
    /// Summary description for Report
    /// </summary>

    public class ReportCache
    {
        public static string REPORT_TRADER
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("acctrader." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("acctrader." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }

        public static string REPORT_DIRECT_BAL
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accdirectbal." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accdirectbal." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }

        public static string REPORT_2A_MATCHWL
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accreport2amatchwl." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                
                IndexusDistributionCache.SharedCache.Add("accreport2amatchwl." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_TRADER_DETAIL
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("acctraderdetail." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("acctraderdetail." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }

        public static string REPORT_SUPER_MOVEMENT_BASE
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accsupermovementbase." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accsupermovementbase." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_SUPER_MOVEMENT_ORIGINAL
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accsupermovementoriginal." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accsupermovementoriginal." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT_COMPANY
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accreportcomstatement." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accreportcomstatement." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accreportstatement." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accreportstatement." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT_DETAIL
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accreportstatementdetail." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accreportstatementdetail." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
        public static string REPORT_STATEMENT_AUDIT
        {
            get
            {
                try
                {
                    return CastAide.AsString(IndexusDistributionCache.SharedCache.Get("accreportstatementaudit." + AppSession.Username));
                }
                catch (Exception ex)
                {
                    
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                    return string.Empty;
                }
            }
            set
            {
                IndexusDistributionCache.SharedCache.Add("accreportstatementaudit." + AppSession.Username, value, DateTime.Now.AddMinutes(WebSettings.CacheExpire));
            }
        }
    }
}
