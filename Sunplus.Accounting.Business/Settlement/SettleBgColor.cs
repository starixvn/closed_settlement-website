namespace Sunplus.Accounting.Business.Settlement
{
    /// <summary>
    /// Summary description for SettleBgColor
    /// </summary>
    public static class SettleBgColor
    {
        private static string _postponed = " class='bgPostponed'";
        private static string _bill = " class='bgBill'";
        private static string _refund = " class='bgRef'";
        private static string _multiple = " class='bgMul'"; // _bill
        private static string _refundMultiple = " class='bgRefMul'"; // _postponed
        private static string _refundFulltime = " class='bgRefFull'"; // _refund
        private static string _htonly = " class='bgHTOnly'"; // HT calculated

        public static string POSTPONED
        {
            get { return _postponed; }
        }

        public static string BILL
        {
            get { return _bill; }
        }

        public static string REFUND
        {
            get { return _refund; }
        }

        public static string MULTIPLE
        {
            get { return _bill; }
        }

        public static string REFUNDMULTIPLE
        {
            get { return _postponed; }
        }

        public static string REFUNDFULLTIME
        {
            get { return _refund; }
        }

        public static string HTCALONLY
        {
            get { return _htonly; }
        }

        public static string DEFAULTCOLOR
        {
            get { return string.Empty; }
        }

        public static bool HasHostPermission { get; set; }

        public static string PendingStatus { get { return "class='bgpending'"; } }
    }
}