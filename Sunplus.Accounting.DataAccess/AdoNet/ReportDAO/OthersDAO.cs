using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class OthersDAO: DataAccess, IReportDAO.IOthersDAO
    {
        public DbResult GetMenuList(string system)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("custid", Sunplus.Accounting.Core.Common.AppSession.AccId);
            hashtable.Add("system", system);
            return DbHelper.Execute("Acc_MenuSelByCustID", hashtable, DBType.DB03, 600);
        }

        public DbResult GetMixParlayWonList(bool isSoccer, DateTime date)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, "MixparlayWon");

            Hashtable hashtable = new Hashtable();
            hashtable.Add("issoccer", isSoccer);
            hashtable.Add("winlostdate", date);
            hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }

        public DbResult CombinationGetMixParlayWonList(bool isSoccer, DateTime date)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, "CombinationMixparlayWon");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("issoccer", isSoccer);
            hashtable.Add("winlostdate", date);
            hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            hashtable.Add("bettype", 29);
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }
       

        public DbResult GetCheckBet(DateTime winlostDate)
        {
            var p = new Hashtable();
            p.Add("winlostdate", winlostDate);
            var spElement =
                GetStoreProcedure(Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others,
                                  "checkhack2");
            return DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
            var result = DbHelper.Execute(spElement.Name,p, spElement.DbName, 180);// DbHelper.Execute("Acc_checkhack2", p, "db052");
            return result;
        }

    }
}
