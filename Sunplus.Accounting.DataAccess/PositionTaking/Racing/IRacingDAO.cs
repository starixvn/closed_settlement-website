using System.Collections;
using System.Data;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.PositionTaking.Racing
{
    public interface IRacingDAO
    {
        DataTable GetRacingStatus(int custId);

        DataTable GetRacingSetting(int custId, int roleId);

        Result UpdateRacingSetting(Hashtable setting, int roleId);

        Result UpdateRacingStatus(Hashtable settingParams);
    }
}