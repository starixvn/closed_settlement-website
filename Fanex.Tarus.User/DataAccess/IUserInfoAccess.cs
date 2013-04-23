using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Tarus.Entities;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.User.DataAccess
{
    public interface IUserInfoAccess
    {
        DataTable GetAllDept();
        DataTable GetAllAccount();
        Result UpdateGroupPolicy(string groupPolicies, string oldGroupPolicies, int groupId);
        Result AddAccount(string userName, string firstName, string lastName, int groupId, string password, int userId);
        Result AddExistedAccount(string custName, int loginId, int groupId);
        Result UpdateAccount(List<PostParam> postParams);
        Result UpdateAdminPolicy(string memberPolicies, string oldMemberPolicies, int custId);
        DataSet GetGroupPolicyList(int groupId);
        Result UpdateDept(int groupID, string groupName, string groupDesc);
        Result DeleteDept(int id);
        Result AddDept(string name, string description);
        Result ChangeAccountStatus(int custId, bool closed);
        DataSet GetUserPolicyList(int custId);
    }
}
