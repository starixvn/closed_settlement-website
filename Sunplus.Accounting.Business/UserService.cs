using System.Linq;
using System;
using System.Collections.Generic;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;
using System.Data;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for UserService
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        private ILogger _logger;
        private string _keyDailyAllTxn = AppSession.Username + ".SelDailyAllTxn";
        private string _keyDailyFinanceTxn = AppSession.Username + ".SelDailyFinanceTxn";

        public UserService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }
              


        #region IUserService Members

        public bool InsResetWinning(DateTime dateTime, int userId, string[] arrDesc, string IP, ref string ErrMsg)
        {
            bool isResetSuccess = false;

            if (dateTime > DateTime.Now || userId <= 0)
                isResetSuccess = false;

            //try
            //{
            //    isResetSuccess = _userDAO.ResetWinning(dateTime);
            //}
            //catch (Exception ex)
            //{                
            //    isResetSuccess = false;
            //}

            try
            {
                DbResult rs = _userDAO.InsResetWinning(dateTime, userId, arrDesc, IP);
                isResetSuccess = (CastAide.AsInt32(rs.Outputs["errcode"]) == 0);
                ErrMsg = rs.Outputs["errmsg"].ToString();
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
                isResetSuccess = false;
            }

            return isResetSuccess;

            //throw new NotImplementedException();
        }

        public List<ResetWinning> GetResetWinning()
        {
            DateTime dtmMoveBetDate = _userDAO.MoveBetDateSel();
            DateTime currentTime = DateTime.Now;

            // to date =  datetime.now 12:00:00
            List<ResetWinning> list = _userDAO.GetResetWinning(dtmMoveBetDate,
                                                               new DateTime(currentTime.Year, currentTime.Month,
                                                                            currentTime.Day, 12, 0, 0));
            return list;
        }


        public List<FinanceTransaction> GetResetWinning_ByDate(DateTime transDate, bool isReset)
        {
            List<FinanceTransaction> list = _userDAO.GetResetWinning_ByDate(transDate, isReset);
            return list;
        }


        public DbResult InsDataTmpExcel(int userId, string arrFromCust, string arrToCust, string arrAmount,
                                        string arrExRate, string arrDescription, ref bool isErr)
        {
            int maxLength = 4000;
            if (userId <= 0 || arrFromCust.Length > maxLength || arrToCust.Length > maxLength ||
                arrAmount.Length > maxLength || arrExRate.Length > maxLength || arrDescription.Length > maxLength)
            {
                isErr = false;
                return null;
            }

            return _userDAO.InsDataTmpExcel(userId, arrFromCust, arrToCust, arrAmount, arrExRate, arrDescription);
        }

        public DbResult InsDataTmpExcelFinanceCrossTransfer(int userId, string arrFromCust, string arrToCust, string arrFromAmount,
                                        string arrToAmount, string arrDescription, ref bool isErr)
        {
            int maxLength = 4000;
            if (userId <= 0 || arrFromCust.Length > maxLength || arrToCust.Length > maxLength ||
                arrFromAmount.Length > maxLength || arrToAmount.Length > maxLength || arrDescription.Length > maxLength)
            {
                isErr = false;
                return null;
            }

            return _userDAO.InsDataTmpExcelFinanceCrossTransfer(userId, arrFromCust, arrToCust, arrFromAmount, arrToAmount, arrDescription);
        }

        public bool DelDataTmpExcel(int userId)
        {
            bool isSuccess = true;
            if (userId <= 0)
                isSuccess = false;

            isSuccess = _userDAO.DelDataTmpExcel(userId);
            return isSuccess;
        }

        public DbResult UpdFinanceTransferMul(int userId, string fileName, int fileSize, string IP)
        {
            if (userId <= 0 || fileSize <= 0 || string.IsNullOrEmpty(fileName))
                return null;
            return _userDAO.UpdFinanceTransferMul(userId, fileName, fileSize, IP);
        }

        public DbResult UpdFinanceCrossTransferMul(int userId, string fileName, int fileSize, string IP, string forexName)
        {
            if (userId <= 0 || fileSize <= 0 || string.IsNullOrEmpty(fileName))
                return null;
            return _userDAO.UpdFinanceCrossTransferMul(userId, fileName, fileSize, IP, forexName);
        }

        /// <summary>
        /// Get daily finance transaction
        /// </summary>
        /// <param name="transDate">valid date must be less than or equal today</param>
        /// <param name="pageIndex">must be > 0</param>
        /// <param name="pageSize">must be > 0</param>
        /// <param name="status">2: All, 1: Verified, 0:Unverified</param>
        /// <param name="type"></param>
        /// <param name="creator">who create transaction</param>
        /// <param name="totalPages"></param>
        /// <returns><![CDATA[List<DailyTransaction>]]></returns>
        public List<DailyTransaction> GetDailyTransaction(DateTime transDate, int pageIndex, int pageSize,
                                                          int status, string type, string creator, ref int totalPages,
                                                          string verifyBy)
        {
            Check.Argument.IsNotNegative(pageIndex, "pageIndex");
            Check.Argument.IsNotNegative(pageSize, "pageSize");
            Check.Argument.IsNotOutOfRange(status, 0, 2, "status");
            Check.Argument.IsNotInvalidDate(transDate, "transDate");
           // Check.Argument.IsNotInFuture(transDate, "transDate");

            var entityList = new List<DailyTransaction>();
            try
            {
                //always get valid transactions
                if (transDate <= DateTime.Now)
                {
                    entityList = _userDAO.GetDailyTransaction(transDate,
                                                              pageIndex, pageSize, status, type, creator, ref totalPages,
                                                              verifyBy);
                }
            }
            catch (Exception ex)
            {
                
                {
                    //will refactor later
                    // _logger = new EntLibLogger();
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }


            return entityList;
        }

        /// <summary>
        /// Get list of total number by transaction group
        /// </summary>
        /// <param name="transList"></param>
        /// <returns></returns>
        public int GetListTotalTransPerGroup(List<DailyTransaction> transList, string RefNo)
        {
            Check.Argument.IsNotNull(transList, "transList");
            Dictionary<string, int> items = transList.AsQueryable()
                .GroupBy(entity => entity.RefNo)
                .Select(entityGroup => new
                                           {
                                               RefNo = entityGroup.Key,
                                               Total = entityGroup.Count()
                                           }
                )
                .ToDictionary(entity => entity.RefNo, entity => entity.Total);
            return items[RefNo];
        }

        public Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> transList)
        {
            Check.Argument.IsNotNull(transList, "transList");
            var  items = new Dictionary<string, int>();
            items = transList.AsQueryable().GroupBy(entity => entity.RefNo).Select(entityGroup => new
                                                                                                      {
                                                                                                          RefNo = entityGroup.Key,
                                                                                                          Total = entityGroup.Count()
                                                                                                      }).ToDictionary(entity => entity.RefNo, entity => entity.Total);
            return items;
        }

        /// <summary>
        /// verify multi transaction
        /// </summary>
        /// <param name="user"></param>
        /// <param name="refNos"></param>
        /// <returns></returns>
        public bool VerfiyDailyTransactions(User user, string refNos)
        {
            Check.Argument.IsNotNull(user, "user");
            Check.Argument.IsNotNullOrEmpty(refNos, "refNos");

            bool isSuccessful = false;
            ;
            try
            {
                refNos = refNos.Trim().TrimEnd(',');
                isSuccessful = _userDAO.UpdateVerifyStatus(user, refNos, true);
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }

            return isSuccessful;
        }

        /// <summary>
        /// unverify transactions have the same refNo
        /// </summary>
        /// <param name="user">who perform action unverify</param>
        /// <param name="refNo"></param>
        /// <returns></returns>
        public bool UnverifyTransaction(User user, string refNo)
        {
            Check.Argument.IsNotNull(user, "user");

            bool isSuccessful = false;
            ;
            try
            {
                isSuccessful = _userDAO.UpdateVerifyStatus(user, refNo.ToString(), false);
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }

            return isSuccessful;
        }

        /// <summary>
        /// Get customer statement
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="balanceUpDay"></param>
        /// <param name="isInlucdeAccBal"></param>
        /// <returns></returns>
        public List<Entities.Statement> GetCustomerStatement(User customer, DateTime fromDate, DateTime toDate,
                                                    DateTime balanceUpDay, bool isInlucdeAccBal)
        {
            Check.Argument.IsNotNull(customer, "customer");
            //Check.Argument.IsNotInFuture(fromDate, "fromDate");
            //Check.Argument.IsNotInFuture(toDate, "toDate");
            
            Check.Argument.IsNotInFuture(balanceUpDay, "balanceUpDay");

            var entityList = new List<Entities.Statement>();
            if (!Check.Argument.IsValidDateRange(fromDate, toDate, "fromDate must be <= toDate"))
            {
                return entityList;
            }
            try
            {
                //get customer id
                customer.Id = GetCustomerIdByUsername(customer.Username);

                if (customer.Id > 0)
                {
                    entityList = _userDAO.GetCustomerStatement(
                        customer,
                        fromDate,
                        toDate,
                        balanceUpDay,
                        isInlucdeAccBal);
                }
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }

            return entityList;
        }

        public int GetCustomerIdByUsername(string custName)
        {
            int custId = 0;

            try
            {
                if (!string.IsNullOrEmpty(custName))
                {
                    custId = _userDAO.GetCustIdByUserName(custName);
                }
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }

            return custId;
        }

        public List<FinanceBettype> GetFinanceBettyles()
        {
            var entityList = new List<FinanceBettype>();
            try
            {
                entityList = _userDAO.GetFinanceBettypes();
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }
            return entityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public Result InsertCustomerToGroup(int custId, int roleId, string system)
        {
            var result = new Result();
            return _userDAO.InsertCustomerToGroup(custId, roleId, system);
            throw new NotImplementedException();
        }

        public List<DailyTransaction> GetDailyAllTransaction(DateTime transDate, string type, string creator, int pageIndex, int pageSize, ref int totalPage)
        {
            Check.Argument.IsNotNegative(pageIndex, "pageIndex");
            Check.Argument.IsNotNegative(pageSize, "pageSize");
            Check.Argument.IsNotInvalidDate(transDate, "transDate");
            //Check.Argument.IsNotInFuture(transDate, "transDate");            

            var entityList = new List<DailyTransaction>();

            try
            {
                //always get valid transactions
                if (transDate <= DateTime.Now)                
                    entityList = _userDAO.GetDailyAllTransaction(transDate, pageIndex, pageSize, type, creator, ref totalPage);
                
            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }
           
            return entityList;
        }

       /* public DailyTransactionDTO GetDailyAllTransaction(DateTime transDate, string type, string creator, int pageIndex, int pageSize, ref int totalPage)
        {
            Check.Argument.IsNotNegative(pageIndex, "pageIndex");
            Check.Argument.IsNotNegative(pageSize, "pageSize");
            Check.Argument.IsNotInvalidDate(transDate, "transDate");
            //Check.Argument.IsNotInFuture(transDate, "transDate");            

            var entityList = new DailyTransactionDTO();

            try
            {
                //always get valid transactions
                if (transDate <= DateTime.Now)
                {
                    DbResult dbResult = _userDAO.GetDailyAllTransaction(transDate, pageIndex, pageSize, type, creator, ref totalPage);
                    if (dbResult != null && dbResult.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dbResult.ResultSet.Tables[0].Rows)
                        {
                            var entity = new DailyTransaction();
                            entity.Username = (string)dr["username"];
                            entity.Date = (DateTime)dr["transdate"];
                            entity.Amount = Convert.ToDouble(dr["amount"]);
                            entity.Description = (string)dr["transdesc"];
                            entity.Type = (string)dr["typenamee"];
                            entity.Creator = (string)dr["creator"];
                            entity.RowIndex = Convert.ToInt32(dr["rownumber"]);
                            entityList.items.Add(entity);
                        }

                        //get total record                
                        entityList.totalRows = dbResult.ResultSet.Tables[0].Rows.Count;
                    }     
                }

            }
            catch (Exception ex)
            {
                
                {
                    _logger = CommonBootStrapper.Locator.GetInstance<Sunplus.Core.LoggingLib.ILogger>();
                    _logger.Error(ex.Message, ex);
                }
            }

            return entityList;
        }*/

        #endregion

       
    }
}
