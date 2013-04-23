using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.Storage.Cache.MemTable
{
    public class ExchangeMem
    {
        private Manager _MemClient = null;
        private string _identify = "exchange_d_{0}";
        public ExchangeMem() 
        {
            _MemClient = new Manager();
        }

        public Exchange this[int exchangeId] 
        {
            get 
            {
                return _MemClient.Get<Exchange>(string.Format(_identify, exchangeId.ToString())); 
            }
            set 
            {
                if (value == null)
                {
                    var key = string.Format(_identify, exchangeId.ToString());
                    _MemClient.Delete<Exchange>(key, null);
                }
                else
                {
                    _MemClient.Insert<Exchange>(string.Format(_identify, exchangeId.ToString()), value);
                }
            }
        }
        ~ExchangeMem() { _MemClient = null; }
    }
}
