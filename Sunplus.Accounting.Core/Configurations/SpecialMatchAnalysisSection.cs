
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class SpecialMatchAnalysisSection : ConfigurationSection
    {
        public enum SpecialMatchAnalysisCollectionType
        {
            Currency
        }

        [ConfigurationProperty("Currency", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialMatchAnalysisCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialMatchAnalysisCollection Currency
        {
            get
            {
                var spCollection = (SpecialMatchAnalysisCollection)base["Currency"];
                return spCollection;
            }
        }

        [ConfigurationProperty("DisplayText", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialMatchAnalysisCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialMatchAnalysisCollection DisplayText
        {
            get
            {
                var spCollection = (SpecialMatchAnalysisCollection)base["DisplayText"];
                return spCollection;
            }
        }


        public static SpecialMatchAnalysisSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("SpecialMatchAnalysisSection") as SpecialMatchAnalysisSection;
                return spSection;
            }
        }

        public SpecialMatchAnalysisCollection this[SpecialMatchAnalysisCollectionType name]
        {
            get
            {

                var spCollection = (SpecialMatchAnalysisCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }

    }

    public class SpecialMatchAnalysisCollection : ConfigurationElementCollection
    {

        public SpecialMatchAnalysisCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(SpecialMatchAnalysisElement specialMatchAnalysisElement)
        {
            BaseAdd(specialMatchAnalysisElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SpecialMatchAnalysisElement specialMatchAnalysisElement)
        {
            if (BaseIndexOf(specialMatchAnalysisElement) >= 0)
                BaseRemove(specialMatchAnalysisElement.Name);
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


        public SpecialMatchAnalysisElement this[int index]
        {
            get
            {
                return (SpecialMatchAnalysisElement)BaseGet(index);
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

        new public SpecialMatchAnalysisElement this[string Name]
        {
            get
            {
                return (SpecialMatchAnalysisElement)BaseGet(Name);
            }
        }

        public int IndexOf(SpecialMatchAnalysisElement specialMatchAnalysisElement)
        {
            return BaseIndexOf(specialMatchAnalysisElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new SpecialMatchAnalysisElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SpecialMatchAnalysisElement)element).Name;
        }
    }

    public class SpecialMatchAnalysisElement : ConfigurationElement
    {
        public SpecialMatchAnalysisElement()
        {

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

        [ConfigurationProperty("id", IsRequired = false)]
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

        [ConfigurationProperty("value", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
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

    }
}
