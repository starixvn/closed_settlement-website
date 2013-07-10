// --------------------------------------------------------------------------
// <copyright file="GetCalcMPStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.Criteria
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetCalcMPStatusCriteria
    {
        public string MatchCodes { get; set; }

        public string GetSettingKey()
        {
            return "Settlement_SelCalcMPStatus";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
