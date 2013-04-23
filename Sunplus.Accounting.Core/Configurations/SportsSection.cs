using System;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class SportsSection : ConfigurationSection
    {
        [ConfigurationProperty("Sports", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BACollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SportsCollection Sports
        {
            get
            {
                var sportsCollection =
                    (SportsCollection)base["Sports"];
                return sportsCollection;
            }
        }

        public static SportsSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("SportsSection") as SportsSection;
                return spSection;
            }
        }
    }

    public class SportsCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(SportsElement sportsElement)
        {
            BaseAdd(sportsElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SportsElement sportsElement)
        {
            if (BaseIndexOf(sportsElement) >= 0)
                BaseRemove(sportsElement.Name);
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


        public SportsElement this[int index]
        {
            get
            {
                return (SportsElement)BaseGet(index);
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

        public new SportsElement this[string name]
        {
            get
            {
                return (SportsElement)BaseGet(name);
            }
        }

        public int IndexOf(SportsElement sportsElement)
        {
            return BaseIndexOf(sportsElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new SportsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SportsElement)element).Name;
        }
    }

    public class SportsElement : ConfigurationElement
    {

        [ConfigurationProperty("id", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public string Id
        {
            get { return (string)this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("name", DefaultValue = "NA", IsRequired = true, IsKey = false)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("active", DefaultValue = true, IsRequired = true, IsKey = true)]
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

        [ConfigurationProperty("minbet", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public String Minbet
        {
            get
            {
                return (String)this["minbet"];
            }
            set
            {
                this["minbet"] = value;
            }
        }

        [ConfigurationProperty("maxbet", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public String Maxbet
        {
            get
            {
                return (String)this["maxbet"];
            }
            set
            {
                this["maxbet"] = value;
            }
        }

        [ConfigurationProperty("maxpermatch", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public String Maxpermatch
        {
            get
            {
                return (String)this["maxpermatch"];
            }
            set
            {
                this["maxpermatch"] = value;
            }
        }

        [ConfigurationProperty("bettype", DefaultValue = "", IsRequired = true, IsKey = true)]
        public String Bettype
        {
            get
            {
                var bettypes = (String)this["bettype"];
                return string.IsNullOrWhiteSpace(bettypes) ? string.Empty : bettypes.Trim();
            }
            set
            {
                this["bettype"] = value;
            }
        }
    }
}
