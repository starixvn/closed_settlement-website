//using Sunplus.Core.LoggingLib;

namespace Fanex.Tarus.Infrastructure.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Sunplus.Accounting.Core.Extensions;

    /// <summary>
    /// Summary description for Settings
    /// </summary>
    public static class AppSettings
    {
        
        //private static bool _useNewSpecialDelete;
        static AppSettings()
        {
            try
            {

              // _calendarTheme = ConfigurationManager.AppSettings["UMCtrlUrl"];

            }
            catch (Exception ex)
            {
                
                {
                    LogExtensions.ExceptionHandler(ex);
                }

                
            }
        }


        
    }
}
