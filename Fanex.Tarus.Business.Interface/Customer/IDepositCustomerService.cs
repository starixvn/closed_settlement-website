using Fanex.Tarus.Entities.Customer.DepositCustomer;
using Sunplus.Accounting.Entities;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.BetAggregator;

namespace Fanex.Tarus.Business.Interface.Customer
{
    public interface IDepositCustomerService
    {
        Result NewLicenseeAddNewSuper(NewLicenseePostParam newLicenseePostParam);
        Result NewLicenseeAddNewMaster(NewLicenseePostParam newLicenseePostParam);
        Result NewLicenseeAddNewAgent(NewLicenseePostParam newLicenseePostParam);
        Result NewLicenseeAddNewMember(NewLicenseePostParam newLicenseePostParam);

        string GetSuperSetting(int custId);
        string GetMasterSetting(int custId);
        string GetAgentSetting(int custId);
        string GetMemberSetting(int custId);

        Result UpdateSuperSetting(NewLicenseePostParam newLicenseePostParam);
        Result UpdateMasterSetting(NewLicenseePostParam newLicenseePostParam);
        Result UpdateAgentSetting(NewLicenseePostParam newLicenseePostParam);
        Result UpdateMemberSetting(NewLicenseePostParam newLicenseePostParam);

        List<LicenseeConfig> SelAllLicensee();
    }
}
