using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class PokerSection : ConfigurationSection
    {
        public enum PokerCollections
        {
            DefaultSetting, CustomerSetting
        }

        [ConfigurationProperty("DefaultSetting", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PokerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PokerCollection DefaultSetting
        {
            get
            {
                return (PokerCollection)base["DefaultSetting"];
            }
        }

        [ConfigurationProperty("CustomerSetting", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PokerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public PokerCollection CustomerSetting
        {
            get
            {
                return (PokerCollection)base["CustomerSetting"];
            }
        }

        public static PokerSection Instance
        {
            get
            {
                var p2pSession = ConfigurationManager.GetSection("Poker") as PokerSection;
                return p2pSession;
            }
        }

        public PokerCollection this[PokerCollections name]
        {
            get
            {
                var PokerCollection = (PokerCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return PokerCollection;
            }
        }
    }

    public class PokerCollection : ConfigurationElementCollection
    {
        public PokerCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(SettingElement settingElement)
        {
            BaseAdd(settingElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SettingElement settingElement)
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

        public SettingElement this[int index]
        {
            get
            {
                return (SettingElement)BaseGet(index);
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

        new public SettingElement this[string Name]
        {
            get
            {
                return (SettingElement)BaseGet(Name);
            }
        }

        public int IndexOf(SettingElement settingElement)
        {
            return BaseIndexOf(settingElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SettingElement)element).Name;
        }
    }

    public class SettingElement : ConfigurationElement
    {
        public SettingElement() { }

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

        [ConfigurationProperty("value", IsRequired = false)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }

        [ConfigurationProperty("min", IsRequired = false)]
        public string Min
        {
            get
            {
                return (string)this["min"];
            }
            set
            {
                this["min"] = value;
            }
        }

        [ConfigurationProperty("max", IsRequired = false)]
        public string Max
        {
            get
            {
                return (string)this["max"];
            }
            set
            {
                this["max"] = value;
            }
        }
    }
}
