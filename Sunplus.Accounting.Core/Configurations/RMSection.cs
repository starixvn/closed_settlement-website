using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class RMSection:ConfigurationSection
    {
        
        public static RMSection Instance
        {
            get
            {
                var RM = ConfigurationManager.GetSection("RMSection") as RMSection;
                return RM;
            }
        }


        [ConfigurationProperty("Report", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RMCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public RMCollection Report
        {
            get
            {
                var spCollection =
                    (RMCollection)base["Report"];
                return spCollection;
            }
        }
      
    }

    public class RMCollection : ConfigurationElementCollection
    {

        public RMCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public RMElement this[int index]
        {
            get
            {
                return (RMElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public RMElement this[string Alias]
        {
            get
            {
                return (RMElement)BaseGet(Alias);
            }
        }

        public int IndexOf(RMElement RMElement)
        {
            return BaseIndexOf(RMElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new RMElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RMElement)element).Id;
        }
    }

    public class RMElement : ConfigurationElement
    {

        public RMElement()
        {

        }

        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }



        [ConfigurationProperty("isActive", IsRequired = false)]
        public bool IsActive
        {
            get
            {
                return (bool)this["isActive"];
            }
            set
            {
                this["isActive"] = value;
            }
        }

        [ConfigurationProperty("displayName", IsRequired = false)]
        public string DisplayName
        {
            get
            {
                return (string)this["displayName"];
            }
            set
            {
                this["displayName"] = value;
            }
        }
        

        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RMCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public RMCollection Stores
        {
            get
            {
                var stores =
                    (RMCollection)base["stores"];
                return stores;
            }
        }
    }
}
