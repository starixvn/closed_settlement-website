using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for LeagueWL
/// </summary>

namespace Sunplus.Accounting.DataAccess.Linq.SqlServer.Report
{
    public class LeagueWLProviderClient : LeagueWLProvider
    {
        #region LeagueWL_Sporttype_Linq

        //public override System.Data.Linq.ISingleResult<LeagueWL_SporttypeResult> LeagueWL_Sporttype(DateTime wldate, ref decimal? winlost_8RM)
        //{
        //    LeagueWL_SporttypeDataContext context = new LeagueWL_SporttypeDataContext( ConString(REPORT)  );
        //    context.CommandTimeout = 600;
        //    return context.Acc_Rpt_LeagueWL_Sporttype(wldate, ref winlost_8RM);
        //}

        #endregion

        #region LeagueWL_Sporttype_Linq14

        //public override System.Data.Linq.ISingleResult<LeagueWL_SporttypeResult> LeagueWL_Sporttype14(DateTime wldate, ref decimal? winlost_8RM)
        //{
        //    LeagueWL_SporttypeDataContext context = new LeagueWL_SporttypeDataContext(ConString(REPORT));
        //    context.CommandTimeout = 600;
        //    return context.Acc_Rpt_LeagueWL_Sporttype14(wldate, ref winlost_8RM);
        //}

        #endregion

        //public override System.Data.Linq.ISingleResult<LeagueWL_DetailsResult> LeagueWL_Details(DateTime? wldate, int? sporttype, int? leagueid, int? bettype, bool? liveindicator, bool? ishedge, ref decimal? winlost_8RM)
        //{
        //    LeagueWL_DetailsDataContext context = new LeagueWL_DetailsDataContext(ConString(REPORT));
        //    context.CommandTimeout = 600;
        //    return context.Acc_Rpt_LeagueWL_Details(wldate, leagueid, sporttype, bettype, liveindicator, ishedge, ref winlost_8RM); 
        //}

        //public override System.Data.Linq.ISingleResult<LeagueWL_DetailsResult> LeagueWL_Details14(DateTime? wldate, int? sporttype, int? leagueid, int? bettype, bool? liveindicator, bool? ishedge, ref decimal? winlost_8RM)
        //{
        //    LeagueWL_DetailsDataContext context = new LeagueWL_DetailsDataContext(ConString(REPORT));
        //    context.CommandTimeout = 600;
        //    return context.Acc_Rpt_LeagueWL_Details14(wldate, leagueid, sporttype, bettype, liveindicator, ishedge, ref winlost_8RM); 
        //}
    }
}