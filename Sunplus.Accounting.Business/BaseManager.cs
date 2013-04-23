using System;
using System.Web;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.CachingLib;
using SCCACHE = SharedCache.WinServiceCommon.Provider.Cache.IndexusDistributionCache;
/// <summary>
/// Summary description for BaseManager
/// </summary>
namespace Sunplus.Accounting.Business
{
    public class BaseManager
    {
        public BaseManager()
        {
            SessionTimeoutHandle();
        }

        public static string CurrentUserIp
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }

        private void SessionTimeoutHandle()
        {
            if (AppSession.AccId == null)
            {
                HttpContext.Current.Response.Redirect("~/SignIn.aspx");
            }
        }

        protected virtual void CacheData(string key, object value, DateTime expire)
        {
            SCCACHE.SharedCache.Add(key, value, expire);
        }

        protected virtual void CacheData(string key, object value, int minutesExpire)
        {
            long expire = minutesExpire*60*1000;
            CacheService.CreateInstance(CacheType.ShareCached).Add(key, value, expire);
        }

        protected virtual string GetCachedData(string key)
        {
            try
            {
                return CastAide.AsString(SCCACHE.SharedCache.Get(key));
            }
            catch (Exception ex)
            {
                //Logs.Write(ex.Message);

                return null;
            }
        }

        protected virtual object GetCachedObjData(string key)
        {
            try
            {
                //return SCCACHE.SharedCache.Get(key);
                return CacheService.CreateInstance(CacheType.ShareCached).Get(key);
            }
            catch (Exception ex)
            {
                //Logs.Write(ex.Message);
                return null;
            }
        }
    }
}
