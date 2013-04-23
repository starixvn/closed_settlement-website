using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Norm;

namespace Sunplus.Data.DataAbstract.Entity
{
    public enum LogType
    {
        Info = 1,
        Error,
        Notify
    }

    [DataContract]
    public class LogItem
    {
        [MongoIdentifier]
        public long Id { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
    }
}
