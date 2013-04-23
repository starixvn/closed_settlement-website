using System;
using Sunplus.Data.DataAbstract.Storage;
using Sunplus.Data.DataAbstract.BootStrappers;

namespace Sunplus.Data.DataAbstract.DAO
{
    public class LogDAO : DataAccess
    {
        public LogDAO():base()
        {
        }

        public void Insert<T>(T data)
        {
            DataStorage.Insert<T>(string.Empty, data);
        }
    }
}
