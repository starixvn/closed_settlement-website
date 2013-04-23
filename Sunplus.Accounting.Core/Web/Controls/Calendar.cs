using System;
using System.Globalization;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Core.Configurations;
using DNAide.Core;

namespace Sunplus.Accounting.Core.Web.Controls
{
    /// <summary>
    /// Summary description for Calendar
    /// </summary>
    public class Calendar
    {
        public static string GenCalendarControl(string txtDateId, string rootPath, string triggerId, string currentDate,
                                                bool noJs, bool showTime, int displayMonths, int displaydays,
                                                string eventOnSelect)
        {
            return GenCalendarControl(txtDateId, rootPath, triggerId, currentDate,
                                      noJs, showTime, displayMonths, displaydays, eventOnSelect, "");
        }

        public static string GenCalendarControl(string txtDateId, string rootPath, string triggerId, string currentDate,
                                                bool noJs, bool showTime, int displayMonths, string eventOnSelect,
                                                string calobj, int displaydays)
        {
            return GenCalendarControl(txtDateId, rootPath, triggerId, currentDate,
                                      noJs, showTime, displayMonths, displaydays, eventOnSelect, "", calobj);
        }

        /// <summary>
        /// Create calendar with theme
        ///
        public static string GenCalendarControl(string txtDateId, string rootPath, string triggerId, string currentDate,
                                                bool noJs, bool showTime, int displayMonths, int displaydays,
                                                string eventOnSelect, string theme)
        {
            return GenCalendarControl(txtDateId, rootPath, triggerId, currentDate,
                                      noJs, showTime, displayMonths, displaydays, eventOnSelect, theme, "");
        }

        /// <summary>
        /// To get Calendar with MinDate/MaxDate specified
        /// </summary>
        /// <param name="mindate"></param>
        /// <param name="maxdate"></param>
        /// <param name="txtDateId"></param>
        /// <param name="rootPath"></param>
        /// <param name="triggerId"></param>
        /// <param name="currentDate"></param>
        /// <param name="noJs"></param>
        /// <param name="showTime"></param>
        /// <param name="eventOnSelect"></param>
        /// <param name="theme"></param>
        /// <param name="calobj"></param>
        /// <returns></returns>
        public static string GenCalendarControl(string mindate, string maxdate, string txtDateId, string rootPath,
                                                string triggerId, string currentDate,
                                                bool noJs, bool showTime, string eventOnSelect, string theme, string calobj)
        {
            CultureInfo cult = CultureInfo.CurrentCulture;

            try
            {
                if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return null;
                DateTime dt;

                if (showTime)
                {
                    if (!Validator.ValidDatetime24(currentDate, out dt).IsSuccess() || currentDate == null)
                        currentDate = "";
                }
                else
                {
                    if (!Validator.ValidDateMMDDYYYY(currentDate, out dt).IsSuccess() || currentDate == null)
                        currentDate = "";
                }

                string htmlCalendar = "";
                htmlCalendar = WebUtils.LoadTemplate("Calendar_v2.htm");
                if (noJs) TemplateHandler.RemoveBlock(ref htmlCalendar, "header");

                if (!string.IsNullOrEmpty(mindate))
                    mindate = "min:" + mindate + ",";
                if (!string.IsNullOrEmpty(maxdate))
                    maxdate = "max:" + maxdate + ",";

                TemplateHandler.Assign(ref htmlCalendar, "{calobj}", calobj);
                TemplateHandler.Assign(ref htmlCalendar, "{calendarTheme}", theme == "" ? WebSettings.CalendarTheme : theme);
                TemplateHandler.Assign(ref htmlCalendar, "{txtDate}", txtDateId);
                TemplateHandler.Assign(ref htmlCalendar, "{btnCal}", triggerId);
                TemplateHandler.Assign(ref htmlCalendar, "{RootPath}", rootPath);
                TemplateHandler.Assign(ref htmlCalendar, "{minDate}", mindate);
                TemplateHandler.Assign(ref htmlCalendar, "{maxDate}", maxdate);
                TemplateHandler.Assign(ref htmlCalendar, "{eventOnSelect}",
                                       eventOnSelect.Length > 0 ? eventOnSelect + ";" : "");
                TemplateHandler.Assign(ref htmlCalendar, "{showTime}", showTime ? "true" : "false");
                TemplateHandler.Assign(ref htmlCalendar, "{showToday}", "true");
                TemplateHandler.Assign(ref htmlCalendar, "{currentDate}",
                                       string.Format(cult, "{0:MM/dd/yyyy}", Convert.ToDateTime(currentDate)));
                TemplateHandler.Assign(ref htmlCalendar, "{SelectedDate}",
                                       string.Format(cult, "{0:yyyyMMdd}", Convert.ToDateTime(currentDate)));

                return htmlCalendar;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Create date time selector
        ///
        public static string GenCalendarControl(string txtDateId, string rootPath, string triggerId, string currentDate,
                                                bool noJs, bool showTime, int displayMonths, int displaydays,
                                                string eventOnSelect, string theme, string calobj)
        {
            CultureInfo cult = CultureInfo.CurrentCulture;

            try
            {
                if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return null;
                DateTime dt;

                if (showTime)
                {
                    if (!Validator.ValidDatetime24(currentDate, out dt).IsSuccess() || currentDate == null)
                        currentDate = "";
                }
                else
                {
                    if (!Validator.ValidDateMMDDYYYY(currentDate, out dt).IsSuccess() || currentDate == null)
                        currentDate = "";
                }

                string htmlCalendar = "";
                htmlCalendar = WebUtils.LoadTemplate("Calendar_v2.htm");
                if (noJs) TemplateHandler.RemoveBlock(ref htmlCalendar, "header");
                string mindate = "";
                string maxdate = "";
                if (displayMonths > 0)
                {
                    var dtnow = new DateTime();
                    dtnow = DateTime.Now;
                    mindate = "min:" + string.Format(cult, "{0:yyyyMMdd}", dtnow.AddMonths(-displayMonths)) + ",";
                    maxdate = "max:" + string.Format(cult, "{0:yyyyMMdd}", dtnow) + ",";
                }
                else
                {
                    if (displaydays > 0)
                    {
                        var dtnow = new DateTime();
                        dtnow = DateTime.Now;
                        mindate = "min:" + string.Format(cult, "{0:yyyyMMdd}", dtnow.AddDays(-displaydays)) + ",";
                        maxdate = "max:" + string.Format(cult, "{0:yyyyMMdd}", dtnow) + ",";
                    }
                    else
                    {
                        mindate = "";
                        maxdate = "";
                    }
                }
                TemplateHandler.Assign(ref htmlCalendar, "{calobj}", calobj);
                TemplateHandler.Assign(ref htmlCalendar, "{calendarTheme}", theme == "" ? WebSettings.CalendarTheme : theme);
                TemplateHandler.Assign(ref htmlCalendar, "{txtDate}", txtDateId);
                TemplateHandler.Assign(ref htmlCalendar, "{btnCal}", triggerId);
                TemplateHandler.Assign(ref htmlCalendar, "{RootPath}", rootPath);
                TemplateHandler.Assign(ref htmlCalendar, "{minDate}", mindate);
                TemplateHandler.Assign(ref htmlCalendar, "{maxDate}", maxdate);
                TemplateHandler.Assign(ref htmlCalendar, "{eventOnSelect}",
                                       eventOnSelect.Length > 0 ? eventOnSelect + ";" : "");
                TemplateHandler.Assign(ref htmlCalendar, "{showTime}", showTime ? "true" : "false");
                TemplateHandler.Assign(ref htmlCalendar, "{showToday}", "true");
                TemplateHandler.Assign(ref htmlCalendar, "{currentDate}",
                                       string.Format(cult, "{0:MM/dd/yyyy}", CastAide.AsDateTime(currentDate)));
                TemplateHandler.Assign(ref htmlCalendar, "{SelectedDate}",
                                       string.Format(cult, "{0:yyyyMMdd}", CastAide.AsDateTime(currentDate)));

                return htmlCalendar;
            }
            catch
            {
                throw;
            }
        }

        public static string GenCalendarControlMaxDays(string txtDateId, string rootPath, string triggerId, string currentDate,
                                               bool noJs, bool showTime, int displayMonths, int displaydays, int maxDays,
                                               string eventOnSelect, string theme, string calobj)
        {
            CultureInfo cult = CultureInfo.CurrentCulture;

            try
            {
                if (string.IsNullOrEmpty(txtDateId) || string.IsNullOrEmpty(triggerId)) return null;
                DateTime dt;

                if (showTime)
                {
                    if (!Validator.ValidDatetime24(currentDate, out dt).IsSuccess() || currentDate == null)
                        currentDate = "";
                }
                else
                {
                    if (!Validator.ValidDateMMDDYYYY(currentDate, out dt).IsSuccess() || currentDate == null)
                        currentDate = "";
                }

                string htmlCalendar = "";
                htmlCalendar = WebUtils.LoadTemplate("Calendar_v2.htm");
                if (noJs) TemplateHandler.RemoveBlock(ref htmlCalendar, "header");
                string mindate = "";
                string maxdate = "";
                if (displayMonths > 0)
                {
                    var dtnow = new DateTime();
                    dtnow = DateTime.Now;
                    mindate = "min:" + string.Format(cult, "{0:yyyyMMdd}", dtnow.AddMonths(-displayMonths)) + ",";
                    maxdate = "max:" + string.Format(cult, "{0:yyyyMMdd}", dtnow.AddDays(maxDays)) + ",";
                }
                else
                {
                    if (displaydays > 0)
                    {
                        var dtnow = new DateTime();
                        dtnow = DateTime.Now;
                        mindate = "min:" + string.Format(cult, "{0:yyyyMMdd}", dtnow.AddDays(-displaydays)) + ",";
                        maxdate = "max:" + string.Format(cult, "{0:yyyyMMdd}", dtnow.AddDays(maxDays)) + ",";
                    }
                    else
                    {
                        mindate = "";
                        maxdate = "";
                    }
                }
                TemplateHandler.Assign(ref htmlCalendar, "{calobj}", calobj);
                TemplateHandler.Assign(ref htmlCalendar, "{calendarTheme}", theme == "" ? WebSettings.CalendarTheme : theme);
                TemplateHandler.Assign(ref htmlCalendar, "{txtDate}", txtDateId);
                TemplateHandler.Assign(ref htmlCalendar, "{btnCal}", triggerId);
                TemplateHandler.Assign(ref htmlCalendar, "{RootPath}", rootPath);
                TemplateHandler.Assign(ref htmlCalendar, "{minDate}", mindate);
                TemplateHandler.Assign(ref htmlCalendar, "{maxDate}", maxdate);
                TemplateHandler.Assign(ref htmlCalendar, "{eventOnSelect}",
                                       eventOnSelect.Length > 0 ? eventOnSelect + ";" : "");
                TemplateHandler.Assign(ref htmlCalendar, "{showTime}", showTime ? "true" : "false");
                TemplateHandler.Assign(ref htmlCalendar, "{showToday}", "true");
                TemplateHandler.Assign(ref htmlCalendar, "{currentDate}",
                                       string.Format(cult, "{0:MM/dd/yyyy}", CastAide.AsDateTime(currentDate)));
                TemplateHandler.Assign(ref htmlCalendar, "{SelectedDate}",
                                       string.Format(cult, "{0:yyyyMMdd}", CastAide.AsDateTime(currentDate)));

                return htmlCalendar;
            }
            catch
            {
                throw;
            }
        }

        public static string CreateDateRangeSelector(string rootPath, string fdateId, string tdateId,
                                                     DateTime fdate, DateTime tdate, bool showTime, int displayMonths,
                                                     int displayDays, string eventOnSelect)
        {
            try
            {
                string ftriggerId = "btn" + fdateId, ttriggerId = "btn" + tdateId;
                string dateRangeTpl = "";
                dateRangeTpl = WebUtils.LoadTemplate("DateRangeSelect.htm");
                TemplateHandler.Assign(ref dateRangeTpl, "{DayNoReset}", "7");
                TemplateHandler.Assign(ref dateRangeTpl, "{Fdate}", fdateId);
                TemplateHandler.Assign(ref dateRangeTpl, "{Tdate}", tdateId);
                TemplateHandler.Assign(ref dateRangeTpl, "{FromDateCal}",
                                       GenCalendarControl(fdateId, rootPath, ftriggerId, fdate.ToShortDateString(), false,
                                                          false, displayMonths, displayDays, eventOnSelect));
                TemplateHandler.Assign(ref dateRangeTpl, "{ToDateCal}",
                                       GenCalendarControl(tdateId, rootPath, ttriggerId, tdate.ToShortDateString(), true,
                                                          false, displayMonths, displayDays, eventOnSelect));
                return dateRangeTpl;
            }
            catch
            {
                throw;
            }
        }


       
    }
}
