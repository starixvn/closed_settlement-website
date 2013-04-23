using System;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.DAO;

namespace Sunplus.Data.DataAbstract.Log
{
    public class Error
    {   
        public static void WriteLog(Exception ex)
        {
            //Write log
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            var identify = (long)span.TotalMilliseconds;
            var message = ex.Source + ":" + ex.StackTrace + ":" + ex.Message;
            //(new LogDAO()).Insert<LogItem>(new LogItem {Id = identify, Type = LogType.Error, Message =  message});
            Console.WriteLine(message);
        }
    }
}
