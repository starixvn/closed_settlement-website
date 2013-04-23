// -----------------------------------------------------------------------
// <copyright file="ResetWinningDTO.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Entities.Report.ResetWinning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 
    [Serializable]
    [DataContract]
    public class ResetWinning
    {
        [DataMember]
        public string Products { get; set; }

        [DataMember]
        public string Winloss { get; set; }

        [DataMember]
        public string Reset { get; set; }

        [DataMember]
        public string Variance { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ResetWinningDTO
    {
        [DataMember]
        public List<ResetWinning> Items { get; set; }

        [DataMember]
        public string Total { get; set; }
    }
}
