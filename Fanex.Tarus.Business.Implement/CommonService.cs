namespace Fanex.Tarus.Business.Implement
{
    using System;

    using Fanex.Tarus.Business.Interface;
    using Fanex.Tarus.Entities;

    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.UMControllerLibrary;

    public class CommonService : BaseService, ICommonService
    {
        public Downtime GetDowntimeInfo()
        {
            bool isDowntime = false;

            var downTime = new Downtime
                               {
                                   From = "0:00",
                                   To = "0:00",
                                   IsDowntime = false
                               };
            if (!WebSettings.IsUMTime)
                return downTime;

            string timeFrom = "", timeTo = "";

            var umCtrl = new UMBusiness();

            try
            {

                
                isDowntime = umCtrl.isUnderMaintainence(Sites.AccountSite, WebSettings.LbServiceUrl, ref timeFrom, ref timeTo);//Get Agnet Site UM Status.

                //isUM = false;

            }

            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }



            if (isDowntime)
            {
                
                downTime.From = timeFrom;
                downTime.To = timeTo;
                downTime.IsDowntime = true;
            }



            return downTime;
        }
    }
}
