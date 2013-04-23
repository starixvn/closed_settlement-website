using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class MonitorSection:ConfigurationSection
    {
        
        public static MonitorSection Instance
        {
            get
            {
                var monitor = ConfigurationManager.GetSection("MonitorSection") as MonitorSection;
                return monitor;
            }
        }

        [ConfigurationProperty("LeaguesType", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MonitorCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MonitorCollection LeaguesType
        {
            get 
            {
                var spCollection =
                    (MonitorCollection)base["LeaguesType"];
                return spCollection;
            }
        }

        [ConfigurationProperty("Product", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MonitorCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MonitorCollection Product
        {
            get
            {
                var spCollection =
                    (MonitorCollection)base["Product"];
                return spCollection;
            }
        }

        [ConfigurationProperty("ReportBy", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MonitorCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MonitorCollection ReportBy
        {
            get
            {
                var spCollection =
                    (MonitorCollection)base["ReportBy"];
                return spCollection;
            }
        }
      
    }

    public class MonitorCollection : ConfigurationElementCollection
    {

        public MonitorCollection()
        {
            //var Monitor = (MonitorElement) CreateNewElement();
            //Add(Monitor);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(MonitorElement monitorElement)
        {
            BaseAdd(monitorElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(MonitorElement MonitorElement)
        {
            if (BaseIndexOf(MonitorElement) >= 0)
                BaseRemove(MonitorElement.Id);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string id)
        {
            BaseRemove(id);
        }

        public void Clear()
        {
            BaseClear();
        }


        public MonitorElement this[int index]
        {
            get
            {
                return (MonitorElement)BaseGet(index);
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

        new public MonitorElement this[string Alias]
        {
            get
            {
                return (MonitorElement)BaseGet(Alias);
            }
        }

        public int IndexOf(MonitorElement MonitorElement)
        {
            return BaseIndexOf(MonitorElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new MonitorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MonitorElement)element).Id;
        }
    }

    public class MonitorElement : ConfigurationElement
    {

        public MonitorElement()
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

        [ConfigurationProperty("name", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
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

        [ConfigurationProperty("key", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("active", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public bool Active
        {
            get
            {
                return (bool)this["active"];
            }
            set
            {
                this["active"] = value;
            }
        }
        

        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(MonitorCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public MonitorCollection Stores
        {
            get
            {
                var stores =
                    (MonitorCollection)base["stores"];
                return stores;
            }
        }
    }
}
