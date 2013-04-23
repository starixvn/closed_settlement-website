using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations
{
    public class UserControlSection:ConfigurationSection
    {
        public enum StoreCollection
        {
            Racing, Licensee
        }


        [ConfigurationProperty("Report", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(UserControlCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public UserControlCollection Report
        {
            get
            {
                var spCollection =
                    (UserControlCollection)base["Report"];
                return spCollection;
            }
        }
      
        
        public static UserControlSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("UserControl") as UserControlSection;
                return spSection;
            }
        }


        public UserControlCollection  this[StoreCollection name ]
        {
            get
            {
                
                var spCollection = (UserControlCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection; 
            }
        }

        
    }

    public class UserControlCollection:ConfigurationElementCollection
    {

        public UserControlCollection()
        {
            //var storeProcedure = (UserControlElement) CreateNewElement();
            //Add(storeProcedure);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(UserControlElement  userControlElement)
        {
            BaseAdd(userControlElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(UserControlElement userControlElement)
        {
            if (BaseIndexOf(userControlElement) >= 0)
                BaseRemove(userControlElement.Alias);
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


        public UserControlElement this[int index]
        {
            get
            {
                return (UserControlElement)BaseGet(index);
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

        new public UserControlElement this[string Alias]
        {
            get
            {
                return (UserControlElement)BaseGet(Alias);
            }
        }

        public int IndexOf(UserControlElement userControlElement)
        {
            return BaseIndexOf(userControlElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new UserControlElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserControlElement)element).Alias;
        }
    }

    public class UserControlElement : ConfigurationElement
    {
        


        public UserControlElement()
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

        [ConfigurationProperty("db", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string DbName
        {
            get
            {
                return (string)this["db"];
            }
            set
            {
                this["db"] = value;
            }
        }

        [ConfigurationProperty("function", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Function
        {
            get
            {
                return (string)this["function"];
            }
            set
            {
                this["function"] = value;
            }
        }

        [ConfigurationProperty("site", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Site
        {
            get
            {
                return (string)this["site"];
            }
            set
            {
                this["site"] = value;
            }
        }

        [ConfigurationProperty("Level", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Level
        {
            get
            {
                return (string)this["Level"];
            }
            set
            {
                this["Level"] = value;
            }
        }

        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(UserControlCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public UserControlCollection Stores
        {
            get
            {
                var stores =
                    (UserControlCollection)base["stores"];
                return stores;
            }
        }
    }
}
