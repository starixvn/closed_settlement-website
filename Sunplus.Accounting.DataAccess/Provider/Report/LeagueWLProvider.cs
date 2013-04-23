using System;
using System.Collections.Generic;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Entity.SqlServer.Report;

/// <summary>
/// Summary description for Report2aProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class LeagueWLProvider : DataAccess
    {
        /// <summary>
        /// DAO Client: AdoNet;
        /// Data Source: SqlServer
        /// </summary>
        /// <returns></returns>
        public static LeagueWLProvider Instance()
        {
            return
                (LeagueWLProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.LeagueWLProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        /// <summary>
        /// Manual DAO Client, Data Source
        /// </summary>
        /// <param name="client"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static LeagueWLProvider Instance(DAOClient client, DataSource source)
        {
            return
                (LeagueWLProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.LeagueWLProviderType, GetDAOClient(client),
                                               GetDataSource(source))));
        }


        public virtual DbResult LeagueWL_Sporttype(DateTime wldate)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult LeagueWL_Sporttype14(DateTime wldate)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult LeagueWL_Bettype(DateTime wldate, int sporttype, int leagueid)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult LeagueWL_Bettype14(DateTime wldate, int sporttype, int leagueid)
        {
            throw new NotImplementedException();
        }

        //public virtual ISingleResult<LeagueWL_SporttypeResult> LeagueWL_Sporttype(DateTime wldate, ref decimal? winlost_8RM)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual ISingleResult<LeagueWL_SporttypeResult> LeagueWL_Sporttype14(DateTime wldate, ref decimal? winlost_8RM)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual List<LeagueSportWLDetails> LeagueSportWL(string winlostdate, ref double? winlost_8rm, string db)
        {
            throw new NotImplementedException();
        }

        //public virtual ISingleResult<LeagueWL_SporttypeResult> LeagueWL_Sporttype14(DateTime wldate, string db, ref decimal? winlost_8RM)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual DbResult LeagueWL_Details(DateTime wldate, int sporttype, int leagueid, int bettype,
                                                 bool liveindicator, bool ishedge)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult LeagueWL_Details14(DateTime wldate, int sporttype, int leagueid, int bettype,
                                                   bool liveindicator, bool ishedge)
        {
            throw new NotImplementedException();
        }

        //public virtual ISingleResult<LeagueWL_DetailsResult > LeagueWL_Details(DateTime? wldate, int? sporttype, int? leagueid, int? bettype, bool? liveindicator, bool? ishedge, ref decimal? winlost_8RM)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual ISingleResult<LeagueWL_DetailsResult> LeagueWL_Details14(DateTime? wldate, int? sporttype, int? leagueid, int? bettype, bool? liveindicator, bool? ishedge, ref decimal? winlost_8RM)
        //{
        //    throw new NotImplementedException();
        //}
    }
}