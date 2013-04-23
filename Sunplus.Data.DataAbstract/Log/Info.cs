using System;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.DAO;

namespace Sunplus.Data.DataAbstract.Log
{
    public class Info
    {
        public static void WriteLog(string message)
        {
            //Write log
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            var identify = (long)span.TotalMilliseconds;
            //(new LogDAO()).Insert<LogItem>(new LogItem { Id = identify, Type = LogType.Info, Message = message });
            Console.WriteLine(message);
        }

        public static void NotifyInfo(string message)
        {
            //Write log
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            var identify = (long)span.TotalMilliseconds;
            //(new LogDAO()).Insert<LogItem>(new LogItem { Id = identify, Type = LogType.Notify, Message = message });
            Console.WriteLine(message);
        }
    }
}
