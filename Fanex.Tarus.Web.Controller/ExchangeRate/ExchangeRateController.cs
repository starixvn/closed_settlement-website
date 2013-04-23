namespace Fanex.Tarus.Web.Controllers.ExchangeRate
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    
    using ExcelUtil.Spreadsheet;

    using Fanex.Tarus.ExchangeRate.Business;
    using Fanex.Tarus.ExchangeRate.Entities;
    using Fanex.Tarus.Web.Controllers.Filters;

    public class ExchangeRateController : BaseController
    {
        //
        // GET: /OANDAExRate/
        public readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController()
            : this(null)
        {
        }
        public ExchangeRateController(IExchangeRateService exRateService)
        {
            _exchangeRateService = exRateService ?? new ExchangeRateService();
        }

        [AuthenticateRead]
        public ActionResult OandaExRate()
        {
            return View("~/Views/ExchangeRate/OandaExRate.cshtml");
        }

        //[HttpPost, AuthenticateRead]
        public JsonResult GetExchangeDailyOanda(string fdate, string tdate)
        {
            var result = new List<ExchangeDailyDTO>();
            result = _exchangeRateService.GetOandaExchangeDaily(fdate, tdate);
            var jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public ActionResult ExportExcelExDailyOanda(string fdate, string tdate)
        {
            ExcelFile result = _exchangeRateService.ExportExcelExDailyOanda(fdate, tdate);
            int exportResult = 0;
            if (result == null)
            {
                exportResult = 1;
            }

            var jsonResult = Json(exportResult);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;     
        }
    }
}
