// --------------------------------------------------------------------------
// <copyright file="SportConstants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.ResultPage
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using DNAide.Core;
    using DNAide.Web;
    using Sunplus.Accounting.Business.BootStrappers;
    using Sunplus.Accounting.Business.Settle;
    using Sunplus.Accounting.Business.Settlement;
    using Sunplus.Accounting.Core.Cache;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HorseRacingFixedOdds
    {
        public static string GenerateHTML(
            int sportId,
            string sportName,
            DataRow[] rows,
            string fromDate,
            CultureInfo currentCulture)
        {
            StringBuilder mainContentResult = new StringBuilder();

            string mainContentTemplate = WebUtils.LoadTemplate("Settle/HorseRacingFixedOdds/MainContent.htm");
            string mainRowTemplate = WebUtils.LoadTemplate("Settle/HorseRacingFixedOdds/MainRow.htm");
            string leagueContentBlock = TemplateHandler.CopyBlock(mainContentTemplate, "leaguename");

            TemplateHandler.RemoveBlock(ref mainContentTemplate, "leaguename");
            TemplateHandler.ReplaceBlock(ref leagueContentBlock, "btnRef", "&nbsp;");
            string currentLeagueId = "";
            string backgroundClass = " rowEven";
            foreach (DataRow dataRow in rows)
            {
                backgroundClass = (backgroundClass == " rowEven") ? string.Empty : " rowEven";
                string mainRow = mainRowTemplate;
                string leagueId = dataRow["leagueid"].ToString();
                if (!currentLeagueId.Equals(leagueId))
                {
                    currentLeagueId = leagueId;
                    string leagueContent = leagueContentBlock;
                    TemplateHandler.Assign(ref leagueContent, "{leaguename}", CacheRef.GetLeagueName(dataRow["leagueid"]));
                    backgroundClass = string.Empty;
                    mainContentResult.Append(leagueContent);
                }

                TemplateHandler.Assign(ref mainRow, "{bgClass}", backgroundClass);
                TemplateHandler.Assign(ref mainRow, "{code}", dataRow["matchcode"].ToString());

                var homeTeamInfo = CacheRef.GetTeamName(dataRow["homeid"]) +
                    (CastAide.AsBoolean(dataRow["isNeutral"]) ?
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") :
                        string.Empty);
                TemplateHandler.Assign(ref mainRow, "{hometeam}", homeTeamInfo);
                TemplateHandler.Assign(ref mainRow, "{awayteam}", CacheRef.GetTeamName(dataRow["awayid"]));

                string kickOffTime = string.Format(currentCulture, "{0:MM/dd/yy hh:mm:ss tt}", dataRow["kickofftime"]);
                TemplateHandler.Assign(ref mainRow, "{date}", kickOffTime);
                TemplateHandler.Assign(ref mainRow, "{homeHT}", (!Convert.IsDBNull(dataRow["hthomescore"])) ? CastAide.AsInt32(dataRow["hthomescore"]).ToString() : "-");
                TemplateHandler.Assign(ref mainRow, "{homeFT}", (!Convert.IsDBNull(dataRow["finalhomescore"])) ? CastAide.AsInt32(dataRow["finalhomescore"]).ToString() : "-");
                TemplateHandler.Assign(ref mainRow, "{awayHT}", (!Convert.IsDBNull(dataRow["htawayscore"])) ? CastAide.AsInt32(dataRow["htawayscore"]).ToString() : "-");
                TemplateHandler.Assign(ref mainRow, "{awayFT}", (!Convert.IsDBNull(dataRow["finalawayscore"])) ? CastAide.AsInt32(dataRow["finalawayscore"]).ToString() : "-");
                string eventstatus = dataRow["eventstatus"].ToString();

                var isPending = false;
                var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                TemplateHandler.Assign(ref mainRow, "{eventstatus}", settle.SelStatusConfirming(dataRow, ref isPending));

                var eventstatusClass = string.Compare(eventstatus, "postponed", true) == 0 ?
                    SettleBgColor.POSTPONED :
                    (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR);
                TemplateHandler.Assign(ref mainRow, "{bgEventStatus}", eventstatusClass);

                bool isRefund = Convert.ToBoolean(dataRow["IsRefund"]);
                bool isCalRefund = Convert.ToBoolean(dataRow["IsCalRefund"]);
                bool isCalculated = Convert.ToBoolean(dataRow["IsCalculated"]);
                bool isMultiCaled = Convert.ToBoolean(dataRow["IsMultiCaled"]);
                mainContentResult.Append(mainRow);
            }

            TemplateHandler.Assign(ref mainContentTemplate, "{fdate}", fromDate);
            TemplateHandler.Assign(ref mainContentTemplate, "{content}", mainContentResult.ToString());
            TemplateHandler.Assign(ref mainContentTemplate, "{sportname}", sportName.ToUpperInvariant());
            return mainContentTemplate;
        }
    }
}
