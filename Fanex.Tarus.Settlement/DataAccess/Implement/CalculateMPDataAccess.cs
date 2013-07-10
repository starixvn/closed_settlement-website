// --------------------------------------------------------------------------
// <copyright file="CalculateMPDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Settlement.DataAccess.Implement
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Settlement.Criteria;
    using Fanex.Tarus.Settlement.DataAccess.Interface;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CalculateMPDataAccess : ICalculateMPDataAccess
    {
        public List<dynamic> GetCalcMPStatus(GetCalcMPStatusCriteria criteria)
        {
            var dataTable = new DataTable();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var ds = objDb.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            var result = new List<dynamic>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new
                    {
                        matchid = CastAide.AsInt32(row["matchid"]),
                        CalcMP = CastAide.AsBoolean(row["CalcMP"])
                    });
                }
            }

            return result;
        }
    }
}
