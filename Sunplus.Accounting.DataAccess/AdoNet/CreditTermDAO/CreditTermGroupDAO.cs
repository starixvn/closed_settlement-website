using System;
using System.Collections.Generic;
using System.Text;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.ICreditTermDAO;

namespace Sunplus.Accounting.DataAccess.AdoNet.CreditTermDAO
{
    public class CreditTermGroupDAO : DataAccess, ICreditTermGroupDAO
    {
        public DbResult GetAllGroup()
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CreditTermSession.StoreCollection.CreditTermControl, "GetAllGroup");
            return DbHelper.Execute(spElement.Name, null, spElement.DbName, 1000);
        }
    }
}
