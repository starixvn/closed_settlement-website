using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Entities.Report
{
    [Serializable]
    public class LiveCasinoWinLoss
    {
        public List<LiveCasinoWinLossDTO> TotalItemByReportType { get; set; }

        public int CountItems { get; set; }

        public int RoleID { get; set; }

        public List<LiveCasinoWinLossDTO> TotalCurrency { get; set; }

        public List<LiveCasinoWinLossDTO> GrandTotalCurrency { get; set; }
    }

    [Serializable]
    public class LiveCasinoWinLossParams
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string MoveBetDay { get; set; }

        public string UserName { get; set; }

        public bool ExcludeTest { get; set; }

        public int SiteID { get; set; }

        public int GameType { get; set; }

        public int GameTable { get; set; }

        public int Currency { get; set; }

        public int ReportBy { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool FromCache { get; set; }

        public string isAgency { get; set; }

        public Sorting Sort { get; set; }
    }

    [Serializable]
    public class LiveCasinoWinLossDTO
    {
        public string ReportType { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public string GameType { get; set; }

        public string GameName { get; set; }

        public string RollingTO { get; set; }

        public string CompanyTO { get; set; }

        public string GrossComm { get; set; }

        public string TotalBets { get; set; }

        public string MemberWinLoss { get; set; }

        public string MemberComm { get; set; }

        public string MemberTotal { get; set; }

        public string AgentComm { get; set; }

        public string AgentWinLoss { get; set; }

        public string AgentToltal { get; set; }

        public string MasterWinLoss { get; set; }

        public string MasterComm { get; set; }

        public string MasterTotal { get; set; }

        public string SuperWinLoss { get; set; }

        public string SuperComm { get; set; }

        public string SuperTotal { get; set; }

        public string Company { get; set; }

        public List<LiveCasinoWinLossDTO> Items { get; set; }
    }

    [Serializable]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        public bool Active { get; set; }

        public bool Selected { get; set; }
    }

    [Serializable]
    public class GameSearch
    {
        public List<Category> Company { get; set; }

        public List<Category> GameType { get; set; }

        public List<Category> GameTable { get; set; }

        public List<Category> ReportType { get; set; }

        public List<Category> Currency { get; set; }
    }

    [Serializable]
    public class Sorting
    {
        public string SortType { get; set; }

        public string SortCol { get; set; }

        public bool IsDecimal { get; set; }
        public bool IsDate { get; set; }
    }
}
