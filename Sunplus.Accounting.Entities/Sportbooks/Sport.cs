using System.Runtime.Serialization;
using Norm;

namespace Sunplus.Accounting.DataAbstract.Entity
{
    [DataContract]
    public class Sport
    {
        [MongoIdentifier]
        public string SportType { get; set; }

        public string SportName { get; set; }

        public string SportNameCh { get; set; }

        public string SportNameCs { get; set; }

        public string SportNameJp { get; set; }

        public string SportNameIt { get; set; }

        public string SportNameTh { get; set; }

        public int ParlayMaxPayout { get; set; }

        public string SportNameKo { get; set; }

        public string SportNameEs { get; set; }

        public string SportNameVn { get; set; }

        public string SportNameEl { get; set; }
    }
}
