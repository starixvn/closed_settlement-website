using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class ViewPtSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            ViewPtControl
        }

        [ConfigurationProperty("ViewPtControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection ViewPtControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["ViewPtControl"];
                return spCollection;
            }
        }

        public static ViewPtSession Instance
        {
            get
            {
                var viewPtSession = ConfigurationManager.GetSection("StoreProcedureGroup/ViewPt") as ViewPtSession;
                return viewPtSession;
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
