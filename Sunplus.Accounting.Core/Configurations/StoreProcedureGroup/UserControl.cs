using System;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class UserControlSession : ConfigurationSection
    {
        public enum StoreCollection
        {
            Policy, AccountInfo, Login
        }

        [ConfigurationProperty("Policy", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Policy
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Policy"];
                return spCollection;
            }
        }

        [ConfigurationProperty("AccountInfo", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection AccountInfo
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["AccountInfo"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Login", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Login
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Login"];
                return spCollection;
            }
        }

        public static UserControlSession Instance
        {
            get
            {
                var section = ConfigurationManager.GetSection("StoreProcedureGroup/UserControl") as UserControlSession;
                return section;
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
