using System;
using System.Threading;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sunplus.Accounting.Entities.LogDisplay;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.ILogDisplay;
using DNAide.Core;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using Sunplus.Accounting.Core.Configurations;
using System.Configuration;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.Common.IPs;
using Sunplus.Accounting.Core.Cache;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public class SettingManager : ISettingManager
    {
        private ILogDisplayDAO _logDAO = null;

        public SettingManager(ILogDisplayDAO logDAO)
        {
            _logDAO = logDAO;
        }
        public Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogDisplay(LogParam param) 
        { 
            ResultLog logResult = new ResultLog();
            logResult.Type = param.Type;
            param.FromDate = new DateTime(param.FromDate.Year, param.FromDate.Month, param.FromDate.Day, 0, 0, 0);
            param.ToDate = new DateTime(param.ToDate.Year, param.ToDate.Month, param.ToDate.Day, 23, 59, 59);
            switch (param.Type)
            {
                case (int)ELogType.Setting:
                    var resultSetting = GetLogSetting(param.CustId, param.CustName, param.FromDate, param.ToDate, param.ViewBy, param.Page, param.PageSize);
                    logResult.Total = resultSetting.Total;
                    logResult.Setting = resultSetting.Rows;
                    break;
                case (int)ELogType.Status:
                    var resultStatus = GetLogStatus(param.CustId, param.CustName, param.FromDate, param.ToDate, param.Page, param.PageSize);
                    logResult.Total = resultStatus.Total;
                    logResult.Status = resultStatus.Rows;
                    break;
                case (int)ELogType.Credit:
                    var resultCredit = GetLogCredit(param.CustId, param.CustName, param.FromDate, param.ToDate, param.Page, param.PageSize);
                    logResult.Total = resultCredit.Total;
                    logResult.Credit = resultCredit.Rows;
                    break;
                case (int)ELogType.Login:
                    var resultLogin = GetLogLogin(param.CustId, param.CustName, param.FromDate, param.ToDate, param.Page, param.PageSize);
                    logResult.Total = resultLogin.Total;
                    logResult.Login = resultLogin.Rows;
                    break;
                default:
                    break;
            }
            return logResult;
        }
        /**
            @Function GetLogSetting
            @CustId INT,
	        @CustName VARCHAR(50) = '',
            @FromDate DATETIME,
            @ToDate DATETIME,
            @ActionTypeId INT = -1,
            @IsInternal TINYINT,
            @LowerActionId INT = NULL,
	        @UpperActionId INT = NULL,
            @PageIndex INT,
            @PageSize INT
         **/
        public Sunplus.Accounting.Entities.LogDisplay.ResultSp GetLogSetting(int custId, string custName, DateTime fromDate, DateTime toDate, int viewBy, int page, int pageSize)
        {
            ResultSp logResult = new ResultSp();
            try
            {
                string logType = "setting";
                int totalRow = 0;
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custname", custName);

                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);

                param.Add("actiontypeid", viewBy); //-1:All, 1:PT, 2:Comm, 3:BetSet, 4:General

                // hard code
                param.Add("isinternal", 1);
                param.Add("LowerActionId", null);
                param.Add("UpperActionId", null);

                param.Add("pageindex", page>0? page-1: 0);
                param.Add("pagesize", pageSize);

                var data = _logDAO.GetLogDisplay(param, logType);
                if (data != null && data.Rows.Count > 0)
                {
                    logResult.Total = _logDAO.GetLogDisplayTotal(param, logType);
                    logResult.Rows = new List<RecordSp>();
                    foreach (DataRow row in data.Rows) {
                        // get record value
                        var record = _GetLogRecord(row);
                        // get tracking value
                        var trackingVal = _ParseTrackingValue(CastAide.AsString(row["trackingvalue"], ""), CastAide.AsInt32(row["actionid"], 0));
                        record.Detail = trackingVal;
                        var diffVals = _DiffsTrackingValue(CastAide.AsString(row["trackingvalue"], string.Empty), CastAide.AsString(row["trackingvalueold"], string.Empty));
                        record.Diffs = diffVals;
                        /*if (diffVals.Count<TrackingValue>() > 0)
                        {
                            record.Old = _ParseTrackingValue(CastAide.AsString(row["trackingvalueold"]), CastAide.AsInt32(row["actionid"], 0));
                        }*/

                        logResult.Rows.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return logResult;
        }
        /**
            @Function GetLogStatus
            @CustId INT,
	        @CustName VARCHAR(50) = '',
            @FromDate DATETIME,
            @ToDate DATETIME,
            @ActionTypeId INT = NULL,
            @IsInternal TINYINT,
            @LowerActionId INT = 1100,
	        @UpperActionId INT = 1199,
            @PageIndex INT,
            @PageSize INT
         **/
        public Sunplus.Accounting.Entities.LogDisplay.ResultSp GetLogStatus(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            ResultSp logResult = new ResultSp();
            try
            {
                string logType = "status";
                int totalRow = 0;
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custname", custName);

                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);

                param.Add("actiontypeid", null); 

                // hard code
                param.Add("isinternal", 1);
                param.Add("loweractionid", 1100);
                param.Add("upperactionid", 1199);

                param.Add("pageindex", page > 0 ? page - 1 : 0);
                param.Add("pagesize", pageSize);

                var data = _logDAO.GetLogDisplay(param, logType);
                if (data != null && data.Rows.Count > 0)
                {
                    logResult.Total = _logDAO.GetLogDisplayTotal(param, logType);
                    logResult.Rows = new List<RecordSp>();
                    foreach (DataRow row in data.Rows)
                    {
                        // get record value
                        var record = _GetLogRecord(row);
                        // get tracking value
                        var trackingVal = _ParseTrackingValue(CastAide.AsString(row["trackingvalue"]), CastAide.AsInt32(row["actionid"], 0));
                        record.Detail = trackingVal;
                        var diffVals = _DiffsTrackingValue(CastAide.AsString(row["trackingvalue"], string.Empty), CastAide.AsString(row["trackingvalueold"], string.Empty));
                        record.Diffs = diffVals;
                        if (diffVals.Count<TrackingValue>() > 0)
                        {
                            record.Old = _ParseTrackingValue(CastAide.AsString(row["trackingvalueold"]), CastAide.AsInt32(row["actionid"], 0));
                        }

                        logResult.Rows.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return logResult;
        }
        /**
            @UserId INT,
	        @UserName AS VARCHAR(50),
            @FromDate DATETIME,
            @ToDate DATETIME,    
            @IsAdminAccess BIT = 0,
            @PageIndex INT,
            @PageSize INT
         **/
        public Sunplus.Accounting.Entities.LogDisplay.ResultLogin GetLogLogin(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            ResultLogin logResult = new ResultLogin();
            try
            {
                string logType = "login";
                int totalRow = 0;
                var param = new Hashtable();
                param.Add("userid", custId);
                param.Add("username", custName);

                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);

                param.Add("isadminaccess", 1);

                // page set
                param.Add("pageindex", page > 0 ? page - 1 : 0);
                param.Add("pagesize", pageSize);

                var data = _logDAO.GetLogDisplay(param, logType);
                if (data != null && data.Rows.Count > 0)
                {
                    logResult.Total = _logDAO.GetLogDisplayTotal(param, logType);
                    logResult.Rows = new List<RecordLogin>();
                    foreach (DataRow row in data.Rows)
                    {
                        // get record value
                        var record = new RecordLogin();
                        record.ID = CastAide.AsInt32(row["logid"], 0);
                        record.Aid = CastAide.AsInt32(row["actionid"], 0);
                        record.Act = CastAide.AsString(row["actionname"], "");
                        record.Date = CastAide.AsString(row["logdate"]);
                        record.UName = CastAide.AsString(row["username"]);
                        record.UNId = CastAide.AsInt32(row["userid"]);
                        record.SP = CastAide.AsString(row["spname"]);
                        // replace ip
                        record.IP = LogFormater.ReplaceIP(CastAide.AsString(row["ip"]));
                        record.Desc = CastAide.AsString(row["doing"]);
                        record.Domain = CastAide.AsString(row["domain"], "&nbsp;");
                        record.Browser = CastAide.AsString(row["browser"], "&nbsp;");
                        record.Country = IpManagement.ReplaceCountryName(CastAide.AsString(row["ip"]));
                        logResult.Rows.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return logResult;
        }
        /**
            @CustID INT ,
	        @CustName varchar(50) = '',
            @FromDate DATETIME ,
            @ToDate DATETIME ,    
            @IsInternal TINYINT ,
            @PageIndex INT ,
            @PageSize INT
         **/
        public Sunplus.Accounting.Entities.LogDisplay.ResultCredit GetLogCredit(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            ResultCredit logResult = new ResultCredit();
            try
            {
                string logType = "credit";
                int totalRow = 0;
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custname", custName);

                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);

                param.Add("isinternal", 1);

                // page set
                param.Add("pageindex", page > 0 ? page - 1 : 0);
                param.Add("pagesize", pageSize);

                var data = _logDAO.GetLogDisplay(param, logType);
                if (data != null && data.Rows.Count > 0)
                {
                    logResult.Total = _logDAO.GetLogDisplayTotal(param, logType);
                    logResult.Rows = new List<RecordCredit>();
                    foreach (DataRow row in data.Rows)
                    {
                        // get record value
                        var record = new RecordCredit();
                        record.ID = CastAide.AsInt32(row["logid"], 0);
                        record.Aid = CastAide.AsInt32(row["actionid"], 0);
                        record.Act = CastAide.AsString(row["actionname"], "");
                        record.Date = CastAide.AsString(row["logdate"]);
                        record.UEdit = CastAide.AsString(row["username"]);
                        record.UEId = CastAide.AsInt32(row["userid"]);
                        record.UName = CastAide.AsString(row["custname"]);
                        record.UNId = CastAide.AsInt32(row["custid"]);
                        record.SP = CastAide.AsString(row["spname"]);
                        record.OldVal = String.Format("{0:#,##0.00}", CastAide.AsDouble(row["oldvalue"]));
                        record.NewVal = String.Format("{0:#,##0.00}", CastAide.AsDouble(row["newvalue"]));
                        // replace ip
                        record.IP = LogFormater.ReplaceIP(CastAide.AsString(row["ip"]));
                        record.Detail = CastAide.AsString(row["doing"]);
                        logResult.Rows.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return logResult;
        }

        private RecordSp _GetLogRecord(DataRow row) 
        {
            var record = new RecordSp();
            record.ID = CastAide.AsInt32(row["logid"], 0);
            record.Aid = CastAide.AsInt32(row["actionid"], 0);
            record.Act = CastAide.AsString(row["actionname"], "");
            record.Date = CastAide.AsString(row["logdate"]);
            record.UEdit = CastAide.AsString(row["username"]);
            record.UEId = CastAide.AsInt32(row["userid"]);
            record.UName = CastAide.AsString(row["custname"]);
            record.UNId = CastAide.AsInt32(row["custid"]);
            return record;
        }

        private TrackingLog _ParseTrackingValue(string trackingVal, int actionId)
        {
            var tracking = new TrackingLog();
            if (!string.IsNullOrEmpty(trackingVal)) 
            {
                try
                {
                    // mapping template
                    var logTracking = LogSection.Instance.Tracking.Cast<TrackingTemplate>();
                    var trackingTemplate = (from tpl in logTracking where (tpl.Filters.Cast<ActionElement>().Where(atc => atc.Id == actionId).SingleOrDefault<ActionElement>() != null) select tpl).SingleOrDefault<TrackingTemplate>();
                    trackingTemplate = trackingTemplate ?? LogSection.Instance.DefaultTracking;

                    // set tracking level
                    tracking.Level  = trackingTemplate.Level;

                    // parse XML
                    XElement xmlLoad = null;

                    Regex regx = new Regex("[ ]+(1x2)|(\a)|(1101)|(1102)|(1103)|(AddFrom)|(1001)|(1002)", RegexOptions.IgnoreCase);
                        if (regx.IsMatch(trackingVal)) 
                        {
                            trackingVal = trackingVal.Replace("1x2", "OnexTwo");
                            trackingVal = trackingVal.Replace("\a", ";");

                            trackingVal = trackingVal.Replace("BetType=\"1001\"", string.Format("BetType=\"{0}\"", CacheRef.GetBettype("1001")));
                            trackingVal = trackingVal.Replace("BetType=\"1002\"", string.Format("BetType=\"{0}\"", CacheRef.GetBettype("1002")));
                            
                            trackingVal = trackingVal.Replace("BetTypeId=\"1101\"", string.Format("BetTypeId=\"{0}\"", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "g1101")));
                            trackingVal = trackingVal.Replace("BetTypeId=\"1102\"", string.Format("BetTypeId=\"{0}\"", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "g1102")));
                            trackingVal = trackingVal.Replace("BetTypeId=\"1103\"", string.Format("BetTypeId=\"{0}\"", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "g1103")));

                            trackingVal = trackingVal.Replace("AddFrom=\"1\"", string.Format("AddFrom=\"{0}\"", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "addfrom01")));
                            trackingVal = trackingVal.Replace("AddFrom=\"2\"", string.Format("AddFrom=\"{0}\"", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "addfrom02")));

                           //trackingVal = trackingVal.Replace("DisableP2Pdownlines", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "DisableP2Pdownlines"));
                           //trackingVal = trackingVal.Replace("DisableFunP2Pdownlines", ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, "DisableFunP2Pdownlines"));
                            
                        }
                        xmlLoad = XElement.Parse(trackingVal);
                
                    if (xmlLoad == null) { return tracking; }

                    tracking.SP     = xmlLoad.Name.ToString();
                    tracking.Attrs  = new List<TrackingElement>();
                    tracking.Vals   = new List<TrackingNode>();

                    // check hidden param
                    var hiddenDefault = trackingTemplate.HiddenDefault.Split(',');
                    var filter = trackingTemplate.Filters.Cast<ActionElement>().Where(action => action.Id == actionId).SingleOrDefault<ActionElement>();
                    var hiddenField = (filter == null || string.IsNullOrEmpty(filter.Hidden)) ? hiddenDefault : filter.Hidden.Split(',');

                    // get formater
                    var formater = (from param in trackingTemplate.Formater.Cast<ParamElement>() select param.Name.ToLower()).ToArray<string>();

                    // check HasAttributes
                    if (xmlLoad.HasAttributes)
                    {
                        var trackingNode = AssignGroupAttribute(xmlLoad, trackingTemplate, filter, hiddenField);
                        tracking.Vals.Add(trackingNode);
                    }
                    // check HasElements
                    if (xmlLoad.HasElements)
                    {
                        //xmlLoad.Nodes().Cast<XElement>().AsParallel().ForEach<XElement>(node =>
                        //{
                        //    var trackingNode = AssignGroupAttribute(node, trackingTemplate, filter, hiddenField);
                        //    tracking.Vals.Add(trackingNode);
                        //});

                        //Parallel.ForEach(xmlLoad.Nodes().Cast<XElement>(), node =>
                        //{
                        //    var trackingNode = AssignGroupAttribute(node, trackingTemplate, filter, hiddenField);
                        //    tracking.Vals.Add(trackingNode);
                        //});
                        foreach (XElement node in xmlLoad.Nodes())
                        {
                            var trackingNode = AssignGroupAttribute(node, trackingTemplate, filter, hiddenField);
                            tracking.Vals.Add(trackingNode);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                }
            }
            return tracking;
        }

        private List<TrackingGroup> _GetSportGroup(IEnumerable<XAttribute> attrs, string groupName, string[] hidden)
        {
            var groups = new List<TrackingGroup>();
            var logGroup = LogSection.Instance.Groups.Cast<GroupElement>();
            var sports = (from sp in
                          (from gr in logGroup
                           where gr.Name == groupName
                           select gr).SingleOrDefault<GroupElement>().Sports.Cast<SportElement>()
                      select sp).ToList();
            foreach(var sport in sports)
            {
                Regex rex = null;
                var sportVals = new List<TrackingElement>();
                if(sport.Id==1) // soccer
                {
                    rex = new Regex("v[0-9]", RegexOptions.IgnoreCase);
                    sportVals = (from attr in attrs
                                 where !rex.IsMatch(attr.Name.ToString()) && !hidden.Contains<string>(attr.Name.ToString().ToLower())
                                 select LogFormater.FormatElement(attr)).ToList();
                }
                else if (sport.Id == 999) //mix sport parlay
                {
                    sportVals = (from attr in attrs
                                 where GetMixedSportTag(attr.Name.ToString()) && !hidden.Contains<string>(attr.Name.ToString())
                                 select new TrackingElement() { N = LogFormater.GetResourceName(attr.Name.ToString()), Val = attr.Value, P = attr.Name.ToString() }).ToList();

                    
                }
                else 
                {
                    var strId = "v" + sport.Id;
                    rex = new Regex(strId, RegexOptions.IgnoreCase);
                    sportVals = (from attr in attrs
                                 //where GetMixedSportTag(attr.Name.ToString().ToLower()) != attr.Name.ToString().ToLower() && attr.Name.ToString().ToLower().IndexOf(strId.ToLower()) != -1 && !hidden.Contains<string>(attr.Name.ToString())
                                 where !GetMixedSportTag(attr.Name.ToString()) && rex.IsMatch(attr.Name.ToString()) && !hidden.Contains<string>(attr.Name.ToString())
                                 select LogFormater.FormatElement(attr)).ToList();
                }
                var group = new TrackingGroup { N = LogFormater.GetResourceName(sport.Label), Vals = sportVals };
                groups.Add(group);
            };
            return groups;
        }

        private bool GetMixedSportTag(string attr)
        {
            // mixparlay: v99mp or v99[a-z]+_9
            var mixedSport = attr.ToLower();
            var pattern = @"[\w]+99[a-zA-z]+_9";
            var pattern2 = @"[\w]+99mp|99mp[a-zA-Z]_9";
            var pattern3 = @"[\w]+99[a-zA-z]+_[0-8]";
            //var pattern = @"v99[0-9a-zA-z]+_9";
            //var pattern2 = @"v99mp";
            var myRegex = new Regex(pattern3, RegexOptions.IgnoreCase);
            if (myRegex.IsMatch(attr)) return false;
            myRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            var matches = myRegex.IsMatch(attr) ? myRegex.Matches(attr) : new Regex(pattern2, RegexOptions.IgnoreCase).Matches(attr);
            if (matches.Count > 0)
                return true;
            return false;
        }

        private TrackingNode AssignGroupAttribute(XElement node, TrackingTemplate trackingTemplate, ActionElement filter, string[] hiddenField)
        {
            var trackingNode = new TrackingNode();
            trackingNode.N = LogFormater.GetResourceName(node.Name.ToString());
            trackingNode.Vals = new List<TrackingElement>();
            trackingNode.Groups = new List<TrackingGroup>();
            if (node.HasAttributes)
            {
                if (trackingTemplate.HasGroup && !node.Name.ToString().Equals("info"))
                {
                    // group elements
                    var groupName = string.IsNullOrEmpty(filter.GroupName) ? trackingTemplate.GroupDefault : filter.GroupName;
                    var groups = _GetSportGroup(node.Attributes(), groupName, hiddenField);
                    trackingNode.Groups = groups;
                }
                else
                {
                    //node.Attributes().AsParallel().ForEach<XAttribute>(attr =>
                    //{
                    //    if (!hiddenField.Contains<string>(attr.Name.ToString().ToLower()))
                    //    {
                    //        var elm = LogFormater.FormatElement(attr, filter);
                    //        trackingNode.Vals.Add(elm);
                    //    }
                    //});

                    //Parallel.ForEach(node.Attributes(), attr =>
                    //{
                    //    if (!hiddenField.Contains<string>(attr.Name.ToString().ToLower()))
                    //    {
                    //        var elm = LogFormater.FormatElement(attr, filter);
                    //        trackingNode.Vals.Add(elm);
                    //    }
                    //});
                    foreach (XAttribute attr in node.Attributes())
                    {
                        if (!hiddenField.Contains<string>(attr.Name.ToString().ToLower()))
                        {
                            var elm = LogFormater.FormatElement(attr, filter);
                            trackingNode.Vals.Add(elm);
                        }
                    }
                }
            }

            return trackingNode;
        }

        private List<TrackingValue> _DiffsTrackingValue(string trackingValue, string trackingValue2)
        {
            var diffs = new List<TrackingValue>();
            if (!trackingValue.Equals(trackingValue2) && !string.IsNullOrEmpty(trackingValue2))
            {


                trackingValue = trackingValue.Replace("\a", "");
                trackingValue2 = trackingValue2.Replace("\a", "");

                var listVal = _GetTrackingParams(trackingValue);
                var listVal2 = _GetTrackingParams(trackingValue2);

                if (listVal.Count == listVal2.Count)
                {
                    // check diff 
                    //listVal.AsParallel().ForEach(elm =>
                    //{
                    //    var elm2 = listVal2[elm.Key];
                    //    if (!elm.Value.Val.Equals(elm2.Val))
                    //    {
                    //        diffs.Add(elm2);
                    //    }
                    //});

                    //Parallel.ForEach(listVal, elm =>
                    //{
                    //    var elm2 = listVal2[elm.Key];
                    //    if (!elm.Value.Val.Equals(elm2.Val))
                    //    {
                    //        diffs.Add(elm2);
                    //    }
                    //});
                    foreach (var elm in listVal)
                    {
                        var elm2 = listVal2[elm.Key];
                        if (!elm.Value.Val.Equals(elm2.Val))
                        {
                            diffs.Add(elm2);
                        }
                    }
                }
            }
            return diffs;
        }

        private Dictionary<string, TrackingValue> _GetTrackingParams(string trackingValue) 
        {
            var listVal = new Dictionary<string, TrackingValue>();
            try
            {
                // parse XML
                var xmlValue = XElement.Parse(trackingValue);

                // fetch all param
                //xmlValue.Attributes().AsParallel().ForEach<XAttribute>(attr =>
                //{
                //    listVal.Add(attr.Name.ToString(), LogFormater.FormatValue(attr));
                //});
                //xmlValue.Nodes().Cast<XElement>().Attributes().AsParallel().ForEach<XAttribute>(attr =>
                //{
                //    listVal.Add(attr.Name.ToString(), LogFormater.FormatValue(attr));
                //});

                //Parallel.ForEach(xmlValue.Attributes(), attr =>
                //{
                //    listVal.Add(attr.Name.ToString(), LogFormater.FormatValue(attr));
                //});
                //Parallel.ForEach(xmlValue.Nodes().Cast<XElement>().Attributes(), attr =>
                //{
                //    listVal.Add(attr.Name.ToString(), LogFormater.FormatValue(attr));
                //});

                foreach (var attr in xmlValue.Attributes())
                {
                    listVal.Add(attr.Name.ToString(), LogFormater.FormatValue(attr));
                }
                foreach (var attr in xmlValue.Nodes().Cast<XElement>().Attributes())
                {
                    listVal.Add(attr.Name.ToString(), LogFormater.FormatValue(attr));
                }
            }
            catch (Exception ex) 
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return listVal;
        }

        private void _LoopParams(XElement xElement, ref Dictionary<string, TrackingValue> listParam)
        {
            if (xElement.Nodes().Cast<XElement>().Attributes().Count<XAttribute>() > 0)
            {
                //_LoopParams(listParam);
            }
        }
    }
}
