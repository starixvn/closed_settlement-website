using System.Web;
using Sunplus.Accounting.Core.Common;
using DNAide.Data;
using System.Collections;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    /// <summary>
    /// Summary description for ExchangeRateDAO
    /// </summary>
    public class ExchangeRateDAO: IExchangeRateDAO
    {
        public ExchangeRateDAO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DbResult ExChangeRateUpdateBaseRate(string currency, string baserate, string minbet, string minbetlive)
        {
            Hashtable hash = new Hashtable();
            hash.Add("userid", AppSession.AccId);
            hash.Add("actualrate", baserate);
            hash.Add("currency", currency.Trim());
            hash.Add("minbet", minbet);
            hash.Add("minbetlive", minbetlive);
            hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
            hash.Add("errcode", 0);
            hash.Add("errmsg", "");

            return DbHelper.Execute("Acc_ExchangeUpdateBaseRate", hash, DBType.MAIN, 300);
        }
        public DbResult ExChangeRateUpdateOpenTote(string currency, string GBPRate, string AUDRate, string ZARRate, string EURORate, string USDRate)
        {
            Hashtable hash = new Hashtable();
            hash.Add("userid", AppSession.AccId);
            hash.Add("currency", currency.Trim());
            if (GBPRate.Length != 0)
                hash.Add("gbprate", GBPRate);
            if (AUDRate.Length != 0)
                hash.Add("audrate", AUDRate);
            if (ZARRate.Length != 0)
                hash.Add("zarrate", ZARRate);
            if (EURORate.Length != 0)
                hash.Add("eurorate", EURORate);
            if (USDRate.Length != 0)
                hash.Add("us$rate", USDRate);
            hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
            hash.Add("errcode", 0);
            hash.Add("errmsg", "");

            return DbHelper.Execute("Acc_ExchangeUpdateOpenToteRate", hash, DBType.MAIN, 300);
        }
    }
}
