using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Configuration;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.Core.Configurations
{
    public class TransactionSection : ConfigurationSection
    {
        public enum StoreCollection
        {
            Transaction
        }

        [ConfigurationProperty("Transaction", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(BACollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public TransactionCollection Transaction
        {
            get
            {
                var spCollection =
                    (TransactionCollection)base["Transaction"];
                return spCollection;
            }
        }



        public static TransactionSection Instance
        {
            get
            {
                var spSection =
                 ConfigurationManager.GetSection("Transaction") as TransactionSection;
                return spSection;
            }
        }


        public TransactionCollection this[StoreCollection name]
        {
            get
            {

                var spCollection = (TransactionCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }


    }

    public class TransactionCollection : ConfigurationElementCollection
    {

        public TransactionCollection()
        {
            //var BA = (BAElement) CreateNewElement();
            //Add(BA);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public void Add(TransactionElement transactionElement)
        {
            BaseAdd(transactionElement);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(TransactionElement BAElement)
        {
            if (BaseIndexOf(BAElement) >= 0)
                BaseRemove(BAElement.Name);
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


        public TransactionElement this[int index]
        {
            get
            {
                return (TransactionElement)BaseGet(index);
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

        new public TransactionElement this[string Name]
        {
            get
            {
                return (TransactionElement)BaseGet(Name);
            }
        }

        public int IndexOf(TransactionElement transactionElement)
        {
            return BaseIndexOf(transactionElement);
        }


        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new TransactionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TransactionElement)element).Name;
        }
    }

    public class TransactionElement : ConfigurationElement
    {



        public TransactionElement()
        {

        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
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

        [ConfigurationProperty("max", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Max
        {
            get
            {
                return (string)this["max"];
            }
            set
            {
                this["max"] = value;
            }
        }

        [ConfigurationProperty("min", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Min
        {
            get
            {
                return (string)this["min"];
            }
            set
            {
                this["min"] = value;
            }
        }

        [ConfigurationProperty("step", IsRequired = true)]
        //[RegexStringValidator(@"\w+:\/\/[\w.]+\S*")]
        public string Step
        {
            get
            {
                return (string)this["step"];
            }
            set
            {
                this["step"] = value;
            }
        }


        [ConfigurationProperty("stores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(TransactionCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public TransactionCollection Stores
        {
            get
            {
                var stores =
                    (TransactionCollection)base["stores"];
                return stores;
            }
        }


    }
}
