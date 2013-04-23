using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class HelperSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            HelperControl
        }

        [ConfigurationProperty("HelperControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection HelperControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["HelperControl"];
                return spCollection;
            }
        }

        public static HelperSession Instance
        {
            get
            {
                var helperSession = ConfigurationManager.GetSection("StoreProcedureGroup/Helper") as HelperSession;
                return helperSession;
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
