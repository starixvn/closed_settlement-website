using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Admin
{
    public class ExternalSite : IExternalSite
    {
       
        private readonly IExternalSiteDAO _custGroupDAO;
        private static string listCustDetail_sessionKey = "ExternalSite_SessionKey";
        public static string groupName_sessionKey = "sessionGroupName";
        public ExternalSite(IExternalSiteDAO custGroupDAO)
        {
            _custGroupDAO = custGroupDAO;
        }

        public List<string> GetGroupName()
        {
            var listGroupName = new List<string>();
            try
            {
                var dbResult = _custGroupDAO.SelGroupName();
                listGroupName.AddRange(from DataRow row in dbResult.Rows select row["GroupName"].ToString());
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return listGroupName;
        }


        public ExternalSiteGridBO PagerCustomers(string groupName, int pageIndex, int pageSize, bool fromcache, string username)
        {
            var externalsitegrid = new ExternalSiteGridBO();
            var listCust = new List<CustDetail>();


            #region get data
            var externalsitegridpaging = new ExternalSiteGridBO();
            try
            {
                var externalsitelistSession = SessionExtension.GetSession(HttpContext.Current.Session, listCustDetail_sessionKey);
                if (externalsitelistSession == null || fromcache == false)
                {
                    listCust = GetInfoCusts(groupName, username);

                }
                else
                {
                    listCust = (List<CustDetail>)externalsitelistSession;
                   

                }
                if (!String.IsNullOrEmpty(username))
                {
                    var filterCust = (from t in listCust
                                      where t.UserName.ToLower().Contains(username.ToLower())
                                      select t);
                    listCust = filterCust.ToList();
                }

                externalsitegrid.rows = listCust;
                externalsitegrid.total = listCust.Count;
                #region paging


                externalsitegridpaging.rows = externalsitegrid.rows.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList(); 
                externalsitegridpaging.total = externalsitegrid.total;
                #endregion
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            #endregion



            return externalsitegridpaging;
        }


        public Entities.Result DeleteCustGroupName(int custId)
        {

            Result result;
            result = _custGroupDAO.DelCust(custId, AppSession.AccId);
            return result;
        }

        public Entities.Result AddCustGroupName(string username, string groupName)
        {
            Result result;
            if(string.IsNullOrEmpty(username))
            {
                result = new Result{ErrorCode = 1, ErrorMessage = "Please input username!"};
                return result;
            }

            result = _custGroupDAO.AddExistCust(username, groupName, AppSession.AccId);
            return result;
        }



        public List<CustDetail> GetInfoCusts(string groupName, string username)
        {

            var listCust = new List<CustDetail>();

            var dbResult = _custGroupDAO.SelInfoCust(groupName);
            foreach (DataRow row in dbResult.Rows)
            {
                var custInfo = new CustDetail();
                custInfo.UserName = row["username"].ToString();
                custInfo.FirstName = row["firstname"].ToString();
                custInfo.LastName = row["lastname"].ToString();
                custInfo.LastLoginIp = row["iplogin"].ToString();
                custInfo.CustId = CastAide.AsInt32(row["custid"].ToString());
                listCust.Add(custInfo);
            }

            //filter by username if not empty
           
            if (listCust.Count > 0)
                SessionExtension.AddSession(HttpContext.Current.Session, listCustDetail_sessionKey, listCust);
            return listCust;

        }
    }
}
