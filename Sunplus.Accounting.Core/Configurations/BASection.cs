

using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class BASection : ConfigurationSection
    {
        public enum StoreCollection
        {
            Commission, Settings, CurrencyGroupSetting
        }

        [ConfigurationProperty("Commission", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BACollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public BACollection Commission
        {
            get
            {
                var spCollection =
                    (BACollection)base["Commission"];
                return spCollection;
            }
        }

        [ConfigurationProperty("CurrencyGroupSetting", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BACollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public BACollection CurrencyGroupSetting
        {
            get
            {
                var spCollection =
                    (BACollection)base["CurrencyGroupSetting"];
                return spCollection;
            }
        }

     

        public static BASection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("BAConfiguration") as BASection;
                return spSection;
            }
        }


        public BACollection this[StoreCollection name]
        {
            get
            {

                var spCollection = (BACollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }


    }

    public class BACollection : ConfigurationElementCollection
    {

        public BACollection()
        {
            //var BA = (BAElement) CreateNewElement();
            //Add(BA);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(BAElement BAElement)
        {
            BaseAdd(BAElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(BAElement BAElement)
        {
            if (BaseIndexOf(BAElement) >= 0)
                BaseRemove(BAElement.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }


        public BAElement this[int index]
        {
            get
            {
                return (BAElement)BaseGet(index);
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

        new public BAElement this[string Name]
        {
            get
            {
                return (BAElement)BaseGet(Name);
            }
        }

        public int IndexOf(BAElement BAElement)
        {
            return BaseIndexOf(BAElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new BAElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BAElement)element).Name;
        }
    }

    public class BAElement : ConfigurationElement
    {



        public BAElement()
        {

        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
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

        [ConfigurationProperty("max", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
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

        [ConfigurationProperty("min", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
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

        [ConfigurationProperty("step", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Step
        {
            get
            {
                return (string)this["step"];
            }
            set
            {
                this["step"] = value;
            }
        }

        [ConfigurationProperty("id", IsRequired = false)]
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

        [ConfigurationProperty("title", IsRequired = false)]
        public string Title
        {
            get
            {
                return (string)this["title"];
            }
            set
            {
                this["title"] = value;
            }
        }

       
        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BACollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public BACollection Stores
        {
            get
            {
                var stores =
                    (BACollection)base["stores"];
                return stores;
            }
        }

        [ConfigurationProperty("value", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BACollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public BACollection Value
        {
            get
            {
                var stores =
                    (BACollection)base["value"];
                return stores;
            }
        }
    }
}
