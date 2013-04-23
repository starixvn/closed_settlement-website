using System;

/// <summary>
/// Summary description for CustomerDetail
/// </summary>
namespace Sunplus.Accounting.Entities
{

    [Serializable]
    public class CustomerDetail
    {
        private int _CustID;
        private int _RoleId;
        private bool _IsDisabledFIUL = false;
        private bool _IsDisabledFI = false;
        private bool _IsDisabledFIDL = false;
        private bool _IsDisabledHRUL = false;
        private bool _IsDisabledHR = false;
        private bool _IsDisabledHRDL = false;
        private bool _IsDisabledOpenToteUL = false;
        private bool _IsDisabledOpenTote = false;
        private bool _IsDisabledOpenToteDL = false;
        private bool _IsDisabledBingoGame = false;
        private bool _IsDisabledBingoGameDL = false;
        private bool _IsDisabledNumberGame = false;
        private bool _IsDisabledNumberGameDL = false;
        private bool _IsInternalUser = false;
        private int _LimitPerIP = 0;

        //@Huynguyen edited
        private bool _IsClosed = false;
        private bool _IsSuspended = false;
        private bool _IsAutoSuspend = false;
        private bool _IsDisableCasino = false;
        private bool _IsCasinoSync = false;
        private bool _IsBingoSync = false;
        private bool _IsSpecial = false;
        private bool _IsAllowMasterAutoPT = false;
        private bool _IsDisableOutright = false;
        private bool _SelectedBetting;
        //end

        private string _UserName2;
        private string _UserName;
        private string _FirstName;
        private string _LastName;
        private string _Phone;
        private string _MobilePhone;
        private string _Email;
        private string _Fax;
        private int _UplineId;

        private string _SuperUserName;
        private string _MasterUserName;
        

        public int Currency { get; set; }


        public CustomerDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int CustID
        {
            get
            {
                return _CustID;
            }
            set
            {
                _CustID = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RoleId
        {
            get
            {
                return _RoleId;
            }
            set
            {
                _RoleId = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UplineId
        {
            get
            {
                return _UplineId;
            }
            set
            {
                _UplineId = value;
            }
        }

     
        /// <summary>
        /// 
        /// </summary>
        public string UserName2
        {
            get
            {
                return _UserName2;
            }
            set
            {
                _UserName2 = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                _Phone = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MobilePhone
        {
            get
            {
                return _MobilePhone;
            }
            set
            {
                _MobilePhone = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            get
            {
                return _Fax;
            }
            set
            {
                _Fax = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledFI
        {
            get
            {
                return _IsDisabledFI;
            }
            set
            {
                _IsDisabledFI = value;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledFIDL
        {
            get
            {
                return _IsDisabledFIDL;
            }
            set
            {
                _IsDisabledFIDL = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledFIUL
        {
            get
            {
                return _IsDisabledFIUL;
            }
            set
            {
                _IsDisabledFIUL = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledHR
        {
            get
            {
                return _IsDisabledHR;
            }
            set
            {
                _IsDisabledHR = value;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledHRDL
        {
            get
            {
                return _IsDisabledHRDL;
            }
            set
            {
                _IsDisabledHRDL = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledOpenTote
        {
            get
            {
                return _IsDisabledOpenTote;
            }
            set
            {
                _IsDisabledOpenTote = value;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledOpenToteDL
        {
            get
            {
                return _IsDisabledOpenToteDL;
            }
            set
            {
                _IsDisabledOpenToteDL = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledHRUL
        {
            get
            {
                return _IsDisabledHRUL;
            }
            set
            {
                _IsDisabledHRUL = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDisabledOpenToteUL
        {
            get
            {
                return _IsDisabledOpenToteUL;
            }
            set
            {
                _IsDisabledOpenToteUL = value;
            }
        }

        public bool IsClosed
        {
            get
            {
                return _IsClosed;
            }
            set
            {
                _IsClosed = value;
            }
        }

        public bool IsSuspended
        {
            get
            {
                return _IsSuspended;
            }
            set
            {
                _IsSuspended = value;
            }
        }

        public bool IsAutoSuspend
        {
            get
            {
                return _IsAutoSuspend;
            }
            set
            {
                _IsAutoSuspend = value;
            }
        }
       

        public bool IsDisableCasino
        {
            get
            {
                return _IsDisableCasino;
            }
            set
            {
                _IsDisableCasino = value;
            }
        }


        public bool IsCasinoSync
        {
            get
            {
                return _IsCasinoSync;
            }
            set
            {
                _IsCasinoSync = value;
            }
        }

        public bool IsBingoSync
        {
            get
            {
                return _IsBingoSync;
            }
            set
            {
                _IsBingoSync = value;
            }
        }


        public bool IsSpecial
        {
            get
            {
                return _IsSpecial;
            }
            set
            {
                _IsSpecial = value;
            }
        }

        public bool IsAllowAutoMasterPT
        {
            get
            {
                return _IsAllowMasterAutoPT;
            }
            set
            {
                _IsAllowMasterAutoPT = value;
            }
        }


        public bool IsDisableOutright
        {
            get
            {
                return _IsDisableOutright;
            }
            set
            {
                _IsDisableOutright = value;
            }
        }

        public bool IsDisabledBingoGame
        {
            get
            {
                return _IsDisabledBingoGame;
            }
            set
            {
                _IsDisabledBingoGame = value;
            }
        }

        public bool IsDisabledBingoGameDL
        {
            get
            {
                return _IsDisabledBingoGameDL;
            }
            set
            {
                _IsDisabledBingoGameDL = value;
            }
        }

        public bool IsDisabledNumberGame
        {
            get
            {
                return _IsDisabledNumberGame;
            }
            set
            {
                _IsDisabledNumberGame = value;
            }
        }

        public bool IsDisabledNumberGameDL
        {
            get
            {
                return _IsDisabledNumberGameDL;
            }
            set
            {
                _IsDisabledNumberGameDL = value;
            }
        }

        public bool IsInternalUser
        {
            get
            {
                return _IsInternalUser;
            }
            set
            {
                _IsInternalUser = value;
            }
        }

        public bool SelectedBetting
        {
            get
            {
                return _SelectedBetting;
            }
            set
            {
                _SelectedBetting = value;
            }
        }

        public string SuperUserName
        {
            get
            {
                return _SuperUserName;
            }
            set
            {
                _SuperUserName = value;
            }
        }

        public string MasterUserName
        {
            get
            {
                return _MasterUserName;
            }
            set
            {
                _MasterUserName = value;
            }
        }

        public int LimitPerIP
        {
            get
            {
                return _LimitPerIP;
            }
            set
            {
                _LimitPerIP = value;
            }
        }

        public bool IsDisableLiveCasino { get; set; }
        public bool IsLiveCasinoSync { get; set; }
        #endregion


    }
}