using System.Runtime.Serialization;
namespace Sunplus.Accounting.Entities
{

    /// <summary>
    /// Summary description for ExchangeRate
    /// </summary>
    /// 
    [DataContract]
    public class ExchangeRate: IExchangRate
    {
        public const int InvalidDataError = 1;
        public const int InvalidCurrencyError = 2;
        public const int DuplicatedCurrencyError = 3;

        #region IExchangRate Members
        public int ExchangeId
        { get; set; }
        [DataMember]
        public string Currency
        { get; set; }
        [DataMember]
        public string ActualRate
        { get; set; }

        public string MinBet
        { get; set; }

        public string MinBetLive
        { get; set; }
        [DataMember]
        public string GBPRate
        { get; set; }
        [DataMember]
        public string AUDRate
        { get; set; }
        [DataMember]
        public string ZARRate
        { get; set; }
        [DataMember]
        public string EURORate
        {get;set;}
        [DataMember]
        public string USDRate
        { get; set; }
        [DataMember]
        public bool IsValidData
        { get; set; }

        [DataMember]
        public ResultBO Result
        {
            get;
            set; 
        }

        

        #endregion
    }
}
