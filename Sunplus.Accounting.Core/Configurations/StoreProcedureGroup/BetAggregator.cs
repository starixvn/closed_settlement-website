using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class BetAggregatorSection : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            BaStores
        }

        [ConfigurationProperty("BaStores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection BaStores
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["BaStores"];
                return spCollection;
            }
        }

        public static BetAggregatorSection Instance
        {
            get
            {
                var baSession = ConfigurationManager.GetSection("StoreProcedureGroup/BetAggregator") as BetAggregatorSection;
                return baSession;
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
