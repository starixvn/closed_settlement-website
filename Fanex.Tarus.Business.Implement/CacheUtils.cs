// -----------------------------------------------------------------------
// <copyright file="CacheUtils.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Business.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using DNAide.Core;
    using Fanex.Tarus.DataAccess.Implement;
    using Sunplus.Accounting.Core.Extensions;
    using System.Linq;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CacheUtils
    {
        public static Dictionary<int, string> Exchange
        {
            get
            {
                Dictionary<int, string> exchange;
                try
                {

                    exchange =
                        SessionExtension.GetSession(HttpContext.Current.Session, "exchangeratedata") as
                        Dictionary<int, string> ??
                        new BaseAccess().GetExchange();
                }
                catch (Exception ex)
                {
                    exchange = new BaseAccess().GetExchange();
                    Exchange = exchange;
                }


                return exchange;
            }
            set { SessionExtension.AddSession(HttpContext.Current.Session, "exchangeratedata", value); }
        }

        public static string GetExchangeName(int id)
        {
            Dictionary<int, string> exchange = Exchange;
            string value = (from k in exchange
                            where k.Key == id
                            select k.Value).FirstOrDefault();
            return CastAide.AsString(value);
        }
        public static string GetExchangeName(Dictionary<int, string> exchange, int id)
        {
            string value = (from k in exchange
                            where k.Key == id
                            select k.Value).FirstOrDefault();
            return CastAide.AsString(value);
        }
    }
}
