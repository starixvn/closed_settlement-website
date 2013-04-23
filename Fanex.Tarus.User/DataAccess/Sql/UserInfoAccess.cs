using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using System.Collections;
using Fanex.Tarus.Entities;
using Sunplus.Accounting.Entities;
using DNAide.Core;

namespace Fanex.Tarus.User.DataAccess.Sql
{
    public class UserInfoAccess: IUserInfoAccess
    {
        public DataTable GetAllDept()
        {
            using (var objDb = new ObjectDb("GroupInfoSel"))
            {
                var dataSet = objDb.ExecuteDataSet();
                return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
            }
        }

        public DataTable GetAllAccount()
        {
            using (var objDb = new ObjectDb("AdminSelByGroupID"))
            {
                var dataSet = objDb.ExecuteDataSet();
                return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
            }
        }

        public Result UpdateGroupPolicy(string groupPolicies, string oldGroupPolicies, int groupId)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("GroupPolicy"))
                {
                    var param = new Hashtable();
                    param.Add("grouppolicys", groupPolicies);
                    param.Add("oldgrouppolicys", oldGroupPolicies);
                    param.Add("userid", AppSession.AccId);
                    param.Add("groupid", groupId);
                    param.Add("language", "E");
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);
                    objDB.AssignParamValues(param);

                    objDB.ExecuteNonQuery();

                    var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                    return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
                }
            }
            catch (Exception ex)
            {
                return new Result { ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        public Result AddAccount(string userName, string firstName, string lastName, int groupId, string password, int userId)
        {
            var param = new Hashtable
            {
                {"groupid", groupId},
                {"firstname", firstName},
                {"lastname", lastName},
                {"userid", userId},
                {"username", userName},
                {"userpwd", password},
                {"custid", 0}
            };
            using (ObjectDb objDB = new ObjectDb("Con_AdminIns"))
            {
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();
                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public DataSet GetGroupPolicyList(int groupId)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("GroupPolicySel"))
                {
                    objDB.AddInParameter("groupid", DbType.Int32, groupId);
                    return objDB.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                return new DataSet();
            }
        }

        public Result AddExistedAccount(string custName, int loginId, int groupId)
        {
            var param = new Hashtable();
            param.Add("language", "E");
            param.Add("username", custName);
            param.Add("creator", loginId);
            param.Add("groupid", groupId);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            using (ObjectDb objDB = new ObjectDb("Con_AdminInsFromExisted"))
            {
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();
                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result UpdateAccount(List<PostParam> postParams)
        {
            var param = new Hashtable();
            foreach (var postParam in postParams)
            {
                if (postParam.Name == "firstname" && postParam.Value == null) postParam.Value = string.Empty;
                if (postParam.Name == "lastname" && postParam.Value == null) postParam.Value = string.Empty;
                param.Add(postParam.Name, postParam.Value);
            }
            using (ObjectDb objDB = new ObjectDb("Con_AdminUpd"))
            {
                param.Add("userid", AppSession.AccId);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                objDB.AssignParamValues(param);

                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result UpdateAdminPolicy(string memberPolicies, string oldMemberPolicies, int custId)
        {
            using (ObjectDb objDB = new ObjectDb("AdminPolicy"))
            {
                var param = new Hashtable();
                param.Add("mempolicys", memberPolicies);
                param.Add("oldmempolicys", oldMemberPolicies);
                param.Add("userid", AppSession.AccId);
                param.Add("custid", custId);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result UpdateDept(int groupID, string groupName, string groupDesc)
        {
            using (ObjectDb objDB = new ObjectDb("GroupInfoUpd"))
            {
                var param = new Hashtable();
                param.Add("groupid", groupID);
                param.Add("groupname", groupName);
                param.Add("groupdesc", groupDesc);
                param.Add("language", "E");
                param.Add("userid", AppSession.AccId);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result DeleteDept(int id)
        {
            using (ObjectDb objDB = new ObjectDb("GroupInfoDel"))
            {
                var param = new Hashtable();
                param.Add("groupid", id);
                param.Add("language", "E");
                param.Add("userid", AppSession.AccId);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result AddDept(string name, string description)
        {
            using (ObjectDb objDB = new ObjectDb("GroupInfoIns"))
            {
                var param = new Hashtable();
                param.Add("groupid", 0);
                param.Add("groupname", name);
                param.Add("groupdesc", description);

                param.Add("language", "E");
                param.Add("userid", AppSession.AccId);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result ChangeAccountStatus(int custId, bool closed)
        {
            using (ObjectDb objDB = new ObjectDb("AdminUpdClosed"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("closed", closed);
                param.Add("language", "E");
                param.Add("userid", AppSession.AccId);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public DataSet GetUserPolicyList(int custId)
        {
            using (ObjectDb objDB = new ObjectDb("AdminPolicySelByCustID"))
            {
                objDB.AddInParameter("custid", DbType.Int32, custId);
                objDB.AddInParameter("groupid", DbType.Int32, 0);
                return objDB.ExecuteDataSet();
            }
        }
    }
}
