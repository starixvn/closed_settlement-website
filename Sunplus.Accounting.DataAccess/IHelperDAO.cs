using System.Data;
using Sunplus.Accounting.Core.Common;
using System.Collections.Generic;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess
{
    public interface IHelperDAO
    {
        IEnumerable<Country> GetCountryList();
        Result AddCountry(string countryName);
        DataTable GetSites(Enumerations.AccountingSystem system);
        DataTable GetCust(Enumerations.AccountingSystem system);
    }
}
