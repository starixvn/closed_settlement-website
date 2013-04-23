using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Configurations;

/// <summary>
/// Summary description for UserControlManager
/// </summary>

namespace Sunplus.Accounting.Business.UserControl
{
    public class UserControlManager
    {
        /// <summary>
        /// IF custid=0 select by username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static bool CheckUserNameExistInListGroupName(string username, int custid)
        {
            if (WebSettings.IgnoreGroups == "")
            {
                return false;
            }
            else
            {
                var param = new Hashtable();
                param.Add("listgroupname", WebSettings.IgnoreGroups);
                param.Add("username", username);
                param.Add("custid", custid);
                var dt = new DataTable();
                dt = DataAccess.IBCDLL.Accounting.UserControl.CheckUserNameExistInListGroupName(param);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region Function Management

        public List<FunctionBO> GetFunctionList()
        {
            var listFunction = new List<FunctionBO>();
            DataTable dtbResult = DataAccess.IBCDLL.Accounting.UserControl.FunctionInfoSelect();
            GroupFunctionById(dtbResult, ref listFunction, 0);
            return listFunction;
        }

        private static void GroupFunctionById(DataTable dt, ref List<FunctionBO> listFunction, int parentId)
        {
            if (dt == null || dt.Rows.Count == 0) return;


            DataRow[] rows = dt.Select("parentid=" + parentId, "[System], [DisplayOrder] ASC");
            foreach (DataRow row in rows)
            {
                var functionBo = new FunctionBO();
                functionBo.ID = int.Parse(row["FuncId"].ToString());
                functionBo.Name = row["FuncName"].ToString();
                functionBo.Desc = row["FuncDesc"].ToString();
                functionBo.System = row["System"].ToString();
                functionBo.ParentId = CastAide.AsInt32(row["parentid"]);
                functionBo.Files = CastAide.AsString(row["FileNames"]);
                functionBo.TotalSub = CastAide.AsInt32(row["SubFuncCount"]);
                functionBo.Order = CastAide.AsInt32(row["DisplayOrder"]);
                var listSubFunction = new List<FunctionBO>();
                GroupFunctionById(dt, ref listSubFunction, functionBo.ID);
                functionBo.SubFuncs = listSubFunction;
                listFunction.Add(functionBo);
            }
        }

        #endregion

        #region User control management

        public List<UserControlBO> GetGroupPolicyList(int groupId)
        {
            var listPolicy = new List<UserControlBO>();
            DbResult dbResult = DataAccess.IBCDLL.Accounting.UserControl.SelectGroupPolicy(groupId);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                GroupPolicyListById(dbResult.ResultSet.Tables[0], ref listPolicy, 0);
            return listPolicy;
        }

        private static void GroupPolicyListById(DataTable dt, ref List<UserControlBO> listPolicy, int parentId)
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
                int policy = CastAide.AsInt32(row["policy"]);
                userControlBo.IsGroupRead = (policy > 0) ? true : false;
                userControlBo.IsGroupHost = (policy == 3) ? true : false;
                var listSubUsercontrol = new List<UserControlBO>();
                GroupPolicyListById(dt, ref listSubUsercontrol, userControlBo.ID);
                userControlBo.SubFuncs = listSubUsercontrol;
                listPolicy.Add(userControlBo);
            }
        }

        private static void UserPolicyListById(DataTable dt, ref List<UserControlBO> listPolicy, int parentId)
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
                userControlBo.IsRead = (policy > 0) ? true : false;
                userControlBo.IsHost = (policy == 3) ? true : false;
                userControlBo.IsGroupRead = (gpolicy > 0) ? true : false;
                userControlBo.IsGroupHost = (gpolicy == 3) ? true : false;
                var listSubUsercontrol = new List<UserControlBO>();
                UserPolicyListById(dt, ref listSubUsercontrol, userControlBo.ID);
                userControlBo.SubFuncs = listSubUsercontrol;
                listPolicy.Add(userControlBo);
            }
        }

        public List<UserControlBO> GetUserPolicyList(int userId)
        {
            var listPolicy = new List<UserControlBO>();
            DbResult dbResult = DataAccess.IBCDLL.Accounting.UserControl.SelectAdminPolicy(userId);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                UserPolicyListById(dbResult.ResultSet.Tables[0], ref listPolicy, 0);
            return listPolicy;
        }

        #endregion
    }
}