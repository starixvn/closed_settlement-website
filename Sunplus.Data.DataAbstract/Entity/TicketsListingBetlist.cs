using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Norm.Attributes;
using DNAide.Utils;
using DNAide.Core;

namespace Sunplus.Data.DataAbstract.Entity
{
    [Serializable]
    [DataContract]
    public class TicketsListingBetlistDTO
    {
        [DataMember]
        public List<TicketsListingBetlist> Items { get; set; }

        [DataMember]
        public int totalItem { get; set; }
    }

    [Serializable]
    [DataContract]
    public class TicketsListingBetlist : BetListDetail
    {
        #region Choice

        protected Bettran _mbettran;

        [MongoIgnore, DataMember]
        public Bettran MBettranDetail
        {
            get { return _mbettran; }
            set { _mbettran = value; }
        }

        #region OddsM
        private string _oddsM;
        [MongoIgnore, DataMember]
        public string oddsM
        {
            get
            {
                return FormatHandler.FormatNumber4(CastAide.AsDecimal(_mbettran.Modds), "en-EN");
            }
            set { _oddsM = value; }

        }
        #endregion

        private SportEventDTO _sportEventDTO;

        [DataMember]
        public override SportEventDTO SportEvent
        {
            get
            {
                var sportEvent = new TicketsListingSportEvent(_bettran);
                return sportEvent.GetSportEvent();
            }
            set { _sportEventDTO = value; }
        }

        private SportEventDTO _msportEventDTO;

        [DataMember]
        public SportEventDTO MSportEvent
        {
            get
            {
                var msportEvent = new TicketsListingSportEvent(_mbettran);
                return msportEvent.GetSportEvent();
            }
            set { _msportEventDTO = value; }
        }


        #endregion
    }
}
