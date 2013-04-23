using System.Configuration;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.Core.Configurations
{
    public class IBCAccounting : ConfigurationSection
    {
        

        [ConfigurationProperty("Casino", IsRequired = true)]
        public CasinoElements Casino
        {
            get { return (CasinoElements) base["Casino"]; }
        }

        [ConfigurationProperty("IBCCustomer", IsRequired = true)]
        public IBCCustomerElements IBCCustomer
        {
            get { return (IBCCustomerElements) base["IBCCustomer"]; }
        }

        [ConfigurationProperty("PositionTaking", IsRequired = true)]
        public PositionTakingElement PositionTaking
        {
            get { return (PositionTakingElement) base["PositionTaking"]; }
        }

        [ConfigurationProperty("Utils", IsRequired = false)]
        public UtilsElements Utils
        {
            get { return (UtilsElements) base["Utils"]; }
        }

        [ConfigurationProperty("Report", IsRequired = false)]
        public ReportElements Report
        {
            get { return (ReportElements) base["Report"]; }
        }

        [ConfigurationProperty("Betlist", IsRequired = false)]
        public BetlistElements Betlist
        {
            get { return (BetlistElements) base["Betlist"]; }
        }

        [ConfigurationProperty("Customer", IsRequired = false)]
        public CustomerElements Customer
        {
            get { return (CustomerElements) base["Customer"]; }
        }

        [ConfigurationProperty("SpecialDelete", IsRequired = true)]
        public SpecialDeleteElements SpecialDelete
        {
            get { return (SpecialDeleteElements) base["SpecialDelete"]; }
        }

        [ConfigurationProperty("CustSetting", IsRequired = true)]
        public CustSettingElements CustSetting
        {
            get { return (CustSettingElements) base["CustSetting"]; }
        }

        [ConfigurationProperty("Settle", IsRequired = true)]
        public SettleElements Settle
        {
            get { return (SettleElements) base["Settle"]; }
        }

        [ConfigurationProperty("ExChangeRate", IsRequired = true)]
        public ExChangeRateElements ExChangeRate
        {
            get { return (ExChangeRateElements) base["ExChangeRate"]; }
        }

        [ConfigurationProperty("News", IsRequired = true)]
        public NewsElements News
        {
            get { return (NewsElements) base["News"]; }
        }

        [ConfigurationProperty("ExportStatement", IsRequired = true)]
        public ExportStatementElements ExportStatement
        {
            get { return (ExportStatementElements) base["ExportStatement"]; }
        }
    }

    public class CasinoElements : ConfigurationElement
    {
        private string _ProviderType;
        private bool _UseWebConfigProviderType = true;

        public bool UseWebConfigProviderType
        {
            get { return _UseWebConfigProviderType; }
            set { _UseWebConfigProviderType = value; }
        }


        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.APIClient.APICasinoProvider")]
        public string ProviderType
        {
            get { return (_UseWebConfigProviderType) ? (string) base["providerType"] : _ProviderType; }
            set
            {
                if (_UseWebConfigProviderType)
                    base["providerType"] = value;
                else
                    _ProviderType = value;
            }
        }

        [ConfigurationProperty("showBetSetting", DefaultValue = false)]
        public bool ShowBetSetting
        {
            get { return true; // CastAide.AsBoolean(base["showBetSetting"], false);
            }
            set { base["showBetSetting"] = value; }
        }
    }

    public class IBCCustomerElements : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider")]
        public string ProviderType
        {
            get { return (string) base["providerType"]; }
            set { base["providerType"] = value; }
        }
    }

    public class PositionTakingElement : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.IBCClient.PositionTakingProviderClient")]
        public string ProviderType
        {
            get { return (string) base["providerType"]; }
            set { base["providerType"] = value; }
        }

        [ConfigurationProperty("providerTypeHR", DefaultValue = "Sunplus.Accounting.DataAccess.IBCClient.HRPositionTakingProviderClient")]
        public string ProviderTypeHR
        {
            get { return (string) base["providerTypeHR"]; }
            set { base["providerTypeHR"] = value; }
        }

        [ConfigurationProperty("sportBookProvider", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.PositionTaking.SportBookClient")]
        public string SportBookProvider
        {
            get { return (string) base["sportBookProvider"]; }
            set { base["sportBookProvider"] = value; }
        }
    }

    public class UtilsElements : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.IBCClient.UtilsProviderClient")]
        public string ProviderType
        {
            get { return (string) base["providerType"]; }
            set { base["providerType"] = value; }
        }
    }


    /// <summary>
    /// Report config section
    /// </summary>
    public class ReportElements : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.Report.SqlClient.ReportProviderClient")]
        public string ProviderType
        {
            get { return (string) base["providerType"]; }
            set { base["providerType"] = value; }
        }

        /// <summary>
        /// {1}: DAO Client: AdoNet, Linq, WCF
        /// {2}: Data Source: SqlServer, DB2, Access
        /// </summary>
        [ConfigurationProperty("LeagueWLProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.LeagueWLProviderClient")]
        public string LeagueWLProviderType
        {
            get { return (string) base["LeagueWLProviderType"]; }
            set { base["LeagueWLProviderType"] = value; }
        }

        [ConfigurationProperty("WLProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.WLProviderClient")]
        public string WLProviderType
        {
            get { return (string) base["WLProviderType"]; }
            set { base["WLProviderType"] = value; }
        }

        [ConfigurationProperty("MatchWLProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.MatchWLProviderClient")]
        public string MatchWLProviderType
        {
            get { return (string) base["MatchWLProviderType"]; }
            set { base["MatchWLProviderType"] = value; }
        }

        [ConfigurationProperty("FIWLProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.FIWLProviderClient")]
        public string FIWLProviderType
        {
            get { return (string) base["FIWLProviderType"]; }
            set { base["FIWLProviderType"] = value; }
        }

        [ConfigurationProperty("PunterAnalysisProviderType",
            DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.PunterAnalysisProviderClient")]
        public string PunterAnalysisProviderType
        {
            get { return (string) base["PunterAnalysisProviderType"]; }
            set { base["PunterAnalysisProviderType"] = value; }
        }

        [ConfigurationProperty("OpenToteSettlementProviderType",
            DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.OpenToteSettlementProviderClient")]
        public string OpenToteSettlementProviderType
        {
            get { return (string) base["OpenToteSettlementProviderType"]; }
            set { base["OpenToteSettlementType"] = value; }
        }

        [ConfigurationProperty("HRGroupDailyProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.HRGroupDailyProviderClient")]
        public string HRGroupDailyProviderType
        {
            get
            {
                return (string)base["HRGroupDailyProviderType"];
            }
            set
            {
                base["HRGroupDailyProviderType"] = value;
            }
        }

        [ConfigurationProperty("NonePTProvider", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.NonePTProviderClient")]
        public string NonePTProvider
        {
            get
            {
                return (string)base["NonePTProvider"];
            }
            set
            {
                base["NonePTProvider"] = value;
            }
        }

        [ConfigurationProperty("DirectBalanceProvider", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.DirectBalanceProviderClient")]
        public string DirectBalanceProvider
        {
            get
            {
                return (string)base["DirectBalanceProvider"];
            }
            set
            {
                base["DirectBalanceProvider"] = value;
            }
        }

        [ConfigurationProperty("DownLineBalanceProvider", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.DownLineBalanceProviderClient")]
        public string DownLineBalanceProvider
        {
            get
            {
                return (string)base["DownLineBalanceProvider"];
            }
            set
            {
                base["DownLineBalanceProvider"] = value;
            }
        }

        [ConfigurationProperty("NonePTMemSummaryProvider", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Report.NonePTMemSummaryProviderClient")]
        public string NonePTMemSummaryProvider
        {
            get
            {
                return (string)base["NonePTMemSummaryProvider"];
            }
            set
            {
                base["NonePTMemSummaryProvider"] = value;
            }
        }
    }

    public class BetlistElements : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.Betlist.SqlClient.BetlistProviderClient")]
        public string ProviderType
        {
            get
            {
                return (string)base["providerType"];
            }
            set
            {
                base["providerType"] = value;
            }
        }
    }

    public class CustomerElements : ConfigurationElement
    {
        [ConfigurationProperty("providerType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Customer.CustomerClient")]
        public string ProviderType
        {
            get
            {
                return (string)base["providerType"];
            }
            set
            {
                base["providerType"] = value;
            }
        }
    }

    public class SpecialDeleteElements : ConfigurationElement
    {
        [ConfigurationProperty("SpecialDeleteProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.SpecialDelete.SpecialDeleteProviderClient")]
        public string SpecialDeleteProviderType
        {
            get
            {
                return (string)base["SpecialDeleteProviderType"];
            }
            set
            {
                base["SpecialDeleteProviderType"] = value;
            }
        }
    }

    public class CustSettingElements : ConfigurationElement
    {
        [ConfigurationProperty("CustSettingProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.SpecialDelete.CustSettingProviderClient")]
        public string CustSettingProviderType
        {
            get
            {
                return (string)base["CustSettingProviderType"];
            }
            set
            {
                base["CustSettingProviderType"] = value;
            }
        }
    }

    public class SettleElements : ConfigurationElement
    {
        [ConfigurationProperty("SettleProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Settle.SettleProviderClient")]
        public string SettleProviderType
        {
            get
            {
                return (string)base["SettleProviderType"];
            }
            set
            {
                base["SettleProviderType"] = value;
            }
        }
    }

    public class ExChangeRateElements : ConfigurationElement
    {
        [ConfigurationProperty("ExChangeRateProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.ExchangeRate.ExChangeRateProviderClient")]
        public string ExChangeRateProviderType
        {
            get
            {
                return (string)base["ExChangeRateProviderType"];
            }
            set
            {
                base["ExChangeRateProviderType"] = value;
            }
        }
    }
    public class NewsElements : ConfigurationElement
    {
        [ConfigurationProperty("NewsProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.Others.NewsProviderClient")]
        public string NewsProviderType
        {
            get
            {
                return (string)base["NewsProviderType"];
            }
            set
            {
                base["NewsProviderType"] = value;
            }
        }
    }

    public class ExportStatementElements : ConfigurationElement
    {
        [ConfigurationProperty("ExportStatementProviderType", DefaultValue = "Sunplus.Accounting.DataAccess.{0}.{1}.ExportStatement.ExportStatementProviderClient")]
        public string ExportStatementProviderType
        {
            get
            {
                return (string)base["ExportStatementProviderType"];
            }
            set
            {
                base["ExportStatementProviderType"] = value;
            }
        }
    }


   
}

