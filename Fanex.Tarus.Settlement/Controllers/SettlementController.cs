// --------------------------------------------------------------------------
// <copyright file="SettlementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Tarus.Settlement.Criteria;
    using Fanex.Tarus.Settlement.Services.Implement;
    using Fanex.Tarus.Settlement.Services.Interface;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class Settlement_CalculateMPController : Controller
    {
        private ICalculateMPService _calculateMPService;
        public Settlement_CalculateMPController()
            : this(null)
        {
        }

        public Settlement_CalculateMPController(ICalculateMPService calculateMPService)
        {
            this._calculateMPService = calculateMPService ?? new CalculateMPService();
        }

        public JsonResult GetCalcMPStatus(GetCalcMPStatusCriteria criteria)
        {
            var result = this._calculateMPService.GetCalcMPStatus(criteria);
            var jsonResult = base.Json(result);
            return jsonResult;
        }

        public JsonResult GetCompletedCalcMPStatus(GetCompletedCalcMPStatusCriteria criteria)
        {
            var result = this._calculateMPService.GetCompletedCalcMPStatus(criteria);
            var jsonResult = base.Json(result);
            return jsonResult;
        }

        public JsonResult DoCalcMP(GetCalcMPStatusCriteria criteria)
        {
            var result = this._calculateMPService.DoCalcMP(criteria);
            var jsonResult = base.Json(result);
            return jsonResult;
        }
    }
}
