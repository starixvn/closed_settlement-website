using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class SpecialDeleteSection:ConfigurationSection
    {
        public enum StoreCollection
        {
            DeleteBet, CC, RecoverBet, NotificationRealCurrency, NotificationTestCurrency, NotificationDBA
        }

        [ConfigurationProperty("DeleteBet", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection DeleteBet
        {
            get
            {
                var spCollection =
                    (SpecialDeleteCollection)base["DeleteBet"];
                return spCollection;
            }
        }


        [ConfigurationProperty("CC", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection CC
        {
            get
            {
                var spCollection =
                    (SpecialDeleteCollection)base["CC"];
                return spCollection;
            }
        }

        [ConfigurationProperty("NotificationRealCurrency", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection NotificationRealCurrency
        {
            get
            {
                var spCollection =
                    (SpecialDeleteCollection)base["NotificationRealCurrency"];
                return spCollection;
            }
        }

        [ConfigurationProperty("NotificationTestCurrency", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection NotificationTestCurrency
        {
            get
            {
                var spCollection =
                    (SpecialDeleteCollection)base["NotificationTestCurrency"];
                return spCollection;
            }
        }

        [ConfigurationProperty("RecoverBet", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection RecoverBet
        {
            get
            {
                var spCollection =
                    (SpecialDeleteCollection)base["RecoverBet"];
                return spCollection;
            }
        }

        [ConfigurationProperty("NotificationDBA", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection NotificationDBA
        {
            get
            {
                var spCollection =
                    (SpecialDeleteCollection)base["NotificationDBA"];
                return spCollection;
            }
        }
        
        public static SpecialDeleteSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("SpecialDeleteSection") as SpecialDeleteSection;
                return spSection;
            }
        }


        public SpecialDeleteCollection  this[StoreCollection name ]
        {
            get
            {
                
                var spCollection = (SpecialDeleteCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection; 
            }
        }

        
    }

    public class SpecialDeleteCollection:ConfigurationElementCollection
    {

        public SpecialDeleteCollection()
        {
            //var SpecialDelete = (SpecialDeleteElement) CreateNewElement();
            //Add(SpecialDelete);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(SpecialDeleteElement  SpecialDeleteElement)
        {
            BaseAdd(SpecialDeleteElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(SpecialDeleteElement SpecialDeleteElement)
        {
            if (BaseIndexOf(SpecialDeleteElement) >= 0)
                BaseRemove(SpecialDeleteElement.Alias);
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


        public SpecialDeleteElement this[int index]
        {
            get
            {
                return (SpecialDeleteElement)BaseGet(index);
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

        new public SpecialDeleteElement this[string Alias]
        {
            get
            {
                return (SpecialDeleteElement)BaseGet(Alias);
            }
        }

        public int IndexOf(SpecialDeleteElement SpecialDeleteElement)
        {
            return BaseIndexOf(SpecialDeleteElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new SpecialDeleteElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SpecialDeleteElement)element).Alias;
        }




    }

    public class SpecialDeleteElement : ConfigurationElement
    {
        


        public SpecialDeleteElement()
        {

        }

        [ConfigurationProperty("alias", IsRequired = true, IsKey = true)]
        public string Alias
        {
            get
            {
                return (string)this["alias"];
            }
            set
            {
                this["alias"] = value;
            }
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

        [ConfigurationProperty("to", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string To
        {
            get
            {
                return (string)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }

        [ConfigurationProperty("cc", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string CC
        {
            get
            {
                return (string)this["cc"];
            }
            set
            {
                this["cc"] = value;
            }
        }

        [ConfigurationProperty("bcc", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string BCC
        {
            get
            {
                return (string)this["bcc"];
            }
            set
            {
                this["bcc"] = value;
            }
        }

        [ConfigurationProperty("DeleteoOtionDefault", IsRequired = false)] //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string DeleteoOtionDefault
        {
            get
            {
                return (string)this["DeleteoOtionDefault"];
            }
            set
            {
                this["DeleteoOtionDefault"] = value;
            }
        }

        [ConfigurationProperty("IsAllowOptionDelete", IsRequired = false)] //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string IsAllowOptionDelete
        {
            get
            {
                return (string)this["IsAllowOptionDelete"];
            }
            set
            {
                this["IsAllowOptionDelete"] = value;
            }
        }

      

        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SpecialDeleteCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public SpecialDeleteCollection Stores
        {
            get
            {
                var stores =
                    (SpecialDeleteCollection)base["stores"];
                return stores;
            }
        }
    }
}
