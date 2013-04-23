using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class HelperSection : ConfigurationSection
    {
        public enum HelperCollectionType
        {
            Currency
        }

       
        [ConfigurationProperty("Currency", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HelperCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public HelperCollection Currency
        {
            get
            {
                var spCollection =
                    (HelperCollection)base["Currency"];
                return spCollection;
            }
        }


      

        public static HelperSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("HelperSection") as HelperSection;
                return spSection;
            }
        }


        public HelperCollection this[HelperCollectionType name]
        {
            get
            {

                var spCollection = (HelperCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }


    }

    public class HelperCollection : ConfigurationElementCollection
    {

        public HelperCollection()
        {
            //var storeProcedure = (HelperElement) CreateNewElement();
            //Add(storeProcedure);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(HelperElement HelperElement)
        {
            BaseAdd(HelperElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(HelperElement HelperElement)
        {
            if (BaseIndexOf(HelperElement) >= 0)
                BaseRemove(HelperElement.Name);
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


        public HelperElement this[int index]
        {
            get
            {
                return (HelperElement)BaseGet(index);
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

        new public HelperElement this[string Name]
        {
            get
            {
                return (HelperElement)BaseGet(Name);
            }
        }

        public int IndexOf(HelperElement HelperElement)
        {
            return BaseIndexOf(HelperElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new HelperElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HelperElement)element).Name;
        }
    }

    public class HelperElement : ConfigurationElement
    {



        public HelperElement()
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
        [ConfigurationCollection(typeof(HelperCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public HelperCollection Items
        {
            get
            {
                var items =
                    (HelperCollection)base["items"];
                return items;
            }
        }

    }
}
