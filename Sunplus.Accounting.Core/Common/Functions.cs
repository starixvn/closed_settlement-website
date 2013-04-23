using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Business class of all thing relate to Functions
    /// </summary>
    public static class Functions
    {
        public static readonly SiteMapNodeCollection SystemFunctions = SiteMap.RootNode.ChildNodes;

        /// <summary>
        /// Check if function is allow to display or not
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool IsVisibleFunction<T>(T func)
        {
            string functionName = Enumerations.Functions.GetName(func).ToLower();
            IEnumerable<SiteMapNode> function =
                SystemFunctions.Cast<SiteMapNode>().Where(p => p.Title.ToLower().Contains(functionName)
                                                               &&
                                                               p.Roles.Contains(AppSession.CurrentSystemNAME.ToString().ToLower()));
            return (function.Count() > 0);
        }

        #region Nested type: Customer

        public static class Customer
        {
            public static bool IsVisibleMaxExposure
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.MaxExposure); }
            }

            public static bool IsVisibleVirtualRate
            {
                get { return IsVisibleFunction(Enumerations.Functions.ExRates.VirtualRate); }
            }

            public static bool IsApplyCssBalanceInfo
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.IsApplyCssBalanceInfo); }
            }

            public static bool IsVisibleClearCredit
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.ClearCredit); }
            }

            public static bool IsAllowResetMaxTransfer
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.ResetMaxTransfer); }
            }

            public static bool AllowMoveCustomer
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.MoveCustomer); }
            }

            public static bool AllowViewLog
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.ViewLog); }
            }

            public static bool AllowChangeUserGroup
            {
                get { return IsVisibleFunction(Enumerations.Functions.Customer.ChangeUserGroup); }
            }
        }

        #endregion

        #region Nested type: ExchangeRate

        public static class ExchangeRate
        {
            public static bool IsAllowExchangeRate
            {
                get { return IsVisibleFunction(Enumerations.Functions.ExRates.ExchangeRate); }
            }
        }

        #endregion

        #region Nested type: Report

        public static class Report
        {
            public static bool IsVisibleReportList
            {
                get { return IsVisibleFunction(Enumerations.Functions.Reports.ReportList); }
            }

            public static bool AllowViewReport2A
            {
                get { return IsVisibleFunction(Enumerations.Functions.Reports.Report2a); }
            }
        }

        #endregion

        #region Nested type: Settings

        /// <summary>
        /// Check IsVisible Functions in Settings
        /// </summary>
        public static class Settings
        {
            public static bool IsVisibleDangerous
            {
                get { return IsVisibleFunction(Enumerations.Functions.Setting.Dangerous); }
            }

            public static bool IsVisibleSpecial
            {
                get { return IsVisibleFunction(Enumerations.Functions.Setting.Special); }
            }

            public static bool IsVisibleInternal
            {
                get { return IsVisibleFunction(Enumerations.Functions.Setting.Internal); }
            }

            public static bool IsVisibleAutoSuspend
            {
                get { return IsVisibleFunction(Enumerations.Functions.Setting.AutoSuspend); }
            }

            public static bool IsVisibleSelectedBetting
            {
                get { return IsVisibleFunction(Enumerations.Functions.Setting.SelectedBetting); }
            }
        }

        #endregion
    }
}