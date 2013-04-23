using System;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for Logs
    /// writer: viet.hoang
    /// </summary>
    public static class Logs
    {
        public static void Write(string msg)
        {
            Write(msg, TraceEventType.Verbose);
        }

        public static void Write(string msg, TraceEventType type)
        {
            try
            {
                LogExtensions.ExceptionHandler(new Exception(msg));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
