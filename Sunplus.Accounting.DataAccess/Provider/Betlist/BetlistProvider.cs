using System;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for BetlistProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Betlist
{
    public abstract class BetlistProvider : DataAccess
    {
        //Define virtual or abstract function
        private static BetlistProvider _Instance;


        public static BetlistProvider Instance
        {
            get { return (BetlistProvider) Activator.CreateInstance(Type.GetType(Globals.IBCSetting.Betlist.ProviderType)); }
            set { _Instance = value; }
        }
    }
}