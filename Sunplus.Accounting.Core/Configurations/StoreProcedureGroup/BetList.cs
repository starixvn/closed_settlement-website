using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class BetListSection : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            BetListStores
        }

        [ConfigurationProperty("BetListStores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection BetListStores
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["BetListStores"];
                return spCollection;
            }
        }

        public static BetListSection Instance
        {
            get
            {
                var betListSession = ConfigurationManager.GetSection("StoreProcedureGroup/BetList") as BetListSection;
                return betListSession;
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
