
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fanex.Tarus.Entities.Report
{
    [Serializable]    
    public class MatchAnalysis
    {        
        public string ItemName { get; set; }        
        public string MemberTO { get; set; }        
        public string MemberWL { get; set; }
        public string BetCount { get; set; }
        public string HouseTO { get; set; }
        public string HouseWL { get; set; }
        public string SMATO { get; set; }
        public string SMAWL { get; set; }
        public string Currency { get; set; }
    }

    [Serializable]   
    public class MatchAnalysisDTO
    {        
        public List<MatchAnalysis> Items { get; set; }        
        public MatchAnalysis Total { get; set; }
    }

    public enum ReportType
    {
        Currency = 0,
        Bettype = 1,
        Super = 2,
        Member = 3,
        Country = 4
    }
    
    public class MatchAnalysisParam
    {
        public string EventDate { get; set; }

        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public int Liveindicator { get; set; }

        public string Bettypes { get; set; }

        public string MoveBetsDay { get; set; }

        public string BalanceUpDay { get; set; }

        public int StatusId { get; set; }

        public int ReportType { get; set; }

        public string LeagueIds { get; set; }

        public int LeagueId { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public string ShowTime { get; set; }
        public int SportTypeId { get; set; }
    }

}
