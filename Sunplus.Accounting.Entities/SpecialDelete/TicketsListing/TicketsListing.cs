using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.SpecialDelete.TicketsListing
{
    [Serializable]
    [DataContract]
    public class LicenseeDTO
    {
        [DataMember]
        public List<Licensee> Items { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BettypeDTO
    {
        [DataMember]
        public List<Bettype> ItemsSB { get; set; }
        [DataMember]
        public List<Bettype> ItemsNGLive { get; set; }
        [DataMember]
        public List<Bettype> ItemsNGNoneLive { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Bettype
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public int LiveIndicator { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Licensee
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name{ get; set; }
    }

    [Serializable]
    [DataContract]
    public class MatchInfo
    {
        [DataMember]
        public string matchcode { get; set; }

        [DataMember]
        public string home { get; set; }

        [DataMember]
        public string away { get; set; }

        [DataMember]
        public string eventdate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class  ProductListingDTO
    {
        [DataMember]
        public List<ProductListing> Items { get; set; }
        
        [DataMember]
        public MatchInfo MatchInfo { get; set; }
        [DataMember]
        public int ErrCode{get;set;}
        [DataMember]
        public string ErrMsg { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ProductListing
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int LiveIndicator { get; set; }

        [DataMember]
        public int TotalTrans { get; set; }

        [DataMember]
        public List<ProductListing> Items { get; set; } 

    }
}
