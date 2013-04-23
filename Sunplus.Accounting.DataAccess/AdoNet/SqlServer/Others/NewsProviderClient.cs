using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Others;

/// <summary>
/// Summary description for NewsProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Others
{
    public class NewsProviderClient : NewsProvider
    {
        public override DbResult NewsIns(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsIns", param, DBType.MAINTENANCE, 300);
        }

        public override DbResult NewsUpd(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsUpd", param, DBType.MAINTENANCE, 300);
        }

        public override DbResult NewsDel(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsDel", param, DBType.MAINTENANCE, 300);
        }

        public override DbResult NewsSel(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsSel", param, DBType.MAINTENANCE, 300);
        }

        public override DbResult NewsSelAll(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsSelAll", param, DBType.MAINTENANCE, 300);
        }

        public override DbResult NewsSelFeature(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsSelFeature", param, DBType.MAINTENANCE, 300);
        }

        public override DbResult NewsUpdFeature(Hashtable param)
        {
            return DbHelper.Execute("Acc_NewsUpdFeature", param, DBType.MAINTENANCE, 300);
        }
    }
}