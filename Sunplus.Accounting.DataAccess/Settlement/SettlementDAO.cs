using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using System.Data;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.Settlement
{
    public class SettlementDAO:DataAccessReport, ISettlementDAO
    {
        private StoreProcedureElement getSPMapping(string key)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, key);
        }

        public DbResult UdpMatch_Pending(string matchid, bool ispending)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchid);
            hashtable.Add("ispending", ispending);
            hashtable.Add("userid", AppSession.AccId);

            var spElement = getSPMapping("UdpMatch_Pending");

            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }

        public string ReCalcNumberGame(int matchid, int firstball_value, int secondball_value, int thirdball_value, string ip)
        {   
            using (ObjectDb objDb = new ObjectDb("Admin_Settlement_s161_Manual", 600))
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("matchid", matchid);
                if (firstball_value != 0)
                    hashtable.Add("firstball_value", firstball_value);
                if (secondball_value != 0)
                    hashtable.Add("secondball_value", secondball_value);
                if (thirdball_value != 0)
                    hashtable.Add("thirdball_value", thirdball_value);
                hashtable.Add("ip", ip);
                hashtable.Add("adminid", AppSession.AccId);

                objDb.AssignParamValues(hashtable);
                objDb.ExecuteNonQuery();

                var errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                if (errCode > 0)
                    return CastAide.AsString(objDb.GetParameterValue("errmsg"));
                return string.Empty;
            }
        }

        public DbResult GetNumberGameBalls(int matchid)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchid);
            var spElement = getSPMapping("GetNumberGameBalls");

            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }


        public ResultNG UpdBallNoByMatchID(int matchid, string ballNo, string siteNo, int adminid, string adminname, string ip )
        {
            var resultNG = new ResultNG();

            var spElement = getSPMapping("UpdBallNoByMatchID");
            var db = GetDatabase(spElement.DbName, false);
            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "matchid", DbType.Int32, matchid);
                db.AddInParameter(dbCommand, "ballno", DbType.String, ballNo);
                db.AddInParameter(dbCommand, "siteno", DbType.String, siteNo);
                db.AddInParameter(dbCommand, "adminid", DbType.Int32, adminid);
                db.AddInParameter(dbCommand, "adminname", DbType.String, adminname);
                db.AddInParameter(dbCommand, "fromip", DbType.String, ip);
                db.AddOutParameter(dbCommand, "errcode", DbType.String, 5);
                db.AddOutParameter(dbCommand, "errmsg", DbType.String, 500);
                db.ExecuteNonQuery(dbCommand);
                resultNG.ErrorCode = dbCommand.Parameters["@errcode"].Value.ToString();
                resultNG.ErrorMessage = dbCommand.Parameters["@errmsg"].Value.ToString();  
            }
            
            
            return resultNG;

        }

        public string RefundRunningBetDaqTickets(int Matchid, int Creator, string IP)
        {   
            using (ObjectDb objDb = new ObjectDb("Admin_Settlement_Refund_BetDaq", 600))
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("matchid", Matchid);
                hashtable.Add("creator", Creator);
                hashtable.Add("ip", IP);

                objDb.AssignParamValues(hashtable);
                objDb.ExecuteNonQuery();

                var errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));

                if (errCode == 0)
                    return string.Empty;
                
                return CastAide.AsString(objDb.GetParameterValue("errmsg"));
            }
        }

        public string RefundRunningBetFairTickets(int Matchid, int Creator, string IP)
        {
            using (ObjectDb objDb = new ObjectDb("Admin_Settlement_Refund_BetFair", 600))
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("matchid", Matchid);
                hashtable.Add("creator", Creator);
                hashtable.Add("ip", IP);

                objDb.AssignParamValues(hashtable);
                objDb.ExecuteNonQuery();

                var errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));

                if (errCode == 0)
                    return string.Empty;

                return CastAide.AsString(objDb.GetParameterValue("errmsg"));
            }
        }

        public void SetMatchIsUndecided(int matchId, bool isUndecided, int userId)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchId);
            hashtable.Add("isundecided", isUndecided);
            hashtable.Add("userid", userId);

            var spElement = getSPMapping("Acc_Set_MatchUpdIsUndecided");
            DbHelper.Execute(spElement.Name, hashtable, spElement.DbName);

        }


        public DataTable MatchSelHedge(int matchId)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", matchId);

            var spElement = getSPMapping("Acc_Set_MatchSelHedge");
            DbResult dbResult = DbHelper.Execute(spElement.Name, hashtable, spElement.DbName);
            DataTable dataTable = new DataTable();
            if(dbResult!= null && dbResult.ResultSet.Tables.Count>0)
            {
                dataTable = dbResult.ResultSet.Tables[0];
            }
            return dataTable;
        }
    }
}
