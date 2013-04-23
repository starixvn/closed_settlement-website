using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Fanex.Data;
using System.Data;
using Fanex.Tarus.Settlement.Core.Common;

namespace Fanex.Tarus.Settlement.DataAccess
{
    public class LiveCasinoDataAccess : BaseDataAccess, ILiveCasinoDataAccess
    {
        
        public DataSet GetMatches(string fdate, string tdate, int LeagueId, int statusId)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_Set_GameSel"))
            {
                var param = new Hashtable();
                param.Add("fromdate", fdate);
                param.Add("todate", tdate);
                param.Add("leagueid", LeagueId);
                param.Add("statusid", statusId);
                objDB.AssignParamValues(param);
                var ds = objDB.ExecuteDataSet();
                return ds;
            }
        }

        public DataSet GetLiveCasinoScore(int MatchId)
        {
            using (ObjectDb objDB = new ObjectDb("TP_Game_GameDetail_ByMatchId"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, MatchId);
                return objDB.ExecuteDataSet();
            }
        }

        public override DataSet GetMatch(int MatchId)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_Set_LiveCSGameSelScore"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, MatchId);
                var ds = objDB.ExecuteDataSet();
                return ds;
            }
        }


        public string RefundAtLiveCasioDB(int MatchID, int AdminID, string AdminName)
        {
            using (ObjectDb objDB = new ObjectDb("TP_Controller_MiniBac_VoidFromAct"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, MatchID);
                objDB.AddInParameter("sysdate", DbType.DateTime, DateTime.Now);
                objDB.AddInParameter("adminid", DbType.Int32, AdminID);
                objDB.AddInParameter("adminname", DbType.String, AdminName);
                objDB.AddInParameter("casinono", DbType.String, "01");
                objDB.AddOutParameter("ErrMsg", DbType.String, 500);
                objDB.AddOutParameter("ErrCode", DbType.String, 5);
                var result = objDB.ExecuteNonQuery();
                return DNAide.Core.CastAide.AsString(objDB.GetParameterValue("ErrMsg"));
            }
        }

        public string RefundAtIBCDB(int MatchID, string Action, int AdminID, string betstatus, decimal CasinoCommRate, string IP, string CardsResult)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("Admin_Settlement_s162"))
                {
                    objDB.AddInParameter("matchid", DbType.Int32, MatchID);
                    objDB.AddInParameter("action", DbType.String, Action);
                    objDB.AddInParameter("adminid", DbType.Int32, AdminID);
                    objDB.AddInParameter("betstatus", DbType.String, betstatus);
                    objDB.AddInParameter("cardsresult", DbType.String, CardsResult);
                    objDB.AddInParameter("ip", DbType.String, IP);
                    objDB.AddInParameter("commrate", DbType.Decimal, CasinoCommRate);
                  
                    objDB.AddOutParameter("errmsg", DbType.String, 500);
                    objDB.AddOutParameter("errcode", DbType.String, 5);
                    var result = objDB.ExecuteNonQuery();
                    return DNAide.Core.CastAide.AsString(objDB.GetParameterValue("errmsg"));

                }
            }
            catch
            {
                return "Error";
            }
        }

        public bool UpdateStatus(int MatchID, int Banker, int Player, string ResultData, int bettypeId)
        {
            using (ObjectDb objDB = new ObjectDb("Dot_Member_LiveCasino_UpdResult"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, MatchID);                
                objDB.AddInParameter("player", DbType.Int32, Player);
                if (bettypeId != (int)Enumerations.Bettype.Routle)
                {
                    objDB.AddInParameter("banker", DbType.Int32, Banker);
                    objDB.AddInParameter("resultdata", DbType.String, ResultData);
                }
                objDB.AddOutParameter("Updresult", DbType.Boolean, 1); 
                objDB.ExecuteNonQuery();
                if (objDB.GetParameterValue("Updresult") is System.DBNull) return true;
                return DNAide.Core.CastAide.AsBoolean(objDB.GetParameterValue("Updresult"));
            }
        }
    }
}
