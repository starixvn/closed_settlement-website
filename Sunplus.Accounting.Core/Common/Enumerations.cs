using System;


namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Summary description for Enumeration
    /// </summary>
    public static class Enumerations
    {
        #region AccountingSystem enum

        public enum AccountingSystem
        {
            Accounting = 1,
            EXTERNAL = 2,
        }

        #endregion

        #region MemberType enum

        public enum MemberType
        {
            Betting = 1,
            Internal = 2
        }

        #endregion

        #region Role enum

        public enum Role
        {
            Super = 4,
            Master = 3,
            Agent = 2,
            Member = 1
        }



        #endregion

        /// <summary>
        /// Get name accounting system 
        /// </summary>
        /// <param name="accountingSystem"></param>
        /// <returns>string</returns>
        public static string GetSystemName(string accountingSystem)
        {
            //return Enum.GetName(typeof(AccountingSystem), accountingSystem);
            return accountingSystem;
        }

        /// <summary>
        /// Convert a string to Accounting System enum
        /// if system name is null or empty AccountingSystem.Default
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public static AccountingSystem ToAccountingSystem(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                return AccountingSystem.Accounting;
            //systemName = systemName.Replace("ACC", string.Empty);

            return (AccountingSystem)Enum.Parse(typeof(AccountingSystem), systemName, true);
        }

        #region Nested type: Functions

        /// <summary>
        /// Enum for function
        /// </summary>
        public static class Functions
        {
            public enum Setting
            {
                Special = 1,
                Dangerous = 2,
                Internal = 3,
                AutoSuspend = 4,
                SelectedBetting = 5
            }

            /// <summary>
            /// </summary>
            public enum Reports
            {
                Report,
                ReportList,
                Report2a,
                Report2d,
                Report2h,
                Report2hSoccer,
                Report2hOtherSports,
                ReportTrader,
                ReportTraderSoccer,
                ReportTraderOtherSports,
                Report18,
                OtherReports,
                HorseRacing,
                ReportCasino,
                Finance,
                ReportM88Winloss,
                ReportFun88Winloss,
                Report12BETWinloss,
                ReportingServices,
                PunterAnalysis,
                NonePTMemSummary,
                ReportDetailWinlossThai,
                ReportDailyWinlossThai,
                ReportALogWinloss,
                Report2HS,
                Report2HO,
                ReportLicenseeWinLossByLicensee,
                SpecialMatchAnalysis
            }


            /// <summary>
            /// Ex460Rate = Ex.Rate
            /// </summary>
            public enum ExRates
            {
                VirtualRate,
                MinMaxExRate,
                ExchangeRate,
                UpdateExchangeRateOpenTote


            }


            public enum Customer
            {
                MaxExposure = 1,
                Credit,
                Password,
                OtherCust,
                ViewInfo,
                ViewLog,
                MoveSite,
                CreditTerm,
                IsApplyCssBalanceInfo,
                Customer,
                ViewPT,
                EditUserName,
                MoveCustomer,
                ChangeUserGroup,
                SuspendCustList,
                DailyTransaction,
                ClearCredit,
                ResetMaxTransfer
            }

            public enum Transfer
            {
                Transfer,
                OtherTrans,
                DeleteTrans,
                AddTransaction,
                CrossCurrencyTransfer
            }

            public enum BALicensee
            {
                BACommSetting,
                BAViewLogComm,
                BAReport,
                BALatestBet
            }

        #endregion

            #region FinancialManagement enum

            public enum FinancialManagement
            {
                FinanceTransfer,
                ViewFinanceAccount,
                CreateFinanceAccount,
                OpenCloseFinanceAccount,
                ManageTransaction,
                AutomateFinanceTransfer,
                ResetWinning,
                DailyFinanceTxn
            }

            #endregion

            #region Others enum

            /// <summary>
            /// Bet320List320400IP410 = Bet List (IP)
            /// </summary>
            public enum Others
            {
                Other,
                WhatNew,
                ExportStatement,
                BetListByIp,
                MeetingComm,
                CasinoSync
            }

            /// <summary>
            /// Calculate470Refund = Calculate/Refund
            /// </summary>
            public enum Settle
            {
                Settlement,
                KeyScore,
                Calculation,
                Unsettle,
                SettlementLog,
                UndecidedSettleMatch
            }
            public enum SpecialDelete
            {
                SpecialDeleteNumberGame,
                RecoverBets,
                SpecialDeleteSportBook,
                UpdateBatchMsg,
                SpecialDeleteMixParlay
            }



            public const string ViewIPVN = "ViewIPVN";
            public const string UploadVR = "UploadVR";
            //public const string SpecialDelete = "SpecialDelete";
            public const string UserControl = "UserControl";


            /// <summary>
            /// Get enum name, 
            /// Replace 200 to '/', 300 to ' ' (space)
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="function"></param>
            /// <returns></returns>
            public static string GetName<T>(T function)
            {
                if (typeof(T).IsEnum)
                    return Enum.GetName(typeof(T), function);
                return function.ToString();
            }
        }

            #endregion


        public enum ReportType
        {
            Horse_Racing_Daily_Report_For_UK = 1,
            Horse_Racing_Daily_Report_For_AU = 2,
            Greyhound_Daily_Report_For_UK = 3,
            Greyhound_Daily_Report_For_AU = 4,
            Harness_Daily_Report_For_AU = 5,
            Horse_Racing_Report_for_SA_Tote_NEW = 6,

            Horse_Racing_Daily_Report_Detail_For_UK = 101,
            Horse_Racing_Daily_Report_Detail_For_AU = 102,
            Greyhound_Daily_Report_Detail_For_UK = 103,
            Greyhound_Daily_Report_Detail_For_AU = 104,
            Harness_Daily_Report_Detail_For_AU = 105,
            Horse_Racing_Report_Detail_for_SA_Tote_NEW = 106,
        }

        public enum Admin
        {
            Admin_ManageExAcc = 1,
            Admin_ManageGlobalRes = 2,
            P2PDefaultSetting = 3
        }
    }

}
