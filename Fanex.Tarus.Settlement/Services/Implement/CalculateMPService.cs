// --------------------------------------------------------------------------
// <copyright file="CalculateMPService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Tarus.Settlement.Constants;
    using Fanex.Tarus.Settlement.Criteria;
    using Fanex.Tarus.Settlement.DataAccess.Implement;
    using Fanex.Tarus.Settlement.DataAccess.Interface;
    using Fanex.Tarus.Settlement.Models;
    using Fanex.Tarus.Settlement.Resources;
    using Fanex.Tarus.Settlement.Services.Interface;
    using Sunplus.Accounting.Core.Extensions;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CalculateMPService : ICalculateMPService
    {
        private ICalculateMPDataAccess _calculateMPDataAccess;
        public CalculateMPService()
            : this(null)
        {
        }

        public CalculateMPService(ICalculateMPDataAccess calculateMPDataAccess)
        {
            this._calculateMPDataAccess = calculateMPDataAccess ?? new CalculateMPDataAccess();
        }

        public CalcInfo GetCalcMPStatus(GetCalcMPStatusCriteria criteria)
        {
            try
            {
                var result = new CalcInfo
                {
                    ChildInfo = new List<CalcInfo>()
                };

                var rawData = this._calculateMPDataAccess.GetCalcMPStatus(criteria);
                for (var i = 0; i < rawData.Count; i++)
                {
                    var info = rawData[i];
                    var calcInfo = new CalcInfo
                    {
                        Id = CastAide.AsString(info.matchid),
                        Status = CastAide.AsBoolean(info.CalcMP) ?
                            (int)CalcStatusConstants.Calculating :
                            (int)CalcStatusConstants.Waiting
                    };

                    calcInfo.StatusName = this.MappingCalcStatus(calcInfo.Status);
                    result.ChildInfo.Add(calcInfo);
                }

                //result.ChildInfo.Add(new CalcInfo
                //{
                //    Id = "4680487",
                //    Status = (int)CalcStatusConstants.Calculating,
                //    StatusName = this.MappingCalcStatus((int)CalcStatusConstants.Completed)
                //});

                //result.ChildInfo.Add(new CalcInfo
                //{
                //    Id = "4714660",
                //    Status = (int)CalcStatusConstants.Calculating,
                //    StatusName = this.MappingCalcStatus((int)CalcStatusConstants.Completed)
                //});

                //result.ChildInfo.Add(new CalcInfo
                //{
                //    Id = "4714505",
                //    Status = (int)CalcStatusConstants.Completed,
                //    StatusName = this.MappingCalcStatus((int)CalcStatusConstants.Completed)
                //});

                //result.ChildInfo.Add(new CalcInfo
                //{
                //    Id = "4680486",
                //    Status = (int)CalcStatusConstants.Waiting,
                //    StatusName = this.MappingCalcStatus((int)CalcStatusConstants.Waiting)
                //});

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        public Settlement_ActionResult DoCalcMP(GetCalcMPStatusCriteria criteria)
        {
            try
            {
                var result = new Settlement_ActionResult();
                result.IsSuccess = true;
                result.ResultMessage = SettlementResources.ActionResult_Success_CalculateMP;

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        private string MappingCalcStatus(int statusId)
        {
            var statusName = string.Empty;
            switch (statusId)
            {
                case (int)CalcStatusConstants.Completed:
                    statusName = "Completed";
                    break;
                case (int)CalcStatusConstants.Waiting:
                    statusName = "Waiting";
                    break;
                case (int)CalcStatusConstants.Calculating:
                    statusName = "Calculating";
                    break;
            }

            return statusName;
        }
    }
}
