using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.SpecialDelete.TicketsListing
{
    public interface ITicketsListingDAO
    {
        DbResult SelAllLicensee();

        DbResult SelBettype();

        DbResult SelTicketsListing(int matchID, int licenseeID, string betTypes, string betTypesLive, int Type, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter);

    }
}
