using System.Data;
using DNAide.Utils;
using Fanex.Data;
using Fanex.Tarus.Entities.Customer.DepositCustomer;
using Sunplus.Accounting.Entities;
using DNAide.Core;


namespace Fanex.Tarus.DataAccess.Implement.Customer
{
    using Fanex.Tarus.DataAccess.Interface.Customer;
    using System.Collections;

    public class DepositCustomerAccess: BaseAccess, IDepositCustomerAccess
    {
        public Result NewLicenseeAddNewSuper(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewSuper"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }
                hashParam.Add("username", newLicenseePostParam.SuperName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);
                
                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    if (!string.IsNullOrWhiteSpace(newLicenseePostParam.MasterName))
                    {
                        newLicenseePostParam.SuperId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                        var result = NewLicenseeAddNewMaster(newLicenseePostParam);
                        if (result.ErrorCode == 0)
                        {
                            if (!string.IsNullOrWhiteSpace(newLicenseePostParam.AgentName))
                            {
                                result = NewLicenseeAddNewAgent(newLicenseePostParam);
                                if (result.ErrorCode != 0) return result;
                            }
                        }
                        else return result;
                    }
                }
                else
                {
                    return new Result(errCode, errMsg);
                }
                return new Result();
            }
        }

        public Result NewLicenseeAddNewMaster(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewMaster"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("srecommend", newLicenseePostParam.SuperId);
                hashParam.Add("username", newLicenseePostParam.MasterName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.MasterId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                }

                return new Result(errCode, errMsg);
            }
        }

        public Result NewLicenseeAddNewAgent(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewAgent"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("mrecommend", newLicenseePostParam.MasterId);
                hashParam.Add("username", newLicenseePostParam.AgentName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.AgentId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                }

                return new Result(errCode, errMsg);
            }
        }

        public Result NewLicenseeAddNewMember(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewMember"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("recommend", newLicenseePostParam.AgentId);
                hashParam.Add("username", newLicenseePostParam.MemberName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.AgentId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                }

                return new Result(errCode, errMsg);
            }
        }

        public DataSet GetSuperSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetSuperSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetMasterSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetMasterSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetAgentSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetAgentSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetMemberSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetMemberSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public Result UpdateSuperSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateSuperSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateMasterSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateMasterSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateAgentSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateAgentSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateMemberSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateMemberSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }


        public DataTable SelAllLicensee()
        {
            using (var objDb = new ObjectDb("Dep_LicenseeSelAll"))
            {
                var dataSet = objDb.ExecuteDataSet();
                if (dataSet.Tables.Count == 0) return null;
                return dataSet.Tables[0];
            }
        }
    }
}
