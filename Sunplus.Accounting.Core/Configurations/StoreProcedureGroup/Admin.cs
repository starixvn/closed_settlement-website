using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class AdminSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            ExternalSite
        }

        [ConfigurationProperty("ExternalSite", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection ExternalSite
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["ExternalSite"];
                return spCollection;
            }
        }

        public static AdminSession Instance
        {
            get
            {
                var adminSession = ConfigurationManager.GetSection("StoreProcedureGroup/Admin") as AdminSession;
                return adminSession;
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
