using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DNAide.Core;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Configurations
{
    /// <summary>
    /// Summary description for Settings
    /// </summary>
    public static class WebSettings
    {
        private static readonly string _templateFolder;
        private static readonly int _serverId;
        private static readonly string _themePath;
        private static readonly string _theme;
        private static readonly string _internalAgentUrl;
        private static readonly string _directBAUrl;
        private static readonly string _directABSUrl;
       
        private static readonly string _getCountry;
        private static readonly string _getDateMixparlayDailyWL;
        private static readonly string _virtualDir;
        private static readonly bool _isSslLogin;



        //Phuong.Nguyen 
        //Create disable horseracing property
        private static readonly bool _disabledHR;
        //Financial disable option
        private static readonly bool _disabledFI;
        private static double _UkMeetingMax = 0.03;
        private static double _OtherMeetingMax = 0.05;
        private static double _UkMeeting = 0.03;
        private static double _OtherMeeting = 0.05;
        private static readonly bool _disableAutoM;
        private static readonly bool _disableAutoMasterFI;
        private static readonly bool _disableAutoMasterHR;
        private static readonly bool _disableCS;
        private static readonly bool _disableOR;
        private static readonly bool _disableInternal;
        private static readonly bool _IncludeAccuBalInStatement;

        private static readonly double _MaxPTFI;
        private static readonly bool _DisableMinPTFI;
        private static readonly bool _Disable1x2Comm;
        private static readonly double _Group1x2Max = 0.0025;
        private static readonly bool _DisableHRPT_Tote;
        private static readonly bool _DisableHRPT_OpenTote;
        private static readonly int _CacheExpire;

        private static readonly string _calendarTheme;
        private static readonly string _ipvnreplace;


        private static string _iplocation;
        private static readonly bool _isAllowMoveToBingo;
    

        //henry
        private static readonly string _version;
        private static readonly bool _disableM88BetList;
        private static readonly bool _disableVR;

        private static readonly string _IgnoreGroups;

        private static string _swapVersionUrl;

        private static string _externalSite;
        private static readonly int _maxTransLength;
        private static int _maxTransIdPerBatch;

        private static string _hiddenCurrencies;

        public static List<string> NotAuthenticateAction { get; set; }

        static bool _IsApply50PercentRacingReport = false;

        static bool _IsApplySplitingDepositeCustomer = false;

        public static readonly int _isShowCustomerSettingRenovation = 0;
        
        // private static bool _useNewSpecialDelete;
        static WebSettings()
        {
            try
            {
                _templateFolder = ConfigurationManager.AppSettings["template_folder"];
                _serverId = 0;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["server_id"]))
                    int.TryParse(ConfigurationManager.AppSettings["server_id"], out _serverId);
                _theme = ConfigurationManager.AppSettings["theme"];
                _themePath = "App_Themes/" + _theme;
                _calendarTheme = ConfigurationManager.AppSettings["CalendarTheme"];

                _virtualDir = ConfigurationManager.AppSettings["VirtualDir"];
                _internalAgentUrl = ConfigurationManager.AppSettings["int_agent_url"];
                _directBAUrl = ConfigurationManager.AppSettings["link_BA_url"];
                _getCountry = ConfigurationManager.AppSettings["link_Country_url"];
                _getDateMixparlayDailyWL = ConfigurationManager.AppSettings["date_MixparlayAdvance"];
                _ipvnreplace = ConfigurationManager.AppSettings["ipvnreplace"];
                _isSslLogin = (ConfigurationManager.AppSettings["ssl_login"] == "1");

                _directABSUrl = ConfigurationManager.AppSettings["link_ABS_url"];

               // _baseCurr = ConfigurationManager.AppSettings["Fanex.Tarus.BaseCurrency"];
                //Phuong.Nguyen
                //Get Horseracing visibility from Web.config
                _disabledHR = (ConfigurationManager.AppSettings["disabledHR"] == "1");
                //Get Financial visibility from webconfig
                _disabledFI = (ConfigurationManager.AppSettings["disabledFI"] == "1");
                _disableAutoM = (ConfigurationManager.AppSettings["disableAutoMaster"] == "1");
                _disableAutoMasterFI = (ConfigurationManager.AppSettings["disableAutoMasterFI"] == "1");
                _disableAutoMasterHR = (ConfigurationManager.AppSettings["disableAutoMasterHR"] == "1");
                _disableCS = (ConfigurationManager.AppSettings["disabledCS"] == "1");
                _disableOR = (ConfigurationManager.AppSettings["disabledOR"] == "1");

                _MaxPTFI = CastAide.AsDouble(ConfigurationManager.AppSettings["maxPTFI"]);

                _DisableMinPTFI = (ConfigurationManager.AppSettings["disableMinPTFI"] == "1");
                _Disable1x2Comm = (ConfigurationManager.AppSettings["disable1x2Comm"] == "1");
                _Group1x2Max = CastAide.AsDouble(ConfigurationManager.AppSettings["group1x2max"]);
                _DisableHRPT_OpenTote = (ConfigurationManager.AppSettings["disableHRPT_OpenTote"] == "1");
                _DisableHRPT_Tote = (ConfigurationManager.AppSettings["disableHRPT_Tote"] == "1");
                _disableInternal = (ConfigurationManager.AppSettings["disableInternal"] == "1");
                _IncludeAccuBalInStatement = (ConfigurationManager.AppSettings["IncludeAccuBalInStatement"] == "1");
                //_useNewSpecialDelete = (System.Configuration.ConfigurationManager.AppSettings["useNewSpecialDelete"] == "1");
                _CacheExpire = CastAide.AsInt32(ConfigurationManager.AppSettings["CacheExpire"]);



                //henry
                _version = ConfigurationManager.AppSettings["version"];
                _disableM88BetList = (ConfigurationManager.AppSettings["disableM88BetList"] == "1");
                _disableVR = (ConfigurationManager.AppSettings["disableVR"] == "1");
                _iplocation = ConfigurationManager.AppSettings["iplocation"];
                _swapVersionUrl = ConfigurationManager.AppSettings["SwapVersionURL"];
                OpenMiniGame = (ConfigurationManager.AppSettings["OpenMiniGame"] == "1");
                _IgnoreGroups = ConfigurationManager.AppSettings["IgnoreGroups"];

                _externalSite = CastAide.AsString(ConfigurationManager.AppSettings["ExternalSite"]);

                _isAllowMoveToBingo = ConfigurationManager.AppSettings["isMoveBingo"] == "1";

                _maxTransLength = CastAide.AsInt32(ConfigurationManager.AppSettings["MaxTransLength"]);

                IsUMTime = ConfigurationManager.AppSettings["UM"] == "1";

                LbServiceUrl = ConfigurationManager.AppSettings["LBUrl"];

                var exceptionAction = ConfigurationManager.AppSettings["NotAuthenticateAction"].Split(';');
                NotAuthenticateAction = new List<string>();
                if (exceptionAction.Length != 0)
                {
                    foreach (var acction in exceptionAction)
                    {
                        if (acction != null) NotAuthenticateAction.Add(acction.ToLower());
                    }
                }
                EnableLiveCasino = (ConfigurationManager.AppSettings["EnableLiveCasino"] == "1");

                _maxTransIdPerBatch = CastAide.AsInt32(ConfigurationManager.AppSettings["MaxTransIdPerBatch"], 150);
                _hiddenCurrencies = CastAide.AsString(ConfigurationManager.AppSettings["HiddenCurrencies"], "");
                _IsApply50PercentRacingReport = ConfigurationManager.AppSettings["IsApply50PercentRacingReport"] == "1";
                _IsApplySplitingDepositeCustomer = ConfigurationManager.AppSettings["IsApplySplitingDepositeCustomer"] == "1";

                _isShowCustomerSettingRenovation = CastAide.AsInt32(ConfigurationManager.AppSettings["IsShowCustomerSettingRenovation"]);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                _templateFolder = "Templates";
                _serverId = 0;
            }
        }


        public static string LbServiceUrl { get; set; }

        public static bool IsUMTime { get; set; }

        public static string DirectABSUrl
        {
            get { return _directABSUrl; }
        }


        public static bool OpenMiniGame { get; set; }

        public static string SwapVersionUrl
        {
            get { return _swapVersionUrl + "?uid=" + AppSession.AccId + "&sid=" + HttpContext.Current.Session.SessionID; }
            set { _swapVersionUrl = value; }
        }

        /// <summary>
        /// Template folder
        /// </summary>
        public static string TemplateFolder
        {
            get { return _templateFolder; }
        }
        public static int ServerId
        {
            get { return _serverId; }
        }
        public static string ThemePath
        {
            get { return _themePath; }
        }
        public static string Theme
        {
            get { return _theme; }
        }
        public static string InternalAgentUrl
        {
            get { return _internalAgentUrl; }
        }
        public static string DirectBAUrl
        {
            get { return _directBAUrl;  }
        }
        public static string getCountryName
        {
            get { return _getCountry; }
        }

        public static string getDateDailyWLMP
        {
            get { return _getDateMixparlayDailyWL; }
        }
        public static bool VirtualDir
        {
            get { return _virtualDir == "1"; }
        }
        public static bool IsSslLogin
        {
            get { return _isSslLogin; }
        }
        public static bool IsLocalTest
        {
            get { return (ConfigurationManager.AppSettings["LocalTest"] == "1"); }
        }

        //public static string baseCurrency
        //{
        //    get { return _baseCurr; }
        //}
        //Phuong.Nguyen 
        //Create horseracing visibility property
        public static bool disabledHR
        {
            get { return _disabledHR; }
        }

        public static bool disabledFI
        {
            get { return _disabledFI; }
        }
        public static bool disableAutoM
        {
            get { return _disableAutoM; }
        }

        public static bool disableAutoMasterFI
        {
            get { return _disableAutoMasterFI; }
        }

        public static bool disableAutoMasterHR
        {
            get { return _disableAutoMasterHR; }
        }

        public static bool disableCS
        {
            get { return _disableCS; }
        }

        public static bool disableOR
        {
            get { return _disableOR; }
        }

        public static double UkMeetingMax
        {
            get { return _UkMeetingMax; }
        }
        public static double OtherMeetingMax
        {
            get { return _OtherMeetingMax; }
        }
        public static double UkMeeting
        {
            get { return _UkMeeting; }
        }
        public static double OtherMeeting
        {
            get { return _OtherMeeting; }
        }

        public static double MaxPTFI
        {
            get { return _MaxPTFI; }
        }


        public static bool DisableMinPTFI
        {
            get { return _DisableMinPTFI; }
        }

        public static bool Disable1x2Comm
        {
            get { return _Disable1x2Comm; }
        }

        public static double Group1x2Max
        {
            get { return _Group1x2Max; }
        }


        public static bool DisableHRPT_Tote
        {
            get { return _DisableHRPT_Tote; }
        }

        public static bool DisableHRPT_OpenTote
        {
            get { return _DisableHRPT_OpenTote; }
        }
        public static bool DisableInternal
        {
            get { return _disableInternal; }
        }

        public static string CalendarTheme
        {
            get { return _calendarTheme; }
        }
        //public static bool UseNewSpecialDelete
        //{
        //    get { return _useNewSpecialDelete; }
        //}

        /// <summary>
        /// Get time from DB server for timeout of internal url
        /// </summary>
        /// <returns></returns>
       

        public static int CacheExpire
        {
            get { return _CacheExpire; }
        }
        public static string IPVNReplace
        {
            get { return _ipvnreplace; }
        }


        public static string Version
        {
            get { return _version; }
        }
        public static bool DisableM88BetList
        {
            get { return _disableM88BetList; }
        }
        public static bool DisableVR
        {
            get { return _disableVR; }
        }

        public static string ipLocation
        {
            get { return _iplocation; }
        }
        public static bool IncludeAccuBalInStatement
        {
            get { return _IncludeAccuBalInStatement; }
        }
        public static string IgnoreGroups
        {
            get { return _IgnoreGroups; }
        }

        public static Dictionary<string,string> ExternalSite
        {
            get
            {
                var exDomain = _externalSite.Split(';');
                return exDomain.ToDictionary(domain => domain);
            }
        }
        public static bool IsAllowMoveToBingo
        {
            get { return _isAllowMoveToBingo; }
        }

        public static int MaxTransLength
        {
            get { return _maxTransLength; }
        }

        public static  bool EnableLiveCasino { get; set; }

        public static int MaxTransIdPerBatch
        {
            get { return _maxTransIdPerBatch; }
        }

        public static string HiddenCurrencies
        {
            get { return _hiddenCurrencies; }
        }

        public static bool IsApply50PercentRacingReport
        {
            get{return _IsApply50PercentRacingReport;}
        }

        public static bool IsApplySplitingDepositeCustomer
        {
            get { return _IsApplySplitingDepositeCustomer; }
        }

        public static int IsShowCustomerSettingRenovation
        {
            get { return _isShowCustomerSettingRenovation; }
        }

        public static string SystemName { get{return (ConfigurationManager.AppSettings["SystemName"]);} }
    }
}
