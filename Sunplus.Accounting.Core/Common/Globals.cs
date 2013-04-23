using System;
using System.Web.Configuration;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Summary description for Globals
    /// </summary>
    public static class Globals
    {
   

        public static readonly IBCAccounting IBCSetting =
            (IBCAccounting) WebConfigurationManager.GetSection("IBCAccounting");

        public static readonly string DELIMITER_RESPONSE = "|";

        //tag block definition
        public static readonly int ErrorCodeInvalid = 1000;
        public static readonly string ErrorMsgInvalid = "Invalid Data format!";
        public static readonly string ErrorResource = "ErrorMsg";

        public static readonly string PRESET_PT = "preset_pt";
        public static readonly string AUTO_PT = "auto_pt";
        public static readonly string MAX_PT = "max_pt";
        public static readonly string MIN_PT = "min_pt";
        public static readonly string COMM_PT = "comm";
        public static readonly string TAG_HEADER = "header";
        public static readonly string TAG_GROUP_SETTING = "group_setting";
        public static readonly string TAG_PRIVATE_SETTING = "private_setting";
        public static readonly string TAG_BETTING_LIMIT = "betting_limit";
        public static readonly string TAG_BET_SETTING = "cs-bet-setting";

        public static readonly string TAG_CASINO_EDIT = "casino-edit";
        public static readonly string TAG_FINANCE_EDIT = "finance-edit";

        //path to template
        public static readonly string CASINO_PT_TEMPLATE = "Customer/v2/CasinoPT.htm";
        public static readonly string CASINO_PT_HEADER_TEMPLATE = "Customer/v2/CasinoPTHeader.htm";

        public static readonly string CASINO_P2P_PT_TEMPLATE = "Customer/v2/CasinoP2PPT.htm";
        public static readonly string CASINO_OTHERS_PT_TEMPLATE = "Customer/v2/CasinoOthersPT.htm";

        ////////////////////////////////


        public static readonly string GAME_P2P = "P2P";
        public static readonly int GAME_P2P_ID = 10;
        public static readonly string GAME_LIVE = "Live";
        public static readonly int GAME_LIVE_ID = 8;
        public static readonly string GAME_RNG = "RnG";
        public static readonly int GAME_RNG_ID = 9;
        public static readonly int GAMEKIND_GROUP = 0;

        //path to controls template (hidden, text, checkbox, button..)
        public static readonly string TPL_CTR_SELECTOPTION = "Controls/SelectOption.htm";
        public static readonly string TPL_CTR_CHECKBOX = "Controls/CheckBox.htm";
        public static readonly string TPL_CTR_HIDDEN = "Hidden.htm";
        public static readonly string TPL_CTR_BUTTON = "Controls/Button.htm";
        public static readonly string TPL_CTR_TEXTBOX = "Controls/TextBox.htm";
        public static readonly string TPL_CTR_INPUT = "Controls/Input.htm";
        public static readonly string TPL_CTR_LABEL= "Controls/Label.htm";
        public static readonly string TPL_CTR_LINK = "Controls/Link.htm";
        // /////////////////////////

        public static readonly int CS_GAME_GROUP = 0;

        public static readonly string strMaxPT = "Max";
        public static readonly string strMinPT = "Min";
        public static readonly string Comm = "Comm";
        public static readonly string strPT = "PT";
        public static readonly string strPresetPT = "Pre";
        public static readonly string strAutoPT = "Auto";

        public static readonly string Super = "S";
        public static readonly string Master = "M";
        public static readonly string Agent = "A";
        public static readonly string Member = "Mem";

        public static readonly int SuperLevel = 4;
        public static readonly int MasterLevel = 3;
        public static readonly int AgentLevel = 2;
        public static readonly int MemberLevel = 1;

        //Define max value of Min, Max, Auto, Preset PT, Comm
        public static readonly double MAX_MaxPTValue = 0.9;
        public static readonly double MAX_MinPTValue = 0.9;
        public static readonly double MAX_AutoPTValue = 0.9;
        public static readonly double MAX_PresetPTValue = 0.9;
        public static readonly double MAX_Comm = 0.01;
        public static readonly double CURRENT_MaxPTValue = 0.9;
        public static readonly double CURRENT_MinPTValue = 0.9;
        public static readonly double CURRENT_AutoPTValue = 0.9;
        public static readonly double CURRENT_PresetPTValue = 0.9;
        public static readonly double CURRENT_Comm = 0.01;

        public static readonly int INTERVAL_PT_HR = 1000;
        public static readonly int INTERVAL_PT = 100;
        public static readonly int INTERVAL_COMM = 5;


        public static readonly string CHKGROUP = "ChkGroupSetting";
        public static readonly string strCHKAUTO = "ChkAuto";
        public static readonly string strCHK = "Chk";
        public static readonly string CHK_STAKE = "ChkStake";

        public static readonly string SEP_GAMEKIND_GAMETYPE = "_";

   
        //hidden
        public static readonly string HIDDEN_CustId = "custid";
        public static readonly string HIDDEN_Role = "roleid";
        public static readonly string HIDDEN_UplineId = "uplineid";
        public static readonly string HIDDEN_GAMEDATA = "hidCSData";
        public static readonly string HIDDEN_RoldID = "hidRoleId";
        public static readonly string HIDDEN_IBCCustID = "hidCustID";
        public static readonly string HIDDEN_USERNAME = "hidUsername";
        public static readonly string HIDDEN_FIRSTNAME = "hidFirstname";
        public static readonly string HIDDEN_PHONE = "hidPhone";
        public static readonly string HIDDEN_FAX = "hidFax";
        public static readonly string HIDDEN_USERNAME2 = "hidUsername2";
        public static readonly string HIDDEN_LASTNAME = "hidLastname";
        public static readonly string HIDDEN_MOBILEPHONE = "hidMobilePhone";
        public static readonly string HIDDEN_SIBCCustID = "hidSCustID";
        public static readonly string HIDDEN_MIBCCustID = "hidMCustID";
        public static readonly string HIDDEN_AIBCCustID = "hidACustID";
        public static readonly string HIDDEN_PTByGameType = "hidPTByGameType";
        public static readonly string HIDDEN_ExchangeID = "hidExchangeID";
        public static readonly string HIDDEN_LangID = "hidLangID"; // hard code
        public static readonly string HIDDEN_CountryID = "hidCountryID";// hard code
        public static readonly string HIDDEN_CLOSED = "hidClosed";
        public static readonly string HIDDEN_SUSPEND = "hidSuspend";
        public static readonly string HIDDEN_PT = "hidPT";
        public static readonly string HIDDEN_UPDDOWNLLINE = "hidUpdDownline";

        public static readonly string CBX_UNLIMITED = "cbxUnlimitedId";

        public static readonly string OPEN_GAMETYPELIST_MARK = "[";
        public static readonly string CLOSE_GAMETYPELIST_MARK = "]";
        public static readonly string OPEN_STAKELIST_MARK = "(";
        public static readonly string CLOSE_STAKELIST_MARK = ")";
        public static readonly string SEP_STAKE = ", ";
        public static readonly string SEP_GAMETYPE = "-";
        public static readonly string SEP_GAMEKIND = ";";
        public static readonly string SEP_PT_VALUE = ":";
        public static readonly string SEP_PT = "|";

    
        //class stylesheet
        public static readonly string CLASS_HIDDEN = "hidden";
        public static readonly string CLASS_ERROR_MAXVALUE = "errMaxValue";
        public static readonly string CLASS_SELECT_COMMISSION = "selectcomm";
        public static readonly string CLASS_SUBMIT = "b_add";
        //Group increase, decrease PT button class, suffix and click function
        public static readonly string BTN_INC1_CLASS = "btn-inc1 btn-enabled";
        public static readonly string BTN_INC10_CLASS = "btn-inc10 btn-enabled";
        public static readonly string BTN_DEC1_CLASS = "btn-dec1 btn-enabled";
        public static readonly string BTN_DEC10_CLASS = "btn-dec10 btn-enabled";

        public static readonly string BTN_INC1_DISABLE_CLASS = "btn-inc1 btn-disabled";
        public static readonly string BTN_INC10_DISABLE_CLASS = "btn-inc10 btn-disabled";
        public static readonly string BTN_DEC1_DISABLE_CLASS = "btn-dec1 btn-disabled";
        public static readonly string BTN_DEC10_DISABLE_CLASS = "btn-dec10 btn-disabled";

        public static readonly string BTN_INC1_SUFFIX = "BtnInc_1";
        public static readonly string BTN_INC10_SUFFIX = "BtnInc_10";
        public static readonly string BTN_DEC1_SUFFIX = "BtnDec_1";
        public static readonly string BTN_DEC10_SUFFIX = "BtnDec_10";


        //SESSION
        public static readonly string SESSION_CUSTOMER = "customer";
        //CS PT setting id

        public static readonly string TXT_MAXWINID = "txtMaxWin";
        public static readonly string TXT_MAXTRANSTERCREDIT = "txtMaxTransferCredit";
        public static readonly string TXT_MAXLOSEID = "txtMaxLose";
        public static readonly string BTN_ResetID = "btnReset";

        //nolimited value
        public static readonly int CS_NOLIMITED_WINLOSE = 0;

    
        //class style
        public static readonly string CLASS_CHECKBOX_HIDDEN = "chk-hidden";
        /////////

        //END

        //constant
        public static readonly string s4Max = "s4Max",
                                      s3Max = "s3Max",
                                      s1Max = "s1Max",
                                      m3Max = "m3Max",
                                      m2Max = "m2Max",
                                      m1Max = "m1Max",
                                      a2Max = "a2Max",
                                      a1Max = "a1Max";

        public static readonly string u3Max = "u3Max",
                                      n4Max = "n4Max",
                                      n3Max = "n3Max",
                                      n2Max = "n2Max",
                                      l4Max = "l4Max",
                                      l3Max = "l3Max",
                                      l2Max = "l2Max";

        public static readonly string m3MaxAuto = "m3MaxAuto", n3MaxAuto = "n3MaxAuto";


        public static readonly string CTR_INPUT_BUTTON = "button";
        public static readonly string CTR_INPUT_HIDDEN = "hidden";
        public static readonly string CTR_INPUT_TEXTBOX = "text";
        public static readonly string CTR_INPUT_TEXTBOX_PREFIX = "txt";
        public static readonly string CTR_INPUT_CHECKBOX = "checkbox";
        public static readonly string CTR_OPTIONS_PREFIX = "opt";

        //Define ID
        public static readonly string ID_CHECKBOX_DISABLEDCASINO = "chkDisabledCasino";
        public static readonly string ID_CHECKBOX_DISABLEDCASINO_DL = "chkDisabledCasino_DL";
        public static readonly string ID_SPAN_DOWNLINE_OPT = "spanDownlineOpt";
        public static readonly string ID_CHECKBOX_CLOSED = "chkClosed";
        public static readonly string ID_CHECKBOX_SUSPEND = "chkSuspend";

        //define prefix of control
        public static readonly string CTR_LABEL_PREFIX = "error";
        public static readonly string PREFIX_CHECKBOX = "chk";

        //Hard code value
        public static readonly int CONST_LangID; // hard code
        public static readonly int CONST_CountryID; // hard code
        public static readonly double CONST_CHANGEPT_1 = 0.01;
        public static readonly double CONST_CHANGEPT_10 = 0.1;


        //Casino Javascript Variables

        public static readonly string JS_arrLiveGameType = "arrLiveGameType";
        public static readonly string JS_arrRnGGameType = "arrRnGGameType";
        public static readonly string JS_arrP2PGameType = "arrP2PGameType";
        public static readonly string JS_arrGameKind = "arrGameKind";
        public static readonly string JS_arrGameKindID = "arrGameKindID";
        public static readonly string JS_VariableFormat = "{0} = {1};";
        public static readonly string JS_VariableDefinitionFormat = "var {0} = {1};";
        public static readonly string JS_arrayFormat = "[{0}]";
        public static readonly string JS_Block = "<script type='text/javascript'>{0}</script>";
        //define javascript functions

        //button click
        public static readonly string BTN_CLICK_INC_DEC_FUNC = "";
        public static readonly string BTN_CLICK_RESET_CS_PT_FUNC = "";
        /// <summary>
        /// {0}: button control, "this"
        /// {1}: value
        /// </summary>
        public static readonly string BTN_CHANGEPT_CLICK = "ChangePT({0}, {1})"; //{0}

        //select option change
        public static readonly string CS_SELECTOPTION_ONCHANGE_GROUP_FUNC = "OnGroupSettingChange({0})";

        public static readonly string CS_SELECTOPTION_ONCHANGE_GROUP_PARAM = "this";
        //"this" is the control use as group setting

        public static readonly string CS_SELECTOPTION_ONCHANGE_FUNC = "";
        public static readonly string CS_SELECTOPTION_ONCHANGE_PARAM = "";

        public static readonly string CS_SELECTOPTION_ONCHANGE_PRESET_FUNC = "ChangePresetPT({0})";
        public static readonly string CS_SELECTOPTION_ONCHANGE_PRESET_PARAM = "this";
        //checkbox click
        public static readonly string CS_ChkAUTO_CLICK_FUNC = "CheckAuto({0})";
        public static readonly string CS_ChkGROUP_CLICK_FUNC = "CheckGroupSetting({0})"; //{0}: arguments of this funciton
        public static readonly string CS_ChkAutoGROUP_CLICK_FUNC = "CheckGroupAuto({0})"; //{0}: arguments of this funciton

        //submit function
        public static readonly string CS_SUBMIT_HREF = "javascript:void();";
        public static readonly string CS_SUBMIT_FUNC = "DelayReloadPage(); obj('btnSubmit').click();";
        //////////END

        //Define provider type
        public static readonly string PROVIDER_CS_API = "DAL.APIClient.APICasinoProvider";
        public static readonly string PROVIDER_IBC_LIB = "DAL.APIClient.IBCLibProvider";
        ///////

        //Cust Info Setting
        public static readonly string CUSTINFO_HTM_TEMPLATE = "settingcontrol.htm";
        //
        //Cust Menu
        public static readonly string CUSTMENU_HTM_TEMPLATE = "linkmenu.htm";
    

        //EditPTOnMemberType:
        public enum PTType : int
        {
            Default = 0, SuperOnMember = 1, MasterOnMember = 2, AgentOnMember = 3, MinMaxPT=4
        }
    
        //Account type
        public enum AccountType:int
        {
            Super = 4, Master = 3, Agent = 2, Member = 1
        }
        //Input control type
        public enum InputType
        {
            Button = 4, Text = 3, Hidden = 2, CheckBox = 1
        }

        //Casino Group
        public enum CasinoGroup
        {
            Group1 = 1, Group2 = 2
        }

        public static readonly string CASINO_GROUP1 = "01";
        public static readonly string CASINO_GROUP2 = "02";

        public static string GetCasinoGroup(CasinoGroup group)
        {
            try
            {
                switch (group)
                {
                    case CasinoGroup.Group1:
                        return CASINO_GROUP1;
                    case CasinoGroup.Group2:
                        return CASINO_GROUP2;
                }
                return "";
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return string.Empty;
            }
        }


        public static string GetInputType(InputType input)
        {
            try
            {
                switch(input)
                {
                    case InputType.Button:
                        return CTR_INPUT_BUTTON;
                    case InputType.CheckBox:
                        return CTR_INPUT_CHECKBOX;
                    case InputType.Hidden:
                        return CTR_INPUT_HIDDEN;
                    case InputType.Text:
                        return CTR_INPUT_TEXTBOX;
                }
                return "";
            }
            catch(Exception ex)
            {
           
                return string.Empty;
            }
       
        }

        public enum CasinoError : int
        {
            Not_Casino_Customer = 0
        }

        public static string GetCasinoError(CasinoError errorCode)
        {
            switch(errorCode)
            {
                case CasinoError.Not_Casino_Customer:
                    return "Not casino's Customer";
            }
            return "";
        }


        

        //User type
        public enum UserType : int
        {
            None =0, Betting = 1, Internal = 2
        }

    }
}
