using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities
{
    [DataContract]
    public class NumbergameResult
    {
        [DataMember]
        public string KickOffTime
        {
            get;
            set;
        }
        [DataMember]
        public string Match
        {
            get;
            set;
        }
        [DataMember]
        public string Result
        {
            get;
            set;
        }
        [DataMember]
        public string FirstBall
        {
            get;
            set;
        }
        [DataMember]
        public string LastBall
        {
            get;
            set;
        }
        [DataMember]
        public string OUFT
        {
            get;
            set;
        }
        [DataMember]
        public string OEFT
        {
            get;
            set;
        }

        [DataMember]
        public string LastLiveBall
        {
            get;
            set;
        }

        [DataMember]
        public string Warrior
        {
            get;
            set;
        }

        [DataMember]
        public string Status
        {
            get;
            set;
        }
    }
}
