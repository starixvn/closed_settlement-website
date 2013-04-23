using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class StoreProcedureGroup : System.Configuration.ConfigurationSectionGroup
    {
        #region Constructors
        public StoreProcedureGroup()
        {
        }
        #endregion

        #region Properties
        [ConfigurationProperty("Helper", IsDefaultCollection = false)]
        public HelperSession Helper
        {
            get { return (HelperSession)base.Sections["Helper"]; }
        }

        [ConfigurationProperty("Admin", IsDefaultCollection = false)]
        public AdminSession Admin
        {
            get { return (AdminSession)base.Sections["example"]; }
        }

        [ConfigurationProperty("Casino", IsDefaultCollection = false)]
        public CasinoSession Casino
        {
            get { return (CasinoSession)base.Sections["Casino"]; }
        }

        [ConfigurationProperty("CreditTerm", IsDefaultCollection = false)]
        public CreditTermSession CreditTerm
        {
            get { return (CreditTermSession)base.Sections["CreditTerm"]; }
        }

        [ConfigurationProperty("Customer", IsDefaultCollection = false)]
        public CustomerSession Customer
        {
            get { return (CustomerSession)base.Sections["Customer"]; }
        }

        [ConfigurationProperty("ExchangeRate", IsDefaultCollection = false)]
        public ExchangeRateSession ExchangeRate
        {
            get { return (ExchangeRateSession)base.Sections["ExchangeRate"]; }
        }

        [ConfigurationProperty("Racing", IsDefaultCollection = false)]
        public RacingSession Racing
        {
            get { return (RacingSession)base.Sections["Racing"]; }
        }

        [ConfigurationProperty("Report", IsDefaultCollection = false)]
        public ReportSession Report
        {
            get { return (ReportSession)base.Sections["Report"]; }
        }

        [ConfigurationProperty("Settlement", IsDefaultCollection = false)]
        public SettlementSession Settlement
        {
            get { return (SettlementSession)base.Sections["Settlement"]; }
        }

        [ConfigurationProperty("SportbookPt", IsDefaultCollection = false)]
        public SportbookPtSession SportbookPt
        {
            get { return (SportbookPtSession)base.Sections["SportbookPt"]; }
        }

        [ConfigurationProperty("ViewPt", IsDefaultCollection = false)]
        public ViewPtSession ViewPt
        {
            get { return (ViewPtSession)base.Sections["ViewPt"]; }
        }
        #endregion
    }
}
