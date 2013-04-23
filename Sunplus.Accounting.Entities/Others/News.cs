using System;
using System.Runtime.Serialization;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.Entities.Others
{
    [Serializable()]
    [DataContract]
    public class NewsItem
    {
        [DataMember]
        public int NewsID { get; set; }
        [DataMember]
        public string NewsContent { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Func { get; set; }
        [DataMember]
        public int AppID { get; set; }
        [DataMember]
        public int Feature { get; set; }
        [DataMember]
        public string Thumbnail { get; set; }
        [DataMember]
        public int OwnerID { get; set; }
        [DataMember]
        public bool HostRole { get; set; }
        [DataMember]
        public string Recipient { get; set; }
    }

    [Serializable()]
    [DataContract]
    public class NewsData
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public List<NewsItem> Rows { get; set; }
        [DataMember]
        public bool HostRole { get; set; }
        [DataMember]
        public string PagerControl { get; set; }
    }
}
