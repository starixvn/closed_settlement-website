using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities.Customer;
using DNAide.Utils;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    public class CustomerDAO : DataAccess, ICustomerDAO
    {

        protected Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
        }
        
        public DataTable GetCustomerInfo(string username)
        {
            var param = new System.Collections.Hashtable();
            param.Add("username", username);
            DataTable dtResult = null;
            var spElement = GetStoreProcedure("CustomerSelSiteByUserName");
            DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                dtResult = ds.ResultSet.Tables[0];
            return dtResult;
        }

        public Result UpdateCustomerSite(int custId, string newSite, int userId, string ip)
        {
            var result = new Result();

            var hash = new Hashtable
                           {
                               {"userid", userId},
                               {"custid", custId},
                               {"site", newSite},
                               {"errcode", 0},
                               {"errmsg", ""},
                               {"ip", ip}
                           };

            DbResult rs = DbHelper.Execute("Acc_CustomerUpdMoveSite", hash, DBType.MAIN, 1000);

            result.ErrorCode =  CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);
            return result;
        }

        public Result UpdateLevelDangerous(int custId, int updateDownline, int dangerType, int dangerLevel, int userId, string ip)
        {
            var result = new Result();
            var param = new Hashtable();
            param.Add("custid", custId);
            param.Add("updatedownline", updateDownline);
            param.Add("dangertype", dangerType);
            param.Add("dangerlevel", dangerLevel);
            param.Add("userid", userId);
            param.Add("ip", ip);

            var spElement = GetStoreProcedure("UpdateDangerous");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            return result;
        }

        public DbResult GetAllCustInfo(string username)
        {
            DbResult ds = null;
            var dtResult = new DataTable();
            try
            {
                var param = new Hashtable
                                {
                                    {"username", username}
                                };
                var spElement = GetStoreProcedure("SelAllInfo");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
              
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        #region Enable Number Game for downlines

        public BetSettingNG CheckEnableNGDownline(string username)
        {
            var betsetting = new BetSettingNG();
            var spElement = GetStoreProcedure("CheckUplineEnableNG");
            var param = new Hashtable();
            param.Add("username", username);
            param.Add("userid", AppSession.AccId);
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            betsetting.errCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            betsetting.errMsg = CastAide.AsString(rs.Outputs["errmsg"]);
            betsetting.minbet = NumberFormat.DecFormat(rs.Outputs["minbet"], 0);
            betsetting.maxbet = NumberFormat.DecFormat(rs.Outputs["maxbet"], 0);
            betsetting.maxperball = NumberFormat.DecFormat(rs.Outputs["maxperball"], 0);
            betsetting.maxpermach = NumberFormat.DecFormat(rs.Outputs["maxpermatch"], 0);
            betsetting.isEnable = CastAide.AsBoolean(rs.Outputs["isenable"]) ? 1 : 0;
            return betsetting;
        }

        public Result UpdateBetSettingNGDownline(string username, int isEnable, string maxBet, string minBet, string maxMatch, string maxBall)
        {
            var result = new Result();
            var spElement = GetStoreProcedure("UpdateDownlineEnableNG");
            var param = new Hashtable();
            param.Add("username", username);
            param.Add("isenable", isEnable);
            param.Add("minbet", CastAide.AsDecimal(minBet));
            param.Add("maxbet", CastAide.AsDecimal(maxBet));
            param.Add("maxperball", CastAide.AsDecimal(maxBall));
            param.Add("maxpermatch", CastAide.AsDecimal(maxMatch));
            param.Add("userid", AppSession.AccId);
            param.Add("ip", HttpContext.Current.Request.UserHostAddress);
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);
            return result;
        }

        #endregion
    }
}
