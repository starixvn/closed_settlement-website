using System.Collections;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using System;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface IReportRacingDAO
    {
        DataTable WinlostRacingTote(Enumerations.ReportType reportType, string winlossDate);

        DataTable WinlostRacingToteNew(Enumerations.ReportType reportType, string winlossDate);

        DataTable WinlostRacingTote_Details(string winlossDate, string poolCurrency, string partnerId);

        DataTable WinlostRacingTote_DetailsNew(string winlossDate, string poolCurrency, string partnerId);
        
        DataTable WinlostRacingToteDumpFiles(string winlossdate, int reportType);

        DataTable WinlostRacingToteBetlist(string winlossdate, string leagueid, string raceNo, string currency);

        DataTable WinlostRacingToteBetlistNew(string winlossdate, string leagueid, string raceNo, string currency);

        DataTable DailyWLDirectRacing(string winlossdate);

        DataTable DailyWLDirectRacingBySport(string sporttype, string winlossdate);

        DataTable DailyWLDirectRacingBySportDetails(string winlossdate, string sporttype, string bettype);

        DataTable WinlostRacing_Group(string fromdate, string todate);

        DbResult DailyWLDirect_Sporttype(string wldate);

        DataTable WinlostRacingLicensee_SiteSel();

        DataTable WinlostRacingLicensee_Details(string winlossdate, string site);

        DataTable WinlostRacingLicensee_BetList_Details(string leagueid, string raceNo, string currency, DateTime winlostdate, string site);

        DataTable SelWinlostHR_Details(string wldate);

        DbResult DailyWLDirect_MixParlay(string winlossdate, int bettype);

        DbResult DailyWLDirect_NumberGame(string winlossdate);
    }
}
