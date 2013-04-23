using System;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface ILicenseDAO
    {
        DbResult SelCurrencyByLicensee(string site);

        DbResult SelReportByLicensee(int LicenseeID, int Currency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest, int BAFilter);

        DbResult SelReportByLicenseeV2(int LicenseeID, int Currency, string FromDate, string ToDate, int ShowSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest);

        DbResult SelBAWinLossByLincensee(int licenseeId, int currency, string fromDate, string toDate, int reportType, string moveBetsDay, int drawfilter, int excludeTest);

        DbResult SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest, int BAFilter);

        DbResult SelReportByLicenseeDetail_AgV2(int LicenseeID, int Currency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest);
        DbResult SelBAWinLossReportByLicenseeAgent(int licenseeId, int currency, string fromDate, string toDate, string moveBetsDay, int drawfilter, int excludeTest);
        
        DbResult SelReportByLicenseeDetail(int LicenseeID, int Currency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest, int BAFilter);

        DbResult SelReportByLicenseeDetailV2(int LicenseeID, int Currency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest);
        DbResult SelBAWinLossByLicenseeDetail(int licenseeId, int currency, int custId, string fromDate, string toDate, string moveBetsDay, int drawfilter, int excludeTest);

        DbResult SelBetlistByLicensee(int CustId, string FromDate, string ToDate, string MoveBetsDay, string BalanceUpDay, bool ShowSB, bool ShowRB, bool ShowNG, int drawFilter, int baFilter);

        DbResult SelBetlistByLicenseeV2(int CustId, string FromDate, string ToDate, string MoveBetsDay, string BalanceUpDay, bool ShowSB, bool ShowRB, bool ShowNG, int ShowBA, int drawFilter);
        DbResult SelBABetListByLicensee(int custId, string fromDate, string toDate, string moveBetsDay, string balanceUpDay, int drawFilter);


        DataTable SelConcurrentUser(DateTime fromDate, DateTime toDate, string licenseSite);

        DataTable GetLicenseeList();

        DataTable SelLicenseeByReseller();

        DataTable SelTotalTicket(int resellerid, int licenseeid, string date, string MoveBetsDay, string BalanceUpDay, bool showSB, bool showNG, bool showRB);

        DataTable SelNegativeBalanceMember(string site);

        DataTable GetRates(string date);
    }
}