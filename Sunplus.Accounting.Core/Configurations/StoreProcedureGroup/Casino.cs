using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class CasinoSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            CasinoControl
        }

        [ConfigurationProperty("CasinoControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection CasinoControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["CasinoControl"];
                return spCollection;
            }
        }

        public static CasinoSession Instance
        {
            get
            {
                var casinoSession = ConfigurationManager.GetSection("StoreProcedureGroup/Casino") as CasinoSession;
                return casinoSession;
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
