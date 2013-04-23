using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class CreditTermSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            CreditTermControl
        }

        [ConfigurationProperty("CreditTermControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection CreditTermControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["CreditTermControl"];
                return spCollection;
            }
        }

        public static CreditTermSession Instance
        {
            get
            {
                var creditTermSession = ConfigurationManager.GetSection("StoreProcedureGroup/CreditTerm") as CreditTermSession;
                return creditTermSession;
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
