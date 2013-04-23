using System;
using Norm;

namespace Sunplus.Accounting.DataAbstract.Entity
{
    public class League
    {
        


        [MongoIdentifier]
        public string Leagueid { get; set; }

        public string Leaguename { get; set; }

        public string Leaguenamec { get; set; }

        public string Leaguenametc { get; set; }

        public string Leaguenamej { get; set; }

        public int Sporttype { get; set; }

        public DateTime Createdate { get; set; }

        public string Leaguenamecn { get; set; }

        public bool IsTest { get; set; }

        public string Leaguenameth { get; set; }

        public string ProgramId { get; set; }

        

        public bool IsActive { get; set; }

        public string Leaguenameko { get; set; }

        public string LeaguenameEs { get; set; }

        public string CountryCode { get; set; }

        public string LeaguenameVn { get; set; }

        public string LeaguenameEl { get; set; }

        
    }
}
