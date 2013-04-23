using System;
using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess
{
    /// <summary>
    /// Summary description for UtilsProvider
    /// </summary>
    public class UtilsProvider
    {
        private static UtilsProvider _Instance;

        public static UtilsProvider Instance
        {
            get
            {
                if (null == _Instance)
                {
                    _Instance =
                        (UtilsProvider) Activator.CreateInstance(Type.GetType(Globals.IBCSetting.Utils.ProviderType));
                }
                return _Instance;
            }
            set { _Instance = value; }
        }

        public virtual List<IPVNBean> GetIPVN()
        {
            throw new NotImplementedException();
        }

        public virtual Dictionary<int, string> GetExchange()
        {
            throw new NotImplementedException();
        }
    }
}