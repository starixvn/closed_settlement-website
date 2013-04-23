using System.Collections.Generic;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.Business
{
    public interface IHelper
    {
        List<SiteDTO> GetSites(Enumerations.AccountingSystem system);

        List<Site> GetSiteLicensee();

        string[] GetStandardCurrencyList();
    }
}