using Sunplus.Accounting.DataAccess.Provider.Others;

/// <summary>
/// Summary description for CommonProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public static class OtherProvider
    {
        public static NewsProvider News
        {
            get { return NewsProvider.Instance(); }
        }
    }
}