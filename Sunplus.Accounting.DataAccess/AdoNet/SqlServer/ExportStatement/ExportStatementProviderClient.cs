using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.ExportStatement;

/// <summary>
/// Summary description for ExportStatementProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.ExportStatement
{
    public class ExportStatementProviderClient : ExportStatementProvider
    {
        public override DbResult AllStatement_SelCheckByUser(string userlist)
        {
            var hash = new Hashtable();
            hash.Add("userlist", userlist);
            return DbHelper.Execute("Acc_AllStatement_SelCheckUsersList", hash, DBType.REPORT, 600);
        }

        public override DbResult AllStatement_UsersList(Hashtable param)
        {
            return DbHelper.Execute("Acc_AllStatement_UsersList", param, DBType.REPORT, 6000);
        }
    }
}