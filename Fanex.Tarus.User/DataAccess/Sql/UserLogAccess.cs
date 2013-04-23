using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Fanex.Data;
using DNAide.Core;

namespace Fanex.Tarus.User.DataAccess.Sql
{
    public class UserLogAccess : IUserLogAccess
    {
        public DataTable UserControlLogSelect(DateTime fromDate, DateTime toDate, int type, int custId, string userName, int pageIndex, int pageSize)
        {
            using (ObjectDb objDB = new ObjectDb("UserLog_UserControlSel"))
            {
                var param = new Hashtable();
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("typeid", type);
                param.Add("custid", custId);
                param.Add("name", string.IsNullOrWhiteSpace(userName) ? string.Empty : userName);
                param.Add("pageindex", pageIndex);
                param.Add("pagesize", pageSize);

                objDB.AssignParamValues(param);
                DataSet dataSet = objDB.ExecuteDataSet();

                if (dataSet.Tables.Count > 0) return dataSet.Tables[0];
                return null;
            }
        }


        public int GetPagesTotal(DateTime fromDate, DateTime toDate, int type, int custId, string userName)
        {
            using (ObjectDb objDB = new ObjectDb("UserLog_UserControlSelTotalRecords"))
            {
                var param = new Hashtable();
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("typeid", type);
                param.Add("custid", custId);
                param.Add("name", string.IsNullOrWhiteSpace(userName) ? string.Empty : userName);

                objDB.AssignParamValues(param);
                DataSet dataSet = objDB.ExecuteDataSet();

                if (dataSet.Tables.Count > 0)
                {
                    DataRow[] rows = dataSet.Tables[0].Select();
                    if (rows.Count() > 0)
                    {
                        return CastAide.AsInt32(rows[0].ItemArray[0].ToString());
                    }
                }
                return 0;
            }
        }


        public int GetNameFromId(int typeId, string name, out string message)
        {
            using (ObjectDb objDB = new ObjectDb("Con_CustomerSelByUserName"))
            {
                var param = new Hashtable();
                param.Add("typeid", typeId);
                param.Add("name", name);
                param.Add("custid", 0);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                objDB.AssignParamValues(param);
                objDB.ExecuteNonQuery();
                if (CastAide.AsInt32(objDB.GetParameterValue("errcode")) == 0)
                {
                    message = string.Empty;
                    return CastAide.AsInt32(objDB.GetParameterValue("custid"));
                }
                message = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                return -1;
            }
        }
    }
}
