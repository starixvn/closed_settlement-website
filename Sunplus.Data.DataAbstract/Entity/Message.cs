using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.DataAbstract.Entity
{
    [Serializable]
    [DataContract]
    public class Message
    {
       

        #region public properties
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string Date { get; set; }
        #endregion public properties


        

        

      
    }
}
