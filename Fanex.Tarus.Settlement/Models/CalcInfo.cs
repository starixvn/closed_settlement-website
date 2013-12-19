// --------------------------------------------------------------------------
// <copyright file="CalcInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CalcInfo
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }

        public bool IsRefunded { get; set; }

        public bool IsMultipleCalculated { get; set; }

        public List<CalcInfo> ChildInfo { get; set; }
    }
}
