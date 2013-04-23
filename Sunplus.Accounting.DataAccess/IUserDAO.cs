using System;
using System.Collections.Generic;
using DNAide.Data;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Common;
using System.Data;

namespace Sunplus.Accounting.DataAccess
{
    /// <summary>
    /// Summary description for IUserDAO
    /// </summary>
    public interface IUserDAO
    {
        DbResult InsResetWinning(DateTime dateTime, int userId, string[] arrDesc, string IP);
        List<ResetWinning> GetResetWinning(DateTime dtmMoveBetDate, DateTime dtmTodate);
        List<FinanceTransaction> GetResetWinning_ByDate(DateTime transDate, bool isReset);
        DateTime MoveBetDateSel();

        DbResult InsDataTmpExcel(int userId, string arrFromCust, string arrToCust, string arrAmount, string arrExRate,
                                 string arrDescription);

        DbResult InsDataTmpExcelFinanceCrossTransfer(int userId, string arrFromCust, string arrToCust, string arrFromAmount,
                                        string arrToAmount, string arrDescription);

        bool DelDataTmpExcel(int userId);
        DbResult UpdFinanceTransferMul(int userId, string fileName, int fileSize, string IP);

        DbResult UpdFinanceCrossTransferMul(int userId, string fileName, int fileSize, string IP, string forexName);

        List<DailyTransaction> GetDailyTransaction(DateTime transDate, int pageIndex, int pageSize,
                                                   int status, string type, string creator, ref int totalPages,
                                                   string verifyBy);
        List<DailyTransaction>GetDailyAllTransaction(DateTime transDate, int pageIndex, int pageSize, string type, string creator, ref int totalPages);

        DbResult GetDailyAllTransaction(DateTime transDate, string type, string creator);

        DailyTransactionDTO GetDailyTransaction(DateTime transDate, int status, string type, string creator, string verifyBy);

        bool UpdateVerifyStatus(User user, string refNoList, bool isVerified);

        List<Statement> GetCustomerStatement(User customer, DateTime fromDate, DateTime toDate, DateTime balanceUpDay,
                                             bool isInlucdeAccBal);

        int GetCustIdByUserName(string custname);

        List<MenuBO> GetUserMenu(int custId);

        List<MenuBO> ManageMenuGetMenuList(string system);

        Result InsMenuItem(MenuBO menu);

        Result UdpMenuItem(MenuBO menu);

        Result DelMenuItem(int menuId);

        List<MenuBO> GetGroupFunc();

        List<FinanceBettype> GetFinanceBettypes();

        Result InsertCustomerToGroup(int custId, int roleId, string system);

        List<MenuBO> GetReportMenu(int custId, string system);
    }
}