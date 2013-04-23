using System;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper.IP;
using Sunplus.Accounting.Entities;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Accounting.Entities.Customer;
using DNAide.Data;
using System.Collections;
using System.Data.Linq;
using Sunplus.Accounting.Entities.BetAggregator;
using Sunplus.Data.DataAbstract.Utils;
using DNAide.Utils;
using Sunplus.Accounting.Core.Extensions;
using System.Web;
using Sunplus.Accounting.Core.Cache.Extension;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    public class BetListDAO: DataAccess, IBetListDAO
    {
        protected StoreProcedureElement GetBetListStoreProcedure(string alias)
        {
            var spElement = GetStoreProcedure(Core.Configurations.StoreProcedureGroup.BetListSection.StoreCollection.BetListStores, alias);
            return spElement;
        }
      

        public List<BetListDetail> GetLatestBetList(int totalRow)
        {
            throw new NotImplementedException();
        }

        public List<BetMonitor> GetLatestBABetList(int totalRow, int amount, Int64 lastTransId)
        {
            var betList = new List<BetMonitor>();
            var spElement = GetBetListStoreProcedure("BALatestBetlist");
            var db = GetDatabase(spElement.DbName);
            

            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "totalrow", DbType.Int32, totalRow);
                db.AddInParameter(dbCommand, "amount", DbType.Int32, amount);
                db.AddInParameter(dbCommand, "lasttransid", DbType.Int64, lastTransId);

                var data = db.ExecuteReader(dbCommand);

                if (data != null)
                {
                    while (data.Read())
                    {
                        var bettran = new Bettran
                                          {
                                              Transid = CastAide.AsString(data["transid"]),
                                              HomeId = CastAide.AsString(data["homeid"]),
                                              AwayId = CastAide.AsString(data["awayid"]),
                                              Bettype = CastAide.AsInt32(data["bettype"]),
                                              SportId = CastAide.AsString(data["sporttype"]),
                                              Refno = CastAide.AsString(data["refno"]),
                                              Matchid = CastAide.AsInt32(data["matchid"]),
                                              Betid = CastAide.AsString(data["betid"]),
                                              Hdp1 = CastAide.AsString(data["hdp1"]),
                                              Hdp2 = CastAide.AsString(data["hdp2"]),
                                              Odds = CastAide.AsString(data["odds"]),
                                              OddsType = CastAide.AsInt32(data["OddsType"]),
                                              Status = CastAide.AsString(data["status"]),
                                              Stake = CastAide.AsString(data["stake"]),
                                              Transdate = CastAide.AsString(data["transdate"]),
                                              Betteam = CastAide.AsString(data["betteam"]),
                                              Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                                              Modds = CastAide.AsString(data["modds"]),
                                              Custid = CastAide.AsInt32(data["custid"]),
                                              Livehomescore = CastAide.AsString(data["livehomescore"]),
                                              Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                                              LeagueId = CastAide.AsString(data["leagueid"]),
                                              IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                                              CurrencyName = CastAide.AsString(data["CurrencyName"]),
                                              UserName = CastAide.AsString(data["UserName"]),
                                              MatchCode = CastAide.AsString(data["matchcode"])
                                              

                        };
                        var betMonitor = new BetMonitor
                                             {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        betList.Add(betMonitor);
                    }
                    data.Close();
                }

                return betList;

            }
            
            
        }
        
        public BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string username, List<IPVNBean> IPVNList, int currency, int bettype, int fromStake, int toStake, int excludeTest)
        {
            var betList = new List<BetListDetail>();
            var spElement = GetBetListStoreProcedure("BABetlist");
            var db = GetDatabase(spElement.DbName);


            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "FromDate", DbType.String,fromdate);
                db.AddInParameter(dbCommand, "ToDate", DbType.String, todate);
                db.AddInParameter(dbCommand, "MoveBetsDay", DbType.String,ReportUtils.BalanceUp.MoveBetsDay.ToString());
                db.AddInParameter(dbCommand, "BalanceUpDay", DbType.String, ReportUtils.BalanceUp.BalanceUpDay.ToString());
                db.AddInParameter(dbCommand, "Username", DbType.String, username);
                db.AddInParameter(dbCommand, "PageSize", DbType.Int32,pagesize );
                db.AddInParameter(dbCommand, "PageIndex", DbType.Int32,pageindex );
                db.AddInParameter(dbCommand, "currency", DbType.Int32, currency);
                db.AddInParameter(dbCommand, "Bettype", DbType.Int32, bettype);
                db.AddInParameter(dbCommand, "FromStake", DbType.Int32, fromStake);
                db.AddInParameter(dbCommand, "ToStake", DbType.Int32, toStake);
                db.AddInParameter(dbCommand, "excludeTest", DbType.Int32, excludeTest);

                db.AddOutParameter(dbCommand, "TotalRows", DbType.Int32, 10);
                dbCommand.CommandTimeout = 6000;
                var data = db.ExecuteReader(dbCommand);
                int resultCount = 0;
                if (data != null)
                {
                    var IsAdminCanViewVNIP = (WebSettings.IPVNReplace == "1") ? true : AppSession.ViewIPVN;
                    
                    while (data.Read())
                    {
                        var bettran = new Bettran()
                        {
                            Transid = CastAide.AsString(data["transid"]),
                            HomeId = CastAide.AsString(data["homeid"]),
                            AwayId = CastAide.AsString(data["awayid"]),
                            Bettype = CastAide.AsInt32(data["bettype"]),
                            SportId = CastAide.AsString(data["sporttype"]),
                            Refno = CastAide.AsString(data["refno"]),
                            Matchid = CastAide.AsInt32(data["matchid"]),
                            Betid = CastAide.AsString(data["betid"]),
                            Hdp1 = CastAide.AsString(data["hdp1"]),
                            Hdp2 = CastAide.AsString(data["hdp2"]),
                            Odds = CastAide.AsString(data["odds"]),
                            OddsType = CastAide.AsInt32(data["OddsType"]),
                            Status = CastAide.AsString(data["status"]),
                            Stake = CastAide.AsString(data["stake"]),
                            actualStake = CastAide.AsString(data["Actual_Stake"]),
                            Transdate = CastAide.AsString(data["transdate"]),
                            Betteam = CastAide.AsString(data["betteam"]),
                            Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                            Modds = CastAide.AsString(data["modds"]),
                            Custid = CastAide.AsInt32(data["custid"]),
                            Livehomescore = CastAide.AsString(data["livehomescore"]),
                            Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                            LeagueId = CastAide.AsString(data["leagueid"]),
                            IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                            CurrencyName = CastAide.AsString(data["CurrencyName"]),
                            UserName = CastAide.AsString(data["UserName"]),
                            MatchCode = CastAide.AsString(data["matchcode"]),
                            Ip=CastAide.AsString(data["ip"]),
                            //CompanyWinloss = CastAide.AsString(data["companywinlost"]),
                            Winlost = CastAide.AsString(data["winlost"]),
                            Virtual = CastAide.AsString(data["AmtDueToBAVirtual"]),
                            Virtualrate = CastAide.AsString(data["virtualrate"]),
                            Real = CastAide.AsString(data["AmtDuetoBAReal"]),
                            Acomm = CastAide.AsString(data["comm"]),
                            Adiscount = CastAide.AsString(data["discount"])
                        };
                        var baBetlist = new BetListDetail()
                        {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        if (!IsAdminCanViewVNIP)
                        {
                            var ipmanager= new IpManager(IPVNList);
                            baBetlist.Ip = ipmanager.ReplaceVnIp(baBetlist.Ip, WebSettings.IPVNReplace);
                        }
                        betList.Add(baBetlist);
                    }
                    data.Close();
                    resultCount=Convert.ToInt32(db.GetParameterValue(dbCommand, "TotalRows"));
                }
                return new BABetList() { Items=betList,totalItem=resultCount};
            }
        }

        private string getSessionName(bool isMain, int Type, string Licensees, string bettypes)
        {
            var sessionName = "";
            sessionName= Type==0? "L_" + Licensees:"B_" + bettypes;
            if (!isMain)
            {
                sessionName += Type == 0 ? "B_" + bettypes : "L_" + Licensees;
            }
            return sessionName;
        }

        public TicketsListingBetlistDTO GetTicketsListingBetlist(bool fromCache, bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, int Language, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            var ticketsListing = new TicketsListingBetlistDTO();
            var sessionName=getSessionName(isMain,  groupBy, Licensees, bettypes);

            //var _ticketsListing = SessionExtension.GetSession(HttpContext.Current.Session, sessionName);
            var _ticketsListing = CacheExtension.Get(sessionName);
          
            if (!fromCache || _ticketsListing == null)
            {

                #region getDataFromDB
                //SessionExtension.AddSession(HttpContext.Current.Session, sessionName, null);
                CacheExtension.Delete(sessionName);
                ticketsListing.Items = new List<TicketsListingBetlist>();
                var spElement = GetStoreProcedure(Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.TicketsListing, "TicketsListingDetail");
                var db = GetDatabase(spElement.DbName);

                using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
                {
                    db.AddInParameter(dbCommand, "matchid", DbType.Int32, matchID);
                    db.AddInParameter(dbCommand, "licenseenames", DbType.String, Licensees);
                    db.AddInParameter(dbCommand, "bettypes", DbType.String, bettypes);
                    db.AddInParameter(dbCommand, "bettypeslive", DbType.String, bettypesLive);
                    db.AddInParameter(dbCommand, "type", DbType.Int32, Type);
                    db.AddInParameter(dbCommand, "livetype", DbType.Int32, DNAide.Core.CastAide.AsString(LiveType, "0"));
                    //db.AddInParameter(dbCommand, "language", DbType.Int32, Language);
                    db.AddInParameter(dbCommand, "safilter", DbType.Int32, SAFilter);
                   
                    if(Type == 0)//sport book
                    {
                        if(HomeScore.Length>0 && AwayScore.Length>0)
                        {
                            db.AddInParameter(dbCommand, "livehomescore", DbType.Int32, CastAide.AsInt32(HomeScore));
                            db.AddInParameter(dbCommand, "liveawayscore", DbType.Int32, CastAide.AsInt32(AwayScore));
                          
                        }
                        if (FromTransDate.Length > 0 && ToTransDate.Length > 0)
                        {
                            db.AddInParameter(dbCommand, "fromtransdate", DbType.DateTime, CastAide.AsDateTime(FromTransDate).AddHours(-12));
                            db.AddInParameter(dbCommand, "totransdate", DbType.DateTime, CastAide.AsDateTime(ToTransDate).AddHours(-12));
                        }
                       
                    }

                    dbCommand.CommandTimeout = 6000;
                    //var data = db.ExecuteReader(dbCommand);
                    var ds = db.ExecuteDataSet(dbCommand);
                    if (ds != null)
                    {
                        var dtBetTrans = ds.Tables[0];
                        var dtMBetTrans = ds.Tables[1];
                        
                        #region none MP bettype
                        foreach (DataRow data in dtBetTrans.Rows)
                        {
                            var bettran = new Bettran
                                              {
                                Transid = CastAide.AsString(data["transid"]),
                                Transdate = CastAide.AsString(data["transdate"]),
                                UserName = CastAide.AsString(data["UserName"]),
                                Odds = CastAide.AsString(data["odds"]),
                                Stake = CastAide.AsString(data["stake"]),
                                Winlost = CastAide.AsString(data["winlost"]),
                                CurrencyName = CastAide.AsString(data["CurrencyName"]),
                                Bettype = CastAide.AsInt32(data["bettype"]),
                                Status = CastAide.AsString(data["status"]),

                                HomeId = CastAide.AsString(data["homeid"]),
                                AwayId = CastAide.AsString(data["awayid"]),
                               
                                Hdp1 = CastAide.AsString(data["hdp1"]),
                                Hdp2 = CastAide.AsString(data["hdp2"]),
                                Betteam = CastAide.AsString(data["betteam"]),
                                Betid = CastAide.AsString(data["betid"]),

                                Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                                Livehomescore = CastAide.AsString(data["livehomescore"]),
                                Liveawayscore = CastAide.AsString(data["liveawayscore"]),

                                OddsType = CastAide.AsInt32(data["OddsType"]),

                                IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                                SATicket = CastAide.AsInt32(data["secacceptflag"])

                            };
                          
                            var tkBetlist = new TicketsListingBetlist()
                            {
                                Transid = bettran.Transid,
                                BettranDetail = bettran,
                                MBettranDetail = new Bettran()

                            };
                            ticketsListing.Items.Add(tkBetlist);
                        }
                        #endregion

                        #region MP bettype
                        foreach (DataRow mdata in dtMBetTrans.Rows)
                        {
                            var bettran = new Bettran()
                            {
                                SystemParlaySelection_Bettran = CastAide.AsString(mdata["selection"]),
                                Transid = CastAide.AsString(mdata["transid"]),
                                Transdate = CastAide.AsString(mdata["transdate"]),
                                UserName = CastAide.AsString(mdata["UserName"]),
                                Odds = CastAide.AsString(mdata["odds"]),
                                Stake = CastAide.AsString(mdata["stake"]),
                                Winlost = CastAide.AsString(mdata["winlost"]),
                                CurrencyName = CastAide.AsString(mdata["CurrencyName"]),
                                Bettype = CastAide.AsInt32(mdata["bettype"]),
                                Status = CastAide.AsString(mdata["status"]),

                                HomeId = CastAide.AsString(mdata["homeid"]),
                                AwayId = CastAide.AsString(mdata["awayid"]),
                                
                                Hdp1 = CastAide.AsString(mdata["hdp1"]),
                                Hdp2 = CastAide.AsString(mdata["hdp2"]),
                                Betteam = CastAide.AsString(mdata["betteam"]),
                               
                                Liveindicator = CastAide.AsBoolean(mdata["liveindicator"]),
                                Livehomescore = CastAide.AsString(mdata["livehomescore"]),
                                Liveawayscore = CastAide.AsString(mdata["liveawayscore"]),

                                OddsType = CastAide.AsInt32(mdata["OddsType"]),
                              

                                IsNeutral = CastAide.AsBoolean(mdata["isNeutral"]),
                                SATicket = CastAide.AsInt32(mdata["secacceptflag"])

                            };
                          
                            var mbetTrans = new Bettran
                            {
                               // SystemParlaySelection_Bettran = CastAide.AsString(mdata["selection"]),
                                Transid = CastAide.AsString(mdata["transid"]),
                                Transdate = CastAide.AsString(mdata["transdate"]),
                                Odds = CastAide.AsString(mdata["odds"]),

                                Bettype = CastAide.AsInt32(mdata["bettypem"]),

                                HomeId = CastAide.AsString(mdata["homeid"]),
                                AwayId = CastAide.AsString(mdata["awayid"]),
                                Hdp1 = CastAide.AsString(mdata["hdp1"]),
                                Hdp2 = CastAide.AsString(mdata["hdp2"]),
                                Betteam = CastAide.AsString(mdata["betteam"]),
                                Liveindicator = CastAide.AsBoolean(mdata["liveindicator"]),
                                Livehomescore = CastAide.AsString(mdata["livehomescore"]),
                                Liveawayscore = CastAide.AsString(mdata["liveawayscore"]),
                                OddsType = CastAide.AsInt32(mdata["OddsType"]),
                                Modds = CastAide.AsString(mdata["oddsm"]),
                                IsNeutral = CastAide.AsBoolean(mdata["isNeutral"])

                            };
                            var tkBetlist = new TicketsListingBetlist()
                            {
                                Transid = bettran.Transid,
                                BettranDetail = bettran,
                                MBettranDetail = mbetTrans

                            };
                            ticketsListing.Items.Add(tkBetlist);
                        }
                        #endregion
                    }
                    ticketsListing.totalItem = ticketsListing.Items.Count;
                }
                #endregion
                //SessionExtension.AddSession(HttpContext.Current.Session, sessionName, ticketsListing);
                CacheExtension.Set(sessionName, ticketsListing, DateTime.Now.AddMinutes(30));
                
            }
            else
            {
                ticketsListing = (TicketsListingBetlistDTO)_ticketsListing;
            }
            return ticketsListing;
        }

        public BABetList GetBABetListByTransId(string refno, List<IPVNBean> IPVNList)
        {
            var betList = new List<BetListDetail>();
            var spElement = GetBetListStoreProcedure("BABetlistByTransID");
            var db = GetDatabase(spElement.DbName);


            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "TransID", DbType.String, refno);
                dbCommand.CommandTimeout = 60;
                var data = db.ExecuteReader(dbCommand);
                int resultCount = 0;
                if (data != null)
                {
                    var IsAdminCanViewVNIP = (WebSettings.IPVNReplace == "1") ? true : AppSession.ViewIPVN;

                    while (data.Read())
                    {
                        var bettran = new Bettran()
                        {
                            Transid = CastAide.AsString(data["transid"]),
                            HomeId = CastAide.AsString(data["homeid"]),
                            AwayId = CastAide.AsString(data["awayid"]),
                            Bettype = CastAide.AsInt32(data["bettype"]),
                            SportId = CastAide.AsString(data["sporttype"]),
                            Refno = CastAide.AsString(data["refno"]),
                            Matchid = CastAide.AsInt32(data["matchid"]),
                            Betid = CastAide.AsString(data["betid"]),
                            Hdp1 = CastAide.AsString(data["hdp1"]),
                            Hdp2 = CastAide.AsString(data["hdp2"]),
                            Odds = CastAide.AsString(data["odds"]),
                            OddsType = CastAide.AsInt32(data["OddsType"]),
                            Status = CastAide.AsString(data["status"]),
                            Stake = CastAide.AsString(data["stake"]),
                            Transdate = CastAide.AsString(data["transdate"]),
                            Betteam = CastAide.AsString(data["betteam"]),
                            Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                            Modds = CastAide.AsString(data["modds"]),
                            Custid = CastAide.AsInt32(data["custid"]),
                            Livehomescore = CastAide.AsString(data["livehomescore"]),
                            Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                            LeagueId = CastAide.AsString(data["leagueid"]),
                            IsNeutral = CastAide.AsBoolean(data["isNeutral"]),
                            CurrencyName = CastAide.AsString(data["CurrencyName"]),
                            UserName = CastAide.AsString(data["UserName"]),
                            MatchCode = CastAide.AsString(data["matchcode"]),
                            Ip = CastAide.AsString(data["ip"]),
                            //CompanyWinloss = CastAide.AsString(data["companywinlost"]),
                            Winlost = CastAide.AsString(data["winlost"]),
                            Virtual = CastAide.AsString(data["AmtDueToBAVirtual"]),
                            Virtualrate = CastAide.AsString(data["virtualrate"]),
                            Real = CastAide.AsString(data["AmtDuetoBAReal"]),
                            Acomm = CastAide.AsString(data["comm"]),
                            Adiscount = CastAide.AsString(data["discount"])
                        };
                        var baBetlist = new BetListDetail()
                        {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        if (!IsAdminCanViewVNIP)
                        {
                            var ipmanager = new IpManager(IPVNList);
                            baBetlist.Ip = ipmanager.ReplaceVnIp(baBetlist.Ip, WebSettings.IPVNReplace);
                        }
                        betList.Add(baBetlist);
                    }
                    data.Close();
                    //resultCount = (int)db.GetParameterValue(dbCommand, "TotalRows");
                    resultCount = betList.Count;
                }
                return new BABetList() { Items = betList, totalItem = resultCount };
            }
        }

        public List<BetListDetail> GetMixParlayTickets(string refNo)
        {
            var betList = new List<BetListDetail>();
            var spElement = GetBetListStoreProcedure("GetMixparlay");
            var db = GetDatabase(spElement.DbName);


            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "@refno", DbType.String, refNo);
                //db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                var listMpTicket = new List<BetListDetail>();
                var data = db.ExecuteReader(dbCommand);
                if (data != null)
                {
                    while (data.Read())
                    {
                        var bettran = new Bettran()
                        {
                            Transid = CastAide.AsString(data["transid"]),
                            HomeId = CastAide.AsString(data["homeid"]),
                            AwayId = CastAide.AsString(data["awayid"]),
                            Bettype = CastAide.AsInt32(data["bettype"]),
                            SportId = CastAide.AsString(data["sporttype"]),
                            MatchCode = CastAide.AsString(data["matchcode"]),
                            Matchid = CastAide.AsInt32(data["matchid"]),
                            Hdp1 = CastAide.AsString(data["hdp1"]),
                            Hdp2 = CastAide.AsString(data["hdp2"]),
                            Odds = CastAide.AsString(data["odds"]),
                            Betteam = CastAide.AsString(data["betteam"]),
                            Liveindicator = CastAide.AsBoolean(data["liveindicator"]),
                            Livehomescore = CastAide.AsString(data["livehomescore"]),
                            Liveawayscore = CastAide.AsString(data["liveawayscore"]),
                            LeagueId = CastAide.AsString(data["leagueid"]),
                            StatusId = CastAide.AsInt32(data["statusId"]),
                           
                            IsNeutral = CastAide.AsBoolean(data["isNeutral"])
                        };

                        var betListDetail = new BetListDetail()
                        {
                            Transid = bettran.Transid,
                            BettranDetail = bettran

                            //Name = reader.GetString(reader["Name"]
                        };
                        listMpTicket.Add(betListDetail);
                    }
                    data.Close();
                }
                return listMpTicket;

            }
            
        }

        #region IBetListDAO Members

        public DataTable GetBettype()
        {
            var dataTable = new DataTable();
            var bettype = new BettypeDTO();
            StoreProcedureElement procedureElement = GetBetListStoreProcedure("GetBettype");
            string spName = procedureElement.Name;
            string db = procedureElement.DbName;
            DbResult dbResult = DbHelper.Execute(spName, db);
            if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
            {
                dataTable = dbResult.ResultSet.Tables[0];
            }

            return dataTable;
        }

        public DataTable GetCurrency()
        {
            var dataTable = new DataTable();
            StoreProcedureElement procedureElement = GetBetListStoreProcedure("GetCurrency");
            string spName = procedureElement.Name;
            string db = procedureElement.DbName;
            DbResult dbResult = DbHelper.Execute(spName, db);
            if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
            {
                dataTable = dbResult.ResultSet.Tables[0];
            }
            return dataTable;
        }

        #endregion

        public List<CombinationDetalDTO> GenDetailMP(string serial, DataTable dataTable)
        {
            var tableDetailMP = new List<CombinationDetalDTO>();
            foreach (DataRow row in dataTable.Rows)
            {
                if (CastAide.AsString(row["serial"]).Trim() == serial)
                {
                    var betlistDto = new CombinationDetalDTO();
                    betlistDto.AwayName = SportBookUtils.GetTeamName(CastAide.AsString(row["awayid"]));
                    betlistDto.HomeName = SportBookUtils.GetTeamName(CastAide.AsString(row["homeid"]));
                    tableDetailMP.Add(betlistDto);
                }

            }
            return tableDetailMP;
        }

        public CombinationDetailMP GetCombinationDetailMP(string refNo)
        {
            var combinationDetailMP = new CombinationDetailMP();
            double totalWLComm = 0.00;
            var dataTable = new DataTable();
            var dataTable1 = new DataTable();
            var combinationMP = new List<BetListDetail>();
            var param = new Hashtable {{"refno", refNo}, {"outstanding", 0}};
            StoreProcedureElement spElement = GetBetListStoreProcedure("GetCombinationDetailMP");
            DbResult dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName);
           
            if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
            {
                dataTable = dbResult.ResultSet.Tables[0];
                dataTable1 = dbResult.ResultSet.Tables[1];
                var outstanding = CastAide.AsDecimal(dbResult.Outputs["outstanding"]);
                combinationDetailMP.outStanding = FormatHandler.FormatNumber2(outstanding, "en-EN"); 
            }
            foreach (DataRow row in dataTable.Rows)
            {
                var betlistDetail = new BetListDetail();
                // betlistDetail.CombinationDetails = new List<CombinationDetalDTO>();
                List<CombinationDetalDTO> combinationdetail = GenDetailMP(CastAide.AsString(row["serial"]), dataTable1);
                var bettran = new Bettran
                                  {
                                      Transdate = CastAide.AsString(row["winlostdate"]),
                                      Status = CastAide.AsString(row["status"]),
                                      Stake = CastAide.AsString(row["stake"]),
                                      Winlost = CastAide.AsString(row["winlost"]),
                                      Odds = CastAide.AsString(row["modds"])
                                  };
                betlistDetail.BettranDetail = bettran;
                betlistDetail.CombinationDetails = combinationdetail;
                combinationMP.Add(betlistDetail);
                totalWLComm += CastAide.AsDouble(row["winlost"]);

            }
            combinationDetailMP.TotalWLComm =FormatHandler.FormatNumber2(totalWLComm,"en-EN");
            combinationDetailMP.items = combinationMP;
            return combinationDetailMP;



        }

        public DataSet GetBetListSystemParlayByTransId(string transId, int custId, DateTime moveBetDay, DateTime balaceUpDay)
        {
            DataSet result = null;
            var param = new Hashtable { { "transid", transId }, { "custid", custId }, { "movebetday", moveBetDay }, { "balanceupday", balaceUpDay } };

            using (ObjectDb db = new ObjectDb("BetlistSystemParlayDetailByTransid", 180))
            {
                db.AssignParamValues(param);
                result = db.ExecuteDataSet();
            }

            return result;
            
        }
    }
}
