using System;
using DNAide.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DataAccess
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public class DataAccess
    {
        //DAO Client

        #region DAOClient enum

        public enum DAOClient
        {
            AdoNet = 1,
            Linq = 2,
            WCF = 3,
            Entity = 4
        }

        #endregion

        #region DataSource enum

        public enum DataSource
        {
            SqlServer = 1,
            Access = 2,
            DB2 = 3,
            Oracle = 4
        }

        #endregion

        protected static string _AdoNet = "AdoNet";
        protected static string _Linq = "Linq";
        protected static string _WCF = "WCF";
        protected static string _Entity = "Entity";

        //Data Source
        protected static string _SqlServer = "SqlServer";
        protected static string _Access = "Access";
        protected static string _DB2 = "DB2";
        protected static string _Oracle = "Oracle";

        /// <summary>
        /// Get DAO Client from Enum
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected static string GetDAOClient(DAOClient client)
        {
            switch (client)
            {
                case DAOClient.WCF:
                    return _WCF;
                case DAOClient.Linq:
                    return _Linq;
                case DAOClient.Entity:
                    return _Entity;
                default:
                    return _AdoNet;
            }
        }

        /// <summary>
        /// Get Data Source from Enum
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static string GetDataSource(DataSource source)
        {
            switch (source)
            {
                case DataSource.Access:
                    return _Access;
                case DataSource.DB2:
                    return _DB2;
                case DataSource.Oracle:
                    return _Oracle;
                default:
                    return _SqlServer;
            }
        }

        protected SqlDatabase GetDatabase(string dbName)
        {
            //var db = new SqlDatabase(ConString(dbName));
            //return db; //EnterpriseLibraryContainer.Current.GetInstance<Database>(dataBaseName);
            return GetDatabase(dbName, true);
        }

        protected SqlDatabase GetDatabase(string dbName, bool isEncrypted)
        {
            var db = isEncrypted ? new SqlDatabase(ConString(dbName)) :  new SqlDatabase(ConfigurationManager.ConnectionStrings[dbName].ConnectionString);
            return db;

        }
                     

        protected static string ConString(string dbtype)
        {
            return DbConnSettings.GetDbConnection(dbtype).ConnectionString;
        }

        protected static object CreateInstance(string provider, DAOClient client, DataSource source)
        {
            return
                Activator.CreateInstance(
                    Type.GetType(string.Format(provider, GetDAOClient(client), GetDataSource(source))));
        }

        protected virtual StoreProcedureElement GetStoreProcedure(AdminSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = AdminSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(CasinoSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = CasinoSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(TransactionGroupSection.StoreCollection storeCollection, string aliasName)
        {
            var sps = TransactionGroupSection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(CreditTermSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = CreditTermSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(CustomerSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = CustomerSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(ExchangeRateSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = ExchangeRateSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(HelperSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = HelperSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(LogDisplaySection.StoreCollection storeCollection, string aliasName)
        {
            var sps = LogDisplaySection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(RacingSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = RacingSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(P2PSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = P2PSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }


        protected virtual StoreProcedureElement GetStoreProcedure(ReportSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(SettlementSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = SettlementSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(SportbookPtSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = SportbookPtSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(ViewPtSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = ViewPtSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }


        protected virtual StoreProcedureElement GetStoreProcedure(BetAggregatorSection.StoreCollection storeCollection, string aliasName)
        {
            var sps = BetAggregatorSection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }


        protected virtual StoreProcedureElement GetStoreProcedure(BetListSection.StoreCollection storeCollection, string aliasName)
        {
            var sps = BetListSection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(UserControlSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = UserControlSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }

        protected virtual StoreProcedureElement GetStoreProcedure(SpecialDeleteGroupSection.StoreCollection storeCollection, string aliasName)
        {
            var sps = SpecialDeleteGroupSection.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;
        }
    }
}
