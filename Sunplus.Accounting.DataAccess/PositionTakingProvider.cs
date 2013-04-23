using System;
using System.Collections;
using DNAide.Data;
using IBCUtils;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for PositionTakingProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public abstract class PositionTakingProvider
    {
        private static PositionTakingProvider _Instance;
        private static PositionTakingProvider _InstanceHR;

        public static PositionTakingProvider Instance
        {
            get
            {
                return
                    (PositionTakingProvider)
                    Activator.CreateInstance(Type.GetType(Globals.IBCSetting.PositionTaking.ProviderType));
            }
            set { _Instance = value; }
        }

        public static PositionTakingProvider InstanceHR
        {
            get
            {
                return
                    (PositionTakingProvider)
                    Activator.CreateInstance(Type.GetType(Globals.IBCSetting.PositionTaking.ProviderTypeHR));
            }
            set { _InstanceHR = value; }
        }


        public virtual DbResult GetPT(int custId, Globals.AccountType accountType, int uplineId)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult GetPTSuperEditMember(int custId, int uplineId)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult GetPTMasterEditMember(int custId, int uplineId)
        {
            throw new NotImplementedException();
        }

        public virtual IbcError UpdatePT(Globals.AccountType accountType, Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult GetMinMaxPT(int custId, Globals.AccountType accountType)
        {
            throw new NotImplementedException();
        }

        public virtual ErrorDetail UpdateMinMaxPT(Globals.AccountType accountType, Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual IbcError UpdatePTMasterOnMember(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual IbcError UpdatePTSuperOnMember(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}