using System.Collections;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Customer;

/// <summary>
/// Summary description for CustomerClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Customer
{
    public class CustomerClient :CustomerProvider
    {
        public override DbResult GetDownLineList(int custid, int pageSize, int pageIndex)
        {
            DbResult rs = null;
            var param = new Hashtable();
            param.Add("custid", custid);
            param.Add("pagesize", pageSize);
            param.Add("pageindex", pageIndex);
            param.Add("totalpages", 0); // 0: Paging
            var spElement = new SpConfig().GetStoreProcedure("CustomerSelDownLineList");
            rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            return rs;
        }

        public override DataRowCollection GetOnLineList(int custid)
        {
            DbResult rs = null;
            var param = new Hashtable();
            param.Add("custid", custid);
            rs = DbHelper.Execute("Acc_CustomerSelOnline", param, DBType.ON_USER);
            return rs.ResultSet.Tables[0].Rows;
        }

        public class SpConfig: DataAccess
        {
            public Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
            {
                return GetStoreProcedure(Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
            }
        }
    }
}