using System;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface IOthersDAO
    {
        DbResult GetMenuList(string system);

        /// <summary>
        /// Get Mixparlay Won Data
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        DbResult GetMixParlayWonList(bool isSoccer, DateTime date);

        DbResult CombinationGetMixParlayWonList(bool isSoccer, DateTime date);

        DbResult GetCheckBet(DateTime winlostDate);
    }
}
