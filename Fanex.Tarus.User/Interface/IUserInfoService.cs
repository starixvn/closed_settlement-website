using System.Collections.Generic;
using Fanex.Tarus.Entities;
using Fanex.Tarus.User.Entities;
using Sunplus.Accounting.Entities;
using UserControlBO = Fanex.Tarus.User.Entities.UserControlBO;
namespace Fanex.Tarus.User.Business.Interface
{
    public interface IUserInfoService
    {
        List<Department> GetAllDept();
        List<Account> GetAllAccount();
        List<UserControlBO> GetGroupPolicyList(int groupId);
        Result UpdateGroupPolicy(string groupPolicies, string oldGroupPolicies, int groupId);
        Result UpdateDept(int id, string name, string description);
        Result DeleteDept(int id);
        Result AddDept(string name, string description);
        Result AddAccount(string userName, string firstName, string lastName, int groupId, string password);
        Result AddExistedAccount(string custName, int groupId);
        Result UpdateAccount(List<PostParam> postParams);
        Result ChangeAccountStatus(int custId, bool closed);
        List<UserControlBO> GetUserPolicyList(int custId);
        Result UpdateAdminPolicy(string memberPolicies, string oldMemberPolicies, int custId);
    }
}
