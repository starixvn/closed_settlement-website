using DNAide.Data;
using System.Collections;

namespace Sunplus.Accounting.DataAccess.PositionTaking
{
    public interface IPokerDAO
    {
        DbResult GetCustomerStatus(int custId);
        DbResult GetCustomerSetting(int custId, int roleId);
        DbResult UpdateP2P(Hashtable Params, int type);
        DbResult UpdateP2PStaus(Hashtable Params);

        DbResult GetDefaultSetting();
        DbResult SelectEnableCustomer(int custId);
        DbResult UpdateDefaultSettingP2P(Hashtable Params);
    }
}
