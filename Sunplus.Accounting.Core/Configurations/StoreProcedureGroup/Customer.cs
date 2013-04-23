using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class CustomerSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            CustomerControl, ConsolidatedGroup
        }

        [ConfigurationProperty("CustomerControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection CustomerControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["CustomerControl"];
                return spCollection;
            }
        }

        [ConfigurationProperty("ConsolidatedGroup", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection ConsolidatedGroup
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["ConsolidatedGroup"];
                return spCollection;
            }
        }

        public static CustomerSession Instance
        {
            get
            {
                var customerSession = ConfigurationManager.GetSection("StoreProcedureGroup/Customer") as CustomerSession;
                return customerSession;
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
