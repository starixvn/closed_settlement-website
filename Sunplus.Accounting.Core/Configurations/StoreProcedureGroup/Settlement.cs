using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class SettlementSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            SettlementControl
        }

        [ConfigurationProperty("SettlementControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection SettlementControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["SettlementControl"];
                return spCollection;
            }
        }

        public static SettlementSession Instance
        {
            get
            {
                var settlementSession = ConfigurationManager.GetSection("StoreProcedureGroup/Settlement") as SettlementSession;
                return settlementSession;
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
