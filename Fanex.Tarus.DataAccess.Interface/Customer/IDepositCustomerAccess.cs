using System.Data;
using Fanex.Tarus.Entities.Customer.DepositCustomer;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.DataAccess.Interface.Customer
{
    public interface IDepositCustomerAccess
    {
        Result NewLicenseeAddNewSuper(NewLicenseePostParam newLicenseePostParam);
        Result NewLicenseeAddNewMaster(NewLicenseePostParam newLicenseePostParam);
        Result NewLicenseeAddNewAgent(NewLicenseePostParam newLicenseePostParam);
        Result NewLicenseeAddNewMember(NewLicenseePostParam newLicenseePostParam);

        DataSet GetSuperSetting(int custId);
        DataSet GetMasterSetting(int custId);
        DataSet GetAgentSetting(int custId);
        DataSet GetMemberSetting(int custId);

        Result UpdateSuperSetting(NewLicenseePostParam newLicenseePostParam);
        Result UpdateMasterSetting(NewLicenseePostParam newLicenseePostParam);
        Result UpdateAgentSetting(NewLicenseePostParam newLicenseePostParam);
        Result UpdateMemberSetting(NewLicenseePostParam newLicenseePostParam);

        DataTable SelAllLicensee();
    }
}
