// -----------------------------------------------------------------------
// <copyright file="ResetWinning.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.DataAccess.Report.ResetWinning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DNAide.Data;
    using Sunplus.Accounting.Core.Configurations;
    using System.Collections;
    using System.Data;
    using DNAide.Core;
    using Sunplus.Accounting.Entities.Report.ResetWinning;
    using Fanex.Data;
    using DNAide.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResetWinningDAO:DataAccessReport, IResetWinningDAO
    {

        private StoreProcedureElement getSPMapping(string key)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.ResetWinning, key);
        }

        public ResetWinningDTO GetDataResetWinning(string date, int data)
        {
            var list = new List<ResetWinning>();
            var result = new ResetWinningDTO();
            string spalias = data == 0 ? "ResetWinningMain" : "ResetWinningRep";
            var datatable = new DataTable();
            double totalBalance = 0;
            double sumWinloss = 0, sumVariance = 0;
            using (ObjectDb objDB = new ObjectDb(spalias))
            {
                objDB.AddInParameter("winlostdate", DbType.DateTime, CastAide.AsDateTime(date));
                objDB.AddOutParameter("totalBalance", DbType.Double, sizeof(double));
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
                totalBalance = CastAide.AsDouble(objDB.GetParameterValue("totalBalance"));
            }
            foreach (DataRow row in datatable.Rows)
            {
                list.Add(new ResetWinning()
                {
                    Products = CastAide.AsString(row["product"]),
                    Winloss = NumberFormat.DecFormat(CastAide.AsString(row["winlost"]),2),
                    Reset = CastAide.AsDouble(row["reset"]) == 0 ? "-" : NumberFormat.DecFormat(CastAide.AsString(row["reset"]),2),
                    Variance = NumberFormat.DecFormat((CastAide.AsDouble(row["winlost"]) + CastAide.AsDouble(row["reset"])).ToString(), 2)
                });
                sumWinloss += CastAide.AsDouble(row["winlost"]);
                sumVariance += CastAide.AsDouble(row["winlost"]) + CastAide.AsDouble(row["reset"]);
            }
            list.Add(new ResetWinning() { 
                Products = "OTHERS",
                Winloss = NumberFormat.DecFormat((totalBalance - sumWinloss).ToString(), 2),
                Reset = "N/A",
                Variance = NumberFormat.DecFormat((totalBalance - sumWinloss).ToString(), 2)
            });
            result.Items = list;
            result.Total = NumberFormat.DecFormat(sumVariance, 0);
            return result;
            
        }

     
    }
}
