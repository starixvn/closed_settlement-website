using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.Provider.ExchangeRate;

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.ExchangeRate
{
    public class ExChangeRateProviderClient : ExChangeRateProvider
    {
        public override DbResult ExchangeSelAccount()
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection.ExchangeRateControl, "EchangeSelAcc");
            return DbHelper.Execute(spElement.Name, spElement.DbName);
        }

        public override DbResult ExchangeIns(Hashtable param)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection.ExchangeRateControl, "EchangeIns");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
        }

        public override DbResult ExchangeSelByID(int exid)
        {
            var hash = new Hashtable {{"exchangeid", exid}};
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection.ExchangeRateControl, "EchangeSelById");
            return DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
        }

        public override DbResult ExchangeUpd(Hashtable param)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection.ExchangeRateControl, "EchangeUpd");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
        }
    }
}