using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class SportbookPtSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            SportBooksPtControl
        }

        [ConfigurationProperty("SportBooksPtControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection SportBooksPtControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["SportBooksPtControl"];
                return spCollection;
            }
        }

        public static SportbookPtSession Instance
        {
            get
            {
                var sportbookPtSession = ConfigurationManager.GetSection("StoreProcedureGroup/SportbookPt") as SportbookPtSession;
                return sportbookPtSession;
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
