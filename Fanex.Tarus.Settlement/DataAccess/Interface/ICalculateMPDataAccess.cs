// --------------------------------------------------------------------------
// <copyright file="ICalculateMPDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.DataAccess.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Settlement.Criteria;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ICalculateMPDataAccess
    {
        List<dynamic> GetCalcMPStatus(GetCalcMPStatusCriteria criteria);

        List<dynamic> GetCompletedCalcMPStatus(GetCompletedCalcMPStatusCriteria criteria);
    }
}
