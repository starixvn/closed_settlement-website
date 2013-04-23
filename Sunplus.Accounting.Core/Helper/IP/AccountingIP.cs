using System;
using System.Configuration;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.Common.IPs;

//using IBC.Core.Common.IPs;

namespace Sunplus.Accounting.Core.Helper.IP
{
    /// <summary>
    /// IP Handle for Accounting site
    /// Created: Ryan
    /// </summary>
    public class AccountingIPFormat : IPFormat
    {
        public AccountingIPFormat(string ip) : base(ip)
        {
        }

        protected override IContextIPInfo Context
        {
            get
            {
                return new ContextIPInfo
                           {
                               AdminId = 1,
                               IsAdminCanViewVNIP = (WebSettings.IPVNReplace == "1") ? true : AppSession.ViewIPVN,
                               DefaultNoIPVN = (WebSettings.IPVNReplace == "1") ? string.Empty : WebSettings.IPVNReplace
                           };
            }
        }

        protected override string LinkFormat
        {
            get
            {
                return String.Format("<a href='{0}' target='_blank'>{1}</a>",
                                     ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", "{0}"), "{0}");
            }
        }
    }
}