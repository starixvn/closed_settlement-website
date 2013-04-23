using System;
using System.Collections.Generic;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for IUserService
    /// </summary>
    public interface IUserService
    {
        bool InsResetWinning(DateTime dateTime, int userId, string[] arrDesc, string IP, ref string ErrMsg);
        List<ResetWinning> GetResetWinning();
        List<FinanceTransaction> GetResetWinning_ByDate(DateTime transDate, bool isReset);

        DbResult InsDataTmpExcel(int userId, string arrFromCust, string arrToCust, string arrAmount, string arrExRate,
                                 string arrDescription, ref bool isErr);

        DbResult InsDataTmpExcelFinanceCrossTransfer(int userId, string arrFromCust, string arrToCust, string arrFromAmount,
                                        string arrToAmount, string arrDescription, ref bool isErr);

        bool DelDataTmpExcel(int UserId);
        DbResult UpdFinanceTransferMul(int userId, string fileName, int fileSize, string IP);

        DbResult UpdFinanceCrossTransferMul(int userId, string fileName, int fileSize, string IP, string forexName);

        List<DailyTransaction> GetDailyTransaction(DateTime transDate, int pageIndex, int pageSize,
                                                   int status, string type, string creator, ref int totalPages,
                                                   string verifyBy);
        List<DailyTransaction> GetDailyAllTransaction(DateTime transDate, string type, string creator, int pageIndex, int pageSize, ref int totalPage);

        //DailyTransactionDTO GetDailyAllTransaction(DateTime transDate, string type, string creator, int pageIndex, int pageSize, int fromcache);

        bool VerfiyDailyTransactions(User user, string refNos);
        bool UnverifyTransaction(User user, string refNo);

        /// <summary>
        /// Get dictionary group, key contanins refNo, value is the total number transaction
        /// has the same ref no
        /// </summary>
        /// <param name="transList"></param>
        /// <returns></returns>
       Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> transList);

        int GetListTotalTransPerGroup(List<DailyTransaction> transList, string RefNo);

        List<Entities.Statement> GetCustomerStatement(User customer, DateTime fromDate, DateTime toDate, DateTime balanceUpDay,
                                             bool isInlucdeAccBal);

        int GetCustomerIdByUsername(string custName);

        List<FinanceBettype> GetFinanceBettyles();

        Result InsertCustomerToGroup(int custId, int roleId, string system);
    }
}
