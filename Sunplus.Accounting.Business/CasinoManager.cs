using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using CasinoLibrary.SBAgent;
using DNAide.Core;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using DNAide.Web;
using DNAide.Utils;


//using Sunplus.Core.LoggingLib;

/// <summary>
/// Manage class for Casino Business
/// </summary>
namespace Sunplus.Accounting.Business
{
    public class CasinoManager
    {
        private static double s4 = 0.9,
                              s3 = 0.9,
                              s1 = 0.9,
                              m4 = 0.9,
                              m3 = 0.9,
                              m2 = 0.9,
                              m1 = 0.9,
                              a4 = 0.9,
                              a2 = 0.9,
                              a1 = 0.9;

        private static double u1 = 0.9, u3 = 0.9, n4, n3, n2, l4 = 0.9, l3 = 0.9, l2 = 0.9;

        private static double s4Max = 0.9,
                              s3Max = 0.9,
                              s1Max = 0.9,
                              m3Max = 0.9,
                              m2Max = 0.9,
                              m1Max = 0.9,
                              a2Max = 0.9,
                              a1Max = 0.9;

        private static double u1Max = 0.9,
                              u3Max = 0.9,
                              n4Max = 0.9,
                              n3Max = 0.9,
                              n2Max = 0.9,
                              l4Max = 0.9,
                              l3Max = 0.9,
                              l2Max = 0.9;

        private static double m3MaxAuto = 0.9, n3MaxAuto = 0.9;

        private static double s4Min, s3Min, s1Min, m3Min, m2Min, m1Min, a2Min, a1Min;
        private static double u1Min, u3Min, n4Min, n3Min, n2Min, l4Min, l3Min, l2Min;
        private static double m3MinAuto, n3MinAuto;
        private static double maxPT = 0.9, minPT, maxAutoPT = 0.9;
        private static List<string> lsJSData;
        private static List<GameKindDetail> listGameKind;
        private static Hashtable htGameType;
        private static string JS_hGameKindRowSpan = "hRowSpan";
        private static string JS_AssignHashtableData = "{0}.put('{1}', {2});";

        private static string Game_Mini = "Mini";
        /// <summary>
        /// Update Customer Close status
        /// </summary>
        /// <param name="IBCCustID"></param>
        /// <param name="Closed"></param>
        /// <param name="FromIP"></param>
        /// <returns></returns>
        public static bool UpdateCustClosed(int IBCCustID, bool Closed, string FromIP)
        {
            try
            {
                return IBCProvider.Casino.UpdateCustClosed(IBCCustID, Closed, FromIP);
            }
            catch (Exception ex)
            {
                //Logs.Write(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Update Customer Enabled Casino status
        /// </summary>
        /// <param name="IBCCustID"></param>
        /// <param name="EnabledCasino"></param>
        /// <param name="UpdateDownline"></param>
        /// <param name="FromIP"></param>
        /// <returns></returns>
        public static bool UpdateCustEnableCasino(int IBCCustID, bool EnabledCasino, bool UpdateDownline, string FromIP)
        {
            try
            {
                return IBCProvider.Casino.UpdateCustEnableCasino(IBCCustID, EnabledCasino, UpdateDownline, FromIP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Cust Password
        /// </summary>
        /// <param name="IBCCustID"></param>
        /// <param name="UserPwdEncrypted"></param>
        /// <param name="FromIP"></param>
        /// <returns></returns>
        public static bool UpdateCustPwd(int IBCCustID, string UserPwdEncrypted, string FromIP)
        {
            try
            {
                return IBCProvider.Casino.UpdateCustPwd(IBCCustID, UserPwdEncrypted, FromIP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Cust Suspend
        /// </summary>
        /// <param name="IBCCustID"></param>
        /// <param name="Suspended"></param>
        /// <param name="FromIP"></param>
        /// <returns></returns>
        public static bool UpdateCustSuspend(int IBCCustID, bool Suspended, string FromIP)
        {
            try
            {
                return IBCProvider.Casino.UpdateCustSuspend(IBCCustID, Suspended, FromIP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CSCustomerDetail GetCasinoStatus(int custId)
        {
            //Helper.ResetCasinoSetting(false, Globals.PROVIDER_IBC_LIB);
            CSCustomerDetail _cust = IBCProvider.IBCCustomer.GetCasinoStatus(custId);
            //Helper.ResetCasinoSetting(true);
            return _cust;
        }

        public static void GetCasinoStatus(int custId, ref bool DisabledCs, ref bool SyncCS)
        {
            //Helper.ResetCasinoSetting(false, Globals.PROVIDER_IBC_LIB);
            CSCustomerDetail _cust = IBCProvider.IBCCustomer.GetCasinoStatus(custId);
            DisabledCs = _cust.CasinoDisabled;
            SyncCS = _cust.CasinoSync;
            //Helper.ResetCasinoSetting(true);
        }

        /// <summary>
        /// Check customer's casino status
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static bool IsDisabledCasino(int custId)
        {
            return GetCasinoStatus(custId).CasinoDisabled;
        }
        /// <summary>
        /// Check customer casino synchronize
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static bool IsSyncCasino(int custId)
        {
            return GetCasinoStatus(custId).CasinoSync;
        }
        /// <summary>
        /// Update Casino status
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="bSyncCS"></param>
        /// <param name="bDisabledCS"></param>
        /// <param name="isUpdDownline"></param>
        /// <returns></returns>
        public static bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            //Helper.ResetCasinoSetting(false, Globals.PROVIDER_IBC_LIB);
            bool status = IBCProvider.IBCCustomer.UpdateCSStatus(custid, bSyncCS, bDisabledCS, isUpdDownline);
            //Helper.ResetCasinoSetting(true);
            return status;
        }

        public static CSCustomerDetail GetCustomerSettingCS(int _IBCCustId)
        {
            try
            {
                return IBCProvider.Casino.GetCustSettingCS(_IBCCustId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void GetCustSettingCS(ref CSCustomerDetail _cust)
        {
            try
            {
                IBCProvider.Casino.GetCustSettingCS(ref _cust);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CSCustomerDetail GetCustInfoIBC(int _IBCCustId)
        {
            try
            {
                //Helper.ResetCasinoSetting(false, Globals.PROVIDER_IBC_LIB);
                CSCustomerDetail _cust = IBCProvider.IBCCustomer.GetCustInfoIBC(_IBCCustId);
                //Helper.ResetCasinoSetting(true);
                return _cust;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GameKindDetail> GetGameKinds()
        {
            try
            {
                return IBCProvider.Casino.GetGameKinds();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get Casino game kind list
        /// </summary>
        /// <param name="casinoNo"></param>
        /// gameGroup = "01", "02"
        /// <returns></returns>
        public static List<GameKindDetail> GetGameKinds(string casinoNo)
        {
            try
            {
                return IBCProvider.Casino.GetGameKinds(casinoNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<GameKindDetail> GetGameKinds(int _IBCCustId)
        {
            try
            {
                return IBCProvider.Casino.GetGameKinds(_IBCCustId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit Super, Master on member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static List<GameKindDetail> GetGameKinds_EditMember(int memberId)
        {
            return IBCProvider.Casino.GetGameKinds_EditMember(memberId);
        }

        /// <summary>
        /// Edit Super, Master on member
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="gameKindID"></param>
        /// <returns></returns>
        public static List<GameTypeDetail> GetGameTypes_EditMember(int memberId, int gameKindID)
        {
            return IBCProvider.Casino.GetGameTypes_EditMember(memberId, gameKindID);
        }

        public static List<GameKindDetail> GetGameKinds_MasterEditMember(int memberid)
        {
            try
            {
                return IBCProvider.Casino.GetGameKinds_MasterEditMember(memberid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<GameTypeDetail> GetGameTypes_MasterEditMember(int memberid, int gameKindID)
        {
            try
            {
                return IBCProvider.Casino.GetGameTypes_MasterEditMember(memberid, gameKindID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<StakeLevelDetail> GetCustStakeLevels(int _IBCCustId, int gameTypeID)
        {
            try
            {
                return IBCProvider.Casino.GetCustStakeLevels(_IBCCustId, gameTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get stake list by game type
        /// </summary>
        /// <param name="gameTypeID"></param>
        /// <returns></returns>
        public static List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, int currencyId)
        {
            try
            {
                return IBCProvider.Casino.GetGameTypeStakeLevels(gameTypeID, currencyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, string currencyName)
        {
            try
            {
                return IBCProvider.Casino.GetGameTypeStakeLevels(gameTypeID, currencyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindID">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public static List<GameTypeDetail> GetGameType(int _IBCCustId, int gameKindID)
        {
            try
            {
                return IBCProvider.Casino.GetGameType(_IBCCustId, gameKindID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindID">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public static List<GameTypeDetail> GetGameType(int gameKindID)
        {
            try
            {
                return IBCProvider.Casino.GetGameType(gameKindID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Didn't synchronize data to CS system
        /// </summary>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        public static string GenCasinoPTHTML(CSCustomerDetail _cust, Globals.CasinoGroup csGroup, Globals.PTType ptType)
        {
            string groupNo = Globals.GetCasinoGroup(csGroup);
            return GenCasinoPTHTML(_cust, groupNo, ptType);
        }

        /// <summary>
        /// Didn't synchronize data to CS system
        /// </summary>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        public static string GenCasinoPTHTML(CSCustomerDetail _cust, string csGroup, Globals.PTType ptType)
        {
            string retVal = "";
            string tplPT = WebUtils.LoadTemplate(Globals.CASINO_PT_TEMPLATE);
            lsJSData = new List<string>();
            htGameType = new Hashtable();
            listGameKind = new List<GameKindDetail>();
            if (!_cust.CasinoSync)
            {
                TemplateHandler.RemoveBlock(ref tplPT, "inserting");
            }

            var accountType = (Globals.AccountType) (_cust.RoleId);
            AssignCommonHeader(ref tplPT);
            try
            {
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        retVal = GenCSHTMLSuper(tplPT, _cust, csGroup);
                        break;

                    case Globals.AccountType.Master:
                        retVal = GenCSHTMLMaster(tplPT, _cust, csGroup);
                        break;

                    case Globals.AccountType.Agent:
                        retVal = GenCSHTMLAgent(tplPT, _cust, csGroup);
                        break;

                    case Globals.AccountType.Member:
                        retVal = GenCSHTMLMember(tplPT, _cust, csGroup, ptType);
                        break;
                }
                GenGameList(ref lsJSData);
                AssignJavaScript(ref retVal, lsJSData);
                return retVal;
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(Globals.GetCasinoError(Globals.CasinoError.Not_Casino_Customer)))
                    return ex.Message;
                //Logs.Write(ex.Message);

                throw ex;
            }
        }

        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static string GenCSHTMLSuper(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            //string retVal = "";
            try
            {
                //there are no bet setting at the super level
                //if (!Globals.IBCSetting.Casino.ShowBetSetting)

                int custId = _cust.IBCCustID;
                string gameKindName = "Live";
                //string tplPT = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate(Globals.CASINO_PT_TEMPLATE);
                //Others game kind
                var data = new StringBuilder();
                string tplHeader = WebUtils.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
                string tplData = WebUtils.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
                string tplDataP2P = "";
                string tplPrivate;
                double maxSPT = Globals.MAX_MaxPTValue, maxSMinPT = Globals.MAX_MinPTValue, maxSComm = Globals.MAX_Comm;
                double maxGroupSPT = Globals.MAX_MaxPTValue,
                       maxGroupSMinPT = Globals.MAX_MinPTValue,
                       maxGroupSComm = Globals.MAX_Comm;
                double SPTGroup = Globals.CURRENT_MaxPTValue,
                       SMinPTGroup = Globals.CURRENT_MinPTValue,
                       SCommGroup = Globals.CURRENT_Comm; //current GROUP value
                double SPT = Globals.CURRENT_MaxPTValue,
                       SMinPT = Globals.CURRENT_MinPTValue,
                       SComm = Globals.CURRENT_Comm; // current value
                string strSPT = Globals.Super + Globals.strPresetPT,
                       strSMinPT = Globals.Super + Globals.strMinPT,
                       strSComm = Globals.Super + Globals.Comm;
                int intervalPT = Globals.INTERVAL_PT; //interval of select options
                int intervalComm = Globals.INTERVAL_COMM;
                string sep = Globals.SEP_GAMEKIND_GAMETYPE;
                int totalGameType = 0;
                int betlimitColspan = 3;
                var csBasicInfo = new BasicInfo();
                ArrayList csSetting = csBasicInfo.GetCasinoSetting(csGroup);
                double maxRnGComm = 0;
                double maxMiniGameComm = 0;

                if (csSetting.Count > 0)
                {
                    maxRnGComm = CastAide.AsDouble(csSetting[4]);
                    maxMiniGameComm = CastAide.AsDouble(csSetting[5]);
                }
                //build header
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.PRESET_PT);
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT);
                TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MinPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                //build content
                TemplateHandler.RemoveBlock(ref tplData, Globals.PRESET_PT);
                TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
                TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

                // get games kind detail from the casino system if this customer 
                //have already been synchronized
                List<GameKindDetail> lsGameKind = (_cust.CasinoSync)
                                                      ? GetGameKinds(_cust.IBCCustID)
                                                      : GetGameKinds(csGroup);
                listGameKind = lsGameKind;
                bool useGroupPT = (_cust.NoCSData) ? true : !_cust.PTByGameType;
                string postGameData = "";
                int totalGameKind = 0;
                bool isFirstGameKind = true;
                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    string tmpTplData = tplData;
                    gameKindName = gameKindDetail.CodeID.ToString();
                    SPTGroup = gameKindDetail.SPositionTaking;
                    SMinPTGroup = (gameKindDetail.MinPositionTaking == 0)
                                      ? 0
                                      : gameKindDetail.MinPositionTaking;
                    maxGroupSMinPT = SPTGroup;
                    SMinPTGroup = Math.Min(SMinPTGroup, maxGroupSMinPT);
                    maxGroupSComm = maxSComm = IsMiniGame(gameKindDetail.CodeName) ? maxMiniGameComm : maxRnGComm;
                    SCommGroup = gameKindDetail.SMAgentRollingRate;
                    SCommGroup = Math.Min(SCommGroup, maxGroupSComm);
                    List<GameTypeDetail> lsGameType = (_cust.CasinoSync)
                                                          ? GetGameType(_cust.IBCCustID, gameKindDetail.CodeID)
                                                          : GetGameType(gameKindDetail.CodeID);
                    htGameType.Add(gameKindDetail.CodeID, lsGameType);
                    var gameTypeData = new StringBuilder();
                    if (!IsP2PGame(gameKindDetail.CodeName))
                    {
                        foreach (GameTypeDetail gameTypeDetail in lsGameType)
                        {
                            tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                            string SPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSPT;
                            string strSMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSMinPT;
                            string strSCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSComm;
                            string funcOnChangeGroupSetting = string.Format(
                                Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM + ", " + (int) Globals.AccountType.Super);
                            string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                                Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                            string funcOnChangePreset = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_PRESET_FUNC,
                                                                      Globals.CS_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                            string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                            if (0 == totalGameType)
                            {
                                postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                                if (isFirstGameKind)
                                    TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                           HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP,
                                                                              false, useGroupPT, funcClickGroup));
                                else
                                    TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting", "");
                                TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                                TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupSPT, SPTGroup, intervalPT,
                                                                              false, SPTGroupID,
                                                                              funcOnChangeGroupSetting, "select-pt"));
                                TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupSMinPT, SMinPTGroup, intervalPT,
                                                                              false, strSMinPTGroupID,
                                                                              funcOnChangeGroupSetting, "select-pt"));
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupSComm, SCommGroup, intervalComm,
                                                                              false,
                                                                              strSCommGroupID, funcOnChangeGroupSetting,
                                                                              "selectcomm"));
                                TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn",
                                                       HtmlHelper.GenChangePTButtons(SPTGroupID, false));
                                TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn",
                                                       HtmlHelper.GenChangePTButtons(strSMinPTGroupID, false));
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                            }
                            //game type value
                            string tmpTplPrivate = tplPrivate;
                            string strSPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSPT;
                            string strSMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSMinPT;
                            string strSCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSComm;
                            postGameData += gameTypeDetail.GameTypeID + Globals.OPEN_STAKELIST_MARK;

                            SPT = gameTypeDetail.SPositionTaking;
                            maxSMinPT = SPT;
                            SMinPT = (gameTypeDetail.MinPositionTaking == 0)
                                         ? 0
                                         : gameTypeDetail.MinPositionTaking;
                            SMinPT = Math.Min(maxSMinPT, SMinPT);
                            SComm = gameTypeDetail.SMAgentRollingRate;
                            SComm = Math.Min(SComm, Globals.MAX_Comm);

                            TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                                                   HtmlHelper.GenSelectOption(maxSPT, SPT, intervalPT, useGroupPT,
                                                                          strSPTID, funcOnChangePreset, "select-pt"));
                            TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                                   HtmlHelper.GenSelectOption(maxSMinPT, SMinPT, intervalPT, useGroupPT,
                                                                          strSMinPTID, funcOnChange, "select-pt"));
                            TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                                   HtmlHelper.GenSelectOption(maxSComm, SComm, intervalComm, useGroupPT,
                                                                          strSCommID, funcOnChange, "selectcomm"));
                            TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                            TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                            //label error

                            TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                                                   HtmlHelper.GenLabel(strSPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));
                            TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                                   HtmlHelper.GenLabel(strSMinPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));
                            TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                                   HtmlHelper.GenLabel(strSCommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));


                            //assign gamekind name
                            //get betting limit list

                            TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBettingLimit"));

                            var lsStakeLevel = new List<StakeLevelDetail>();
                            lsStakeLevel = (_cust.CasinoSync)
                                               ? GetCustStakeLevels(_cust.IBCCustID, gameTypeDetail.GameTypeID)
                                               : GetGameTypeStakeLevels(gameTypeDetail.GameTypeID, _cust.Currency);
                            string stakeLevels = "";
                            string funcChkStakeClick = "";
                            int totalStake = 0;
                            foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                            {
                                if (!stakeDetail.IsChecked && !_cust.CasinoSync)
                                    continue;
                                string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                                    Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                                stakeLevels += HtmlHelper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                                  funcChkStakeClick)
                                               + stakeDetail.StakeLevelName;
                                postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                                totalStake++;
                            }

                            //Assign Stake levels
                            TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                            gameTypeData.Append(tmpTplPrivate);
                            totalGameType++;

                            int subLength = postGameData.Length - ((lsStakeLevel.Count == 0) ? 0 : 2);
                            postGameData = postGameData.Substring(0, subLength) +
                                           Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),
                            if (totalGameType == lsGameType.Count)
                            {
                                TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                       GetGameKindRowSpan(totalGameType, useGroupPT,
                                                                          gameKindDetail.CodeName));
                                TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                                TemplateHandler.Assign(ref tmpTplData, "gamekindid", gameKindDetail.CodeID);
                                TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                             gameTypeData.ToString());

                                data.Append(tmpTplData);
                                totalGameType = 0;
                                gameTypeData = new StringBuilder();
                                postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                               Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                            }
                        }
                    }
                    else //p2p game
                    {
                        tplDataP2P = WebUtils.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                        TemplateHandler.Assign(ref tplDataP2P, Globals.COMM_PT,
                                               string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                        //build content
                        tmpTplData = tplDataP2P;
                        TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 2);
                        TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT,
                                               string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                        foreach (GameTypeDetail gameTypeDetail in lsGameType)
                        {
                            tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                            string strSCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSComm;
                            string funcOnChangeGroupSetting = string.Format(
                                Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM + ", " + Globals.AccountType.Super);
                            string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                                Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                            string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                            if (0 == totalGameType)
                            {
                                postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                                TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                       HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                          useGroupPT, funcClickGroup));
                                TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupSComm, SCommGroup, intervalComm,
                                                                              !useGroupPT,
                                                                              strSCommGroupID, funcOnChangeGroupSetting,
                                                                              "selectcomm"));
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_txt",
                                                       HtmlHelper.GenTextBox(
                                                           strSCommGroupID + sep + Globals.CTR_INPUT_TEXTBOX_PREFIX,
                                                           strSCommGroupID, SCommGroup.ToString(), Globals.CLASS_HIDDEN,
                                                           useGroupPT, ""));
                            }

                            //game type value
                            string tmpTplPrivate = tplPrivate;
                            string strSMaxPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSPT;
                            string strSMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSMinPT;
                            string strSCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSComm;
                            postGameData += gameTypeDetail.GameTypeID + Globals.SEP_GAMETYPE;

                            SComm = gameTypeDetail.SMAgentRollingRate;

                            TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                                   HtmlHelper.GenSelectOption(maxSComm, SComm, intervalComm, useGroupPT,
                                                                          strSCommID, funcOnChange, "selectcomm"));
                            TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                            //label error

                            TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                                   HtmlHelper.GenLabel(strSCommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));


                            gameTypeData.Append(tmpTplPrivate);
                            totalGameType++;
                            //assign gamekind name

                            if (totalGameType == lsGameType.Count)
                            {
                                TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                       GetGameKindRowSpan(totalGameType, useGroupPT,
                                                                          gameKindDetail.CodeName));
                                TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                                TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                                TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                             gameTypeData.ToString());

                                data.Append(tmpTplData);
                                totalGameType = 0;
                                gameTypeData = new StringBuilder();
                                postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                               Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                            }
                        }
                    }
                    totalGameKind++;
                    if (lsGameType.Count > 0)
                        isFirstGameKind = false;
                }

                TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
                TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
                TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");


                AssignCSCustInfo(ref tplPT, _cust, false);

                //assign IBC customer info

                AssignIBCCustInfo(ref tplPT, _cust);

                //assign post data
                TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
                HttpContext.Current.Session[Globals.SESSION_CUSTOMER] = _cust;
                return tplPT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //master
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static string GenCSHTMLMaster(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            //remove bet setting from the pt

            //if (!Globals.IBCSetting.Casino.ShowBetSetting)

            int custId = _cust.IBCCustID;
            string gameKindName = "Live";
            //string tplPT = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate(Globals.CASINO_PT_TEMPLATE);
            //Others game kind
            var data = new StringBuilder();
            string tplHeader = WebUtils.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = WebUtils.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplDataP2P = "";
            string tplPrivate;
            double maxMPT = Globals.MAX_MaxPTValue,
                   maxMMinPT = Globals.MAX_MinPTValue,
                   maxMComm = Globals.MAX_Comm,
                   maxSPresetPT = Globals.MAX_PresetPTValue,
                   maxSAutoPT = Globals.MAX_AutoPTValue; //max value
            double maxGroupMPT = Globals.MAX_MaxPTValue,
                   maxGroupMMinPT = Globals.MAX_MinPTValue,
                   maxGroupMComm = Globals.MAX_Comm,
                   maxGroupSPresetPT = Globals.MAX_PresetPTValue,
                   maxGroupSAutoPT = Globals.MAX_AutoPTValue; //max group value

            double MPTGroup = Globals.CURRENT_MaxPTValue,
                   MMinPTGroup = Globals.CURRENT_MinPTValue,
                   MCommGroup = Globals.CURRENT_Comm,
                   SPresetPTGroup = Globals.MAX_PresetPTValue,
                   SAutoPTGroup = Globals.MAX_AutoPTValue; //current GROUP value
            double MPT = Globals.CURRENT_MaxPTValue,
                   MMinPT = Globals.CURRENT_MinPTValue,
                   MComm = Globals.CURRENT_Comm,
                   SPresetPT = Globals.MAX_PresetPTValue,
                   SAutoPT = Globals.MAX_AutoPTValue; // current value
            string strMPT = Globals.Master + Globals.strPresetPT,
                   strMMinPT = Globals.Master + Globals.strMinPT,
                   strMComm = Globals.Master + Globals.Comm,
                   strSPresetPT = Globals.Super + Globals.strPresetPT,
                   strSAutoPT = Globals.Super + Globals.strAutoPT;
            int intervalPT = Globals.INTERVAL_PT; //interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 6;
            //build header
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPresetPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"))); //super preset
            TemplateHandler.Assign(ref tplHeader, Globals.AUTO_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"))); //super auto

            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MinPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
            TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));

            //build content
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);
            //_cust.CasinoSync = true;
            List<GameKindDetail> lsGameKind = (_cust.CasinoSync)
                                                  ? GetGameKinds(_cust.IBCCustID)
                                                  : GetGameKinds(_cust.UpperCustID);
            listGameKind = lsGameKind; //save game kind list to memories
            bool useGroupPT = (_cust.NoCSData) ? (!_cust.PTByGameTypeUpline) : !_cust.PTByGameType;

            string postGameData = "";
            string hidPTData = "";

            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;

                gameKindName = gameKindDetail.CodeID.ToString();

                string MPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPT;
                string strMMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMMinPT;
                string strMCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMComm;
                string strSPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSPresetPT;
                string strSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSAutoPT;
                string strChkSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + Globals.strCHK +
                                              strSAutoPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                         Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                string funcClickChkAutoGroup = string.Format(Globals.CS_ChkAutoGROUP_CLICK_FUNC,
                                                             "this," + useGroupPT.ToString().ToLower());
                //super pt from company S4
                //s4 = (gameKindDetail.SPositionTaking == 0) ? maxGroupSPresetPT : gameKindDetail.SPositionTaking;

                //maxPT = 0.9;
                List<GameTypeDetail> lsGameType = (_cust.CasinoSync)
                                                      ? GetGameType(_cust.IBCCustID, gameKindDetail.CodeID)
                                                      : GetGameType(_cust.UpperCustID, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType); //save game type list to memory
                var gameTypeData = new StringBuilder();
                maxGroupSPresetPT = 0;
                maxGroupSAutoPT = 0;
                maxGroupMPT = 0;
                maxGroupMMinPT = 0;
                maxGroupMComm = 0;
                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            //hidPTData += MMaxPTGroupID + sep + Globals.m3Max + Globals.SEP_PT_VALUE + m3Max.ToString() + Globals.SEP_PT;
                            // hidPTData += MPTGroupID + sep + Globals.m3MaxAuto + Globals.SEP_PT_VALUE + m3MaxAuto.ToString() + Globals.SEP_PT;
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        //game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPT;
                        string strMMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMMinPT;
                        string strMCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMComm;
                        string strSPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSPresetPT;
                        string strSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSAutoPT;
                        string strChkSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + Globals.strCHK +
                                                 strSAutoPT;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                            Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        string funcOnChangePreset = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_PRESET_FUNC,
                                                                  Globals.CS_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                        string funcClickChkAuto = string.Format(Globals.CS_ChkAUTO_CLICK_FUNC, "this");
                        postGameData += gameTypeDetail.GameTypeID + Globals.OPEN_STAKELIST_MARK;

                        //super pt from company S4
                        s4 = (_cust.CasinoSync) ? gameTypeDetail.SPositionTakingUpline : gameTypeDetail.SPositionTaking;
                        //group max S Preset PT
                        maxGroupSPresetPT = Math.Max(s4, maxGroupSPresetPT);
                        //group max PT
                        //maxPT = Math.Min(s4, maxPT);
                        //super pt s3
                        s3 = gameTypeDetail.SPositionTaking;
                        //super auto pt
                        u3Max = s4;
                        //max group s auto 
                        maxGroupSAutoPT = Math.Max(u3Max, maxGroupSAutoPT);
                        u3 = (_cust.CasinoSync) ? gameTypeDetail.AutoPositionTaking : u3Max;
                        /////////END Super///////////////

                        //max master max pt
                        m4 = (gameTypeDetail.IsAutoPositionTaking) ? s4 : (s4 - s3);
                        m3Max = m4;
                        //master max pt
                        //group max M PT
                        maxGroupMPT = Math.Max(m3Max, maxGroupMPT);
                        m3 = (_cust.CasinoSync) ? (Math.Min(gameTypeDetail.MPositionTaking, m3Max)) : m3Max;
                            //current master preset pt
                        //master min pt
                        n3Max = m3;
                        maxGroupMMinPT = Math.Max(maxGroupMMinPT, n3Max);
                        //max master max/min pt when auto pt is used
                        m3MaxAuto = s4;
                        n3MaxAuto = s4;
                        /////////END Master//////////
                        s3Max = s4;
                        n3 = (_cust.CasinoSync)
                                 ? (Math.Min(gameTypeDetail.MinPositionTaking, n3Max))
                                 : Math.Min(s3Min, n3Max);

                        maxSPresetPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : s3Max;
                        maxSAutoPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : u3Max;
                        maxMPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : m3Max;
                        maxMMinPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : n3Max;
                        //Commission
                        maxMComm = (_cust.CasinoSync)
                                       ? gameTypeDetail.SMAgentRollingRateUpline
                                       : gameTypeDetail.SMAgentRollingRate;
                        maxMComm = (!_cust.PTByGameType) ? Globals.MAX_Comm : Math.Min(maxMComm, Globals.MAX_Comm);
                        //max group comm
                        maxGroupMComm = Math.Max(maxGroupMComm, maxMComm);

                        MComm = (_cust.CasinoSync)
                                    ? gameTypeDetail.MAgentRollingRate
                                    : gameTypeDetail.SMAgentRollingRate;
                        MComm = Math.Min(MComm, Globals.MAX_Comm);
                        //////////END Commission///////
                        SPresetPT = s3;
                        SAutoPT = u3;
                        MPT = m3;
                        MMinPT = n3;

                        hidPTData += strMPTID + sep + Globals.m3MaxAuto + Globals.SEP_PT_VALUE + m3MaxAuto +
                                     Globals.SEP_PT;

                        //super preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                                               HtmlHelper.GenSelectOption(maxSPresetPT, SPresetPT, intervalPT,
                                                                      (useGroupPT || gameTypeDetail.IsAutoPositionTaking),
                                                                      strSPresetPTID, funcOnChangePreset, "select-pt"));

                        TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt",
                                               HtmlHelper.GenSelectOption(maxSAutoPT, SAutoPT, intervalPT,
                                                                      (useGroupPT ||
                                                                       !gameTypeDetail.IsAutoPositionTaking),
                                                                      strSAutoPTID, funcOnChange, "select-pt"));

                        TemplateHandler.Assign(ref tmpTplPrivate, "chkAuto",
                                               HtmlHelper.GenCheckBox(strChkSAutoPTID, useGroupPT,
                                                                  gameTypeDetail.IsAutoPositionTaking, funcClickChkAuto));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                                               HtmlHelper.GenSelectOption(maxMPT, MPT, intervalPT, useGroupPT,
                                                                      strMPTID, funcOnChangePreset, "select-pt"));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                               HtmlHelper.GenSelectOption(maxMMinPT, MMinPT, intervalPT, useGroupPT,
                                                                      strMMinPTID, funcOnChange, "select-pt"));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               HtmlHelper.GenSelectOption(maxMComm, MComm, intervalComm, useGroupPT,
                                                                      strMCommID, funcOnChange,
                                                                      Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        //label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                               HtmlHelper.GenLabel(strSPresetPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt_error",
                                               HtmlHelper.GenLabel(strSAutoPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                                               HtmlHelper.GenLabel(strMPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               HtmlHelper.GenLabel(strMMinPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               HtmlHelper.GenLabel(strMCommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));


                        //assign gamekind name
                        //get betting limit list

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBettingLimit"));

                        var lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (_cust.CasinoSync)
                                           ? GetCustStakeLevels(_cust.IBCCustID, gameTypeDetail.GameTypeID)
                                           : GetCustStakeLevels(_cust.UpperCustID, gameTypeDetail.GameTypeID);
                        string stakeLevels = "";
                        string funcChkStakeClick = "";
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !_cust.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                                Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += HtmlHelper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick)
                                           + stakeDetail.StakeLevelName;
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        //Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        //postGameData = postGameData.Substring(0, postGameData.Length - 2) +
                        //               Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            //assign group value
                            if (_cust.CasinoSync)
                            {
                                s4 = (!_cust.PTByGameType) ? gameKindDetail.SPositionTakingUpline : maxGroupSPresetPT;
                                maxGroupMComm = (!_cust.PTByGameType)
                                                    ? gameKindDetail.SMAgentRollingRateUpline
                                                    : maxGroupMComm;
                            }
                            else
                            {
                                s4 = gameKindDetail.SPositionTaking;
                                maxGroupMComm = gameKindDetail.SMAgentRollingRate;
                            }

                            m3MaxAuto = s4;
                            hidPTData += MPTGroupID + sep + Globals.m3MaxAuto + Globals.SEP_PT_VALUE + m3MaxAuto +
                                         Globals.SEP_PT;
                            //super pt s3
                            s3 = gameKindDetail.SPositionTaking;
                            //super auto pt
                            u3Max = s4;

                            u3 = (_cust.CasinoSync) ? gameKindDetail.AutoPositionTaking : u3Max;
                            /////////END Super///////////////

                            //max master max pt
                            m4 = (gameKindDetail.IsAutoPositionTaking) ? s4 : (s4 - s3);
                            m3Max = m4;
                            m3Max = Math.Max(maxGroupMPT, m3Max);
                            //master max pt
                            m3 = (_cust.CasinoSync) ? (Math.Min(gameKindDetail.MPositionTaking, m3Max)) : m3Max;
                                //current master preset pt
                            //master min pt
                            n3Max = m3;
                            n3 = (_cust.CasinoSync) ? gameKindDetail.MinPositionTaking : 0;

                            //max master max/min pt when auto pt is used
                            m3MaxAuto = s4;
                            n3MaxAuto = s4;
                            /////////END Master//////////
                            s3Max = s4 - s3Min;
                            maxGroupSPresetPT = s3Max;
                            maxGroupSAutoPT = u3Max;
                            maxGroupMPT = m3Max;
                            //maxGroupMMinPT = (!_cust.PTByGameType) ? n3Max : maxGroupMMinPT;
                            maxGroupMMinPT = n3Max;

                            //Commission

                            MCommGroup = (_cust.CasinoSync)
                                             ? Math.Min(gameKindDetail.MAgentRollingRate, maxGroupMComm)
                                             : Math.Min(gameKindDetail.SMAgentRollingRate, maxGroupMComm);
                            //////////END Commission///////
                            SPresetPTGroup = s3;
                            SAutoPTGroup = u3;
                            MPTGroup = m3;
                            MMinPTGroup = n3;

                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                   HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                      useGroupPT, "", Globals.CLASS_CHECKBOX_HIDDEN));
                                //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupMPT, MPTGroup, intervalPT,
                                                                          false, MPTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupMMinPT, MMinPTGroup, intervalPT,
                                                                          false, strMMinPTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupMComm, MCommGroup, intervalComm, false,
                                                                          strMCommGroupID, funcOnChangeGroup,
                                                                          Globals.CLASS_SELECT_COMMISSION));

                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupSPresetPT, SPresetPTGroup, intervalPT,
                                                                          gameKindDetail.IsAutoPositionTaking,
                                                                          strSPresetPTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));

                            TemplateHandler.Assign(ref tmpTplData, "group_auto_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupSAutoPT, SAutoPTGroup, intervalPT,
                                                                          !gameKindDetail.IsAutoPositionTaking,
                                                                          strSAutoPTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));


                            TemplateHandler.Assign(ref tmpTplData, "chkGroupAuto",
                                                   HtmlHelper.GenCheckBox(strChkSAutoPTGroupID, false,
                                                                      gameKindDetail.IsAutoPositionTaking,
                                                                      funcClickChkAutoGroup));

                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(MPTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(strMMinPTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(strSPresetPTGroupID,
                                                                             gameKindDetail.IsAutoPositionTaking));
                            TemplateHandler.Assign(ref tmpTplData, "group_auto_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(strSAutoPTGroupID,
                                                                             !gameKindDetail.IsAutoPositionTaking));

                            //END------------
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                   GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                           Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                    #region P2P game

                else
                {
                    tplDataP2P = WebUtils.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                    TemplateHandler.Assign(ref tplDataP2P, Globals.COMM_PT,
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                    tmpTplData = tplDataP2P;
                    TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 5);
                    TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT,
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        strMCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                                        Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        //game type value
                        string tmpTplPrivate = tplPrivate;
                        string strMCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMComm;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                            Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID + Globals.SEP_GAMETYPE;

                        maxMComm = (_cust.CasinoSync)
                                       ? gameTypeDetail.SMAgentRollingRateUpline
                                       : gameTypeDetail.SMAgentRollingRate;
                        MComm = (_cust.CasinoSync)
                                    ? gameTypeDetail.MAgentRollingRate
                                    : gameKindDetail.SMAgentRollingRate;

                        maxMComm = Math.Min(maxMComm, Globals.MAX_Comm);
                        maxGroupMComm = Math.Max(maxGroupMComm, maxMComm);
                        MComm = Math.Min(MComm, Globals.MAX_Comm);

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               HtmlHelper.GenSelectOption(maxMComm, MComm, intervalComm, useGroupPT,
                                                                      strMCommID, funcOnChange,
                                                                      Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                        //label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               HtmlHelper.GenLabel(strMCommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));


                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;
                        //assign gamekind name
                        if (totalGameType == lsGameType.Count)
                        {
                            //Group
                            if (_cust.CasinoSync)
                            {
                                maxGroupMComm = (!_cust.PTByGameType)
                                                    ? gameKindDetail.SMAgentRollingRateUpline
                                                    : maxGroupMComm;
                                MCommGroup = gameKindDetail.MAgentRollingRate;
                            }
                            else
                            {
                                maxGroupMComm = (!_cust.PTByGameTypeUpline)
                                                    ? gameKindDetail.SMAgentRollingRate
                                                    : maxGroupMComm;
                                MCommGroup = gameKindDetail.SMAgentRollingRate;
                            }
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                   HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                      useGroupPT, "", Globals.CLASS_CHECKBOX_HIDDEN));
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupMComm, MCommGroup, intervalComm,
                                                                          !useGroupPT,
                                                                          strMCommGroupID, funcOnChangeGroupSetting,
                                                                          Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                            //END
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                   GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                           Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                #endregion
            }

            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");
            //assign Casino customer info
            AssignCSCustInfo(ref tplPT, _cust, false);
            //assign IBC customer info
            AssignIBCCustInfo(ref tplPT, _cust);
            //assign post data
            var hHiddenData = new Hashtable();
            hHiddenData.Add(Globals.HIDDEN_GAMEDATA, postGameData);
            hHiddenData.Add(Globals.HIDDEN_PT, hidPTData);
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(hHiddenData, _cust));
            HttpContext.Current.Session[Globals.SESSION_CUSTOMER] = _cust;
            return tplPT.Replace(Environment.NewLine, string.Empty).Replace("  ", " ");
        }

        //agent
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static string GenCSHTMLAgent(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            //if (!Globals.IBCSetting.Casino.ShowBetSetting)

            int custId = _cust.IBCCustID;
            string gameKindName = "Live";
            //string tplPT = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate(Globals.CASINO_PT_TEMPLATE);
            //Others game kind
            var data = new StringBuilder();
            string tplHeader = WebUtils.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
            string tplData = WebUtils.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
            string tplDataP2P = "";
            string tplPrivate;
            double maxAPT = Globals.MAX_MaxPTValue,
                   maxAMinPT = Globals.MAX_MinPTValue,
                   maxAComm = Globals.MAX_Comm,
                   maxMPresetPT = Globals.MAX_PresetPTValue; //max value
            double maxGroupAPT = Globals.MAX_MaxPTValue,
                   maxGroupAMinPT = Globals.MAX_MinPTValue,
                   maxGroupAComm = Globals.MAX_Comm,
                   maxGroupMPresetPT = Globals.MAX_PresetPTValue; //max group value

            double APTGroup = Globals.CURRENT_MaxPTValue,
                   AMinPTGroup = Globals.CURRENT_MinPTValue,
                   ACommGroup = Globals.CURRENT_Comm,
                   MPresetPTGroup = Globals.MAX_PresetPTValue; //current GROUP value
            double APT = Globals.CURRENT_MaxPTValue,
                   AMinPT = Globals.CURRENT_MinPTValue,
                   AComm = Globals.CURRENT_Comm,
                   MPresetPT = Globals.MAX_PresetPTValue; // current value
            string strAPT = Globals.Agent + Globals.strPresetPT,
                   strAMinPT = Globals.Agent + Globals.strMinPT,
                   strAComm = Globals.Agent + Globals.Comm,
                   strMPresetPT = Globals.Master + Globals.strPresetPT;
            int intervalPT = Globals.INTERVAL_PT; //interval of select options
            int intervalComm = Globals.INTERVAL_COMM;
            string sep = Globals.SEP_GAMEKIND_GAMETYPE;
            int totalGameType = 0;
            int betlimitColspan = 4;
            //build header
            TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT); //there are no auto master on agent function
            TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPresetPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"))); //master preset
            TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));
            TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MinPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));
            TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT,
                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));

            //build content
            TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
            TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);
            List<GameKindDetail> lsGameKind = (_cust.CasinoSync)
                                                  ? GetGameKinds(_cust.IBCCustID)
                                                  : GetGameKinds(_cust.UpperCustID);
            listGameKind = lsGameKind;
            bool useGroupPT = (_cust.NoCSData) ? !_cust.PTByGameTypeUpline : !_cust.PTByGameType;
            string postGameData = "";
            //maxPT = 0.9;
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                string tmpTplData = tplData;
                gameKindName = gameKindDetail.CodeID.ToString();
                string APTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPT;
                string strAMinPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAMinPT;
                string strACommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAComm;
                string strMPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPresetPT;
                string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                         Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);


                List<GameTypeDetail> lsGameType = (_cust.CasinoSync)
                                                      ? GetGameType(_cust.IBCCustID, gameKindDetail.CodeID)
                                                      : GetGameType(_cust.UpperCustID, gameKindDetail.CodeID);
                htGameType.Add(gameKindDetail.CodeID, lsGameType);
                var gameTypeData = new StringBuilder();
                maxGroupAPT = 0;
                maxGroupMPresetPT = 0;
                maxGroupAComm = 0;
                if (!IsP2PGame(gameKindDetail.CodeName))
                {
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                        }

                        //game type value
                        string tmpTplPrivate = tplPrivate;
                        string strAPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPT;
                        string strAMinPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAMinPT;
                        string strACommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAComm;
                        string strMPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPresetPT;
                        string funcOnChangePreset = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_PRESET_FUNC,
                                                                  Globals.CS_SELECTOPTION_ONCHANGE_PRESET_PARAM);
                        postGameData += gameTypeDetail.GameTypeID + Globals.OPEN_STAKELIST_MARK;

                        if (_cust.CasinoSync)
                        {
                            m3 = gameTypeDetail.MPositionTakingUpline;
                            m2Max = m3;
                            m2 = gameTypeDetail.MPositionTaking;
                            a2Max = m3 - m2;
                            a2 = Math.Min(a2Max, gameTypeDetail.APositionTaking);
                            n2Max = a2;
                            n2 = Math.Min(gameTypeDetail.MinPositionTaking, n2Max);

                            //commission
                            maxAComm = gameTypeDetail.MAgentRollingRateUpline;
                            AComm = gameTypeDetail.AgentRollingRate;
                        }
                        else //copy from master
                        {
                            m3 = gameTypeDetail.MPositionTaking;
                            m2Max = m3;
                            m2 = m2Max;
                            a2Max = m3 - m2;
                            a2 = a2Max;
                            n2Max = a2;
                            n2 = Math.Min(gameTypeDetail.MinPositionTaking, n2Max);
                            //commission
                            maxAComm = gameTypeDetail.MAgentRollingRate;
                            AComm = gameTypeDetail.MAgentRollingRate;
                        }
                        //maxPT = Math.Min(m3, maxPT); //max group value when use pt by game type
                        maxAComm = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : Math.Min(maxAComm, Globals.MAX_Comm);
                        AComm = Math.Min(AComm, Globals.MAX_Comm);
                        maxMPresetPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : m2Max;
                        maxAPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : a2Max;
                        maxAMinPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : n2Max;

                        maxGroupAPT = Math.Max(maxGroupAPT, maxAPT);
                        maxGroupMPresetPT = Math.Max(maxGroupMPresetPT, maxMPresetPT);
                        maxGroupAComm = Math.Max(maxGroupAComm, maxAComm);

                        APT = a2;
                        AMinPT = n2;
                        MPresetPT = m2;
                        //master preset
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                                               HtmlHelper.GenSelectOption(maxMPresetPT, MPresetPT, intervalPT, useGroupPT,
                                                                      strMPresetPTID, funcOnChangePreset));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                                               HtmlHelper.GenSelectOption(maxAPT, APT, intervalPT, useGroupPT,
                                                                      strAPTID, funcOnChangePreset));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                               HtmlHelper.GenSelectOption(maxAMinPT, AMinPT, intervalPT, useGroupPT,
                                                                      strAMinPTID, ""));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               HtmlHelper.GenSelectOption(maxAComm, AComm, intervalComm, useGroupPT,
                                                                      strACommID, "", Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                        TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                        //label error
                        TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                               HtmlHelper.GenLabel(strMPresetPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));

                        TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                                               HtmlHelper.GenLabel(strAPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                               HtmlHelper.GenLabel(strAMinPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));
                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               HtmlHelper.GenLabel(strACommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));


                        //assign gamekind name
                        //get betting limit list

                        TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBettingLimit"));

                        var lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (_cust.CasinoSync)
                                           ? GetCustStakeLevels(_cust.IBCCustID, gameTypeDetail.GameTypeID)
                                           : GetCustStakeLevels(_cust.UpperCustID, gameTypeDetail.GameTypeID);
                        string stakeLevels = "";
                        string funcChkStakeClick = "";
                        int totalStake = 0;
                        foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                        {
                            if (!stakeDetail.IsChecked && !_cust.CasinoSync)
                                continue;
                            string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                                Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                            stakeLevels += HtmlHelper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                              funcChkStakeClick)
                                           + stakeDetail.StakeLevelName;
                            postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                            totalStake++;
                        }

                        //Assign Stake levels
                        TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;

                        int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                        postGameData = postGameData.Substring(0, subLength) +
                                       Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                        if (totalGameType == lsGameType.Count)
                        {
                            //assign group value
                            if (_cust.CasinoSync)
                            {
                                m3 = (!_cust.PTByGameType) ? gameKindDetail.MPositionTakingUpline : maxGroupMPresetPT;
                                m2Max = m3;
                                m2 = gameKindDetail.MPositionTaking;
                                a2Max = m3 - m2;
                                a2 = Math.Min(a2Max, gameKindDetail.APositionTaking);
                                n2Max = a2;
                                n2 = Math.Min(gameKindDetail.MinPositionTaking, n2Max);

                                //commission
                                maxGroupAComm = (!_cust.PTByGameType)
                                                    ? gameKindDetail.MAgentRollingRateUpline
                                                    : maxGroupAComm;
                                ACommGroup = gameKindDetail.AgentRollingRate;
                            }
                            else //copy from master
                            {
                                m3 = gameKindDetail.MPositionTaking;
                                m2Max = m3;
                                m2 = m2Max;
                                a2Max = m3 - m2;
                                a2 = a2Max;
                                n3 = gameKindDetail.MinPositionTaking;
                                n2Max = a2;
                                n2 = Math.Min(gameKindDetail.MinPositionTaking, n2Max);
                                //commission
                                maxGroupAComm = (!_cust.PTByGameTypeUpline)
                                                    ? gameKindDetail.MAgentRollingRate
                                                    : maxGroupAComm;
                                ACommGroup = gameKindDetail.MAgentRollingRate;
                            }
                            maxGroupAComm = Math.Min(maxGroupAComm, Globals.MAX_Comm);
                            ACommGroup = (!_cust.PTByGameType)
                                             ? Math.Min(ACommGroup, Globals.MAX_Comm)
                                             : Math.Min(ACommGroup, maxGroupAComm);
                            maxGroupMPresetPT = m2Max;
                            maxGroupAPT = a2Max;
                            maxGroupAMinPT = n2Max;

                            APTGroup = a2;
                            AMinPTGroup = n2;
                            MPresetPTGroup = m2;

                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                   HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                      useGroupPT, "", Globals.CLASS_CHECKBOX_HIDDEN));
                                //remove group setting check box
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupAPT, APTGroup, intervalPT,
                                                                          false, APTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupAMinPT, AMinPTGroup, intervalPT,
                                                                          false, strAMinPTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupAComm, ACommGroup, intervalComm, false,
                                                                          strACommGroupID, funcOnChangeGroup,
                                                                          Globals.CLASS_SELECT_COMMISSION));

                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupMPresetPT, MPresetPTGroup, intervalPT,
                                                                          false,
                                                                          strMPresetPTGroupID, funcOnChangeGroup,
                                                                          "select-pt"));

                            TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(APTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(strAMinPTGroupID, false));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                            TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn",
                                                   HtmlHelper.GenChangePTButtons(strMPresetPTGroupID, false));
                            //END


                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                   GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                           Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                    #region P2P game

                else
                {
                    tplDataP2P = WebUtils.LoadTemplate(Globals.CASINO_P2P_PT_TEMPLATE);
                    TemplateHandler.Assign(ref tplDataP2P, Globals.COMM_PT,
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));
                    tmpTplData = tplDataP2P;
                    TemplateHandler.Assign(ref tmpTplData, "col-span-pt", 3);
                    TemplateHandler.Assign(ref tmpTplData, Globals.COMM_PT,
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                    foreach (GameTypeDetail gameTypeDetail in lsGameType)
                    {
                        tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                        strACommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAComm;
                        string funcOnChangeGroupSetting = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                                        Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                        string funcClickGroup = string.Format(Globals.CS_ChkGROUP_CLICK_FUNC, "this");
                        if (0 == totalGameType)
                        {
                            postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                   HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                      useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, ""));
                            TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                   HtmlHelper.GenSelectOption(maxGroupAComm, ACommGroup, intervalComm,
                                                                          !useGroupPT,
                                                                          strACommGroupID, funcOnChangeGroupSetting,
                                                                          Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                        }

                        //game type value
                        string tmpTplPrivate = tplPrivate;
                        string strACommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAComm;
                        string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                            Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                        postGameData += gameTypeDetail.GameTypeID + Globals.SEP_GAMETYPE;

                        if (_cust.CasinoSync)
                        {
                            //commission
                            maxAComm = gameTypeDetail.MAgentRollingRate;
                            AComm = gameTypeDetail.AgentRollingRate;
                        }
                        else //copy from master
                        {
                            //commission
                            maxAComm = gameKindDetail.MAgentRollingRate;
                            AComm = gameKindDetail.MAgentRollingRate;
                        }

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                               HtmlHelper.GenSelectOption(maxAComm, AComm, intervalComm, useGroupPT,
                                                                      strACommID, funcOnChange,
                                                                      Globals.CLASS_SELECT_COMMISSION));
                        TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);

                        //label error

                        TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                               HtmlHelper.GenLabel(strACommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                               Globals.CLASS_ERROR_MAXVALUE));


                        gameTypeData.Append(tmpTplPrivate);
                        totalGameType++;
                        //assign gamekind name
                        if (totalGameType == lsGameType.Count)
                        {
                            TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                   GetGameKindRowSpan(totalGameType, useGroupPT, gameKindDetail.CodeName));
                            TemplateHandler.Assign(ref tmpTplData, "row-span-pt", totalGameType);
                            TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);
                            TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                         gameTypeData.ToString());

                            data.Append(tmpTplData);
                            totalGameType = 0;
                            gameTypeData = new StringBuilder();
                            postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                           Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                        }
                    }
                }
                #endregion
            }


            TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
            TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
            TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");
            AssignCSCustInfo(ref tplPT, _cust, false);
            //assign IBC customer info
            AssignIBCCustInfo(ref tplPT, _cust);
            //assign post data
            TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
            HttpContext.Current.Session[Globals.SESSION_CUSTOMER] = _cust;
            return tplPT;
        }

        //member
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static string GenCSHTMLMember(string tplPT, CSCustomerDetail _cust, string csGroup,
                                              Globals.PTType editType)
        {
            switch (editType)
            {
                case Globals.PTType.SuperOnMember:
                    return GenCSHTMLSuperOnMember(tplPT, _cust, csGroup);
                case Globals.PTType.MasterOnMember:
                    return GenCSHTMLMasterOnMember(tplPT, _cust, csGroup);
                default: //Agent on member
                    return GenCSHTMLAgentOnMember(tplPT, _cust, csGroup);
            }
            return "";
        }

        /// <summary>
        /// Edit super PT on Member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        private static string GenCSHTMLSuperOnMember(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            try
            {
                if (!_cust.CasinoSync)
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"); //this member have not been synchronized to CS yet.
                int custId = _cust.IBCCustID;
                string gameKindName = "Live";
                //Others game kind
                if (!Globals.IBCSetting.Casino.ShowBetSetting)
                    TemplateHandler.RemoveBlock(ref tplPT, Globals.TAG_BET_SETTING);
                var data = new StringBuilder();
                string tplHeader = WebUtils.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
                string tplData = WebUtils.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
                string tplPrivate;
                double maxAPT = Globals.MAX_MaxPTValue; //max value
                double APT = Globals.CURRENT_MaxPTValue;
                double maxMaPT = Globals.MAX_MaxPTValue,
                       maxSPresetPT = Globals.MAX_PresetPTValue,
                       maxSAutoPT = Globals.MAX_AutoPTValue; //max value
                double maxGroupMaPT = Globals.MAX_MaxPTValue,
                       maxGropuAPT = Globals.MAX_MaxPTValue,
                       maxGroupSPresetPT = Globals.MAX_PresetPTValue,
                       maxGroupSAutoPT = Globals.MAX_AutoPTValue; //max group value
                double MaPTGroup = Globals.CURRENT_MaxPTValue,
                       APTGroup = Globals.CURRENT_MaxPTValue,
                       SPresetPTGroup = Globals.MAX_PresetPTValue,
                       SAutoPTGroup = Globals.MAX_AutoPTValue; //current GROUP value
                double MaPT = Globals.CURRENT_MaxPTValue,
                       MMinPT = Globals.CURRENT_MinPTValue,
                       MComm = Globals.CURRENT_Comm,
                       SPresetPT = Globals.MAX_PresetPTValue,
                       SAutoPT = Globals.MAX_AutoPTValue; // current value
                string strMaPT = Globals.Master + Globals.strPT,
                       strAPT = Globals.Agent + Globals.strPT,
                       strSPresetPT = Globals.Super + Globals.strPresetPT,
                       strSAutoPT = Globals.Super + Globals.strAutoPT;
                int intervalPT = Globals.INTERVAL_PT; //interval of select options
                int intervalComm = Globals.INTERVAL_COMM;
                string sep = Globals.SEP_GAMEKIND_GAMETYPE;
                int totalGameType = 0;
                int betlimitColspan = 5;
                //build header
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.COMM_PT);
                TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"))); //super preset
                TemplateHandler.Assign(ref tplHeader, Globals.AUTO_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"))); //super auto

                TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                TemplateHandler.Assign(ref tplHeader, Globals.MIN_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));

                //build content
                TemplateHandler.RemoveBlock(ref tplData, Globals.COMM_PT);
                TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

                List<GameKindDetail> lsGameKind = GetGameKinds_EditMember(_cust.IBCCustID);
                listGameKind = lsGameKind;
                bool useGroupPT = (_cust.NoCSData) ? !_cust.PTByGameTypeUpline : !_cust.PTByGameType;
                string postGameData = "";
                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    string tmpTplData = tplData;

                    gameKindName = gameKindDetail.CodeID.ToString();
                    string MaPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMaPT;
                    string APTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPT;
                    string strSPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSPresetPT;
                    string strSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strSAutoPT;
                    string strChkSAutoPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + Globals.strCHK +
                                                  strSAutoPT;
                    string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                             Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                    string funcClickChkAutoGroup = string.Format(Globals.CS_ChkAutoGROUP_CLICK_FUNC,
                                                                 "this," + useGroupPT.ToString().ToLower());
                    List<GameTypeDetail> lsGameType = GetGameTypes_EditMember(_cust.IBCCustID, gameKindDetail.CodeID);
                    htGameType.Add(gameKindDetail.CodeID, lsGameType);
                    var gameTypeData = new StringBuilder();
                    maxGroupSPresetPT = 0;
                    maxGroupSAutoPT = 0;
                    if (!IsP2PGame(gameKindDetail.CodeName))
                    {
                        foreach (GameTypeDetail gameTypeDetail in lsGameType)
                        {
                            tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                            if (0 == totalGameType)
                            {
                                postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            }

                            //game type value
                            string tmpTplPrivate = tplPrivate;
                            string strMaPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMaPT;
                            string strAPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPT;
                            string strSPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSPresetPT;
                            string strSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strSAutoPT;
                            string strChkSAutoPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep +
                                                     Globals.strCHK + strSAutoPT;
                            string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                                Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                            string funcClickChkAuto = string.Format(Globals.CS_ChkAUTO_CLICK_FUNC, "this");
                            postGameData += gameTypeDetail.GameTypeID + Globals.OPEN_STAKELIST_MARK;

                            s4 = gameTypeDetail.SuperMaxPositionTaking;
                            m1 = gameTypeDetail.MPositionTaking;
                            a1 = gameTypeDetail.APositionTaking;
                            s1Max = s4 - (m1 + a1);
                            s1Min = gameTypeDetail.SuperMinPositionTaking;
                            s1Min = ((m1 + a1) >= s1Min) ? minPT : (s1Min - (m1 + a1));
                            //maxPT = Math.Min(s3Max, maxPT);
                            u1Max = s4;
                            //maxAutoPT = Math.Min(maxAutoPT, u1Max);
                            u1 = gameTypeDetail.AutoPositionTaking;
                            s1 = gameTypeDetail.SPositionTaking;

                            maxSPresetPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : s1Max;
                            maxSAutoPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : u1Max;
                            //group value
                            maxGroupSPresetPT = Math.Max(s1Max, maxGroupSPresetPT);
                            maxGroupSAutoPT = Math.Max(u1Max, maxGroupSAutoPT);
                            MaPT = m1;
                            APT = a1;

                            SPresetPT = Math.Min(s1, s1Max);
                            SAutoPT = Math.Min(u1, u1Max);

                            //super preset
                            TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                                                   HtmlHelper.GenSelectOption(maxSPresetPT, SPresetPT, intervalPT,
                                                                          (useGroupPT ||
                                                                           gameTypeDetail.IsAutoPositionTaking),
                                                                          strSPresetPTID, funcOnChange));

                            TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt",
                                                   HtmlHelper.GenSelectOption(maxSAutoPT, SAutoPT, intervalPT,
                                                                          (useGroupPT ||
                                                                           !gameTypeDetail.IsAutoPositionTaking),
                                                                          strSAutoPTID, funcOnChange));

                            TemplateHandler.Assign(ref tmpTplPrivate, "chkAuto",
                                                   HtmlHelper.GenCheckBox(strChkSAutoPTID, useGroupPT,
                                                                      gameTypeDetail.IsAutoPositionTaking,
                                                                      funcClickChkAuto));

                            TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                                                   HtmlHelper.GenTextBox(strMaPTID, MaPT.ToString(), "", true, ""));
                            TemplateHandler.Assign(ref tmpTplPrivate, "min_pt",
                                                   HtmlHelper.GenTextBox(strAPTID, APT.ToString(), "", true, ""));
                            TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                            TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                            //label error
                            TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                                   HtmlHelper.GenLabel(strSPresetPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));
                            TemplateHandler.Assign(ref tmpTplPrivate, "auto_pt_error",
                                                   HtmlHelper.GenLabel(strSAutoPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));
                            TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                                                   HtmlHelper.GenLabel(strMaPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));
                            TemplateHandler.Assign(ref tmpTplPrivate, "min_pt_error",
                                                   HtmlHelper.GenLabel(strAPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));


                            //assign gamekind name
                            //get betting limit list

                            TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBettingLimit"));

                            var lsStakeLevel = new List<StakeLevelDetail>();
                            lsStakeLevel = (_cust.CasinoSync)
                                               ? GetCustStakeLevels(_cust.AIBCCustID, gameTypeDetail.GameTypeID)
                                               : GetCustStakeLevels(gameTypeDetail.GameTypeID, _cust.Currency);
                            string stakeLevels = "";
                            string funcChkStakeClick = "";
                            int totalStake = 0;
                            foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                            {
                                if (!stakeDetail.IsChecked && !_cust.CasinoSync)
                                    continue;
                                string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                                    Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                                stakeLevels += HtmlHelper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                                  funcChkStakeClick) + stakeDetail.StakeLevelName;
                                postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                                totalStake++;
                            }

                            //Assign Stake levels
                            TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                            gameTypeData.Append(tmpTplPrivate);
                            totalGameType++;

                            int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                            postGameData = postGameData.Substring(0, subLength) +
                                           Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                            //postGameData = postGameData.Substring(0, postGameData.Length - 2) +
                            //               Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                            if (totalGameType == lsGameType.Count)
                            {
                                //Group
                                s4 = gameKindDetail.SuperMaxPositionTaking;
                                m1 = gameKindDetail.MPositionTaking;
                                a1 = gameKindDetail.APositionTaking;
                                s1Max = (!_cust.PTByGameType) ? (s4 - (m1 + a1)) : maxGroupSPresetPT;
                                s1Min = gameKindDetail.SuperMinPositionTaking;
                                s1Min = ((m1 + a1) >= s1Min) ? minPT : (s1Min - (m1 + a1));
                                u1Max = (!_cust.PTByGameType) ? s4 : maxGroupSAutoPT;
                                u1 = gameKindDetail.AutoPositionTaking;
                                s1 = gameKindDetail.SPositionTaking;

                                maxGroupSPresetPT = s1Max;
                                maxGroupSAutoPT = u1Max;

                                MaPTGroup = m1;
                                APTGroup = a1;

                                SPresetPTGroup = Math.Min(s1, s1Max);
                                SAutoPTGroup = Math.Min(u1, u1Max);

                                TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                       HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                          useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, ""));
                                    //remove group setting check box
                                TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                                //master pt
                                TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                       HtmlHelper.GenTextBox(MaPTGroupID, MaPTGroup.ToString(), "", true, ""));
                                //agent pt
                                TemplateHandler.Assign(ref tmpTplData, "group_min_pt",
                                                       HtmlHelper.GenTextBox(APTGroupID, APTGroup.ToString(), "", true, ""));

                                TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupSPresetPT, SPresetPTGroup,
                                                                              intervalPT,
                                                                              gameKindDetail.IsAutoPositionTaking,
                                                                              strSPresetPTGroupID, funcOnChangeGroup,
                                                                              "select-pt"));

                                TemplateHandler.Assign(ref tmpTplData, "group_auto_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupSAutoPT, SAutoPTGroup, intervalPT,
                                                                              !gameKindDetail.IsAutoPositionTaking,
                                                                              strSAutoPTGroupID, funcOnChangeGroup,
                                                                              "select-pt"));


                                TemplateHandler.Assign(ref tmpTplData, "chkGroupAuto",
                                                       HtmlHelper.GenCheckBox(strChkSAutoPTGroupID, false,
                                                                          gameKindDetail.IsAutoPositionTaking,
                                                                          funcClickChkAutoGroup));

                                TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", "");
                                TemplateHandler.Assign(ref tmpTplData, "group_min_pt_btn", "");
                                TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn",
                                                       HtmlHelper.GenChangePTButtons(strSPresetPTGroupID,
                                                                                 gameKindDetail.IsAutoPositionTaking));
                                TemplateHandler.Assign(ref tmpTplData, "group_auto_pt_btn",
                                                       HtmlHelper.GenChangePTButtons(strSAutoPTGroupID,
                                                                                 !gameKindDetail.IsAutoPositionTaking));

                                //END

                                TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                       useGroupPT ? (totalGameType + 1) : ((totalGameType*2) + 1));
                                TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                                TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                             gameTypeData.ToString());

                                data.Append(tmpTplData);
                                totalGameType = 0;
                                gameTypeData = new StringBuilder();
                                postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                               Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                            }
                        }
                    }
                }

                TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
                TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
                TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");
                //assign Casino customer info
                AssignCSCustInfo(ref tplPT, _cust, true);
                //assign IBC customer info
                AssignIBCCustInfo(ref tplPT, _cust);
                //assign post data
                TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
                HttpContext.Current.Session[Globals.SESSION_CUSTOMER] = _cust;
                return tplPT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Master PT on member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        public static string GenCSHTMLMasterOnMember(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            try
            {
                if (!_cust.CasinoSync)
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
                int custId = _cust.IBCCustID;
                string gameKindName = "Live";
                //string tplPT = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate(Globals.CASINO_PT_TEMPLATE);
                //Others game kind
                if (!Globals.IBCSetting.Casino.ShowBetSetting)
                    TemplateHandler.RemoveBlock(ref tplPT, Globals.TAG_BET_SETTING);
                var data = new StringBuilder();
                string tplHeader = WebUtils.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
                string tplData = WebUtils.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
                string tplPrivate;
                double maxMPT = Globals.MAX_PresetPTValue, maxMPresetPT = Globals.MAX_PresetPTValue; //max value
                double maxGroupMPT = Globals.MAX_PresetPTValue, maxGroupAPT = Globals.MAX_PresetPTValue;
                    //max group value

                double MPTGroup = Globals.CURRENT_PresetPTValue, APTGroup = Globals.MAX_PresetPTValue;
                    //current GROUP value
                double MPT = Globals.CURRENT_MaxPTValue, APT = Globals.MAX_PresetPTValue; //current value
                string strMPT = Globals.Master + Globals.strPT, strAPT = Globals.Agent + Globals.strPT;
                int intervalPT = Globals.INTERVAL_PT; //interval of select options
                int intervalComm = Globals.INTERVAL_COMM;
                string sep = Globals.SEP_GAMEKIND_GAMETYPE;
                int totalGameType = 0;
                int betlimitColspan = 2;
                //build header
                TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"))); //master pt

                TemplateHandler.Assign(ref tplHeader, Globals.MAX_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"))); //agent pt

                //remove min, max, auto pt
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT);
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.MIN_PT);

                //build content
                TemplateHandler.RemoveBlock(ref tplData, Globals.COMM_PT);
                TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
                TemplateHandler.RemoveBlock(ref tplData, Globals.MIN_PT);
                TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

                List<GameKindDetail> lsGameKind = GetGameKinds(_cust.IBCCustID);
                listGameKind = lsGameKind;
                bool useGroupPT = (_cust.NoCSData) ? !_cust.PTByGameTypeUpline : !_cust.PTByGameType;
                string postGameData = "";
                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    string tmpTplData = tplData;
                    gameKindName = gameKindDetail.CodeID.ToString();
                    string strMPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMPT;
                    string strAPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPT;
                    string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                             Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                    List<GameTypeDetail> lsGameType = GetGameType(_cust.IBCCustID, gameKindDetail.CodeID);
                    htGameType.Add(gameKindDetail.CodeID, lsGameType);
                    var gameTypeData = new StringBuilder();
                    maxGroupMPT = 0;
                    if (!IsP2PGame(gameKindDetail.CodeName))
                    {
                        foreach (GameTypeDetail gameTypeDetail in lsGameType)
                        {
                            tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);

                            if (0 == totalGameType)
                            {
                                postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            }

                            //game type value
                            string tmpTplPrivate = tplPrivate;
                            string strMPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMPT;
                            string strAPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPT;
                            string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                                Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                            postGameData += gameTypeDetail.GameTypeID + Globals.OPEN_STAKELIST_MARK;

                            m3 = gameTypeDetail.MasterMaxPositionTaking;
                            m1 = gameTypeDetail.MPositionTaking;
                            a1 = gameTypeDetail.APositionTaking;
                            m1Min = gameKindDetail.MasterMinPositionTaking;
                            m1Min = (a1 >= m1Min) ? minPT : (m1Min - a1);
                            m1Max = m3 - s1 - a1;

                            maxMPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : m1Max;
                            //max group mpt
                            maxGroupMPT = Math.Max(maxGroupMPT, m1Max);
                            MPT = m1;
                            APT = a1;

                            //super preset
                            TemplateHandler.Assign(ref tmpTplPrivate, "max_pt",
                                                   HtmlHelper.GenLabel(strAPTID, APT.ToString(), ""));

                            TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                                                   HtmlHelper.GenSelectOption(maxMPT, m1Min, MPT, intervalPT, useGroupPT,
                                                                          strMPTID, funcOnChange));
                            TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                            TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                            //label error
                            TemplateHandler.Assign(ref tmpTplPrivate, "max_pt_error",
                                                   HtmlHelper.GenLabel(strAPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));

                            TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                                   HtmlHelper.GenLabel(strMPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));

                            TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBettingLimit"));
                            var lsStakeLevel = new List<StakeLevelDetail>();
                            //lsStakeLevel = (_cust.CasinoSync) ? GetStakeLevels(_cust.IBCCustID, gameTypeDetail.GameTypeID) : GetStakeLevels(gameTypeDetail.GameTypeID);
                            lsStakeLevel = (_cust.CasinoSync)
                                               ? GetCustStakeLevels(_cust.IBCCustID, gameTypeDetail.GameTypeID)
                                               : GetCustStakeLevels(gameTypeDetail.GameTypeID, _cust.Currency);
                            string stakeLevels = "";
                            string funcChkStakeClick = "";
                            int totalStake = 0;
                            foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                            {
                                if (!stakeDetail.IsChecked && !_cust.CasinoSync)
                                    continue;
                                string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                                    Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                                stakeLevels += HtmlHelper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                                  funcChkStakeClick) + stakeDetail.StakeLevelName;
                                postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                                totalStake++;
                            }

                            //Assign Stake levels
                            TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                            gameTypeData.Append(tmpTplPrivate);
                            totalGameType++;

                            int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                            postGameData = postGameData.Substring(0, subLength) +
                                           Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                            //postGameData = postGameData.Substring(0, postGameData.Length - 2) +
                            //               Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                            if (totalGameType == lsGameType.Count)
                            {
                                //Group
                                s1 = gameKindDetail.SPositionTaking;
                                m3 = gameKindDetail.MasterMaxPositionTaking;
                                m1 = gameKindDetail.MPositionTaking;
                                a1 = gameKindDetail.APositionTaking;
                                m1Min = gameKindDetail.MasterMinPositionTaking;
                                m1Min = (a1 >= m1Min) ? minPT : (m1Min - a1);
                                m1Max = (!_cust.PTByGameType) ? (m3 - s1 - a1) : maxGroupMPT;

                                maxGroupMPT = m1Max;
                                MPTGroup = m1;
                                APTGroup = a1;
                                TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                       HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                          useGroupPT, Globals.CLASS_CHECKBOX_HIDDEN, ""));
                                    //remove group setting check box
                                TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                                //member commission
                                TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                       HtmlHelper.GenLabel(strMPTGroupID, MPTGroup.ToString(), ""));

                                TemplateHandler.Assign(ref tmpTplData, "group_max_pt",
                                                       HtmlHelper.GenLabel(strAPTGroupID, APTGroup.ToString(), ""));
                                TemplateHandler.Assign(ref tmpTplData, "group_max_pt_btn", "");
                                TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn", "");
                                //END Group
                                TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                       useGroupPT ? (totalGameType + 1) : ((totalGameType*2) + 1));
                                TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                                TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                             gameTypeData.ToString());

                                data.Append(tmpTplData);
                                totalGameType = 0;
                                gameTypeData = new StringBuilder();
                                postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                               Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                            }
                        }
                    }
                }

                TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
                TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
                TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");
                AssignCSCustInfo(ref tplPT, _cust, true);
                //assign IBC customer info
                AssignIBCCustInfo(ref tplPT, _cust);
                //assign post data
                TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
                HttpContext.Current.Session[Globals.SESSION_CUSTOMER] = _cust;
                return tplPT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit Agent PT on Member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        private static string GenCSHTMLAgentOnMember(string tplPT, CSCustomerDetail _cust, string csGroup)
        {
            try
            {
                int custId = _cust.IBCCustID;
                string gameKindName = "Live";
                if (!Globals.IBCSetting.Casino.ShowBetSetting)
                    TemplateHandler.RemoveBlock(ref tplPT, Globals.TAG_BET_SETTING);
                var data = new StringBuilder();
                string tplHeader = WebUtils.LoadTemplate(Globals.CASINO_PT_HEADER_TEMPLATE);
                string tplData = WebUtils.LoadTemplate(Globals.CASINO_OTHERS_PT_TEMPLATE);
                string tplPrivate;
                double maxMComm = Globals.MAX_Comm, maxAPresetPT = Globals.MAX_PresetPTValue; //max value
                double maxGroupMComm = Globals.MAX_Comm, maxGroupAPresetPT = Globals.MAX_PresetPTValue;
                    //max group value

                double MCommGroup = Globals.CURRENT_Comm, APresetPTGroup = Globals.MAX_PresetPTValue;
                    //current GROUP value
                double MComm = Globals.CURRENT_Comm, APresetPT = Globals.MAX_PresetPTValue; // current value
                string strMComm = Globals.Member + Globals.Comm, strAPresetPT = Globals.Agent + Globals.strPresetPT;
                int intervalPT = Globals.INTERVAL_PT; //interval of select options
                int intervalComm = Globals.INTERVAL_COMM;
                string sep = Globals.SEP_GAMEKIND_GAMETYPE;
                int totalGameType = 0;
                int betlimitColspan = 2;
                //build header
                TemplateHandler.Assign(ref tplHeader, Globals.PRESET_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPresetPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"))); //agent pt

                TemplateHandler.Assign(ref tplHeader, Globals.COMM_PT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member")));

                //remove min, max, auto pt
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.AUTO_PT);
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.MAX_PT);
                TemplateHandler.RemoveBlock(ref tplHeader, Globals.MIN_PT);

                //build content
                TemplateHandler.RemoveBlock(ref tplData, Globals.AUTO_PT);
                TemplateHandler.RemoveBlock(ref tplData, Globals.MAX_PT);
                TemplateHandler.RemoveBlock(ref tplData, Globals.MIN_PT);
                TemplateHandler.ReplaceBlock(ref tplData, Globals.TAG_HEADER, tplHeader);

                List<GameKindDetail> lsGameKind = (_cust.CasinoSync)
                                                      ? GetGameKinds(_cust.IBCCustID)
                                                      : GetGameKinds(_cust.UpperCustID);
                listGameKind = lsGameKind;
                bool useGroupPT = (_cust.NoCSData) ? !_cust.PTByGameTypeUpline : !_cust.PTByGameType;
                string postGameData = "";

                foreach (GameKindDetail gameKindDetail in lsGameKind)
                {
                    string tmpTplData = tplData;
                    gameKindName = gameKindDetail.CodeID.ToString();
                    List<GameTypeDetail> lsGameType = (_cust.CasinoSync)
                                                          ? GetGameType(_cust.IBCCustID, gameKindDetail.CodeID)
                                                          : GetGameType(_cust.UpperCustID, gameKindDetail.CodeID);
                    htGameType.Add(gameKindDetail.CodeID, lsGameType);
                    var gameTypeData = new StringBuilder();
                    maxGroupAPresetPT = 0;
                    maxGroupMComm = 0;
                    if (!IsP2PGame(gameKindDetail.CodeName))
                    {
                        foreach (GameTypeDetail gameTypeDetail in lsGameType)
                        {
                            tplPrivate = TemplateHandler.CopyBlock(tmpTplData, Globals.TAG_PRIVATE_SETTING);
                            string strMCommGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strMComm;
                            string strAPresetPTGroupID = gameKindName + sep + Globals.CS_GAME_GROUP + sep + strAPresetPT;
                            string funcOnChangeGroup = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_GROUP_FUNC,
                                                                     Globals.CS_SELECTOPTION_ONCHANGE_GROUP_PARAM);
                            if (0 == totalGameType)
                            {
                                postGameData += gameKindDetail.CodeID + Globals.OPEN_GAMETYPELIST_MARK;
                            }

                            //game type value
                            string tmpTplPrivate = tplPrivate;
                            string strMCommID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strMComm;
                            string strAPresetPTID = gameKindName + sep + gameTypeDetail.GameTypeID + sep + strAPresetPT;
                            string funcOnChange = string.Format(Globals.CS_SELECTOPTION_ONCHANGE_FUNC,
                                                                Globals.CS_SELECTOPTION_ONCHANGE_PARAM);
                            postGameData += gameTypeDetail.GameTypeID + Globals.OPEN_STAKELIST_MARK;

                            if (_cust.CasinoSync)
                            {
                                a2 = gameTypeDetail.APositionTakingUpline;
                                a1Max = a2;
                                a1 = (gameTypeDetail.APositionTaking > a1Max) ? a1Max : gameTypeDetail.APositionTaking;
                                a1Min = gameTypeDetail.MinPositionTakingUpline;
                                //commission
                                maxMComm = gameTypeDetail.AgentRollingRateUpline;
                                MComm = gameTypeDetail.MemberRollingRate;
                            }
                            else //copy from master
                            {
                                a2 = gameTypeDetail.APositionTaking;
                                a1Max = a2;
                                a1 = gameTypeDetail.APositionTaking;
                                a1Min = gameTypeDetail.MinPositionTaking;
                                //commission
                                maxMComm = gameTypeDetail.AgentRollingRate;
                                MComm = gameTypeDetail.AgentRollingRate;
                            }
                            //maxPT = Math.Min(a2, maxPT);
                            //minPT = Math.Max(minPT, a1Min);
                            maxMComm = (!_cust.PTByGameType) ? Globals.MAX_Comm : Math.Min(maxMComm, Globals.MAX_Comm);
                            MComm = Math.Min(MComm, Globals.MAX_Comm);

                            maxAPresetPT = (!_cust.PTByGameType) ? Globals.MAX_MaxPTValue : a1Max;
                            //group value
                            maxGroupMComm = Math.Max(maxMComm, maxGroupMComm);
                            maxGroupAPresetPT = Math.Max(maxGroupAPresetPT, maxAPresetPT);
                            APresetPT = a1;
                            //super preset
                            TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt",
                                                   HtmlHelper.GenSelectOption(maxAPresetPT, a1Min, APresetPT, intervalPT,
                                                                          useGroupPT,
                                                                          strAPresetPTID, funcOnChange));

                            TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt",
                                                   HtmlHelper.GenSelectOption(maxMComm, MComm, intervalComm, useGroupPT,
                                                                          strMCommID, funcOnChange,
                                                                          Globals.CLASS_SELECT_COMMISSION));
                            TemplateHandler.Assign(ref tmpTplPrivate, "gametype", gameTypeDetail.GameTypeName);
                            TemplateHandler.Assign(ref tmpTplPrivate, "col-span-bl", betlimitColspan);

                            //label error
                            TemplateHandler.Assign(ref tmpTplPrivate, "preset_pt_error",
                                                   HtmlHelper.GenLabel(strAPresetPTID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));

                            TemplateHandler.Assign(ref tmpTplPrivate, "comm_pt_error",
                                                   HtmlHelper.GenLabel(strMCommID + sep + Globals.CTR_LABEL_PREFIX, "",
                                                                   Globals.CLASS_ERROR_MAXVALUE));

                            TemplateHandler.Assign(ref tmpTplPrivate, "lblBettingLimit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBettingLimit"));

                            var lsStakeLevel = new List<StakeLevelDetail>();
                            lsStakeLevel = (_cust.CasinoSync)
                                               ? GetCustStakeLevels(_cust.IBCCustID, gameTypeDetail.GameTypeID)
                                               : GetCustStakeLevels(_cust.UpperCustID, gameTypeDetail.GameTypeID);
                            string stakeLevels = "";
                            string funcChkStakeClick = "";
                            int totalStake = 0;
                            foreach (StakeLevelDetail stakeDetail in lsStakeLevel)
                            {
                                if (!stakeDetail.IsChecked && !_cust.CasinoSync)
                                    continue;
                                string chkStakeID = gameKindDetail.CodeID + sep + gameTypeDetail.GameTypeID + sep +
                                                    Globals.CHK_STAKE + sep + stakeDetail.StakeLevelID;
                                stakeLevels += HtmlHelper.GenCheckBox(chkStakeID, false, stakeDetail.IsChecked,
                                                                  funcChkStakeClick)
                                               + stakeDetail.StakeLevelName;
                                postGameData += stakeDetail.StakeLevelID + Globals.SEP_STAKE;
                                totalStake++;
                            }

                            //Assign Stake levels
                            TemplateHandler.Assign(ref tmpTplPrivate, "betlimit", stakeLevels);

                            gameTypeData.Append(tmpTplPrivate);
                            totalGameType++;

                            int subLength = postGameData.Length - ((lsStakeLevel.Count == 0 || totalStake == 0) ? 0 : 2);
                            postGameData = postGameData.Substring(0, subLength) +
                                           Globals.CLOSE_STAKELIST_MARK + Globals.SEP_GAMETYPE; // ),

                            if (totalGameType == lsGameType.Count)
                            {
                                //assign group
                                if (_cust.CasinoSync)
                                {
                                    a2 = (!_cust.PTByGameType)
                                             ? Math.Min(gameKindDetail.APositionTakingUpline,
                                                        gameKindDetail.MaxPositionTakingUpline)
                                             : maxGroupAPresetPT;
                                    maxGroupMComm = (!_cust.PTByGameType)
                                                        ? gameKindDetail.AgentRollingRateUpline
                                                        : maxGroupMComm;
                                    MCommGroup = gameKindDetail.MemberRollingRate;
                                    a1Min = (!_cust.PTByGameType) ? gameKindDetail.MinPositionTakingUpline : minPT;
                                }
                                else
                                {
                                    a2 = (!_cust.PTByGameTypeUpline)
                                             ? gameKindDetail.APositionTaking
                                             : maxGroupAPresetPT;
                                    maxGroupMComm = (!_cust.PTByGameTypeUpline)
                                                        ? gameKindDetail.AgentRollingRate
                                                        : maxGroupMComm;
                                    MCommGroup = gameKindDetail.AgentRollingRate;
                                    a1Min = (!_cust.PTByGameTypeUpline) ? gameKindDetail.MinPositionTaking : minPT;
                                }
                                maxGroupMComm = Math.Min(maxGroupMComm, Globals.MAX_Comm);
                                MCommGroup = Math.Min(MCommGroup, Globals.MAX_Comm);
                                maxGroupAPresetPT = a2;
                                a1 = gameKindDetail.APositionTaking;
                                a1Min = Math.Min(a1, a1Min);
                                APresetPTGroup = a1;
                                TemplateHandler.Assign(ref tmpTplData, "chkGroupSetting",
                                                       HtmlHelper.GenCheckBox(gameKindName + sep + Globals.CHKGROUP, false,
                                                                          useGroupPT, "", Globals.CLASS_CHECKBOX_HIDDEN));
                                    //remove group setting check box
                                TemplateHandler.Assign(ref tmpTplData, "lblGroupSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGroupSetting"));
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupMComm, MCommGroup, intervalComm,
                                                                              false,
                                                                              strMCommGroupID, funcOnChangeGroup,
                                                                              Globals.CLASS_SELECT_COMMISSION));

                                TemplateHandler.Assign(ref tmpTplData, "group_preset_pt",
                                                       HtmlHelper.GenSelectOption(maxGroupAPresetPT, a1Min, APresetPTGroup,
                                                                              intervalPT, false,
                                                                              strAPresetPTGroupID, funcOnChangeGroup,
                                                                              "select-pt"));
                                TemplateHandler.Assign(ref tmpTplData, "group_comm_pt_btn", "");
                                TemplateHandler.Assign(ref tmpTplData, "group_preset_pt_btn",
                                                       HtmlHelper.GenChangePTButtons(strAPresetPTGroupID, false));

                                TemplateHandler.Assign(ref tmpTplData, "row-span-gamekind",
                                                       GetGameKindRowSpan(totalGameType, useGroupPT,
                                                                          gameKindDetail.CodeName));
                                TemplateHandler.Assign(ref tmpTplData, "gamekind", gameKindDetail.CodeName);

                                TemplateHandler.ReplaceBlock(ref tmpTplData, Globals.TAG_PRIVATE_SETTING,
                                                             gameTypeData.ToString());
                                //END Group

                                data.Append(tmpTplData);
                                totalGameType = 0;
                                gameTypeData = new StringBuilder();
                                postGameData = postGameData.Substring(0, postGameData.Length - 1) +
                                               Globals.CLOSE_GAMETYPELIST_MARK + Globals.SEP_GAMEKIND;
                            }
                        }
                    }
                }

                TemplateHandler.ReplaceBlock(ref tplPT, "data", data.ToString());
                TemplateHandler.Assign(ref tplPT, "display_tr_gametype", (useGroupPT) ? "none" : "");
                TemplateHandler.Assign(ref tplPT, "display_td_gametype", (useGroupPT) ? "" : "none");
                AssignCSCustInfo(ref tplPT, _cust, false);
                //assign IBC customer info
                AssignIBCCustInfo(ref tplPT, _cust);
                //assign post data
                TemplateHandler.Assign(ref tplPT, "hidden", GenHiddenDatas(postGameData, _cust));
                HttpContext.Current.Session[Globals.SESSION_CUSTOMER] = _cust;
                return tplPT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //End Member


        private static void AssignJavaScript(ref string tplPT, List<string> lsData)
        {
            string jsData = "<script type='text/javascript'>";
            if (lsData != null)
            {
                foreach (string sData in lsData)
                {
                    jsData += sData;
                }
            }
            jsData += "var ID_CHECKBOX_DISABLEDCASINO = '" + Globals.ID_CHECKBOX_DISABLEDCASINO + "';\r\n";
            jsData += "var ID_SPAN_DOWNLINE_OPT = '" + Globals.ID_SPAN_DOWNLINE_OPT + "';\r\n";
            jsData += "var ID_CHECKBOX_DISABLEDCASINO_DL = '" + Globals.ID_CHECKBOX_DISABLEDCASINO_DL + "';\r\n";
            jsData += "var strHidUpdDownline = '" + Globals.HIDDEN_UPDDOWNLLINE + "';";
            jsData += "</script>";
            TemplateHandler.Assign(ref tplPT, "jsdata", jsData);
        }

        private static void GenGameList(ref List<string> lsJS)
        {
            try
            {
                string arrGameKind = "";
                string arrGameKindID = "";
                string arrGameTypeValue = "";
                string arrGameTypeName = "arrGameType";
                string hRowSpanGameKind = "";
                int totalGameType = 0;
                List<GameKindDetail> lsGameKind = listGameKind;
                if (lsGameKind.Count > 0)
                {
                    foreach (GameKindDetail gameKindDetail in lsGameKind)
                    {
                        arrGameTypeValue = "";
                        arrGameTypeName = "arrGameType";
                        totalGameType = 0;

                        List<GameTypeDetail> lsGameType = (htGameType[gameKindDetail.CodeID] != null)
                                                              ? (List<GameTypeDetail>) htGameType[gameKindDetail.CodeID]
                                                              : null;
                        if (lsGameType != null && lsGameType.Count > 0)
                        {
                            foreach (GameTypeDetail gameTypeDetail in lsGameType)
                            {
                                if (0 == totalGameType)
                                    arrGameKindID += gameKindDetail.CodeID + ",";
                                arrGameTypeValue += gameTypeDetail.GameTypeID + ",";
                                totalGameType++;
                                if (totalGameType == lsGameType.Count)
                                {
                                    hRowSpanGameKind += string.Format(JS_AssignHashtableData, JS_hGameKindRowSpan,
                                                                      "rowspan_" + gameKindDetail.CodeID,
                                                                      GetGameKindRowSpan(totalGameType, false,
                                                                                         gameKindDetail.CodeName));
                                    hRowSpanGameKind += string.Format(JS_AssignHashtableData, JS_hGameKindRowSpan,
                                                                      "rowspan_" + gameKindDetail.CodeID + "_usegroup",
                                                                      GetGameKindRowSpan(totalGameType, true,
                                                                                         gameKindDetail.CodeName));
                                    arrGameTypeValue = arrGameTypeValue.Substring(0, arrGameTypeValue.Length - 1);
                                    arrGameTypeName = arrGameTypeName + Globals.SEP_GAMEKIND_GAMETYPE +
                                                      gameKindDetail.CodeID;
                                    lsJS.Add(string.Format(Globals.JS_VariableDefinitionFormat, arrGameTypeName,
                                                           string.Format(Globals.JS_arrayFormat, arrGameTypeValue)));
                                    arrGameKind += arrGameTypeName + ",";
                                }
                            }
                        }
                    }
                }
                arrGameKind = (!string.IsNullOrEmpty(arrGameKind))
                                  ? string.Format(Globals.JS_VariableFormat, Globals.JS_arrGameKind,
                                                  string.Format(Globals.JS_arrayFormat,
                                                                arrGameKind.Substring(0, arrGameKind.Length - 1)))
                                  : "[]";
                arrGameKindID = (!string.IsNullOrEmpty(arrGameKindID))
                                    ? string.Format(Globals.JS_VariableFormat, Globals.JS_arrGameKindID,
                                                    string.Format(Globals.JS_arrayFormat,
                                                                  arrGameKindID.Substring(0, arrGameKindID.Length - 1)))
                                    : "[]";
                lsJS.Add(hRowSpanGameKind);
                lsJS.Add(arrGameKindID);
                lsJS.Add(arrGameKind);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static bool IsP2PGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Globals.GAME_P2P.ToLower()) == -1)
                return false;
            return true;
        }

        private static bool IsLiveGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Globals.GAME_LIVE.ToLower()) == -1)
                return false;
            return true;
        }

        private static bool IsRnGGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Globals.GAME_RNG.ToLower()) == -1)
                return false;
            return true;
        }

        private static bool IsMiniGame(string gameKindName)
        {
            if (gameKindName.ToLower().IndexOf(Game_Mini.ToLower()) == -1)
                return false;
            return true;
        }


        private static void AssignCSCustInfo(ref string tplCSPT, CSCustomerDetail cust)
        {
            try
            {
                TemplateHandler.RemoveBlock(ref tplCSPT, "max-win-lose");

                TemplateHandler.Assign(ref tplCSPT, "lblAccumulated", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAccumulatedWinLoss"));
                TemplateHandler.Assign(ref tplCSPT, "lblMaxTransferCredit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxTransferCredit"));
                TemplateHandler.Assign(ref tplCSPT, "lblMaxWin", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxWinAllowed"));
                TemplateHandler.Assign(ref tplCSPT, "lblMaxLose", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxLoseAllowed"));
                TemplateHandler.Assign(ref tplCSPT, "lblMin", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMin"));
                TemplateHandler.Assign(ref tplCSPT, "lblNoLimited", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNoLimited"));
                TemplateHandler.Assign(ref tplCSPT, "Reset",
                                       !cust.IsMaxTransfer
                                           ? ""
                                           : HtmlHelper.GenButton(Globals.BTN_ResetID, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Reset"), "", false,
                                                              Globals.BTN_CLICK_RESET_CS_PT_FUNC));
                TemplateHandler.Assign(ref tplCSPT, "accumulatedValue",
                                       CastAide.AsString(
                                           (!cust.IsMaxTransfer ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "unlimited") : cust.TotalWinLoss.ToString()), ""));

                TemplateHandler.Assign(ref tplCSPT, "unlimited", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "unlimited"));
                

                TemplateHandler.Assign(ref tplCSPT, "maxTransferCredit", HtmlHelper.GenTextBox(Globals.TXT_MAXTRANSTERCREDIT, CastAide.AsString(cust.MaxTransferCredit, ""), "", !cust.IsMaxTransfer, ""));

                TemplateHandler.Assign(ref tplCSPT, "maxTransferCredit",
                                       HtmlHelper.GenTextBox(Globals.TXT_MAXTRANSTERCREDIT,
                                                         CastAide.AsString(cust.MaxTransferCredit, ""), "",
                                                         !cust.IsMaxTransfer, ""));

                TemplateHandler.Assign(ref tplCSPT, "MaxTransferCreditValue", cust.MaxTransferCredit);

                TemplateHandler.Assign(ref tplCSPT, "checked", !cust.IsMaxTransfer ? "checked" : "");
                TemplateHandler.Assign(ref tplCSPT, "transferCreditId", Globals.TXT_MAXTRANSTERCREDIT);

                TemplateHandler.Assign(ref tplCSPT, "maxWinValue",
                                       HtmlHelper.GenTextBox(Globals.TXT_MAXWINID, CastAide.AsString(cust.MaxWin, ""), "",
                                                         false, ""));
                TemplateHandler.Assign(ref tplCSPT, "maxLoseValue",
                                       HtmlHelper.GenTextBox(Globals.TXT_MAXLOSEID, CastAide.AsString(cust.MaxLose, ""), "",
                                                         false, ""));
                TemplateHandler.Assign(ref tplCSPT, "minValue", CastAide.AsString(cust.MinCreditBase, ""));
                TemplateHandler.Assign(ref tplCSPT, "noLimitedValue", Globals.CS_NOLIMITED_WINLOSE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AssignCSCustInfo(ref string tplCSPT, CSCustomerDetail cust, bool readOnly)
        {
            try
            {
                if (Globals.IBCSetting.Casino.ShowBetSetting)
                {
                    TemplateHandler.RemoveBlock(ref tplCSPT, "max-win-lose");
                }

                TemplateHandler.Assign(ref tplCSPT, "lblAccumulated", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAccumulatedWinLoss"));
                TemplateHandler.Assign(ref tplCSPT, "lblMaxTransferCredit", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxTransferCredit"));
                TemplateHandler.Assign(ref tplCSPT, "lblMaxWin", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxWinAllowed"));
                TemplateHandler.Assign(ref tplCSPT, "lblMaxLose", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxLoseAllowed"));
                TemplateHandler.Assign(ref tplCSPT, "lblMin", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMin"));


                TemplateHandler.Assign(ref tplCSPT, "Reset",
                                       (readOnly || !cust.CasinoSync)
                                           ? ""
                                           : HtmlHelper.GenLink(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Reset"), "javascript:;", "",
                                                            "ResetMaxTransfer('" + cust.IBCCustID + "')"));

                TemplateHandler.Assign(ref tplCSPT, "Reset", (readOnly || !cust.CasinoSync) ? "" : HtmlHelper.GenLink(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Reset"), "javascript:;", "", "ResetMaxTransfer('" + cust.IBCCustID + "')"));

                TemplateHandler.Assign(ref tplCSPT, "accumulatedValue", cust.TotalWinLoss.ToString());
                TemplateHandler.Assign(ref tplCSPT, "maxTransferCredit",
                                       (readOnly)
                                           ? HtmlHelper.GenLabel(Globals.TXT_MAXTRANSTERCREDIT,
                                                             cust.MaxTransferCredit.ToString(), "")
                                           : HtmlHelper.GenTextBox(Globals.TXT_MAXTRANSTERCREDIT,
                                                               !cust.CasinoSync
                                                                   ? cust.DefaultTransferCredit.ToString()
                                                                   : cust.MaxTransferCredit.ToString(),
                                                               "",
                                                               false,
                                                               "",
                                                               "return numberOnly(event)"
                                                 )
                    );


                TemplateHandler.Assign(ref tplCSPT, "cbxAutoResetchecked",
                                       (cust.IsAutoReset || !cust.CasinoSync) ? "checked" : "");

                TemplateHandler.Assign(ref tplCSPT, "autoReset", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "autoReset"));


                TemplateHandler.Assign(ref tplCSPT, "isSync", (!cust.CasinoSync).ToString().ToLower());
                TemplateHandler.Assign(ref tplCSPT, "exitConfirmMsg",
                                       (!cust.CasinoSync) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "exitCSConfirmMsg") : "");
                TemplateHandler.Assign(ref tplCSPT, "MaxTransferCreditValue", cust.MaxTransferCredit);

                TemplateHandler.Assign(ref tplCSPT, "disabled", readOnly ? "disabled" : "");
                TemplateHandler.Assign(ref tplCSPT, "transferCreditId", Globals.TXT_MAXTRANSTERCREDIT);

                TemplateHandler.Assign(ref tplCSPT, "maxWinValue",
                                       (readOnly)
                                           ? HtmlHelper.GenLabel(Globals.TXT_MAXWINID, CastAide.AsString(cust.MaxWin, ""),
                                                             "")
                                           : HtmlHelper.GenTextBox(Globals.TXT_MAXWINID, CastAide.AsString(cust.MaxWin, ""),
                                                               "", false, ""));
                TemplateHandler.Assign(ref tplCSPT, "maxLoseValue",
                                       (readOnly)
                                           ? HtmlHelper.GenLabel(Globals.TXT_MAXLOSEID, CastAide.AsString(cust.MaxLose, ""),
                                                             "")
                                           : HtmlHelper.GenTextBox(Globals.TXT_MAXLOSEID,
                                                               CastAide.AsString(cust.MaxLose, ""), "", false, ""));
                TemplateHandler.Assign(ref tplCSPT, "minValue", CastAide.AsString(cust.MinCreditBase, ""));

                TemplateHandler.Assign(ref tplCSPT, "noLimitedValue",
                                       (cust.RoleId == 4)
                                           ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNoLimited")
                                           : FormatHandler.FormatNumber2(cust.MaxTransferCreditLimit, "E"));
                TemplateHandler.Assign(ref tplCSPT, "minLimitedValue",
                                       FormatHandler.FormatNumber2(Math.Min(cust.MinTransferCreditLimit, cust.MaxTransferCreditLimit), "E"));

                TemplateHandler.Assign(ref tplCSPT, "submit",
                                       (readOnly)
                                           ? ""
                                           : HtmlHelper.GenLink(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Submit"), Globals.CS_SUBMIT_HREF,
                                                            Globals.CLASS_SUBMIT, Globals.CS_SUBMIT_FUNC));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AssignIBCCustInfo(ref string tplCSPT, CSCustomerDetail cust)
        {
            try
            {
                TemplateHandler.Assign(ref tplCSPT, "lblUsername", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUsername"));
                TemplateHandler.Assign(ref tplCSPT, "lblFirstname", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFirstname"));
                TemplateHandler.Assign(ref tplCSPT, "lblPhone", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPhone"));
                TemplateHandler.Assign(ref tplCSPT, "lblFax", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFax"));
                TemplateHandler.Assign(ref tplCSPT, "lblUsername2", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUsername2"));
                TemplateHandler.Assign(ref tplCSPT, "lblLastname", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblLastname"));
                TemplateHandler.Assign(ref tplCSPT, "lblMobile", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMobilePhone"));
                TemplateHandler.Assign(ref tplCSPT, "lblClosed", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatusClosed"));
                TemplateHandler.Assign(ref tplCSPT, "lblSuspend", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSuspend"));
                TemplateHandler.Assign(ref tplCSPT, "username", cust.UserName);
                TemplateHandler.Assign(ref tplCSPT, "fax", cust.Fax);
                TemplateHandler.Assign(ref tplCSPT, "firstname", cust.FirstName);
                TemplateHandler.Assign(ref tplCSPT, "phone", cust.Phone);
                TemplateHandler.Assign(ref tplCSPT, "username2", cust.UserName2);
                TemplateHandler.Assign(ref tplCSPT, "lastname", cust.LastName);
                TemplateHandler.Assign(ref tplCSPT, "mobile", cust.MobilePhone);
                TemplateHandler.Assign(ref tplCSPT, "closed",
                                       HtmlHelper.GenCheckBox(Globals.ID_CHECKBOX_CLOSED, true, cust.Closed, ""));
                TemplateHandler.Assign(ref tplCSPT, "suspend",
                                       HtmlHelper.GenCheckBox(Globals.ID_CHECKBOX_SUSPEND, true, cust.Suspend, ""));
                TemplateHandler.Assign(ref tplCSPT, "lblEnableCS", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledCasino"));

                int parentId = 0;
                switch (cust.RoleId)
                {
                    case 3:
                        parentId = cust.SIBCCustID;
                        break;
                    case 2:
                        parentId = cust.MIBCCustID;
                        break;
                    case 1:
                        parentId = cust.AIBCCustID;
                        break;
                }
                bool disableChkStatus = false;
                if (cust.RoleId != 4 && Security.Permissions.IsDisabledCS(parentId))
                    disableChkStatus = true;


                string strCbxLB1 = "";
                string strCbxLB2 = "";

                if (cust.RoleId == 2 && cust.CasinoSync)
                {
                    strCbxLB1 = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent");
                }
                else if (cust.RoleId == 3 && cust.CasinoSync)
                {
                    strCbxLB1 = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master");
                }
                else if (cust.RoleId == 4 && cust.CasinoSync)
                {
                    strCbxLB1 = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super");
                }
                strCbxLB2 = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "casinodownlines");

                strCbxLB1 = "<label for='" + Globals.ID_CHECKBOX_DISABLEDCASINO + "'>" + strCbxLB1 + "</label>";
                strCbxLB2 = "<label for='" + Globals.ID_CHECKBOX_DISABLEDCASINO_DL + "'>" + strCbxLB2 + "</label>";

                string strCbkBox = "&nbsp;" +
                                   HtmlHelper.GenCheckBox(Globals.ID_CHECKBOX_DISABLEDCASINO, disableChkStatus,
                                                      !cust.CasinoDisabled && (cust.CasinoSync), "MarkChangeStatus()") +
                                   strCbxLB1;

                if (cust.RoleId != 1 && cust.CasinoSync)
                {
                    if (cust.CasinoDisabled)
                    {
                        strCbkBox += "&nbsp;<span id=\"" + Globals.ID_SPAN_DOWNLINE_OPT +
                                     "\" style=\"visibility: hidden; font-weight: normal\">" +
                                     HtmlHelper.GenCheckBox(Globals.ID_CHECKBOX_DISABLEDCASINO_DL, false, false, "") +
                                     strCbxLB2 + HtmlHelper.GenHidden(Globals.HIDDEN_UPDDOWNLLINE, "") + "</span>";
                    }
                    else
                    {
                        strCbkBox += "&nbsp;&nbsp;&nbsp;&nbsp;<span id=\"" + Globals.ID_SPAN_DOWNLINE_OPT +
                                     "\" style=\"visibility: hidden; font-weight: normal; color: red\">(" +
                                     ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "casinodownlinesdisabled") + ")</span>";
                    }
                }

                TemplateHandler.Assign(ref tplCSPT, "disableCS", strCbkBox);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GenHiddenDatas(string gameData, CSCustomerDetail cust)
        {
            try
            {
                string strHiddens = "";
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_GAMEDATA, gameData);
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_RoldID, cust.RoleId.ToString()); //master role id = 3
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_PTByGameType, cust.PTByGameType.ToString());
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_LangID, Globals.CONST_LangID.ToString()); // Hard code
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_CountryID, Globals.CONST_CountryID.ToString());
                    // Hard code
                return strHiddens;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GenHiddenDatas(Hashtable datas, CSCustomerDetail cust)
        {
            try
            {
                string strHiddens = "";
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_RoldID, cust.RoleId.ToString()); //master role id = 3
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_LangID, Globals.CONST_LangID.ToString()); // Hard code
                strHiddens += HtmlHelper.GenHidden(Globals.HIDDEN_CountryID, Globals.CONST_CountryID.ToString());
                    // Hard code
                IDictionaryEnumerator _enumerator = datas.GetEnumerator();
                while (_enumerator.MoveNext())
                {
                    strHiddens += HtmlHelper.GenHidden(_enumerator.Key.ToString(), _enumerator.Value.ToString());
                }

                return strHiddens;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static void AssignCommonHeader(ref string tplCSPT)
        {
            try
            {
                TemplateHandler.Assign(ref tplCSPT, "lblCustomerInfomation", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomerInfomation"));
                TemplateHandler.Assign(ref tplCSPT, "lblBetSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                TemplateHandler.Assign(ref tplCSPT, "lblPositionTaking", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
            }
            catch (Exception ex)
            {
            }
        }

        private static int GetParentID(CSCustomerDetail _cust)
        {
            if (_cust == null)
                return 0;
            switch ((Globals.AccountType) _cust.RoleId)
            {
                case Globals.AccountType.Master:
                    return _cust.SIBCCustID;
                case Globals.AccountType.Agent:
                    return _cust.MIBCCustID;
                case Globals.AccountType.Member:
                    return _cust.AIBCCustID;
            }
            return 0;
        }

        private static int GetGameKindRowSpan(int totalGameType, bool useGroupSetting, string gameKindName)
        {
            int rowSpan = 0;
            string browser = HttpContext.Current.Request.Browser.Browser;
            if (IsP2PGame(gameKindName))
            {
                if (browser == "IE")
                    rowSpan = totalGameType + 1;
                else
                    rowSpan = useGroupSetting ? totalGameType : totalGameType + 1;
            }
            else
            {
                if (browser == "IE")
                {
                    string version = HttpContext.Current.Request.Browser.Version;
                    if (version.Equals("8.0"))
                    {
                        rowSpan = useGroupSetting ? totalGameType + 1 : (totalGameType*2) + 1;
                    }
                    else
                    {
                        rowSpan = (totalGameType*2) + 1;
                    }
                }
                else
                    rowSpan = useGroupSetting ? (totalGameType + 1) : ((totalGameType*2) + 1);
            }
            return rowSpan;
        }
        /// <summary>
        /// Full reset casino max transfer
        /// </summary>
        /// <param name="listIBCCustID"></param>
        /// <param name="FromIP"></param>
        /// <param name="ModifyBy"></param>
        /// <param name="ModifyByUsername"></param>
        /// <param name="FromSystem">2: Agent, 3: Accounting</param>
        /// <returns></returns>
        public static bool FullResetMaxTransfer(string listIBCCustID, string FromIP, int ModifyBy,
                                                string ModifyByUsername, int FromSystem)
        {
            try
            {
                return IBCProvider.Casino.FullResetMaxTransfer(listIBCCustID, FromIP, ModifyBy, ModifyByUsername,
                                                               FromSystem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
