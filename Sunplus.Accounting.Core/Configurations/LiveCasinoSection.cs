using System.Collections.Generic;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class LiveCasinoSection : ConfigurationSection
    {
        [ConfigurationProperty("Report", IsDefaultCollection = true)]
        public LiveCasinoCollection Report
        {
            get { return (LiveCasinoCollection)this["Report"]; }
        }

        public static LiveCasinoSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("LiveCasinoSection") as LiveCasinoSection;
            }
        }
    }

    [ConfigurationCollection(typeof(LiveCasinoTemplate), AddItemName = "Items")]
    public class LiveCasinoCollection : ConfigurationElementCollection
    {
        public new LiveCasinoTemplate this[string level]
        {
            get { return (LiveCasinoTemplate)BaseGet(level); }
        }

        public LiveCasinoTemplate this[int index]
        {
            get { return (LiveCasinoTemplate)BaseGet(index); }
        }

        public int IndexOf(LiveCasinoTemplate LiveCasinoTemplate)
        {
            return BaseIndexOf(LiveCasinoTemplate);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LiveCasinoTemplate();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LiveCasinoTemplate)element).Name;
        }

        protected override string ElementName
        {
            get { return "Items"; }
        }
    }

    public class LiveCasinoTemplate : ConfigurationElement
    {
        
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }


        [ConfigurationProperty("GameSearch", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(FilterCollection),
           AddItemName = "action",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public FilterCollection GameSearch
        {
            get { return (FilterCollection)this["GameSearch"]; }
        }

        [ConfigurationProperty("ReportType", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(FilterCollection),
           AddItemName = "action",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public FilterCollection ReportType
        {
            get { return (FilterCollection)this["ReportType"]; }
        }

    }

    [ConfigurationCollection(typeof(FilterElement), AddItemName = "filter")]
    public class FilterCollection : ConfigurationElementCollection
    {
        public FilterElement this[int id]
        {
            get { return (FilterElement)BaseGet(id); }
        }

        public int IndexOf(FilterElement FilterElement)
        {
            return BaseIndexOf(FilterElement);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FilterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilterElement)element).Id;
        }

        protected override string ElementName
        {
            get { return "filter"; }
        }
    }
    public class FilterElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public int Id
        {
            get { return (int)this["id"]; }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("active", DefaultValue = false, IsRequired = false)]
        public bool Active
        {
            get { return (bool)this["active"]; }
        }

        [ConfigurationProperty("filtertype", DefaultValue = "", IsRequired = false)]
        public string FilterType
        {
            get { return this["filtertype"].ToString(); }
        }

        [ConfigurationProperty("parent", DefaultValue = "", IsRequired = false)]
        public string Parent
        {
            get { return this["parent"].ToString(); }
        }

        [ConfigurationProperty("selected", DefaultValue = false, IsRequired = false)]
        public bool Selected
        {
            get { return (bool)this["selected"]; }
        }
    }
}
