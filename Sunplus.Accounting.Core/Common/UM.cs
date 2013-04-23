using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Common
{
    public static class UM
    {
        public static string LBUrl
        {
            get { return ConfigurationManager.AppSettings["LBUrl"]; }
        }

        private static string _UMTime_From = "";
        public static string UMTime_From
        {
            get { return _UMTime_From; }
            set { _UMTime_From = value; }
        }

        private static string _UMTime_To = "";
        public static string UMTime_To
        {
            get { return _UMTime_To; }
            set { _UMTime_To = value; }
        }
        

        public static bool isUMTime
        {
            get { return ConfigurationManager.AppSettings["UM"] == "1"; }
        }

      

        public static bool GetDownTime()
        {
            bool isUM = false;

            LoadBalancerLibrary.LoadBalancer LB = null;

            try
            {

                LB = (LoadBalancerLibrary.LoadBalancer)Activator.GetObject(typeof(LoadBalancerLibrary.LoadBalancer), LBUrl);
                isUM = LB.isUnderMaintainence(LoadBalancerLibrary.IBCSites.Agency);
                
                //isUM = false;
                
                
            }

            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                } 
            }

            if (isUM)
            {
                string[] UMTime = LB.GetUMTimeSetting();
                UMTime_From = UMTime[0];
                UMTime_To = UMTime[1];
            }

          

            return isUM;
        }
    }
}
