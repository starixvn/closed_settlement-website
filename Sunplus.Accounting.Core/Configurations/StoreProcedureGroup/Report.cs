using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class ReportSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            Main, WinLoss, Racing, Gaming, Licensee, Others, External, Trader, Balance, DailyTurnover, ResetWinning
        }

        [ConfigurationProperty("Main", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Main
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Main"];
                return spCollection;
            }
        }

        [ConfigurationProperty("WinLoss", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection WinLoss
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["WinLoss"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Racing", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Racing
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Racing"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Gaming", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Gaming
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Gaming"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Licensee", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Licensee
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Licensee"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Others", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Others
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Others"];
                return spCollection;
            }
        }

        [ConfigurationProperty("External", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection External
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["External"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Trader", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Trader
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Trader"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Balance", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Balance
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Balance"];
                return spCollection;
            }
        }


        [ConfigurationProperty("DailyTurnover", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection DailyTurnover
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["DailyTurnover"];
                return spCollection;
            }
        }


        [ConfigurationProperty("ResetWinning", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection ResetWinning
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["ResetWinning"];
                return spCollection;
            }
        }
        public static ReportSession Instance
        {
            get
            {
                var reportSession = ConfigurationManager.GetSection("StoreProcedureGroup/Report") as ReportSession;
                return reportSession;
            }
        }


        public StoreProcedureCollection this[StoreCollection name]
        {
            get
            {
                var spCollection = (StoreProcedureCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }
    }
}
