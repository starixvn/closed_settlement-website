using System.Collections.Generic;
using Sunplus.Accounting.Entities.Settle;

namespace Sunplus.Accounting.Business.Settle
{
    public interface IViewLogManager
    {
        SettleLogResult SelSettleLogByMachCode(string matchCode, string fDate, string tDate);
    }
}
