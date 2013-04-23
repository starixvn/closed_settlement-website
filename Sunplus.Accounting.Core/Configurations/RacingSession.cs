using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class RacingSection:ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true,IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public RacingCollection Racings
        {
            get
            {
                return (RacingCollection)base[""];
            }

        }


        [ConfigurationProperty("ptSites", DefaultValue = "12bet", IsRequired = false)]
        public string PtSites
        {
            get { return (string)this["ptSites"]; }
            set { this["ptSites"] = value.ToLower(); }
        }

        [ConfigurationProperty("hideCopyBetSetting", DefaultValue = true, IsRequired = true)]
        public bool HideCopyBetSetting
        {
            get { return (bool)this["hideCopyBetSetting"]; }
            set { this["hideCopyBetSetting"] = value; }
        }

        [ConfigurationProperty("maxPT", IsRequired = true)]
        public decimal MaxPT
        {
            get { return (decimal)this["maxPT"]; }
            set { this["maxPT"] = value; }
        }

        
        public static RacingSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("RacingSection") as RacingSection;
                return spSection;
            }
        }
    }

    public class RacingCollection:ConfigurationElementCollection
    {

        public new RacingElement this[string id]
        {
            get
            {
                //if (IndexOf(id) < 0) return null;

                return (RacingElement)BaseGet(id);
            }
        }

        public RacingElement this[int index]
        {
            get { return (RacingElement)BaseGet(index); }
        }

        public int IndexOf(RacingElement racingElement)
        {
            return BaseIndexOf(racingElement);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RacingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RacingElement)element).Id;
        }

        protected override string ElementName
        {
            get { return "racing"; }
        }
        
    }

    public class RacingElement:ConfigurationElement
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

        [ConfigurationProperty("istop", DefaultValue = false, IsRequired = true, IsKey = true)]
        public bool IsTop
        {
            get
            {
                return (bool)this["istop"];
            }
            set
            {
                this["istop"] = value;
            }
        }

        [ConfigurationProperty("currency", DefaultValue = "*", IsRequired = false, IsKey = true)]
        public string Currency
        {
            get{return (string)this["currency"];}
            set{this["currency"] = value;}
        }

        [ConfigurationProperty("tote", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ToteCollection),
           AddItemName = "add",
           ClearItemsName = "clear",
           RemoveItemName = "remove")]
        public ToteCollection Totes
        {
            get { return (ToteCollection)base["tote"]; }
        }



        
    }

    public class ToteCollection : ConfigurationElementCollection
    {
        public ToteCollection()
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

        public void Add(ToteElement  toteElement)
        {
            BaseAdd(toteElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ToteElement toteElement)
        {
            if (BaseIndexOf(toteElement) >= 0)
                BaseRemove(toteElement.Id);
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


        public ToteElement this[int index]
        {
            get
            {
                return (ToteElement)BaseGet(index);
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

        new public ToteElement this[string id]
        {
            get
            {
                return (ToteElement)BaseGet(id);
            }
        }

        public int IndexOf(ToteElement storeProcedureElement)
        {
            return BaseIndexOf(storeProcedureElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ToteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ToteElement)element).Id;
        }
    }

    public class ToteElement : ConfigurationElement
    {

        public ToteElement(string id, string name, string bettypes)
        {
            this.Id = id;
            this.Name = name;
            this.BetType = bettypes;
        }

        public ToteElement()
        {
            this.Id = "0";
            this.Name = "na";
            this.BetType = "na";
        }

        [ConfigurationProperty("id", DefaultValue = "0",IsRequired = true, IsKey = true)]
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

        [ConfigurationProperty("istop", DefaultValue = false, IsRequired = true, IsKey = true)]
        public bool IsTop
        {
            get
            {
                return (bool)this["istop"];
            }
            set
            {
                this["istop"] = value;
            }
        }

        [ConfigurationProperty("name", DefaultValue = "NA",IsRequired = true)]
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

        [ConfigurationProperty("bettype", DefaultValue = "NA",IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string BetType
        {
            get
            {
                return (string)this["bettype"];
            }
            set
            {
                this["bettype"] = value;
            }
        }

        [ConfigurationProperty("currency", DefaultValue = "*", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Currency
        {
            get { return (string)this["currency"]; }
            set { this["currency"] = value; }
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
    }
}
