using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.PositionTaking.Racing;

namespace Sunplus.Accounting.DataAccess.AdoNet.PositionTaking.Racing
{
    public class MeetingCommDAO : DataAccess, IMeetingCommDAO
    {
        public DbResult GetPartnerList()
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.MeetingComm, "GetPartnerList");
            return DbHelper.Execute(spElement.Name, null, spElement.DbName, 1000);
        }

        public DbResult GetPartnerDetail(int partnerId)
        {
            var param = new Hashtable { { "partnerid", partnerId } };
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.MeetingComm, "GetPartnerDetail");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
        }
    }
}
