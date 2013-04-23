using Norm;

namespace Sunplus.Accounting.DataAbstract.Entity
{
    public class Team
    {
        [MongoIdentifier]
        public string Teamid { get; set; }

        public string Teamname { get; set; }

        public string Teamnamec { get; set; }

        public string Teamdesc { get; set; }

        public int Creator { get; set; }

        public string Teamnamesc { get; set; }

        public string TeamnamecUni { get; set; }

        public string Teamnamej { get; set; }

        public string Teamnamecn { get; set; }

        public string Teamnameth { get; set; }

        public string Teamnameko { get; set; }

        public string TeamnameEs { get; set; }

        public string TeamnameVn { get; set; }

        public byte SportType { get; set; }

        public bool IsActive { get; set; }

        public string TeamnameEl { get; set; }
    }
}
