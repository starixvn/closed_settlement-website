using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class CustomerSection : ConfigurationSection
    {
        public enum CustomerCollections
        {
            Currencies, Sites
        }

        [ConfigurationProperty("Currencies", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CustomerCollection Currencies
        {
            get
            {
                return (CustomerCollection)base["Currencies"];
            }
        }

        [ConfigurationProperty("Sites", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CustomerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CustomerCollection Sites
        {
            get
            {
                return (CustomerCollection)base["Sites"];
            }
        }

        public static CustomerSection Instance
        {
            get
            {
                var customerSession = ConfigurationManager.GetSection("CustomerSection") as CustomerSection;
                return customerSession;
            }
        }

        public CustomerCollection this[CustomerCollections name]
        {
            get
            {
                var UserGroupCollection = (CustomerCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return UserGroupCollection;
            }
        }
    }

    public class CustomerCollection : ConfigurationElementCollection
    {
        public CustomerCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(CustomerElement settingElement)
        {
            BaseAdd(settingElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(CustomerElement settingElement)
        {
            if (BaseIndexOf(settingElement) >= 0)
                BaseRemove(settingElement.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        public CustomerElement this[int index]
        {
            get
            {
                return (CustomerElement)BaseGet(index);
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

        new public CustomerElement this[string Name]
        {
            get
            {
                return (CustomerElement)BaseGet(Name);
            }
        }

        public int IndexOf(CustomerElement settingElement)
        {
            return BaseIndexOf(settingElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new CustomerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomerElement)element).Name;
        }
    }

    public class CustomerElement : ConfigurationElement
    {
        public CustomerElement() { }

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

        [ConfigurationProperty("group", IsRequired = false)]
        public string Group
        {
            get
            {
                return (string)this["group"];
            }
            set
            {
                this["group"] = value;
            }
        }
    }
}
