using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.DataAccess.Interface;
using Fanex.Tarus.Entities;
using Fanex.Data;
using Fanex.Common.Extensions;
using System.Data;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.DataAccess.Implement
{

    public class AdminAccess: BaseAccess, IAdminAccess
    {
        public List<AgentAccess> GetListAgentAcess()
        {
            try
            {
                var entityList = new List<AgentAccess>();
                using (ObjectDb objDB = new ObjectDb("Acc_CustAllowListSel"))
                {
                    using (var reader = objDB.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            int index = 0;
                            while (reader.Read())
                            {
                                ++index;
                                entityList.Add(new AgentAccess
                                {
                                    #region assign value
                                    Index = index,
                                    AccountId = reader.ToInt("custid"),
                                    Account = reader.ToString("username"),
                                    Role = reader.ToString("Role"),
                                    GrantAccess = System.DBNull.Value.Equals(reader["isAllowed"]) ? false : reader.ToBool("isAllowed"),
                                    AddedDate = System.DBNull.Value.Equals(reader["CreatedDate"]) ? string.Empty : reader.ToDateTime("CreatedDate").ToString("MM/dd/yyyy hh:mm tt"),
                                    AddedBy = reader.ToString("CreatedBy"),
                                    LastModified = System.DBNull.Value.Equals(reader["LastModifiedTime"]) ? string.Empty : reader.ToDateTime("LastModifiedTime").ToString("MM/dd/yyyy hh:mm tt"),
                                    LastModifiedBy = reader.ToString("LastModifiedBy"),
                                    Remark = reader.ToString("Remark"),
                                    #endregion
                                });
                            }
                        }
                    }
                }
                return entityList;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<AgentAccess>();
            }
        }


        public Result AddUser(string userName, int creator, string remark)
        {
            var result = new Result();
            try
            {
              
                using (ObjectDb objDB = new ObjectDb("Acc_CustAllowListIns"))
                {
                    objDB.AddInParameter("UserName", DbType.String, userName);
                    objDB.AddInParameter("Creator", DbType.Int32, creator);
                    objDB.AddInParameter("ip", DbType.String, Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
                    objDB.AddInParameter("Remark", DbType.String, remark);
                    objDB.AddOutParameter("ErrMsg", DbType.String, 4000);
                    objDB.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                    objDB.ExecuteNonQuery();
                    result.ErrorMessage = DNAide.Core.CastAide.AsString(objDB.GetParameterValue("ErrMsg"));
                    result.ErrorCode = DNAide.Core.CastAide.AsInt32(objDB.GetParameterValue("ErrCode"));
                }
               
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                //return ex.Message;
            }
            return result;
        }


        public Result Update(int custId, bool isAllowed, string remark)
        {
            var result = new Result();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_CustAllowListUpd"))
                {
                    objDB.AddInParameter("CustID", DbType.Int32, custId);
                    objDB.AddInParameter("IsAllowed", DbType.Boolean, isAllowed);
                    objDB.AddInParameter("ip", DbType.String, Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
                    objDB.AddInParameter("Remark", DbType.String, remark);
                    objDB.AddInParameter("LastModifiedBy", DbType.Int32, AppSession.AccId);
                    objDB.AddOutParameter("ErrMsg", DbType.String, 4000);
                    objDB.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                    objDB.ExecuteNonQuery();
                    result.ErrorMessage =  DNAide.Core.CastAide.AsString(objDB.GetParameterValue("ErrMsg"));
                    result.ErrorCode = DNAide.Core.CastAide.AsInt32(objDB.GetParameterValue("ErrCode"));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                //return ex.Message;
            }
            return result;
        }


        public Result Delete(int custId)
        {
            var result = new Result();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_CustAllowListDel"))
                {
                    objDB.AddInParameter("CustID", DbType.Int32, custId);
                    objDB.AddInParameter("ip", DbType.String, Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
                    objDB.AddInParameter("LastModifiedBy", DbType.Int32, AppSession.AccId);
                    objDB.AddOutParameter("ErrMsg", DbType.String, 4000);
                    objDB.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                    objDB.ExecuteNonQuery();
                    result.ErrorMessage =  DNAide.Core.CastAide.AsString(objDB.GetParameterValue("ErrMsg"));
                    result.ErrorCode = DNAide.Core.CastAide.AsInt32(objDB.GetParameterValue("ErrCode"));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
               // return ex.Message;
            }
            return result;
        }

    }
}
