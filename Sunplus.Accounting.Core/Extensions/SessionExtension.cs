
using System.Web.SessionState;

namespace Sunplus.Accounting.Core.Extensions
{
    public static class SessionExtension
    {
        public delegate object UponCreate();
        public static T GetSessionClass<T>(this HttpSessionState session,
           string key, UponCreate uponCreate) where T : class
        {
            if (null == session[key])
            {
                var item = uponCreate() as T;
                session[key] = item;
                return item;
            }
            return session[key] as T;
        }

        public static T GetSessionValue<T>(this HttpSessionState session, string key, 
            UponCreate uponCreate) where T : struct
        {
            if (null == session[key])
            {
                var item = uponCreate();
                session[key] = item;
                return (T)item;
            }
            return (T)session[key];
        }

        public static object GetSession (HttpSessionState sessionState, string key)
        {
           
              return sessionState[key];   
           
        }

        public static void AddSession(HttpSessionState sessionState, string key, object value)
        {
            sessionState[key] = value;
        }

    }
}
