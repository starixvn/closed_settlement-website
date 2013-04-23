using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class TransactionGroupSection : ConfigurationSection
    {
        public enum StoreCollection
        {
            DailyTxn, Bettrans
        }

        [ConfigurationProperty("DailyTxn", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection DailyTxn
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["DailyTxn"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Bettrans", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Bettrans
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Bettrans"];
                return spCollection;
            }
        }    

        public static TransactionGroupSection Instance
        {
            get
            {
                var section = ConfigurationManager.GetSection("StoreProcedureGroup/Transaction") as TransactionGroupSection;
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
