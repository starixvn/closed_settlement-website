using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IBetAggregatorDAO;
using Sunplus.Accounting.Entities;

using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.AdoNet.BetAggregatorDAO
{
    public class LicenseeConfigDAO : DataAccess, ILicenseeConfigDAO
    {
        protected StoreProcedureElement GetBaStoreProcedure(string storeAlias)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.BetAggregatorSection.StoreCollection.BaStores, storeAlias);
            return spElement;
        }


        public DataTable SelectAllLicensee()
        {
            DataTable dtResult = null;
            var spElement = GetBaStoreProcedure("SelAllLicensee");
           
            var ds = DbHelper.Execute(spElement.Name, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                dtResult = ds.ResultSet.Tables[0];
            return dtResult;
        }

        public DataTable SelLicenseeConfigCommLog(int licenseeId)
        {
            DataTable dtResult = null;
            var spElement = GetBaStoreProcedure("SelLicenseeConfigCommLog");
            var param = new Hashtable();
            if (licenseeId > 0)
            { param.Add("licenseeid", licenseeId); }
            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                dtResult = ds.ResultSet.Tables[0];
            return dtResult;
        }

        public DataTable SelectLicenseeConfigList()
        {
            DataTable dtResult = null;
            var spElement = GetBaStoreProcedure("SelLicenseeConfig");
            var ds = DbHelper.Execute(spElement.Name, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                dtResult = ds.ResultSet.Tables[0];
            return dtResult;
            
        }


        public Result InsertLicenseeConfig(int licenseeId, double hdpComm, double oneXTwoComm, double otherComm, double ngComm, bool enableBa)
        {
            var result = new Result();
            var param = new Hashtable();
            param.Add("licenseeid",licenseeId);
            param.Add("hdpcomm",hdpComm);
            param.Add("1x2comm", oneXTwoComm);
            param.Add("othercomm",otherComm);
            param.Add("ngcomm",ngComm);
            param.Add("enableba", enableBa);
            param.Add("userid", AppSession.AccId);
            param.Add("ip", HttpContext.Current.Request.UserHostAddress);
            var spElement = GetBaStoreProcedure("InsLicenseeConfig");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            return result;
        }

        public Result UpdateLicenseeConfig(int licenseeId, double groupa, double groupb, double groupc, double groupd, double oneXTwoComm, double otherComm, bool enableBa)
        {
            var result = new Result();
            var param = new Hashtable();
            param.Add("licenseeid",licenseeId);
            param.Add("groupa", groupa);
            param.Add("groupb", groupb);
            param.Add("groupc", groupc);
            param.Add("groupd", groupd);
            //param.Add("ugroup", ugroup);
            param.Add("1x2comm", oneXTwoComm);
            param.Add("othercomm", otherComm);
            param.Add("enableba", enableBa);
            param.Add("userid", AppSession.AccId);
            param.Add("ip", HttpContext.Current.Request.UserHostAddress);
            var spElement = GetBaStoreProcedure("UpdLicenseeConfig");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            return result;
        }

        public Result DeleteLicenseeConfig(int licenseeId)
        {
            var result = new Result();
            var param = new Hashtable();
            param.Add("licenseeid", licenseeId);
            param.Add("userid", AppSession.AccId);
            param.Add("ip", HttpContext.Current.Request.UserHostAddress);
            var spElement = GetBaStoreProcedure("DelLicenseeConfig");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);

            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            return result;
            
        }

        public int CheckDownlineBA(int licenseeId)
        {
            var param = new Hashtable();
            param.Add("licenseeid", licenseeId);
            param.Add("bamembercount", 0);
            var spElement = GetBaStoreProcedure("DownlineCheckBA");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            return CastAide.AsInt32(rs.Outputs["bamembercount"]);
        }

        public DataTable GetBALicWinlossDaily(System.DateTime wldate)
        {
            var spElement = GetBaStoreProcedure("Acc_Rpt_BALicWinlossDaily");
            var param = new Hashtable();
            param.Add("date", wldate.ToShortDateString());

            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName, 36000);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                return ds.ResultSet.Tables[0];
            return null;
        }

        public DataTable GetBALicWinlossMonthToDate(System.DateTime wldate)
        {
            var spElement = GetBaStoreProcedure("Acc_Rpt_BALicWinloss");
            var param = new Hashtable();
            param.Add("fromdate", wldate.Month.ToString() + "/1/" + wldate.Year.ToString());
            param.Add("todate", wldate.ToShortDateString());

            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName, 36000);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                return ds.ResultSet.Tables[0];
            return null;
        }

        public DataTable GetBALicWinlossMonthly(System.DateTime wldate)
        {
            var spElement = GetBaStoreProcedure("Acc_Rpt_BALicWinlossMonthly");
            var param = new Hashtable();
            param.Add("fromdate", wldate.Month.ToString() + "/1/" + wldate.Year.ToString());
            param.Add("todate", wldate.ToShortDateString());

            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName, 36000);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                return ds.ResultSet.Tables[0];
            return null;
        }

        public DataTable GetBALicWinlossYearToDate(System.DateTime wldate)
        {
            var spElement = GetBaStoreProcedure("Acc_Rpt_BALicWinloss");
            var param = new Hashtable();
            param.Add("fromdate", "1/1/" + wldate.Year.ToString());
            param.Add("todate", wldate.ToShortDateString());

            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                return ds.ResultSet.Tables[0];
            return null;
        }

        public DataTable GetAmtLimitLicensees()
        {
            var spElement = GetBaStoreProcedure("Task_Licensee_BAGetListLicenseeWithLimitAmt");
            var rs = DbHelper.Execute(spElement.Name, spElement.DbName);
            if (rs != null || rs.ResultSet.Tables.Count > 0)
            {
                return rs.ResultSet.Tables[0];
            }
            return null;
        }

        public DbResult BALimitWarning(int licenseeID, string licenseeName, double limitAmt)
        {
            
            
            var param = new Hashtable();
            param.Add("licenseeid", licenseeID);
            param.Add("licenseename", licenseeName);
            param.Add("limitamt", limitAmt);
            var spElement = GetBaStoreProcedure("Task_Licensee_BALimitWarning");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);

            return rs;
        }


        public DataTable SelBAWinLossByCommLevel(int licenseeId, DateTime fromDate, DateTime toDate, int currencyId, int drawFilter, bool isExcludeTest)
        {
            var spElement = GetBaStoreProcedure("LicWLByCommLevel");
            var param = new Hashtable();
            param.Add("licenseeid", licenseeId);
            param.Add("fromdate", fromDate);
            param.Add("todate", toDate);
            param.Add("currency", currencyId);
            param.Add("drawfilter", drawFilter);
            param.Add("excludetest", isExcludeTest);
            
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (rs != null || rs.ResultSet.Tables.Count > 0)
            {
                return rs.ResultSet.Tables[0];
            }
            return null;
        }


        public DbResult SelGroupCurrencyByLicensee(int licenseeId)
        {
            var spElement = GetBaStoreProcedure("BAGroupCurrencySelByLicensee");
            var param = new Hashtable();
            param.Add("licenseeid", licenseeId);

            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            return rs;
        }


        public Result UpdateGroupSettingByLicensee(int licenseeId, string setting)
        {
            var result = new Result();
            var param = new Hashtable();
            param.Add("licenseeid", licenseeId);
            param.Add("modifyby", AppSession.AccId);
            param.Add("str", setting);
            param.Add("errcode", 0);
            param.Add("errmsg", "");
            param.Add("ip", HttpContext.Current.Request.UserHostAddress);

            var spElement = GetBaStoreProcedure("BAGroupCurrencyUpdByLicensee");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);

            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            return result;
        }
    }
}
