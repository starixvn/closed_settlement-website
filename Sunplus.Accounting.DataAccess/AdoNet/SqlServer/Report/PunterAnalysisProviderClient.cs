using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class PunterAnalysisProviderClient : PunterAnalysisProvider
    {
        public override DbResult SelTopPunterSummary(Hashtable param)
        {
            try
            {
                return DbHelper.Execute("Acc_SelTopPunterSummary", param, DBType.PUNTER, 3600);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            return null;
        }

        public override DbResult SelTopPunterByCustId(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelTopPunterByCustID", param, DBType.PUNTER, 3600);
        }

        public override DbResult SelTopPunterBySporttype(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelTopPunterBySporttype", param, DBType.PUNTER, 3600);
        }

        public override DbResult SelTopPunterByBettype(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelTopPunterByBettype", param, DBType.PUNTER, 3600);
        }

        public override DbResult SelIPByCustomer(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelIPByCustomer", param, DBType.PUNTER, 3600);
        }

        public override DbResult SelBetCountByCustomer(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelBetCountByCustomer", param, DBType.PUNTER, 3600);
        }

        public override DbResult DirectCustomerReport(Hashtable param)
        {
            return DbHelper.Execute("Acc_DirectCustomerReport", param, DBType.PUNTER, 3600);
        }

        public override DbResult SelSports()
        {
            return DbHelper.Execute("Acc_TopPunterSelSports", DBType.PUNTER);
        }

        public override DbResult SelLeagues(Hashtable param)
        {
            return DbHelper.Execute("Acc_TopPunterSelLeagues", param, DBType.PUNTER, 600);
        }

        public override DbResult SelTopPunterSummaryByLeague(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelTopPunterSummaryByLeague", param, DBType.PUNTER, 3600);
        }

        public override DbResult SelTopPunterByLeagueCustId(Hashtable param)
        {
            return DbHelper.Execute("Acc_SelTopPunterByLeagueCustID", param, DBType.PUNTER, 3600);
        }
    }
}