using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.Settlement;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Settle;

namespace Sunplus.Accounting.Business.Settle
{
    public class Settlement : ISettlement
    {
        private ISettlementDAO _settlementDao;
        private string _keyDefineStatus = "SettlementDefineStatus." + AppSession.Username;

        public Settlement(ISettlementDAO iSettlementDao)
        {
            _settlementDao = iSettlementDao ?? new SettlementDAO();
        }

        public Settlement()
            : this(null)
        {
            //_settlementDao = new SettlementDAO();

        }


        #region DefineStatus
        Hashtable DefineStatus()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("100closed", "Pending");
            hashtable.Add("110closed", "Pending");
            hashtable.Add("101closed", "Pending");
            hashtable.Add("100running", "Pending");

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyDefineStatus, hashtable);
            return hashtable;
        }
        #endregion

        #region SelStatusConfirming
        public string SelStatusConfirming(DataRow dataRow, ref bool isPending)
        {
            string stastus = string.Empty;
            try
            {
                Hashtable hashtable = new Hashtable();
                if (AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyDefineStatus) != null)
                    hashtable = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyDefineStatus) as Hashtable;
                else
                    hashtable = DefineStatus();

                string key = (CastAide.AsBoolean(dataRow["IsPending"]) ? "1" : "0") +
                             (CastAide.AsBoolean(dataRow["IsHTCal"]) ? "1" : "0") +
                             (CastAide.AsBoolean(dataRow["Calculate"]) ? "1" : "0") +
                             CastAide.AsString(dataRow["eventstatus"]).ToLower();

                if (hashtable.ContainsKey(key))
                {
                    stastus = CastAide.AsString(hashtable[key]);
                    isPending = true;
                }
                else
                    stastus = CastAide.AsString(dataRow["eventstatus"]);

                stastus = Calculate.GetStatusStr(stastus);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return stastus;
        }
        #endregion

        #region IsPending
        public bool IsPending(DataRow dataRow)
        {
            return CastAide.AsInt32(dataRow["IsPending"]) == 1;
        }
        #endregion

        #region UdpMatch_Pending
        public Result UdpMatch_Pending(string matchid, bool ispending)
        {
            Result result = new Result(0, string.Empty);

            try
            {
                _settlementDao.UdpMatch_Pending(matchid, ispending);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        #endregion

        public string ReCalcNumberGame(int matchid, int firstball_value, int secondball_value, int thirdball_value, string ip)
        {
            return _settlementDao.ReCalcNumberGame(matchid, firstball_value, secondball_value, thirdball_value, ip);
        }

        public List<NumberGameBalls> GetNumberGameBalls(int matchid)
        {
            var dbResult = _settlementDao.GetNumberGameBalls(matchid);
            if (dbResult.ResultSet.Tables.Count <= 0 || dbResult.ResultSet.Tables[0].Rows.Count <= 0)
                return new List<NumberGameBalls>();

            DataTable tbl = dbResult.ResultSet.Tables[0];

            var listResult = new List<NumberGameBalls>();
            if (tbl.Rows.Count >= 1)
            {
                listResult.Add(new NumberGameBalls
                {
                    MatchId = matchid,
                    Seq = CastAide.AsInt32(tbl.Rows[0]["seq"]),
                    Number = CastAide.AsInt32(tbl.Rows[0]["number"])
                });
            }

            if (tbl.Rows.Count >= 2)
            {
                listResult.Add(new NumberGameBalls
                {
                    MatchId = matchid,
                    Seq = CastAide.AsInt32(tbl.Rows[1]["seq"]),
                    Number = CastAide.AsInt32(tbl.Rows[1]["number"])
                });
            }

            if (tbl.Rows.Count >= 3)
            {
                listResult.Add(new NumberGameBalls
                {
                    MatchId = matchid,
                    Seq = CastAide.AsInt32(tbl.Rows[2]["seq"]),
                    Number = CastAide.AsInt32(tbl.Rows[2]["number"])
                });
            }
            return listResult;
        }


        public void UpdBallNoByMatchID(int matchid, string ballNo, string siteNo, int adminid, string adminname, string ip)
        {

            try
            {
                var resultNG = _settlementDao.UpdBallNoByMatchID(matchid, ballNo, siteNo, adminid, adminname, ip);
                if (!string.IsNullOrEmpty(resultNG.ErrorCode))
                    throw new Exception(resultNG.ErrorMessage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

        }

        public string RefundRunningBetDaqTickets(int Matchid, int Creator, string IP)
        {
            return _settlementDao.RefundRunningBetDaqTickets(Matchid, Creator, IP);
        }

        public void AssignDataUnDecided(ref string tpl, string permission, DataRow dr)
        { 
            try
            {

                bool isUndecided = CastAide.AsBoolean(dr["isUndecided"]);
                string matchId = CastAide.AsString(dr["matchid"]);
                string matchcode = CastAide.AsString(dr["matchcode"]);
                TemplateHandler.Assign(ref tpl, "matchId", matchId);
                TemplateHandler.Assign(ref tpl, "matchCode", matchcode);
                bool isHasPermission = this.IsHasPermissionUnDecided(permission);

                if (isUndecided)
                {
                    TemplateHandler.Assign(ref tpl, "isdecided", "1");
                    TemplateHandler.Assign(ref tpl, "ContentOfTooltip", isHasPermission ? "This match was marked as undecided. &#13;" + Environment.NewLine + "Click to remove this &#34;undecided&#34; status." : "This match was marked as undecided.");
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "isdecided", "0");
                    TemplateHandler.Assign(ref tpl, "ContentOfTooltip", isHasPermission ? "This match is fine to key score. &#13;" + Environment.NewLine + "Click to mark it as &#34;undecided&#34;." : "This match is fine to key score.");
                }

                string _event = "onclick=\"undecided.SetUnDecidedSettleMatch('" + matchId + "','" + matchcode + "','" + (isUndecided ? 1 : 0) + "')\"";
                if (!isHasPermission)
                {
                    TemplateHandler.Assign(ref tpl, "hand", "");
                    TemplateHandler.Assign(ref tpl, "event", string.Empty);
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "hand", "hand");
                    TemplateHandler.Assign(ref tpl, "event", _event);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public bool IsHasPermissionUnDecided(string permission)
        {
            return permission.IndexOf("[D]") > -1;
        }


        public void SetMatchIsUndecided(int matchId, bool isUndecided, int userId)
        {
            var permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(userId);
            if (IsHasPermissionUnDecided(permissions))
            {
                _settlementDao.SetMatchIsUndecided(matchId, isUndecided, userId);
            }

        }


        public void AssignApplyHedgingMatch(ref string tpl, int matchid)
        {
            string tplHedging = "";
            try
            {
                tplHedging = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/ApplyHedging.htm");
                string tplItem = TemplateHandler.CopyBlock(tplHedging, "Item");

                StringBuilder html = new StringBuilder();
                DataTable dataTable = _settlementDao.MatchSelHedge(matchid);
                if (dataTable != null)
                {
                    DataRow[] rows = dataTable.Select();
                    string matchids_hedging = string.Empty;
                    //for (int i = 0; i <= 2; i++)
                    {

                        foreach (DataRow row in rows)
                        {
                            string _row = tplItem;
                            string _matchIdHedging = CastAide.AsString(row["matchid"]);
                            string _matchcodeHedging = CastAide.AsString(row["matchcode"]);

                            matchids_hedging += _matchIdHedging + "^";

                            TemplateHandler.Assign(ref _row, "matchid_hedging", _matchIdHedging);
                            TemplateHandler.Assign(ref _row, "matchcode_hedging", _matchcodeHedging);
                            TemplateHandler.Assign(ref _row, "checked", string.Format("{0}", "checked='checked'"));

                            html.Append(_row);
                        }
                    }


                    TemplateHandler.ReplaceBlock(ref tplHedging, "Item", html.ToString());
                    TemplateHandler.Assign(ref tplHedging, "matchids_hedging", matchids_hedging);
                }

                TemplateHandler.Assign(ref tpl, "ApplyHedging", tplHedging);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
