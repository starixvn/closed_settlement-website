using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess
{
    /// <summary>
    /// Summary description for DBType
    /// </summary>
    public class DBType
    {
        private static string _main = "main";
        private static string _archive = "archive";
        private static string _archive_bk = "archive_bk";
        private static string _onUser = "on_user";
        private static string _wareHouse = "warehouse";
        private static string _CSBackupDB = "CS_BackupDB";
        private static string _message = "message";
        private static string _report = "report";
        private static string _log = "log";
        private static string _ipdb = "ipdb";
        private static string _localtest = "localtest";
        private static string _miniGame = "CS_MiniBackupDB";
        private static string _db03 = "db03";
        private static string _dw = "dw";
        private static string _db053 = "db053";

        private static readonly string _maintenance = "maintenance";


        private static readonly string _punter = "punter";
        private static readonly string _external = "ex";


        public static string MiniGame
        {
            get { return _miniGame; }
        }

        public static string MAINTENANCE
        {
            get { return _maintenance; }
        }


        public static string PUNTER
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _punter
                           : _punter + "_" + _external;
            }
        }

        public static string MAIN
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _main
                           : _main + "_" + _external;
            }
        }

        public static string REPORT
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _report
                           : _report + "_" + _external;
            }
        }


        public static string ARCHIVE
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _archive
                           : _archive + "_" + _external;
            }
        }

        public static string ARCHIVE_BK
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _archive_bk
                           : _archive_bk + "_" + _external;
            }
        }

        public static string ON_USER
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _onUser
                           : _onUser + "_" + _external;
            }
        }

        public static string WAREHOUSE
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _wareHouse
                           : _wareHouse + "_" + _external;
            }
        }

        public static string DB03
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _db03
                           : _db03 + "_" + _external;
            }
        }

        public static string DW
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _dw
                           : _dw + "_" + _external;
            }
        }

        public static string DB053
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _db053
                           : _db053 + "_" + _external;
            }
        }

        public static string CS_BackupDB
        {
            get { return _CSBackupDB; }
        }

        public static string MESSAGE
        {
            get { return _message; }
        }

        public static string LOG
        {
            get
            {
                return AppSession.SYSTEM.ToLower() == Enumerations.AccountingSystem.Accounting.ToString().ToLower()
                           ? _log
                           : _log + "_" + _external;
            }
        }

        public static string IPDB
        {
            get { return _ipdb; }
        }

        public static string LOCALTEST
        {
            get { return _localtest; }
        }
    }
}
