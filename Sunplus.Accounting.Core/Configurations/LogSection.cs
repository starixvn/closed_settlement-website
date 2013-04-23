using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class LogSection : ConfigurationSection
    {
        [ConfigurationProperty("LogType", IsDefaultCollection = true)]
        public LogTypeCollection LogType
        {
            get { return (LogTypeCollection)this["LogType"]; }
        }        

        [ConfigurationProperty("TrackingTemplate", IsDefaultCollection = true)]
        public TrackingCollection Tracking
        {
            get { return (TrackingCollection)this["TrackingTemplate"]; }
        }        

        [ConfigurationProperty("Groups", IsDefaultCollection=true)]
        public GroupCollection Groups
        {
            get { return (GroupCollection)this["Groups"]; }
        }

        [ConfigurationProperty("LogFormater", IsDefaultCollection = true)]
        public ParamCollection Formater
        {
            get { return (ParamCollection)this["LogFormater"]; }
        }

        public TrackingTemplate DefaultTracking
        {
            get { return (TrackingTemplate)this.Tracking.Cast<TrackingTemplate>().Where(tracking => tracking.IsDefault==true).SingleOrDefault<TrackingTemplate>(); }
        }

        public static LogSection Instance
        {
            get
            {
                 return ConfigurationManager.GetSection("LogSection") as LogSection;
            }
        }
    }

    #region Tracking Configuration
    [ConfigurationCollection(typeof(LogTypeElement), AddItemName = "type")]
    public class LogTypeCollection : ConfigurationElementCollection
    {
        public new LogTypeElement this[string name]
        {
            get { return (LogTypeElement)BaseGet(name); }
        }

        public LogTypeElement this[int index]
        {
            get { return (LogTypeElement)BaseGet(index); }
        }

        public int IndexOf(LogTypeElement element)
        {
            return BaseIndexOf(element);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LogTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogTypeElement)element).Id;
        }

        protected override string ElementName
        {
            get { return "type"; }
        }
    }
    public class LogTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public string Id
        {
            get { return (string)this["id"]; }
        }
        [ConfigurationProperty("name", DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("isTracking", DefaultValue = false, IsRequired = false)]
        public bool IsTracking
        {
            get { return (bool)this["isTracking"]; }
        }       
    }
    [ConfigurationCollection(typeof(TrackingTemplate), AddItemName = "tracking")]
    public class TrackingCollection : ConfigurationElementCollection
    {
        public new TrackingTemplate this[string level]
        {
            get { return (TrackingTemplate)BaseGet(level); }
        }

        public TrackingTemplate this[int index]
        {
            get { return (TrackingTemplate)BaseGet(index); }
        }

        public int IndexOf(TrackingTemplate TrackingTemplate)
        {
            return BaseIndexOf(TrackingTemplate);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TrackingTemplate();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TrackingTemplate)element).Level;
        }

        protected override string ElementName
        {
            get { return "tracking"; }
        }
    }
    public class TrackingTemplate : ConfigurationElement
    {
        [ConfigurationProperty("level", DefaultValue = "0", IsRequired = true, IsKey = true)]
        public int Level
        {
            get { return (int)this["level"]; }
        }
        
        [ConfigurationProperty("action", DefaultValue = "*", IsRequired = true)]
        public string Action
        {
            get { return (string)this["action"]; }
        }

        [ConfigurationProperty("hasGroup", DefaultValue = false, IsRequired = false)]
        public bool HasGroup
        {
            get { return (bool)this["hasGroup"]; }
        }

        [ConfigurationProperty("groupDefault", DefaultValue = "", IsRequired = false)]
        public string GroupDefault
        {
            get { return (string)this["groupDefault"]; }
        }

        [ConfigurationProperty("isDefault", DefaultValue = false, IsRequired = false)]
        public bool IsDefault
        {
            get { return (bool)this["isDefault"]; }
        }

        [ConfigurationProperty("hiddenDefault", DefaultValue = "", IsRequired = false)]
        public string HiddenDefault
        {
            get { return (string)this["hiddenDefault"]; }
        }

        
        [ConfigurationProperty("filters", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ActionCollection),
           AddItemName = "action",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public ActionCollection Filters
        {
            get { return (ActionCollection)this["filters"]; }
        }

        [ConfigurationProperty("formater", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ParamCollection),
           AddItemName = "param",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public ParamCollection Formater
        {
            get { return (ParamCollection)this["formater"]; }
        }

        [ConfigurationProperty("params", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ParamCollection),
           AddItemName = "param",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public ParamCollection Params
        {
            get { return (ParamCollection)this["params"]; }
        }

        [ConfigurationProperty("information", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(InformationCollection),
           AddItemName = "info",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public InformationCollection Information
        {
            get { return (InformationCollection)this["information"]; }
        }
    }
    [ConfigurationCollection(typeof(ActionElement), AddItemName = "action")]
    public class ActionCollection : ConfigurationElementCollection
    {
        public ActionElement this[int id]
        {
            get { return (ActionElement)BaseGet(id); }
        }

        public int IndexOf(ActionElement actionElement)
        {
            return BaseIndexOf(actionElement);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ActionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ActionElement)element).Id;
        }

        protected override string ElementName
        {
            get { return "action"; }
        }
    }
    public class ActionElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public int Id
        {
            get { return (int)this["id"]; }
        }

        [ConfigurationProperty("hidden", DefaultValue = "", IsRequired = false)]
        public string Hidden
        {
            get { return this["hidden"].ToString(); }
        }

        [ConfigurationProperty("groupName", DefaultValue="", IsRequired=false)]
        public string GroupName
        {
            get { return this["groupName"].ToString(); }
        }

        [ConfigurationProperty("prefix", DefaultValue = "")]
        public string Prefix
        {
            get { return this["prefix"].ToString(); }
        }

        [ConfigurationProperty("regx", DefaultValue = "")]
        public string Regx
        {
            get { return this["regx"].ToString(); }
        }
    }

    [ConfigurationCollection(typeof(ParamElement), AddItemName = "param")]
    public class ParamCollection : ConfigurationElementCollection
    {
        public ParamElement this[string name]
        {
            get { return (ParamElement)BaseGet(name); }
        }

        public int IndexOf(ParamElement element)
        {
            return BaseIndexOf(element);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ParamElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParamElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "param"; }
        }
    }
    public class ParamElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"].ToString(); }
        }
    }

    [ConfigurationCollection(typeof(ParamElement), AddItemName = "info")]
    public class InformationCollection : ConfigurationElementCollection
    {
        public InfoElement this[string name]
        {
            get { return (InfoElement)BaseGet(name); }
        }

        public int IndexOf(InfoElement element)
        {
            return BaseIndexOf(element);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new InfoElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InfoElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "info"; }
        }
    }
    public class InfoElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("params", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ParamCollection),
           AddItemName = "param",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public ParamCollection Params
        {
            get { return (ParamCollection)this["params"]; }
        }

        [ConfigurationProperty("groupSport", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(GroupSportCollection),
           AddItemName = "sport",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public GroupSportCollection GroupSport
        {
            get { return (GroupSportCollection)this["groupSport"]; }
        }
    }

    [ConfigurationCollection(typeof(ParamElement), AddItemName = "sport")]
    public class GroupSportCollection : ConfigurationElementCollection
    {
        public SportInfoElement this[string name]
        {
            get { return (SportInfoElement)BaseGet(name); }
        }

        public int IndexOf(SportInfoElement element)
        {
            return BaseIndexOf(element);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SportInfoElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SportInfoElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "sport"; }
        }
    }
    public class SportInfoElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("params", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ParamCollection),
           AddItemName = "param",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public ParamCollection Params
        {
            get { return (ParamCollection)this["params"]; }
        }
    }
    #endregion Tracking Configuration

    #region Group Configuration
    [ConfigurationCollection(typeof(GroupElement), AddItemName = "group")]
    public class GroupCollection : ConfigurationElementCollection
    {
        public GroupElement this[string name]
        {
            get { return (GroupElement)BaseGet(name); }
        }

        public GroupElement this[int index]
        {
            get { return (GroupElement)BaseGet(index); }
        }

        public int IndexOf(GroupElement groupElement)
        {
            return BaseIndexOf(groupElement);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GroupElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GroupElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "group"; }
        }
    }
    public class GroupElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }

        [ConfigurationProperty("sports", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SportCollection),
           AddItemName = "add",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public SportCollection Sports
        {
            get { return (SportCollection)this["sports"]; }
        }
    }
    [ConfigurationCollection(typeof(SportElement), AddItemName = "add")]
    public class SportCollection : ConfigurationElementCollection
    {
        public SportElement this[string name]
        {
            get { return (SportElement)BaseGet(name); }
        }

        public SportElement this[int index]
        {
            get { return (SportElement)BaseGet(index); }
        }

        public int IndexOf(SportElement groupElement)
        {
            return BaseIndexOf(groupElement);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SportElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SportElement)element).Id;
        }

        protected override string ElementName
        {
            get { return "add"; }
        }
    }
    public class SportElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public int Id
        {
            get { return (int)this["id"]; }
        }

        [ConfigurationProperty("label", IsRequired = true)]
        public string Label
        {
            get { return this["label"].ToString(); }
        }
    }
    #endregion Group Configuration
    
}
