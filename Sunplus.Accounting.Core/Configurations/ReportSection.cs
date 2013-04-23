using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class ReportSection : ConfigurationSection
    {
        public enum ReportCollectionType
        {
            NumberGame
        }


        [ConfigurationProperty("NumberGame", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ReportCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ReportCollection NumberGame
        {
            get
            {
                var spCollection =
                    (ReportCollection)base["NumberGame"];
                return spCollection;
            }
        }


      

        public static ReportSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("ReportSection") as ReportSection;
                return spSection;
            }
        }


        public ReportCollection this[ReportCollectionType name]
        {
            get
            {

                var spCollection = (ReportCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }


    }

    public class ReportCollection : ConfigurationElementCollection
    {

        public ReportCollection()
        {
            //var storeProcedure = (ReportElement) CreateNewElement();
            //Add(storeProcedure);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(ReportElement ReportElement)
        {
            BaseAdd(ReportElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ReportElement ReportElement)
        {
            if (BaseIndexOf(ReportElement) >= 0)
                BaseRemove(ReportElement.Name);
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


        public ReportElement this[int index]
        {
            get
            {
                return (ReportElement)BaseGet(index);
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

        new public ReportElement this[string Name]
        {
            get
            {
                return (ReportElement)BaseGet(Name);
            }
        }

        public int IndexOf(ReportElement ReportElement)
        {
            return BaseIndexOf(ReportElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ReportElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReportElement)element).Name;
        }
    }

    public class ReportElement : ConfigurationElement
    {



        public ReportElement()
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

        [ConfigurationProperty("items", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ReportCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ReportCollection Items
        {
            get
            {
                var items =
                    (ReportCollection)base["items"];
                return items;
            }
        }

    }
}
