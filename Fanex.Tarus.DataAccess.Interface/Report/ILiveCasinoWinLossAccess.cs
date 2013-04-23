namespace Fanex.Tarus.DataAccess.Interface.Report
{
    using System.Data;
    using Fanex.Tarus.Entities.Report;

    public interface ILiveCasinoWinLossAccess
    {        
        DataSet GetLiveCasinoWinLoss(LiveCasinoWinLossParams Params);
        DataTable GetCompanyAllowPlayLiveCasino();
        DataTable GetCurrency();
        DataTable GetGameTableByGameType(int gameType);
        DataTable GetGameType();
    }
}
