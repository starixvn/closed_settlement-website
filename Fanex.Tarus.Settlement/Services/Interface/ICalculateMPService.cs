// --------------------------------------------------------------------------
// <copyright file="ICalculateMPService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.Services.Interface
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Settlement.Criteria;
    using Fanex.Tarus.Settlement.Models;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ICalculateMPService
    {
        CalcInfo GetCalcMPStatus(GetCalcMPStatusCriteria criteria);

        Settlement_ActionResult DoCalcMP(GetCalcMPStatusCriteria criteria);
    }
}
