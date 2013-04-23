namespace Fanex.Tarus.User.Business.Services
{
    using Fanex.Tarus.User.Business.Interface;
    using Fanex.Tarus.User.DataAccess;
    using System.Collections.Generic;
    using System;
    using Sunplus.Accounting.Core.Extensions;
    using DNAide.Core;
    using System.Linq;
    using System.Data;
    using Fanex.Tarus.User.DataAccess.Sql;
    using Sunplus.Accounting.Core.Common;
    using Fanex.Tarus.Entities;
    using Fanex.Tarus.User.Entities;
    using Sunplus.Accounting.Entities;
    using UserControlBO = Fanex.Tarus.User.Entities.UserControlBO;

    public class UserInfoService : IUserInfoService
    {
        private readonly IUserInfoAccess _userInfoAccess;

        public UserInfoService()
        {
            _userInfoAccess = new UserInfoAccess();
        }

        public List<Department> GetAllDept()
        {
            try
            {
                var dataTable = _userInfoAccess.GetAllDept();
                if (dataTable == null) return new List<Department>();
                return (from DataRow row in dataTable.Rows
                        select new Department
                        {
                            NoOfUser = CastAide.AsInt32(row["nouser"]),
                            Description = CastAide.AsString(row["groupdesc"]),
                            Id = CastAide.AsInt32(row["groupid"]),
                            Name = CastAide.AsString(row["groupname"])
                        }).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<Department>();
            }
        }

        public List<Account> GetAllAccount()
        {
            try
            {
                var dataTable = _userInfoAccess.GetAllAccount();
                if (dataTable == null) return new List<Account>();
                return (from DataRow row in dataTable.Rows
                        select new Account
                        {
                            Id = CastAide.AsInt32(row["custid"]),
                            CreateDate = CastAide.AsString(row["createdate"]),
                            Department = CastAide.AsString(row["groupname"]),
                            Creator = CastAide.AsString(row["creator"]),
                            DeptId = CastAide.AsString(row["groupid"]),
                            FirstName = CastAide.AsString(row["firstname"]),
                            LastName = CastAide.AsString(row["lastname"]),
                            Status = ((!Convert.IsDBNull(row["closed"]) ? (bool)row["closed"] : false) ? "Closed" : "Active"),
                            UserName = CastAide.AsString(row["username"])
                        }).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<Account>();
            }
        }


        public List<UserControlBO> GetGroupPolicyList(int groupId)
        {
            try
            {
                var listPolicy = new List<UserControlBO>();
                var dataSet = _userInfoAccess.GetGroupPolicyList(groupId);
                if (dataSet.Tables.Count > 0)
                    GroupPolicyListById(dataSet.Tables[0], ref listPolicy, 0);
                return listPolicy.OrderBy(f => f.FuncName).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<UserControlBO>();
            }
        }

        private void GroupPolicyListById(DataTable dt, ref List<UserControlBO> listPolicy, int parentId)
        {
            if (dt == null || dt.Rows.Count == 0) return;


            DataRow[] rows = dt.Select("parentid=" + parentId, "[DisplayOrder] ASC");
            foreach (DataRow row in rows)
            {
                var userControlBo = new UserControlBO();
                userControlBo.ID = int.Parse(row["FuncId"].ToString());
                userControlBo.FuncName = row["FuncName"].ToString();
                userControlBo.FuncDesc = row["FuncDesc"].ToString();
                userControlBo.ParentId = CastAide.AsInt32(row["parentid"]);
                userControlBo.TotalSub = CastAide.AsInt32(row["SubFuncCount"]);
                userControlBo.GroupName = CastAide.AsString(row["groupname"]);
                var policy = CastAide.AsInt32(row["policy"]);
                userControlBo.IsGroupRead = (policy > 0);
                userControlBo.IsGroupHost = (policy == 3);
                var listSubUsercontrol = new List<UserControlBO>();
                GroupPolicyListById(dt, ref listSubUsercontrol, userControlBo.ID);
                userControlBo.SubFuncs = listSubUsercontrol;
                listPolicy.Add(userControlBo);
            }
        }

        public Result UpdateGroupPolicy(string groupPolicies, string oldGroupPolicies, int groupId)
        {
            try
            {
                return _userInfoAccess.UpdateGroupPolicy(groupPolicies, oldGroupPolicies, groupId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result UpdateDept(int id, string name, string description)
        {
            try
            {
                return _userInfoAccess.UpdateDept(id, name, ReplaceSpecifiedCharacter(description));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result DeleteDept(int id)
        {
            try
            {
                return _userInfoAccess.DeleteDept(id);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result AddDept(string name, string description)
        {
            try
            {
                return _userInfoAccess.AddDept(name, ReplaceSpecifiedCharacter(description));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result AddAccount(string userName, string firstName, string lastName, int groupId, string password)
        {
            try
            {
                return _userInfoAccess.AddAccount(userName, ReplaceSpecifiedCharacter(firstName), ReplaceSpecifiedCharacter(lastName), groupId,
                                                                IBCUtils.Encryption.CfsEnCode(password), AppSession.AccId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result AddExistedAccount(string custName, int groupId)
        {
            try
            {
                return _userInfoAccess.AddExistedAccount(custName, AppSession.AccId, groupId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result UpdateAccount(List<PostParam> postParams)
        {
            try
            {
                foreach (var param in postParams)
                {
                    if (param.Name == "firstname" || param.Name == "lastname") param.Value = ReplaceSpecifiedCharacter(param.Value == null ? string.Empty : param.Value.ToString());
                }
                return _userInfoAccess.UpdateAccount(postParams);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result ChangeAccountStatus(int custId, bool closed)
        {
            try
            {
                return _userInfoAccess.ChangeAccountStatus(custId, closed);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public List<UserControlBO> GetUserPolicyList(int custId)
        {
            try
            {
                var listPolicy = new List<UserControlBO>();
                var dataSet = _userInfoAccess.GetUserPolicyList(custId);
                if (dataSet.Tables.Count > 0)
                    UserPolicyListById(dataSet.Tables[0], ref listPolicy, 0);
                return listPolicy.OrderBy(f => f.FuncName).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<UserControlBO>();
            }
        }

        private void UserPolicyListById(DataTable dt, ref List<UserControlBO> listPolicy, int parentId)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] rows = dt.Select("parentid=" + parentId, "[DisplayOrder] ASC");
            foreach (DataRow row in rows)
            {
                var userControlBo = new UserControlBO();
                userControlBo.ID = int.Parse(row["FuncId"].ToString());
                userControlBo.FuncName = row["FuncName"].ToString();
                userControlBo.FuncDesc = row["FuncDesc"].ToString();
                userControlBo.ParentId = CastAide.AsInt32(row["parentid"]);
                userControlBo.TotalSub = CastAide.AsInt32(row["SubFuncCount"]);
                int policy = CastAide.AsInt32(row["policy"]);
                int gpolicy = CastAide.AsInt32(row["gpolicy"]);
                userControlBo.IsRead = (policy > 0);
                userControlBo.IsHost = (policy == 3);
                userControlBo.IsGroupRead = (gpolicy > 0);
                userControlBo.IsGroupHost = (gpolicy == 3);
                var listSubUsercontrol = new List<UserControlBO>();
                UserPolicyListById(dt, ref listSubUsercontrol, userControlBo.ID);
                userControlBo.SubFuncs = listSubUsercontrol;
                listPolicy.Add(userControlBo);
            }
        }

        public Result UpdateAdminPolicy(string memberPolicies, string oldMemberPolicies, int custId)
        {
            try
            {
                return _userInfoAccess.UpdateAdminPolicy(memberPolicies, oldMemberPolicies, custId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        private string ReplaceSpecifiedCharacter(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            value = value.Replace("&apos;", "'");
            value = value.Replace("&quot;", "\"");
            return value;
        }
    }
}
