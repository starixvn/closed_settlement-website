using System.Linq;
using System.Configuration;

namespace Sunplus.Data.DataAbstract.Configurations
{
    public static class StorageConfiguration
    {
        public static bool Caching()
        {
            return StorageSection.Instance.IsCaching;
        }

        public static string CachingType()
        {
            return StorageSection.Instance.CachingType;
        }

        public static StorageSection.NoSQLType NoSQLType()
        {
            return StorageSection.Instance.NoSQLInstant;
        }

        public static NoSQLElement NoSQL()
        {
            return StorageSection.Instance[NoSQLType()];
        }
    }

    public class StorageSection : ConfigurationSection
    {
        public enum StorageType
        {
            Caching, NoSQL
        }

        public enum NoSQLType
        {
            MongoDB, Cassandra, Membase
        }

        [ConfigurationProperty("IsCaching", IsDefaultCollection = false, DefaultValue = true)]
        public bool IsCaching
        {
            get { return (bool)base["IsCaching"]; }
        }

        [ConfigurationProperty("Caching", IsDefaultCollection = false, DefaultValue = "memcached")]
        public string CachingType
        {
            get { return (string)base["Caching"]; }
        }

        [ConfigurationProperty("NoSQLType", IsDefaultCollection = false, DefaultValue = "MongoDB")]
        public NoSQLType NoSQLInstant
        {
            get { return (NoSQLType)base["NoSQLType"]; }
        }
        

        [ConfigurationProperty("NoSQL", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NoSQLCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public NoSQLCollection NoSQL
        {
            get
            {
                var noSQLCollection =
                    (NoSQLCollection)base["NoSQL"];
                return noSQLCollection;
            }
        }

        public static StorageSection Instance
        {
            get
            {
                var storageSection =
                 ConfigurationManager.GetSection("StorageSection") as StorageSection;
                return storageSection;
            }
        }


        public NoSQLElement this[NoSQLType name]
        {
            get
            {
                var NoSQLElement = NoSQL.Cast<NoSQLElement>().Where(e => e.Type.ToString() == name.ToString()).Single<NoSQLElement>();
                return NoSQLElement;
            }
        }

    }

    public class NoSQLCollection : ConfigurationElementCollection
    {

        public NoSQLCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(NoSQLElement NoSQLElement)
        {
            BaseAdd(NoSQLElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(NoSQLElement NoSQLElement)
        {
            if (BaseIndexOf(NoSQLElement) >= 0)
                BaseRemove(NoSQLElement.Type);
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


        public NoSQLElement this[int index]
        {
            get
            {
                return (NoSQLElement)BaseGet(index);
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

        public int IndexOf(NoSQLElement NoSQLElement)
        {
            return BaseIndexOf(NoSQLElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new NoSQLElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NoSQLElement)element).Type;
        }
    }

    public class NoSQLElement : ConfigurationElement
    {

        public NoSQLElement()
        {

        }

        [ConfigurationProperty("dbname", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string DbName
        {
            get
            {
                return (string)this["dbname"];
            }
            set
            {
                this["dbname"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        [ConfigurationProperty("host", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("option", IsRequired = false, DefaultValue = "")]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Option
        {
            get
            {
                return (string)this["option"];
            }
            set
            {
                this["option"] = value;
            }
        }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 25)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("username", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }


        [ConfigurationProperty("password", IsRequired = false)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }        
    }
}
