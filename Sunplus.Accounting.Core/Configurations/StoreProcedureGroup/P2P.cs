using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class P2PSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            P2PControl,
            P2PReports
        }

        [ConfigurationProperty("P2PControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection P2PControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["P2PControl"];
                return spCollection;
            }
        }

         [ConfigurationProperty("P2PReports", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection P2PReports
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["P2PReports"];
                return spCollection;
            }
        }


        public static P2PSession Instance
        {
            get
            {
                var p2pSession = ConfigurationManager.GetSection("StoreProcedureGroup/P2P") as P2PSession;
                return p2pSession;
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
