using System;
using System.Data;
using Sunplus.Accounting.Entities;
using System.Collections.Generic;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.IBetAggregatorDAO
{
    public interface ILicenseeConfigDAO
    {
        DataTable SelectAllLicensee();
        DataTable SelectLicenseeConfigList();
        DataTable SelLicenseeConfigCommLog(int licenseeId);
        Result InsertLicenseeConfig(int licenseeId, double hdpComm, double oneXTwoComm, double otherComm, double ngComm, bool enableBa);
        Result UpdateLicenseeConfig(int licenseeId, double groupa, double groupb, double groupc, double groupd, double oneXTwoComm, double otherComm, bool enableBa);
        Result DeleteLicenseeConfig(int licenseeId);
        int CheckDownlineBA(int licenseeId);

        DataTable GetAmtLimitLicensees();
        DbResult BALimitWarning(int licenseeID, string licenseeName, double limitAmt);
        DataTable GetBALicWinlossDaily(System.DateTime wldate);
        DataTable GetBALicWinlossMonthToDate(System.DateTime wldate);
        DataTable GetBALicWinlossYearToDate(System.DateTime wldate);
        DataTable GetBALicWinlossMonthly(System.DateTime wldate);

        DataTable SelBAWinLossByCommLevel(int licenseeId, DateTime fromDate, DateTime toDate, int currencyId, int drawFilter, bool isExcludeTest);
        DbResult SelGroupCurrencyByLicensee(int licenseeId);
        Result UpdateGroupSettingByLicensee(int licenseeId, string setting);
    }
}
