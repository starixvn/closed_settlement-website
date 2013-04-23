using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class ExternalSection : ConfigurationSection
    {
        public enum ExternalCollections
        {
            Sorting
        }

        [ConfigurationProperty("Sorting", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof (ExternalCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ExternalCollection Sorting
        {
            get { return (ExternalCollection) base["Sorting"]; }
        }

        public static ExternalSection Instance
        {
            get
            {
                var externalSession = ConfigurationManager.GetSection("ExternalSection") as ExternalSection;
                return externalSession;
            }
        }

        public ExternalCollection this[ExternalCollections name]
        {
            get
            {
                var externalCollection =
                    (ExternalCollection)
                    this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField,
                                                                         null, null, null);
                return externalCollection;
            }
        }
    }

    public class ExternalCollection : ConfigurationElementCollection
    {
        public ExternalCollection()
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

        public ExternalElement this[int index]
        {
            get
            {
                return (ExternalElement)BaseGet(index);
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

        new public ExternalElement this[string Name]
        {
            get
            {
                return (ExternalElement)BaseGet(Name);
            }
        }

        public int IndexOf(ExternalElement settingElement)
        {
            return BaseIndexOf(settingElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ExternalElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExternalElement)element).Name;
        }
    }

    public class ExternalElement : ConfigurationElement
    {
        public ExternalElement() { }

        [ConfigurationProperty("name", IsRequired = true)]
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

        [ConfigurationProperty("users", IsRequired = false)]
        public string Users
        {
            get
            {
                return (string)this["users"];
            }
            set
            {
                this["users"] = value;
            }
        }

        [ConfigurationProperty("index", IsRequired = false)]
        public int Index
        {
            get
            {
                return (int)this["index"];
            }
            set
            {
                this["index"] = value;
            }
        }
    }

}
