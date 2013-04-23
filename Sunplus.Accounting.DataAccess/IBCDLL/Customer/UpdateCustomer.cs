using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using DNAide.Core;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    public class UpdateCustomer : DataAccess
    {
        /// <summary>
        /// call Store CustomerInsMember_Com to Insert data
        /// writer: Vu Hanh
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="hash">hash</param>
        /// <returns>Boolean</returns>
        public static Hashtable CustomerInsMemberComRpt(Hashtable hash)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "InsertSuper");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerInsMember_Com", hash, DBType.MAIN, 600);
                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hashUpdated;
        }

        /// <summary>
        /// The fuction is using for add new Super
        /// Using DNAide's Library
        /// Tho Nguyen updated : 11/26/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static Hashtable Acc_CustomerInsMember_Com(Hashtable hash)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;

                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "InsertSuper");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hashUpdated;
        }

        /// <summary>
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 11/29/2008
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IbcError Acc_CustomerPwdUpd(int CustId, string UserPwd, int UserId, string language)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var p = new Hashtable();
                p.Add("custid", CustId);
                p.Add("userpwd", UserPwd);
                p.Add("userid", UserId);
                p.Add("langguage", language);
                p.Add("loginip", HttpContext.Current.Request.UserHostAddress);
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.UserControlSession.StoreCollection.AccountInfo, "UpdPwdReset");
                DbResult ds = DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName);
                if (ds != null)
                {
                    if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                    {
                        error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                        error.ErrorMsg = ds.Outputs["errmsg"] + "";
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Insert new agent
        /// @PhuongDN
        /// </summary>
        /// <param name="param"></param>
        /// <returns>string:error message;empty: successfully</returns>
        public static Hashtable AddNewAgent(Hashtable param)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "InsertAgent");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "InsertAgent");
                ds = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 600);
                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hashUpdated;
        }

        /// <summary>
        /// call SP CustomerInsMember_SAg
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho Nguyen update: 26/11/2008
        /// </summary>
        /// <param name="hash">hash</param>
        /// <returns>Boolean</returns>
        public static Hashtable CustomerInsMemberSAgRpt(Hashtable hash)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "InsertMaster");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "InsertMaster");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerInsMember_SAg", hash, DBType.MAIN, 600);
                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hashUpdated;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 
        /// 05/12/2006
        /// insert new member
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Hashtable InsertMember(Hashtable param)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "InsertMember");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "InsertMember");
                ds = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 600);
                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hashUpdated;
        }

        /// <summary>
        /// call SP CustomerInsOperator to Insert data into database
        /// writer: Vu Hanh
        /// </summary>
        /// <param name="hash">hash</param>
        /// <returns>Boolean</returns>
        public static Hashtable CustomerInsOperatorRpt(Hashtable hash)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;

                //ds = DbHelper.ExecuteCmd("Acc_OperatorIns", hash, DBType.MAIN);
                var spElement = new UpdateCustomer().GetStoreProcedure(UserControlSession.StoreCollection.AccountInfo, "OperatorIns");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName);

                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return hashUpdated;
        }

        /// <summary>
        /// call SP CustomerUpdOperator to Update data into database
        /// writer: Vu Hanh
        /// </summary>
        /// <param name="hash">hash</param>
        /// <returns>Boolean</returns>
        public static Hashtable CustomerUpdOperatorRpt(Hashtable hash)
        {
            var hashUpdated = new Hashtable();
            try
            {
                DbResult ds = null;

                //ds = DbHelper.ExecuteCmd("Acc_OperatorUpd", hash, DBType.MAIN);
                var spElement = new UpdateCustomer().GetStoreProcedure(UserControlSession.StoreCollection.AccountInfo, "OperatorUpd");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName);

                if (ds != null)
                {
                    hashUpdated.Add("superId", ds.Outputs["custid"]);
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return hashUpdated;
        }

        /// <summary>
        /// call SP CustomerDelOperator to Delete data from database
        /// writer: Vu Hanh
        /// </summary>
        /// <param name="custId">custId</param>
        /// <param name="language">language</param>
        /// <param name="userId">userId</param>
        /// <returns>Boolean</returns>
        public static Hashtable CustomerDelOperatorRpt(int custId, string language, int userId)
        {
            var hashUpdated = new Hashtable();
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId); //int
                hash.Add("language", language); //varchar
                hash.Add("userid", userId); //int

                DbResult ds = null;
                //ds = DbHelper.ExecuteCmd("Acc_OperatorDel", hash, DBType.MAIN);
                var spElement = new UpdateCustomer().GetStoreProcedure(UserControlSession.StoreCollection.AccountInfo, "OperatorDel");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName);
                if (ds != null)
                {
                    hashUpdated.Add("errCode", ds.Outputs["errcode"]);
                    hashUpdated.Add("errMsg", ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hashUpdated;
        }

        /// <summary>
        /// Update super agent
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static IbcError CustomerUpdEditCustomerCom(Hashtable hash)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                hash["returnerrcode"] = true;
                hash["errcode"] = 0;
                hash["errmsg"] = string.Empty;

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateSuper");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateSuper");
                var ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("Acc_CustomerUpdSuper_All", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// The Fuction is using update super
        /// Using DNAide's Library
        /// Tho Nguyen updated : 11/26/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static IbcError CustomerUpdEditCustomerCom_acc(Hashtable hash)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateSuper");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateSuper");
                var ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("Acc_CustomerUpdSuper_All", hash, DBType.MAIN, 600);

                if (ds != null)
                {
                    error.ErrorMsg = "";
                    AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, ds.Outputs["errcode"].ToString(),
                                            Convert.ToString(ds.Outputs["errmsg"]));
                }
            }
            catch (Exception ex)
            {
                error = IbcError.InternalError;
                error.ErrorMsg = ex.Message.Replace("'", "");

                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        //Writer: Hanh
        /// <summary>
        /// call SP CustomerUpdMatchLimit to update MatchLimit  
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 11/29/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="maxExp"></param>
        /// <returns></returns>
        public static IbcError UpdateMaxExposure(string custid, string maxExp, object updator)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var hash = new Hashtable();

                DbResult ds = null;
                hash.Add("custid", custid);
                hash.Add("userid", updator);
                hash.Add("matchlimit", maxExp);
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0);
                hash.Add("errmsg", string.Empty);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateMatchLimit");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateMatchLimit");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMatchLimit", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) != 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return error;
        }

        //Writer: Hanh
        /// <summary>        
        /// execute sp Acc_CustomerUpdUserInfo to update Customer Information       
        /// Using DNAide's library
        /// Tho.Nguyen updated: 11/29/2008
        /// </summary>
        /// <param name="ht"></param>
        public static IbcError CustomerUpdUserInfoRpt(Hashtable hash)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateUserInfo");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateUserInfo");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                
                if (Convert.ToInt32(ds.Outputs["errcode"]) != 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Execute SP CustOverrideUpd to update Max, Min, Mid position taking of Dead ball soccer, Live soccer, Basket ball
        /// @PhuongDN
        /// </summary>
        /// <param name="p">Hashtable contain all parameter for Sp CustOverrideUpd</param>
        /// <returns>error_message: ==""--> successfull, else error</returns>
        public static IbcError UpdMaxMinPT(Hashtable p)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateMinMaxPT");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateMinMaxPT");
                ds = DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustOverrideUpd", p, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) != 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// The fuction is using update min and max of PT
        /// Using DNAide's Library
        /// Tho Nguyen updated: 26/11/2008
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static IbcError UpdMaxMinPT_acc(Hashtable p)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateMinMaxPT");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateMinMaxPT");
                ds = DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustOverrideUpd", p, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) != 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Execute Sp CustomerUpdEditCustomer_MAg to update agent
        /// @PhuongDN
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>string:error message;empty: successfully</returns>
        public static IbcError UpdateAgent(Hashtable param)
        {
            IbcError error = IbcError.SuccessResult;
            DbResult ds = null;
            if (param == null)
            {
                throw new Exception();
            }
            try
            {
                CultureInfo cult = CultureInfo.CurrentCulture;

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateAgent");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateAgent");
                ds = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_All", param, DBType.MAIN, 600);
                if (ds != null)
                {
                    //if (Convert.ToInt32(ds.Outputs["errcode"]) != 0)
                    //{
                    //    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    //    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                    //}
                    error.ErrorMsg = "";
                    AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, ds.Outputs["errcode"].ToString(),
                                            Convert.ToString(ds.Outputs["errmsg"]));
                }
            }
            catch (SqlException e)
            {
                if (e.Class == 16)
                {
                    error.ErrorMsg = e.Message;
                    error.ErrorCode = IbcError.INTERNAL_ERROR;
                    return error;
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// call SP CustomerUpdEditCustomer_SAg
        /// writer: Vu Hanh
        /// Using DNAide's Library
        /// Tho Nguyen updated: 11/26/2008
        /// </summary>
        /// <param name="hash">hash</param>
        /// <returns>Boolean</returns>
        public static IbcError CustomerUpdEditCustomerSAgRpt(Hashtable hash)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                hash.Add("errcode", string.Empty);
                hash.Add("errmsg", string.Empty);


                DbResult ds = null;
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateMaster");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateMaster");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_All", hash, DBType.MAIN, 600);
                if (ds != null)
                {
                    error.ErrorMsg = "";
                    AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, ds.Outputs["errcode"].ToString(),
                                            Convert.ToString(ds.Outputs["errmsg"]));
                }
            }
            catch (Exception ex)
            {
                error.ErrorCode = IbcError.INTERNAL_ERROR;
                error.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 07/12/2006
        /// execute sp CustomerUpdEditCustomer_Ag
        /// update data for singer member
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DbResult CustomerUpdEditCustomerAgRpt(Hashtable param)
        {
            IbcError error = IbcError.SuccessResult;
            DbResult ds = null;
            try
            {
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateMember");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateMember");
                ds = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 600);
                //ds = DbHelper.Execute("Acc_CustomerUpdMember_All", param, DBType.MAIN, 600);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 12/12/2006
        /// call sp CustomerUpdUserName for change userName
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IbcError CustomerUpdUserNameRpt(string oldUname, string newUname, string accId)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                var hash = new Hashtable();
                hash.Add("oldname", oldUname);
                hash.Add("newname", newUname);
                hash.Add("userid", accId);
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateUserName");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateUserName");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdUserName", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                error.ErrorCode = IbcError.INTERNAL_ERROR;
                error.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;

                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        public static IbcError CustomerUpdUserNameRpt(string oldUname, string newUname, string accId, out int custid)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                var hash = new Hashtable();
                hash.Add("oldname", oldUname);
                hash.Add("newname", newUname);
                hash.Add("userid", accId);
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateUserName");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateUserName");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdUserName", hash, DBType.MAIN, 600);

                custid = Convert.ToInt32(ds.Outputs["returncustid"]);

                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
                return error;
            }
            catch (Exception ex)
            {
                error.ErrorCode = IbcError.INTERNAL_ERROR;
                error.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;

                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 13/12/2006
        /// call sp CustomerUpdClearCredit for clear credit
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IbcError CustomerUpdClearCreditRpt(string username, string accId)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                var hash = new Hashtable();
                hash.Add("username", username);
                hash.Add("userid", accId);
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateClearCredit");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateClearCredit");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdClearCredit", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = ds.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 13/12/2006
        /// call sp CustomerUpdMoveMaster for move master to new super
        /// call sp CustomerUpdMoveAgent for move agent to new master
        /// </summary>
        /// <param name="param"></param>
        /// <param name="type">type = master move master to new super
        /// type = agent move agent to new master
        /// </param>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// <returns></returns>
        public static IbcError CustomerUpdMoveCustRpt(string custId, string recommendId, string accId, string type)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                var hash = new Hashtable();
                hash.Add("userid", accId);
                hash.Add("ip", ip);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");

                //var spElement =
                //    new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt,
                //                                           type == "master" ? "UpdateMoveMaster" : "UpdateMoveAgent");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, type == "master" ? "UpdateMoveMaster" : "UpdateMoveAgent");
                if (type == "master")
                {
                    hash.Add("supername", recommendId);
                    hash.Add("mastername", custId);

                    
                    ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                    //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMoveMaster", hash, DBType.MAIN, 600);
                    if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                    {
                        error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                        error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                    }
                }
                else if (type == "agent")
                {
                    hash.Add("agentname", custId);
                    hash.Add("mastername", recommendId);

                    ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                    //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMoveAgent", hash, DBType.MAIN, 600);
                    if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                    {
                        error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                        error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        public static string MoveMasterDepositCustomer(string custId, string recommendId, int accId)
        {
            var hashParam = new Hashtable();

            hashParam.Add("supername", recommendId);
            hashParam.Add("mastername", custId);

            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
            hashParam.Add("userid", accId);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);
            using (var objDb = new Fanex.Data.ObjectDb("CustomerUpdMoveDepositMaster"))
            {
                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                if (CastAide.AsInt32(objDb.GetParameterValue("ErrCode")) != 0)
                    return CastAide.AsString(objDb.GetParameterValue("ErrMsg"));
                return "Move master " + custId + " to super " + recommendId + " successfully!";
            }
        }

        public static string MoveAgentDepositCustomer(string custId, string recommendId, int accId)
        {
            var hashParam = new Hashtable();

            hashParam.Add("mastername", recommendId);
            hashParam.Add("agentname", custId);

            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
            hashParam.Add("userid", accId);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);
            using (var objDb = new Fanex.Data.ObjectDb("CustomerUpdMoveDepositAgent"))
            {
                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                if (CastAide.AsInt32(objDb.GetParameterValue("ErrCode")) != 0)
                    return CastAide.AsString(objDb.GetParameterValue("ErrMsg"));
                return "Move agent " + custId + " to master " + recommendId + " successfully!";
            }
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 14/12/2006
        /// call sp CustomerUpdUserGroup for update user group
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IbcError CustomerUpdUserGroupRpt(string username, string ugroup, string accId)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                DbResult ds = null;
                var hash = new Hashtable();
                hash.Add("username", username);
                hash.Add("ugroup", ugroup);
                hash.Add("userid", accId);
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateUserGroup");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateUserGroup");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdUserGroup", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// call SP CustomerUpdAutoSuspend to update Suspend
        /// writer:Vu Hanh
        /// </summary>
        /// <param name="custId">custId</param>
        /// <param name="autoSuspend">autoSuspend</param>
        /// <param name="userId">userId</param>
        /// <returns>Boolean</returns>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        public static IbcError CustomerUpdAutoSuspendRpt(int custId, int autoSuspend, int userId)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId); //int
                hash.Add("autosuspend", autoSuspend); //bit
                hash.Add("userid", userId); //int
                hash.Add("returnerrcode", 1);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateAutoSuspend");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateAutoSuspend");
                var ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("Acc_CustomerUpdAutoSuspend", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// This function is using add/update member
        /// Using DNAide's Library
        /// Tho Nguyen updated: 11/26/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="IsInsert">IsInsert = true : insert , IsInsert = false: update</param>
        /// <returns></returns>
        public static IbcError AgentInsUdpMember(Hashtable hash, bool IsInsert, ref string custid)
        {
            IbcError error = IbcError.SuccessResult;
            DbResult ds = null;
            try
            {
                //if (IsInsert) ds = DbHelper.ExecuteCmd("Acc_CustomerInsMember_Ag", hash, DBType.MAIN, 600);
                //else ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMember_All", hash, DBType.MAIN, 600);

                //var spElement =
                //    new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt,
                //                                           IsInsert ? "InsertMember" : "UpdateMember");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, IsInsert ? "InsertMember" : "UpdateMember");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);

                if (ds != null)
                {
                    if (IsInsert && ds.Outputs["custid"] != null)
                    {
                        custid = ds.Outputs["custid"].ToString();
                        error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                        error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                    }
                    else
                    {
                        error.ErrorMsg = "";
                        AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg,
                                                ds.Outputs["errcode"].ToString(), Convert.ToString(ds.Outputs["errmsg"]));
                    }
                }
            }
            catch (Exception ex)
            {
                error = IbcError.InternalError;
                
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Using DNAide's Lib
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static DbResult CustomerSelEditCustomer(Hashtable hash)
        {
            DbResult ds = null;
            try
            {
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "SelEditCustomer");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "SelEditCustomer");
                ds = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.Execute("acc_CustomerSelEditCustomer", hash, DBType.ARCHIVE, 600);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 11/29/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static DbResult UpdTransferOption(Hashtable hash)
        {
            DbResult ds = null;
            try
            {
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateTransferOption");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateTransferOption");
                ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //ds = DbHelper.ExecuteCmd("Acc_CustomerUpdTransferOption", hash, DBType.MAIN, 600);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static IbcError UpdVirtualRate(string custId, decimal virtualRate, string userId)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId); //int
                hash.Add("virtualmultiplier", virtualRate); //bit
                hash.Add("userid", userId); //int                
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateVirtualRate");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateVirtualRate");
                var ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("Acc_CustomerUpdVirtualMultiplier", hash, DBType.MAIN, 600);
                if (Convert.ToInt32(ds.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                    error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return error;
        }

        /// <summary>
        /// The function is using Super edit member
        /// Using DNAide's Library
        /// Tho Nguyen updated: 11/27/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static IbcError UpdSuperOnMember(Hashtable hash)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateSuperOnMember");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateSuperOnMember");
                var ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMember_Super", hash, DBType.MAIN, 600);
                if (ds != null)
                {
                    error.ErrorMsg = "";
                    AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, ds.Outputs["errcode"].ToString(),
                                            Convert.ToString(ds.Outputs["errmsg"]));
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// The function is using Master edit member
        /// Using DNAide's Library
        /// Tho Nguyen updated: 11/27/2008
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static IbcError UpdMasterOnMember(Hashtable hash)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateMasterOnMember");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateMasterOnMember");
                var ds = DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("UpdateMasterOnMember", hash, DBType.MAIN, 600);
                if (ds != null)
                {
                    error.ErrorMsg = "";
                    AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, ds.Outputs["errcode"].ToString(),
                                            Convert.ToString(ds.Outputs["errmsg"]));
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        public static void CustomerUpdUserSearch(int userid, string search, string firstname, string lastname)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("userid", userid);
                param.Add("search", search);
                param.Add("firstname", firstname);
                param.Add("lastname", lastname);

                //var spElement = new UpdateCustomer().GetStoreProcedure(StoreProcedureSection.StoreCollection.SportBooksPt, "UpdateUserSearch");
                var spElement = new UpdateCustomer().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "UpdateUserSearch");
                var ds = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 600);
                //DbResult ds = DbHelper.ExecuteCmd("Acc_CustomerUpdUserSearch", param, DBType.ARCHIVE);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}