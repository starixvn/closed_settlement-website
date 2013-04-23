using System.Collections;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface IReportMainDAO
    {
         DbResult SelectMixparlayDetail(string winlossDate, int bettype);

    }
}