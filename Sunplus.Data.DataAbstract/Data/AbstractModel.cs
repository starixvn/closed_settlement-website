using System;
using System.Runtime.Serialization;

namespace Sunplus.Data.DataAbstract.Data
{
    [Serializable]
    [DataContract]
    public class AbstractModel
    {
        protected Storage.Cache.Manager Caching;
        protected Storage.NoSQL.INoSQL storage;

        public AbstractModel(string modelName)
        {
            Caching = new Storage.Cache.Manager();
            /*switch(modelName) 
            {
                case "message":
                    storage = Storage.NoSQL.Abstract.Message();
                    break;

                case "bettran":
                    storage = Storage.NoSQL.Abstract.Bettrans();
                    break;
                 

                default:
                    break;
            }*/
            
        }
    }
}
