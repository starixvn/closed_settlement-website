using System.Configuration;
using DNAide.Core;
using DNAide.Utils;

namespace Sunplus.Accounting.Core.Configurations
{
    /// <summary>
    /// Summary description for MailSetting
    /// </summary>
    public class NumberGameSection : ConfigurationSection
    {
        public enum NumberGameCollectionType
        {
            Bettype, PTSetting
        }
        public enum BallCollectionType 
        { 
            Deadball, Liveball
        }
        
        public enum Level
        {
            Super, Master, Agent, Member, SuperOnMember, MasterOnMember, MinMaxPT
        }

        [ConfigurationProperty("Bettype", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NumberGameCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public NumberGameCollection Bettype
        {
            get
            {
                var spCollection =
                    (NumberGameCollection)base["Bettype"];
                return spCollection;
            }
        }

        [ConfigurationProperty("PTSetting", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NumberGameCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public NumberGameCollection PTSetting
        {
            get
            {
                var spCollection =
                    (NumberGameCollection)base["PTSetting"];
                return spCollection;
            }
        }


        public static NumberGameSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("NumberGameSection") as NumberGameSection;
                return spSection;
            }
        }


        public NumberGameCollection this[NumberGameCollectionType name]
        {
            get
            {

                var spCollection = (NumberGameCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }
    }


    public class NumberGameCollection : ConfigurationElementCollection
    {

        public NumberGameCollection()
        {
            //var storeProcedure = (StoreProcedureElement) CreateNewElement();
            //Add(storeProcedure);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(NumberGameElement numberGameElement)
        {
            BaseAdd(numberGameElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(NumberGameElement numberGameElement)
        {
            if (BaseIndexOf(numberGameElement) >= 0)
                BaseRemove(numberGameElement.Name);
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


        public NumberGameElement this[int index]
        {
            get
            {
                return (NumberGameElement)BaseGet(index);
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

        new public NumberGameElement this[string Name]
        {
            get
            {
                return (NumberGameElement)BaseGet(Name);
            }
        }

        public int IndexOf(NumberGameElement numberGameElement)
        {
            return BaseIndexOf(numberGameElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new NumberGameElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NumberGameElement)element).Name;
        }
    }

    public class NumberGameElement : ConfigurationElement
    {
        public NumberGameElement()
        {

        }


        [ConfigurationProperty("name", IsRequired = false)]
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

        [ConfigurationProperty("id", IsRequired = false, DefaultValue = 1)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public int Id
        {
            get
            {
                return (int)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("active", IsRequired = false, DefaultValue = true)]
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

        [ConfigurationProperty("displayname", IsRequired = false, DefaultValue = "")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string DisplayName
        {
            get
            {
                return (string)this["displayname"];
            }
            set
            {
                this["displayname"] = value;
            }
        }

        [ConfigurationProperty("blockname", IsRequired = false, DefaultValue = "")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string BlockName
        {
            get
            {
                return (string)this["blockname"];
            }
            set
            {
                this["blockname"] = value;
            }
        }

        [ConfigurationProperty("liveball", IsRequired = false, DefaultValue = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public bool Liveball
        {
            get
            {
                return (bool)this["liveball"];
            }
            set
            {
                this["liveball"] = value;
            }
        }

        [ConfigurationProperty("tagvalue", IsRequired = false, DefaultValue = "")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string TagValue
        {
            get
            {
                return (string)this["tagvalue"];
            }
            set
            {
                this["tagvalue"] = value;
            }
        }

        [ConfigurationProperty("key", IsRequired = false, DefaultValue = "")]
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

        [ConfigurationProperty("edittag", IsRequired = false, DefaultValue = "")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string EditTag
        {
            get
            {
                return (string)this["edittag"];
            }
            set
            {
                this["edittag"] = value;
            }
        }

        [ConfigurationProperty("eidtkey", IsRequired = false, DefaultValue = "")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string EidtKey
        {
            get
            {
                return (string)this["eidtkey"];
            }
            set
            {
                this["eidtkey"] = value;
            }
        }

        [ConfigurationProperty("isdefault", IsRequired = false, DefaultValue = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public bool IsDefault
        {
            get
            {
                return (bool)this["isdefault"];
            }
            set
            {
                this["isdefault"] = value;
            }
        }

        [ConfigurationProperty("issetting", IsRequired = false, DefaultValue = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public bool IsSetting
        {
            get
            {
                return (bool)this["issetting"];
            }
            set
            {
                this["issetting"] = value;
            }
        }

        [ConfigurationProperty("Settings", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NumberGameCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public NumberGameCollection Settings
        {
            get
            {
                var stores =
                    (NumberGameCollection)base["Settings"];
                return stores;
            }
        }
    }
}
