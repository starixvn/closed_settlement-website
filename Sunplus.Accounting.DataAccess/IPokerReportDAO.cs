using DNAide.Data;

namespace Sunplus.Accounting.DataAccess
{
    public interface IPokerReportDAO
    {
        DbResult SelWinlostP2P(string FromDate, string ToDate);
        DbResult SelWinlostP2PDetail(int currency, string date);
    }
}
