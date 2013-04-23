using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.UserControl;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.WcfServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserControlService : IUserControlService
    {
        #region IUserControlService Members

        public List<FunctionBO> GetFunctionList()
        {
            var userControlMng = new UserControlManager();
            return userControlMng.GetFunctionList();
        }

        public List<UserControlBO> GetGroupPolicyList(int groupId)
        {
            var userControlMng = new UserControlManager();
            return userControlMng.GetGroupPolicyList(groupId);
        }

        public List<UserControlBO> GetUserPolicyList(int userId)
        {
            var userControlMng = new UserControlManager();
            return userControlMng.GetUserPolicyList(userId);
        }

        #endregion
    }
}