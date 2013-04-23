// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Entities
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
    public class Result
    {
        [DataMember]
        public int ErrCode { get; set; }

        [DataMember]
        public string ErrMsg { get; set; }
    }
}
