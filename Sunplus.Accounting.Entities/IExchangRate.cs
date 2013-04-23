namespace Sunplus.Accounting.Entities
{
    
    

    /// <summary>
    /// Summary description for IExchangRate
    /// </summary>
    public interface IExchangRate
    {
        int ExchangeId
        { get; set; }

        string Currency
        { get; set; }


        string ActualRate
        { get; set; }

        string MinBet
        { get; set; }

        string MinBetLive
        { get; set; }

        string GBPRate
        { get; set; }

        string AUDRate
        { get; set; }

        string ZARRate
        { get; set; }

        string EURORate
        { get; set; }
        
        string USDRate
        { get; set; }

        bool IsValidData
        { get; set; }
    }
}
