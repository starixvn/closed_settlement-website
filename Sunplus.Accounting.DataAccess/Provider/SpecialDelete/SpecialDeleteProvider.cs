using System;
using System.Collections;
using System.Collections.Generic;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Entity.SqlServer.SpecialDelete;

/// <summary>
/// Summary description for SpecialDeleteProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.SpecialDelete
{
    public abstract class SpecialDeleteProvider : DataAccess
    {
        /// <summary>
        /// DAO Client: AdoNet;
        /// Data Source: SqlServer
        /// </summary>
        /// <returns></returns>
        public static SpecialDeleteProvider Instance(DAOClient client)
        {
            return
                (SpecialDeleteProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.SpecialDelete.SpecialDeleteProviderType,
                                               GetDAOClient(client), GetDataSource(DataSource.SqlServer))));
        }

        public virtual ArrayList GetSpecialDelete(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual List<MatchInfoRow> MatchSeById(int MatchId)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult BettransSelSpecialDel(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult BettransDelSpecial_OutrightSel(int LeagueID)
        {
            throw new NotImplementedException();
        }


        public virtual DbResult BettransSelDelSpecial_Outright(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult BettransSelBet_BatchMsg(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}