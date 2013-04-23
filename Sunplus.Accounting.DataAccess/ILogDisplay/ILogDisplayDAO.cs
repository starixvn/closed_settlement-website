using System;
using System.Data;
using System.Collections;

namespace Sunplus.Accounting.DataAccess.ILogDisplay
{
    public interface ILogDisplayDAO
    {
        DataTable GetLogDisplay(Hashtable param, string logType);
        int GetLogDisplayTotal(Hashtable param, string logType);
    }
}
