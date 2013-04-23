namespace Fanex.Tarus.DataAccess.Implement
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using DNAide.Core;

    public class BaseAccess
    {
        public Dictionary<int, string> GetExchange()
        {
            var exchange = new Dictionary<int, string>();

            //get data from database
            using (ObjectDb objDB = new ObjectDb("ExchangeCurrencySel"))
            {
                DataSet ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select();
                    foreach (DataRow row in dr)
                    {
                        exchange.Add(CastAide.AsInt32(row["exchangeid"]), row["currency"].ToString());
                    }
                }
            }
            return exchange;

        }
    }
}
