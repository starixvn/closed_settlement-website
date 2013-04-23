using System;
using System.Collections;
using System.Collections.Generic;

using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for CasinoProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public abstract class CasinoProvider
    {
        private static CasinoProvider _Instance;

        public static CasinoProvider Instance
        {
            get { return (CasinoProvider) Activator.CreateInstance(Type.GetType(Globals.IBCSetting.Casino.ProviderType)); }
            set { _Instance = value; }
        }


        /// <summary>
        /// Update Casino status to SP database
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="bSyncCS"></param>
        /// <param name="bDisabledCS"></param>
        /// <returns></returns>
        public abstract bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS);
        public abstract bool UpdateCSStatusAfterSync(int custid, bool bSyncCS, bool bDisabledCS, int userId, string ip);

        public abstract bool UpdateCSStatusWithoutSync(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline);
        public abstract bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline);
        public abstract CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r);

        public abstract List<GameKindDetail> GetGameKinds();

        /// <summary>
        /// Get game kind list
        /// <param name="casinoNo"/>
        /// casinoNo= "01", "02"
        /// </summary>
        public abstract List<GameKindDetail> GetGameKinds(string casinoNo);


        public abstract List<GameKindDetail> GetGameKinds(int custId);
        public abstract List<GameKindDetail> GetGameKinds_EditMember(int memberId);


        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindID">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public abstract List<GameTypeDetail> GetGameType(int gameKindID);

        public abstract List<GameTypeDetail> GetGameType(int custId, int gameKindID);
        public abstract List<GameTypeDetail> GetGameTypes_EditMember(int memberId, int gameKindID);

        /// <summary>
        /// Get game type stake levels
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="gameTypeID"></param>
        /// <returns></returns>
        public abstract List<StakeLevelDetail> GetCustStakeLevels(int custId, int gameTypeID);


        /// <summary>
        /// Get game type stake levels list
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="gameTypeID"></param>
        /// <returns></returns>
        public abstract List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, int currencyId);

        public abstract List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, string currencyName);

        public abstract CSCustomerDetail GetCustSettingCS(int _IBCCustId);

        public abstract void GetCustSettingCS(ref CSCustomerDetail _custIBC);

        public abstract CSCustomerDetail GetCustInfoIBC(int _IBCCustId);

        public virtual List<GameKindDetail> GetGameKinds_MasterEditMember(int memberid)
        {
            throw new NotImplementedException();
        }

        public virtual List<GameTypeDetail> GetGameTypes_MasterEditMember(int memberid, int gameKindId)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsDisabledCasino(int custId)
        {
            return true;
        }

        public virtual CSCustomerDetail GetCasinoStatus(int custId)
        {
            throw new NotImplementedException();
        }

        public virtual bool UpdateCustSuspend(int IBCCustID, bool Suspended, string FromIP)
        {
            throw new NotImplementedException();
        }

        public virtual bool UpdateCustClosed(int IBCCustID, bool Closed, string FromIP)
        {
            throw new NotImplementedException();
        }

        public virtual bool UpdateCustEnableCasino(int IBCCustID, bool EnabledCasino, bool UpdateDownline, string FromIP)
        {
            throw new NotImplementedException();
        }

        public virtual bool UpdateCustPwd(int IBCCustID, string UserPwdEncrypted, string FromIP)
        {
            throw new NotImplementedException();
        }

        //Full Reset Max Transfer
        //FromSystem	Tinyint	2:update from agent site
        //                      3:update from account site	
        public virtual bool FullResetMaxTransfer(string listIBCCustID, string FromIP, int ModifyBy,
                                                 string ModifyByUsername, int FromSystem)
        {
            throw new NotImplementedException();
        }

        public virtual List<ErrorDetail> MoveMaster(string MasterUsername, string SuperUsername, string FromIP,
                                                    int ModifyByUserId, string ModifyByUser)
        {
            throw new NotImplementedException();
        }

        public virtual List<ErrorDetail> MoveAgent(string AgentUsername, string MasterUsername, string FromIP,
                                                   int ModifyByUserId, string ModifyByUser)
        {
            throw new NotImplementedException();
        }

        public virtual bool RollbackMaster(string MasterUsername, bool IsNeedRollback)
        {
            throw new NotImplementedException();
        }

        public virtual bool RollbackAgent(string AgentUsername, bool IsNeedRollBack)
        {
            throw new NotImplementedException();
        }

        public static void ResetCasinoSetting(bool useWebConfigProvider, string newProvider)
        {
            try
            {
                Globals.IBCSetting.Casino.UseWebConfigProviderType = useWebConfigProvider;
                Instance = null;
                if (!useWebConfigProvider)
                    Globals.IBCSetting.Casino.ProviderType = newProvider;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        public static void ResetCasinoSetting(bool useWebConfigProvider)
        {
            try
            {
                Globals.IBCSetting.Casino.UseWebConfigProviderType = useWebConfigProvider;
                Instance = null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }
    }
}