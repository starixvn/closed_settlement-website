using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.Storage.Cache.MemTable;
using DNAide.Core;

namespace Sunplus.Data.DataAbstract.Business
{
    public class ExchangeManager
    {
        public ExchangeManager() { }

        public Exchange GetExchangeDetail(string exchangeId)
        {
            Exchange exchange = null;
            var objMem = new ExchangeMem();
            exchange = objMem[CastAide.AsInt32(exchangeId)];
            return exchange;
        }

        public bool Insert(Exchange data)
        {
            if (data != null)
            {
                var exchangeId = CastAide.AsInt32(data.Exchangeid);
                (new ExchangeMem())[exchangeId] = data;
                return true;
            }
            return false;
        }

        public bool Update(Exchange data)
        {
            if (data != null)
            {
                var exchangeId = CastAide.AsInt32(data.Exchangeid);
                (new ExchangeMem())[exchangeId] = data;
                return true;
            }
            return false;
        }

        public bool Delete(object data)
        {
            if (data != null)
            {
                var exchangeId = CastAide.AsInt32(data);
                (new ExchangeMem())[exchangeId] = null;
                return true;
            }
            return false;
        }
    }
}
