using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class SpecialDeleteGroupSection : ConfigurationSection
    {
        public enum StoreCollection
        {
            EarlyKO, Sportbook, NumberGame, Mixparlay, TicketsListing
        }

        [ConfigurationProperty("EarlyKO", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection EarlyKO
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["EarlyKO"];
                return spCollection;
            }
        }

        [ConfigurationProperty("TicketsListing", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection TicketsListing
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["TicketsListing"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Sportbook", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Sportbook
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Sportbook"];
                return spCollection;
            }
        }

        [ConfigurationProperty("NumberGame", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection NumberGame
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["NumberGame"];
                return spCollection;
            }
        }
        [ConfigurationProperty("Mixparlay", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection Mixparlay
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["Mixparlay"];
                return spCollection;
            }
        }
        
        public static SpecialDeleteGroupSection Instance
        {
            get
            {
                var section = ConfigurationManager.GetSection("StoreProcedureGroup/SpecialDelete") as SpecialDeleteGroupSection;
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
