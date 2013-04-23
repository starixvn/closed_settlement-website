using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities.Report.NumberGame;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class NumberGameReportDAO: DataAccess, INumberGameReportDAO
    {
        private enum ReportName
        {
            MonthlyWinloss, WinlossByDate, WinlossByMatch
        }

        private StoreProcedureElement GetStore(ReportName reportName)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Gaming,
                                 reportName.ToString());
        }

        public MonthlyWinloss MonthlyWinloss(int month, int year)
        {
            var spElement = GetStore(ReportName.MonthlyWinloss);

            var monthlyWinloss = new MonthlyWinloss();
           
            var db = GetDatabase(spElement.DbName);
            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "month", DbType.Int32, month);
                db.AddInParameter(dbCommand, "year", DbType.Int32, year);
                dbCommand.CommandTimeout = 180;
                var reader = db.ExecuteReader(dbCommand);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        monthlyWinloss = new MonthlyWinloss()
                        {
                            Winlost = CastAide.AsDouble(NumberFormat.DecFormat(reader["Winlost"], 2)),
                            Position = CastAide.AsDouble(NumberFormat.DecFormat(reader["Position"],2)),
                            Commission = CastAide.AsDouble(NumberFormat.DecFormat(reader["Commission"],2)),
                            LineTotal = CastAide.AsDouble(NumberFormat.DecFormat(reader["LineTotal"],2)),
                            TurnOver = CastAide.AsDouble(NumberFormat.DecFormat(reader["TurnOver"],2)),
                            Margin = CastAide.AsDouble(NumberFormat.DecFormat(reader["Margin"],2)),
                        };
                        
                    }
                }
            }
            return monthlyWinloss;
        }
    }
}
