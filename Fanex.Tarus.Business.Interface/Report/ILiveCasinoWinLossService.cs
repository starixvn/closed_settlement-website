
namespace Fanex.Tarus.Business.Interface.Report
{
    using System.Collections.Generic;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Entities.Report;
    using Fanex.Tarus.Entities.Sport;

    public interface ILiveCasinoWinLossService
    {
        LiveCasinoWinLoss GetLiveCasinoWinLoss(LiveCasinoWinLossParams Params);

        GameSearch GetGameFilterInfo();

        List<Category> GetGameTableByGameType(int gametype);

        void ExportLiveCasinoAgencyWinLossReport(LiveCasinoWinLossParams parameters);
    }
}
