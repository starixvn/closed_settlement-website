using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.User.Entities;
using System.Collections;
using Fanex.Data;
using DNAide.Core;
using System.Data;
using Sunplus.Accounting.Entities;
using FunctionBO = Fanex.Tarus.User.Entities.FunctionBO;

namespace Fanex.Tarus.User.DataAccess.Sql
{
    public class FunctionAccess : IFunctionAccess
    {
        public List<FunctionBO> GetFunctionParentList(string systemName)
        {
            using (ObjectDb objDB = new ObjectDb("FuncInfoSelParent"))
            {
                var listEntity = new List<FunctionBO>();
                var param = new Hashtable();
                param.Add("system", systemName);
                objDB.AssignParamValues(param);
                using (var dr = objDB.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listEntity.Add(new FunctionBO
                        {
                            ID = CastAide.AsInt32(dr["FuncId"]),
                            Name = CastAide.AsString(dr["FuncName"])
                        });
                    }
                }
                return listEntity;
            }
        }

        public List<FunctionBO> GetFunctionList()
        {
            var listFunction = new List<FunctionBO>();
            using (ObjectDb objDB = new ObjectDb("FuncInfoSel"))
            {
                var dataSet = objDB.ExecuteDataSet();
                if (dataSet.Tables.Count <= 0) return new List<FunctionBO>();
                GroupFunctionById(dataSet.Tables[0], ref listFunction, 0);
                return listFunction;
            }
        }

        private void GroupFunctionById(DataTable dt, ref List<FunctionBO> listFunction, int parentId)
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

        public Result UpdateFunction(int intFuncId, int intUserId, string strName, string strDesc,
                                              string strFileNames, string System, string parentId)
        {
            var param = new Hashtable();
            param.Add("funcid", intFuncId);
            param.Add("funcname", strName);
            param.Add("funcnote", "");
            param.Add("funcdesc", strDesc);
            param.Add("displayorder", 1);
            param.Add("filenames", strFileNames);
            param.Add("userid", intUserId);
            param.Add("language", "E");
            param.Add("system", System);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("parentid", string.IsNullOrEmpty(parentId) ? "0" : parentId);

            using (ObjectDb objDB = new ObjectDb("FuncInfoUpd"))
            {
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result InsertFunction(string strName, string strDesc, string strFileNames, string strParent,
                                              int intUserId, string system)
        {
            var param = new Hashtable();
            param.Add("funcid", 0);
            param.Add("funcname", strName);
            param.Add("funcnote", "");
            param.Add("funcdesc", strDesc);
            param.Add("displayorder", 1);
            if (!string.IsNullOrEmpty(strParent))
                param.Add("parentid", strParent);
            param.Add("filenames", strFileNames);
            param.Add("userid", intUserId);
            param.Add("language", "E");
            param.Add("system", system);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            
            using (ObjectDb objDB = new ObjectDb("FuncInfoIns"))
            {
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }

        public Result DeleteFunction(int intFuncId, int intUserId)
        {
            var param = new Hashtable();
            param.Add("funcid", intFuncId);
            param.Add("language", "E");
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("userid", intUserId);
           
            using (ObjectDb objDB = new ObjectDb("FuncInfoDel"))
            {
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();

                var errorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                var errorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage };
            }
        }
    }
}
