using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities.Report;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Report.WinLoss
{
    /// <summary>
    /// Summary description for AgentWinLossDataAccess
    /// </summary>
    public class WinLossDataAccess : DataAccessReport, IWinLossDataAccess
    {
        #region IWinLossDataAccess Members

        public int GetTotalOnlineMember(Enumerations.AccountingSystem system)
        {
            int totalOnlineMember = 0;
            var param = new Hashtable();
            param.Add("system", system.ToString());
            using (IDataReader reader =
                DbHelper.ExecuteReader("Acc_Rpt_OnlineMemberTotalEx", param, DBType.ON_USER, 300))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]);
                    }
                }
            }
            // totalOnlineMember = Convert.ToInt32(DbHelper.ExecuteScalar("Acc_Rpt_OnlineMemberTotalEx", null, DBType.ON_USER, 120));

            return totalOnlineMember;
        }

        public List<MemberWinLoss> GetMemberWinLossData(Hashtable param)
        {
            var entityList = new List<MemberWinLoss>();
            var spElement =
                GetStoreProcedure(Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.External,
                                  "WinLossDetail_Ag");
            using (IDataReader reader =
                DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName, 300))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        IWinLoss entity = new MasterWinLoss();
                        //read info from datareader
                        SetBaseInfo(ref entity, reader);
                        SetMemberWinLoss(ref entity, reader);
                        SetAgentWinLoss(ref entity, reader);

                        entityList.Add(entity as MasterWinLoss);
                    }
                }
            }

            return entityList;
        }

        public List<AgentWinLoss> GetAgentWinLossData(Hashtable param)
        {
            var entityList = new List<AgentWinLoss>();
            var spElement =
                GetStoreProcedure(Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.External,
                                  "WinLossDetail_MAg_New");
            
            using (IDataReader reader =
                DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName, 300))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        IWinLoss entity = new AgentWinLoss();
                        //read info from datareader
                        SetBaseInfo(ref entity, reader);
                        SetMemberWinLoss(ref entity, reader);
                        SetAgentWinLoss(ref entity, reader);
                        SetMasterWinLoss(ref entity, reader);

                        entityList.Add(entity as AgentWinLoss);
                    }
                }
            }

            return entityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<MasterWinLoss> GetMasterWinLossData(Hashtable param)
        {
            var spElement =
                GetStoreProcedure(Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.External,
                                  "WinLossDetail_SAg");
            
            return GetReportData(param, spElement.Name, spElement.DbName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<MasterWinLoss> GetSuperWinLossData(Hashtable param)
        {
            string strMapping = "WinLossDetail_ComEx";

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "External").Stores[strMapping];
            var spElement = GetStoreProcedure(Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.External, strMapping);
            return GetReportData(param, spElement.Name, spElement.DbName);
        }

        #endregion

        /// <summary>
        /// Company and super
        /// </summary>
        /// <param name="param"></param>
        /// <param name="SpName"></param>
        /// <returns></returns>
        private List<MasterWinLoss> GetReportData(Hashtable param, string SpName, string dbName)
        {
            var entityList = new List<MasterWinLoss>();

            using (IDataReader reader =
                DbHelper.ExecuteReader(SpName, param, dbName, 300))
            {
                if (reader != null)
                {
                    var totalItem = new MasterWinLoss
                                        {
                                            Member = new WinLossComm(),
                                            Agent = new WinLossComm(),
                                            Master = new WinLossComm(),
                                            Super = new WinLossComm(),
                                            CustId = -1,
                                            Username = "Total"
                                        };

                    while (reader.Read())
                    {
                        IWinLoss entity = new MasterWinLoss();
                        //read info from datareader
                        SetBaseInfo(ref entity, reader);
                        SetAgentWinLoss(ref entity, reader);
                        SetMemberWinLoss(ref entity, reader);
                        SetMasterWinLoss(ref entity, reader);
                        SetSuperWinLoss(ref entity, reader);

                        entityList.Add(entity as MasterWinLoss);
                    }
                }
            }

            return entityList;
        }

        #region Read data from data reader

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="hasGetFromBestTrans14"></param>
        /// <param name="reportLevel"></param>
        /// <returns></returns>
        public List<WinLossDailyView> GetCompanyWinLossDaily(Hashtable param, bool hasGetFromBestTrans14,
                                                             int reportLevel)
        {
            var entityList = new List<WinLossDailyView>();
            string spName = GetSPNameWinLossDaily(reportLevel, hasGetFromBestTrans14);

            using (IDataReader reader =
                DbHelper.ExecuteReader(spName, param, DBType.REPORT, 300))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        WinLossDailyView entity = CreateWinlossDailyEntity(reader);

                        entityList.Add(entity);
                    }
                }
            }

            return entityList;
        }

        /// <summary>
        /// user IWinLoss for set real object later
        /// injection policy modern Lewin :D
        /// </summary>
        /// <param name="entity">IWinLoss</param>
        /// <param name="reader"></param>
        private void SetBaseInfo(ref IWinLoss entity, IDataReader reader)
        {
            entity.CustId = Convert.ToInt32(reader["CustId"]);
            entity.Username = Convert.ToString(reader["UserName"]);
            entity.Turnover = Convert.ToDouble(reader["Turnover"]);
            entity.GrossComm = Convert.ToDouble(reader["GrossComm"]);
            entity.UplineTotal = Convert.ToDouble(reader["Company"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reader"></param>
        private void SetMemberWinLoss(ref IWinLoss entity, IDataReader reader)
        {
            var agent = new WinLossComm
                            {
                                Comm = Convert.ToDouble(reader["AgComm"]),
                                WinLoss = Convert.ToDouble(reader["AgWinlost"])
                            };

            (entity as MemberWinLoss).Agent = agent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reader"></param>
        private void SetAgentWinLoss(ref IWinLoss entity, IDataReader reader)
        {
            var member = new WinLossComm
                             {
                                 WinLoss = Convert.ToDouble(reader["MemberWinlost"]),
                                 Comm = Convert.ToDouble(reader["PlayerComm"])
                             };

            ((MemberWinLoss) entity).Member = member;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reader"></param>
        private void SetMasterWinLoss(ref IWinLoss entity, IDataReader reader)
        {
            var master = new WinLossComm
                             {
                                 Comm = Convert.ToDouble(reader["MAgComm"]),
                                 WinLoss = Convert.ToDouble(reader["MAgWinlost"])
                             };

            ((AgentWinLoss) entity).Master = master;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="reader"></param>
        private void SetSuperWinLoss(ref IWinLoss entity, IDataReader reader)
        {
            var super = new WinLossComm
                            {
                                Comm = Convert.ToDouble(reader["SAgComm"]),
                                WinLoss = Convert.ToDouble(reader["SAgWinlost"])
                            };

            ((MasterWinLoss) entity).Super = super;
        }

        /// <summary>
        /// get sp name of winloss daily by level report
        /// and should check get from besttran14
        /// </summary>
        /// <param name="reportLevel"></param>
        /// <param name="hasGetFromBestTrans14"></param>
        /// <returns></returns>
        private string GetSPNameWinLossDaily(int reportLevel, bool hasGetFromBestTrans14)
        {
            string SpName = string.Empty;
            switch (reportLevel)
            {
                case 1:
                    SpName = (hasGetFromBestTrans14) ? "Acc_Rpt_WinLossDaily_SAgEx14" : "Acc_Rpt_WinLossDaily_SAgEx";
                    break;
                case 2:
                    SpName = (hasGetFromBestTrans14) ? "Acc_Rpt_WinLossDaily_MAgEx14" : "Acc_Rpt_WinLossDaily_MAgEx";
                    break;
                case 3:
                    SpName = (hasGetFromBestTrans14) ? "Acc_Rpt_WinLossDaily_AgEx14" : "Acc_Rpt_WinLossDaily_AgEx";
                    break;
                default:
                    SpName = (hasGetFromBestTrans14) ? "Acc_Rpt_WinLossDaily_ComEx14" : "Acc_Rpt_WinLossDaily_ComEx";
                    break;
            }

            return SpName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private WinLossDailyView CreateWinlossDailyEntity(IDataReader reader)
        {
            var entity = new WinLossDailyView
                             {
                                 CustId = Convert.ToInt32(reader["custid"]),
                                 Username = Convert.ToString(reader["username"]),
                                 TotalTO = Convert.ToDouble(reader["totalTO"]),
                                 Company =
                                     {
                                         Turnover = Convert.ToDouble(reader["ComTO"]),
                                         Winloss = Convert.ToDouble(reader["ComWinlost"])
                                     },
                                 SA =
                                     {
                                         Turnover = Convert.ToDouble(reader["SAgTO"]),
                                         Winloss = Convert.ToDouble(reader["SAgWinlost"])
                                     },
                                 MA =
                                     {
                                         Turnover = Convert.ToDouble(reader["MAgTO"]),
                                         Winloss = Convert.ToDouble(reader["MAgWinlost"])
                                     },
                                 Agent =
                                     {
                                         Turnover = Convert.ToDouble(reader["AgTO"]),
                                         Winloss = Convert.ToDouble(reader["AgWinlost"])
                                     }
                             };

            return entity;
        }

        #endregion
    }
}