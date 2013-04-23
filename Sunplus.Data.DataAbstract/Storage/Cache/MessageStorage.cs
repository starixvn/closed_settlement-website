using System;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Sunplus.Accounting.DataAbstract.Entity;

namespace Sunplus.Accounting.DataAbstract.Data
{
    [Serializable]
    [DataContract]
    public class MessageStorage : AbstractModel, IDataModel
    {
        #region private variable
        private string _keyDetail = "message-d{0}";
        private string _keyList100 = "message-list-100";
        private string _keyListAll = "message-list-all";
        private string _keyListDiff = "message-list-diff";
        private string _keyListRealTime   = "message-list-realtime";
        private string _modelName = "message";
        #endregion private variable

        #region public properties
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string Date { get; set; }
        #endregion public properties


        #region constructor
        public MessageStorage() : base("message")
        {
        }
        #endregion constructor

        #region public method
        public object Get(int mid)
        {
            return this.GetItem(mid);
        }

        public bool Insert(string key, object message)
        {
            return this.Insert(key, null, false);
        }

        public bool Insert(string key, MessageStorage message, bool fromNoSQL = false)
        {
            bool inserted = false;
            if (message != null)
            {
                var keyCache = string.Format(_keyDetail, key);
                inserted = Caching.Insert(keyCache, JsonConvert.SerializeObject(message));
                if (!fromNoSQL)
                {
                    storage.Insert(key, message);
                }
            }
            return inserted;
        }

        public bool Delete(int mid)
        {
            var keyCache = string.Format(_keyDetail, mid.ToString());
            var deleted = Caching.Delete<Message>(keyCache, null);
            //delete from NoSQL
            if (deleted)
            {
                storage.Delete<Message>(mid.ToString(), null);
            }
            return deleted;
        }

        // adding public method
        public MessageStorage GetItem(int mid)
        {
            var keyCache = string.Format(_keyDetail, mid.ToString());
            var message = Caching.Get<string>(keyCache);
            if (message == null)
            {
                var messageNoSQL = storage.Get<MessageStorage>(mid.ToString());
                if (messageNoSQL != null)
                {
                    this.Insert(mid.ToString(), messageNoSQL, true);
                }
            }
            if (message != null)
            {
                return JsonConvert.DeserializeObject<MessageStorage>(message);
            }
            return null;
        }

        public List<MessageStorage> GetList()
        {
           List<MessageStorage> listItem = new List<MessageStorage>();

            var listTmp = Caching.Get<List<int>>(_keyListRealTime);

            if (listTmp != null)
            {
                foreach (var ID in listTmp)
                {
                    listItem.Add(this.GetItem(ID));
                }
            }

            return listItem;
        }

        public void SetRealTime(List<int> IDs)
        {
            Caching.Insert(_keyListRealTime, IDs);
        }
        #endregion public method

        ~MessageStorage() { }
    }
}
