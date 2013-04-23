using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.DataAccess.SpecialDelete.TicketsListing
{
    public class TicketsListingDAO: DataAccessReport, ITicketsListingDAO
    {
        private StoreProcedureElement getSPMapping(string key)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.TicketsListing, key);
        }

        public DbResult SelAllLicensee()
        {
            var element = getSPMapping("SelAllLicensee");
            return DbHelper.Execute(element.Name, element.DbName);
        }


        public DbResult SelBettype()
        {
            var element = getSPMapping("SelAllBettype");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("mode", 3);
            return DbHelper.Execute(element.Name, hashtable, element.DbName);
        }

        public DbResult SelTicketsListing(int matchID, int licenseeID, string betTypes, string betTypesLive, int Type, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            var element = getSPMapping("TicketsListing");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchID);
            hashtable.Add("licenseeid", licenseeID);
            hashtable.Add("bettypes", betTypes);
            hashtable.Add("bettypeslive", betTypesLive);
            hashtable.Add("type", Type);
            hashtable.Add("safilter", SAFilter);
            hashtable.Add("errcode", 0);
            hashtable.Add("errmsg","");
            hashtable.Add("livetype", DNAide.Core.CastAide.AsString(LiveType,"0"));
            if (Type == 0) //sport book
            {

                if (HomeScore.Length > 0 && AwayScore.Length > 0)
                {
                    hashtable.Add("livehomescore", HomeScore);
                    hashtable.Add("liveawayscore", AwayScore);
                }
                if (FromTransDate.Length > 0 && ToTransDate.Length > 0)
                {
                    hashtable.Add("fromtransdate", CastAide.AsDateTime(FromTransDate).AddHours(-12));
                    hashtable.Add("totransdate", CastAide.AsDateTime(ToTransDate).AddHours(-12));
                }

            }

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 6000); 
        }
    }
}
