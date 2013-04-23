using System;
using System.Collections;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for IBCCustomerProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public abstract class IBCCustomerProvider
    {
        private static IBCCustomerProvider _Instance;

        public static IBCCustomerProvider Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance =
                        (IBCCustomerProvider)
                        Activator.CreateInstance(Type.GetType(Globals.IBCSetting.IBCCustomer.ProviderType));
                }
                return _Instance;
            }
            set { _Instance = value; }
        }

        public virtual bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS)
        {
            return false;
        }

        public virtual bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            return false;
        }

        public virtual CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r)
        {
            return null;
        }

        public virtual CSCustomerDetail GetCustInfoIBC(int _IBCCustId)
        {
            return null;
        }

        public virtual CSCustomerDetail GetBasicCustInfoIBC(int _IBCCustId)
        {
            return null;
        }

        public virtual bool IsDisabledCasino(int custId)
        {
            return false;
        }

        public virtual CSCustomerDetail GetCasinoStatus(int custId)
        {
            return null;
        }

        public virtual CustomerDetail GetCustomerStatus(int custId)
        {
            return null;
        }

        public virtual CustomerDetail GetCustomerDetail(string username)
        {
            return null;
        }
    }
}