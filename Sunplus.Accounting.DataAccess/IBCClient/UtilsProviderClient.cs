using System;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for UtilsProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.IBCClient
{
    public class UtilsProviderClient : UtilsProvider
    {
        public override List<IPVNBean> GetIPVN()
        {
            var listIPVN = new List<IPVNBean>();
            try
            {
                //get data from database
                DataSet ds = DbHelper.Execute("GetIPVN", DBType.IPDB).ResultSet;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] rows = ds.Tables[0].Select();
                    Array.ForEach(rows,
                                  row =>
                                  listIPVN.Add(new IPVNBean(CastAide.AsDouble(row["IPFrom"]),
                                                            CastAide.AsDouble(row["IPTo"]))));
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            return listIPVN;
        }

        public override Dictionary<int, string> GetExchange()
        {
            var exchange = new Dictionary<int, string>();
            try
            {
                //get data from database
                DataSet ds;
                var spElement =
                    new SpConfig().GetExchangeStoreProcedure(
                        Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection.ExchangeRateControl,
                        "ExchangeCurrencySel");
                ds = DbHelper.Execute(spElement.Name, spElement.DbName).ResultSet;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select();
                    foreach (DataRow row in dr)
                    {
                        exchange.Add(CastAide.AsInt32(row["exchangeid"]), row["currency"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return exchange;
            ;
        }

        private class SpConfig: DataAccess
        {
            public StoreProcedureElement GetExchangeStoreProcedure(Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}