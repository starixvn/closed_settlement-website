using System;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// Description for Accounting Exception
    /// @viet hoang
    /// </summary>
    public class AccException : Exception
    {
        public AccException(string message) : base(message)
        {
        }
    }
}