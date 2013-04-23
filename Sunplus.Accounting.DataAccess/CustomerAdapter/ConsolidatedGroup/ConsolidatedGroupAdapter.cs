using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using DNAide.Data;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.CustomerAdapter.ConsolidatedGroup
{
    public class ConsolidatedGroupAdapter
    {
        public static DataSet GetAllGroup()
        {
            var hsParam = new Hashtable();
            var sp = CustomerSession.Instance[CustomerSession.StoreCollection.ConsolidatedGroup]["Acc_CustomerSelConsolidatedGroup"];
            return DbHelper.Execute(sp.Name, hsParam, sp.DbName, 600).ResultSet;
        }

        public static Sunplus.Accounting.Entities.Result UpdateGroup(string UserName, int GroupID, int isHeader,
            string Remark, int UserID, string IP)
        {
            var hsParam = new Hashtable();
            hsParam.Add("username", UserName.Trim());
            //hsParam.Add("groupid", GroupID);
            //hsParam.Add("isheader", isHeader);
            hsParam.Add("remark", Remark);
            hsParam.Add("userid", UserID);
            hsParam.Add("ip", IP);
            var sp = CustomerSession.Instance.ConsolidatedGroup["Acc_CustomerUpdConsolidatedGroup"];
            var dr = DbHelper.Execute(sp.Name, hsParam, sp.DbName, 600);
            return new Sunplus.Accounting.Entities.Result(){
                ErrorMessage = CastAide.AsString(dr.Outputs["errmsg"])
                ,
                ErrorCode = CastAide.AsInt32(dr.Outputs["errcode"])
            };
        }

        public static DataSet GetMemberByGroupID(int GroupID)
        {
            var hsParam = new Hashtable();
            hsParam.Add("groupid", GroupID);
            var sp = CustomerSession.Instance[CustomerSession.StoreCollection.ConsolidatedGroup]["Acc_CustomerSelByConsolidatedGroupID"];
            return DbHelper.Execute(sp.Name, hsParam, sp.DbName, 600).ResultSet;
        }

        public static Sunplus.Accounting.Entities.Result AddUserToGroup(int GroupID, string UserName, string IP)
        {
            var hsParam = new Hashtable();
            hsParam.Add("username", UserName.Trim());
            hsParam.Add("groupid", GroupID);
            hsParam.Add("userid", AppSession.AccId);
            hsParam.Add("ip", IP);
            var sp = CustomerSession.Instance.ConsolidatedGroup["Acc_CustomerInsConsolidatedGroup_User"];
            var dr = DbHelper.Execute(sp.Name, hsParam, sp.DbName, 600);
            return new Sunplus.Accounting.Entities.Result()
            {
                ErrorMessage = CastAide.AsString(dr.Outputs["errmsg"])
                ,
                ErrorCode = CastAide.AsInt32(dr.Outputs["errcode"])
            };
        }

        public static Sunplus.Accounting.Entities.Result RemoveUsers(int GroupID, string CustIDList, string IP)
        {
            var hsParam = new Hashtable();
            hsParam.Add("custidlist", CustIDList);
            hsParam.Add("groupid", GroupID);
            hsParam.Add("userid", AppSession.AccId);
            hsParam.Add("ip", IP);
            var sp = CustomerSession.Instance.ConsolidatedGroup["Acc_CustomerDelConsolidatedGroup"];
            var dr = DbHelper.Execute(sp.Name, hsParam, sp.DbName, 600);
            return new Sunplus.Accounting.Entities.Result()
            {
                ErrorMessage = CastAide.AsString(dr.Outputs["errmsg"])
                ,
                ErrorCode = CastAide.AsInt32(dr.Outputs["errcode"])
            };
        }

        public static Sunplus.Accounting.Entities.Result ChangeHeadline(int GroupID, int CustID, string IP)
        {
            var hsParam = new Hashtable();
            hsParam.Add("custid", CustID);
            hsParam.Add("groupid", GroupID);
            hsParam.Add("userid", AppSession.AccId);
            hsParam.Add("ip", IP);
            var sp = CustomerSession.Instance.ConsolidatedGroup["Acc_CustomerUpdConsolidatedGroupHeader"];
            var dr = DbHelper.Execute(sp.Name, hsParam, sp.DbName, 600);
            return new Sunplus.Accounting.Entities.Result()
            {
                ErrorMessage = CastAide.AsString(dr.Outputs["errmsg"])
                ,
                ErrorCode = CastAide.AsInt32(dr.Outputs["errcode"])
            };
        }
    }
}
