using Sunplus.Accounting.DataAccess.Provider.Log;

/// <summary>
/// Summary description for LogClient
/// </summary>
namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Log
{
    public class LogClient:LogProvider
    {
        public override System.Data.DataRowCollection GeneralLog(int custId)
        {
            return null;
        }

        public override System.Data.DataRowCollection CreditLog(int custId)
        {
            return base.CreditLog(custId);
        }
    }
}