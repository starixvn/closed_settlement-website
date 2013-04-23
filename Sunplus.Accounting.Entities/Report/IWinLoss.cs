namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for IWinLoss
    /// </summary>
    public interface IWinLoss
    {
        int CustId { get; set; }

        string Username { get; set; }

        double Turnover { get; set; }

        double GrossComm { get; set; }

        double UplineTotal { get; set; }
    }
}