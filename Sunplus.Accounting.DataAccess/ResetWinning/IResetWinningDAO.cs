// -----------------------------------------------------------------------
// <copyright file="IResetWinning.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.DataAccess.Report.ResetWinning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DNAide.Data;
    using Sunplus.Accounting.Entities.Report.ResetWinning;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IResetWinningDAO
    {
        ResetWinningDTO GetDataResetWinning(string date, int data);
    }
}
