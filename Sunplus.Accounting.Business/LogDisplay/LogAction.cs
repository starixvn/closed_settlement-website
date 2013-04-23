using System;
using System.Collections.Generic;
using System.Text;
using DNAide.Core;
using DNAide.Web;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Shared.ViewLogLib;
using Sunplus.Shared.ViewLogLib.Model;
using Sunplus.Shared.ViewLogLib.ServiceLayger;

//using IBC.Shared.ViewLogLib;
//using IBC.Shared.ViewLogLib.ServiceLayger;
//using IBC.Shared.ViewLogLib.Model;
//using Sunplus.Core.LoggingLib;


namespace Sunplus.Accounting.Business.LogDisplay
{
    public class LogAction
    {
        public static string GetActionFilterContent(string filter, string lang, string actionType)
        {
            try
            {
                LanguageCode languageCode = ToLanguageCode(lang);
                int actionTypeID = CastAide.AsInt32(actionType, -1);
                List<Sunplus.Shared.ViewLogLib.Model.LogAction> list = (new LogActionService()).Search(filter,
                                                                                                       languageCode,
                                                                                                       actionTypeID);
                string templateBody = WebUtils.LoadTemplate("LogDisplay/LogActionBody.htm");

                //Action Name Title
                templateBody = SetLanguageTitle(templateBody, languageCode);

                if (!ReferenceEquals(list, null))
                {
                    string templateRow = WebUtils.LoadTemplate("LogDisplay/LogActionRow.htm");
                    if (list.Count > 0)
                    {
                        var stringBuilder = new StringBuilder();
                        string odd_even = "evenrow";
                        List<LogActionType> LogActonTypes = (new LogActionTypeService()).GetAll();
                        foreach (Sunplus.Shared.ViewLogLib.Model.LogAction logAction in list)
                        {
                            odd_even = (odd_even == "evenrow") ? "oddrow" : "evenrow";
                            string r = templateRow;
                            TemplateHandler.Assign(ref r, "{ActionID}", logAction.ActionID);
                            TemplateHandler.Assign(ref r, "{Internal}", (logAction.IsInternal == 1) ? "Yes" : "No");
                            TemplateHandler.Assign(ref r, "{SPName}", logAction.SPName);
                            string actionName = GetActionName(languageCode, logAction);
                            string actionTypeName = GetActionTypeName(LogActonTypes, languageCode,
                                                                      CastAide.AsInt32(logAction.ActionTypeID, 0));

                            TemplateHandler.Assign(ref r, "{ActionName}", actionName);
                            TemplateHandler.Assign(ref r, "{ActionType}", actionTypeName);
                            TemplateHandler.Assign(ref r, "{Edit}", "Edit");
                            TemplateHandler.Assign(ref r, "{odd_even}", odd_even);
                            int index = (odd_even == "evenrow") ? 1 : 0;
                            TemplateHandler.Assign(ref r, "{index}", index.ToString());
                            stringBuilder.Append(r);
                        }
                        TemplateHandler.Assign(ref templateBody, "{DataContent}", stringBuilder.ToString());
                    }
                    else
                    {
                        TemplateHandler.Assign(ref templateBody, "{DataContent}", AccCommon.GenNoInformation("5"));
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref templateBody, "{DataContent}", AccCommon.GenNoInformation("5"));
                }
                return templateBody;
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }

            return string.Empty;
        }
        public static string GetActionTypeContent()
        {
            try
            {
                List<LogActionType> list = (new LogActionTypeService()).GetAll();
                string templateBody = WebUtils.LoadTemplate("LogDisplay/LogActionTypeBody.htm");
                if (!ReferenceEquals(list, null))
                {
                    string templateRow = WebUtils.LoadTemplate("LogDisplay/LogActionTypeRow.htm");
                    if (list.Count > 0)
                    {
                        var stringBuilder = new StringBuilder();
                        string odd_even = "evenrow";
                        foreach (LogActionType logActionType in list)
                        {
                            string r = templateRow;
                            odd_even = (odd_even == "evenrow") ? "oddrow" : "evenrow";
                            int index = (odd_even == "evenrow") ? 1 : 0;
                            TemplateHandler.Assign(ref r, "{index}", index.ToString());
                            TemplateHandler.Assign(ref r, "{odd_even}", odd_even);
                            TemplateHandler.Assign(ref r, "ActionTypeID", logActionType.ActionTypeID);
                            TemplateHandler.Assign(ref r, "ActionTypeName", logActionType.ActionTypeNameEN);
                            TemplateHandler.Assign(ref r, "{Edit}", "Edit");
                            stringBuilder.Append(r);
                        }
                        TemplateHandler.Assign(ref templateBody, "{DataContent}", stringBuilder.ToString());
                    }
                    else
                    {
                        TemplateHandler.Assign(ref templateBody, "{DataContent}", AccCommon.GenNoInformation("3"));
                    }
                }

                else
                {
                    TemplateHandler.Assign(ref templateBody, "{DataContent}", AccCommon.GenNoInformation("3"));
                }
                return templateBody;
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
            return string.Empty;
        }
        public static string GetActionTypeOptions()
        {
            var stringBuilder = new StringBuilder();
            List<LogActionType> list = (new LogActionTypeService()).GetAll();
            foreach (LogActionType l in list)
            {
                stringBuilder.Append("<option value='" + l.ActionTypeID + "'>" + l.ActionTypeNameEN + "</option>");
            }

            return stringBuilder.ToString();
        }

        public static string GetActionName(LanguageCode languageCode,
                                           Sunplus.Shared.ViewLogLib.Model.LogAction logAction)
        {
            string actionName = string.Empty;
            switch (languageCode)
            {
                case LanguageCode.EN:
                    actionName = logAction.ActionNameEN;
                    break;
                case LanguageCode.CH:
                    actionName = logAction.ActionNameCH;
                    break;
                case LanguageCode.CS:
                    actionName = logAction.ActionNameCS;
                    break;
                case LanguageCode.JP:
                    actionName = logAction.ActionNameJP;
                    break;
                case LanguageCode.TH:
                    actionName = logAction.ActionNameTH;
                    break;
                case LanguageCode.KO:
                    actionName = logAction.ActionNameKO;
                    break;
                case LanguageCode.IT:
                    actionName = logAction.ActionNameIT;
                    break;
            }
            return actionName;
        }
        public static string SetLanguageTitle(string templateBody, LanguageCode languageCode)
        {
            switch (languageCode)
            {
                case LanguageCode.EN:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "English");
                    break;
                case LanguageCode.CH:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "Chinese Traditional");
                    break;
                case LanguageCode.CS:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "Chinese Simplified");
                    break;
                case LanguageCode.JP:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "Japanese");
                    break;
                case LanguageCode.TH:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "Thai");
                    break;
                case LanguageCode.KO:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "Korean");
                    break;
                case LanguageCode.IT:
                    TemplateHandler.Assign(ref templateBody, "{lang}", "Italian");
                    break;
            }

            return templateBody;
        }
        public static string GetActionTypeEditForm(string id)
        {
            int actionTypeID = CastAide.AsInt32(id);
            try
            {
                string template = WebUtils.LoadTemplate("LogDisplay/LogActionTypeEdit.htm");
                LogActionType logActionType = (new LogActionTypeService()).GetById(actionTypeID);
                if (!ReferenceEquals(logActionType, null))
                {
                    TemplateHandler.Assign(ref template, "{_ActionTypeID}", logActionType.ActionTypeID);
                    TemplateHandler.Assign(ref template, "{_txtEN}", logActionType.ActionTypeNameEN);
                    TemplateHandler.Assign(ref template, "{_txtCH}", logActionType.ActionTypeNameCH);
                    TemplateHandler.Assign(ref template, "{_txtCS}", logActionType.ActionTypeNameCS);
                    TemplateHandler.Assign(ref template, "{_txtJP}", logActionType.ActionTypeNameJP);
                    TemplateHandler.Assign(ref template, "{_txtTH}", logActionType.ActionTypeNameTH);
                    TemplateHandler.Assign(ref template, "{_txtKO}", logActionType.ActionTypeNameKO);
                    TemplateHandler.Assign(ref template, "{_txtIT}", logActionType.ActionTypeNameIT);
                    return template;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
            return string.Empty;
        }
        public static string GetActionEditForm(string aID)
        {
            int actionID = CastAide.AsInt32(aID);
            var stringBuilder = new StringBuilder();
            try
            {
                string template = WebUtils.LoadTemplate("LogDisplay/LogActionEdit.htm");
                Sunplus.Shared.ViewLogLib.Model.LogAction logAction = (new LogActionService()).GetById(actionID);
                if (!ReferenceEquals(logAction, null))
                {
                    TemplateHandler.Assign(ref template, "{_ActionID}", logAction.ActionID.ToString());
                    TemplateHandler.Assign(ref template, "{_txtEN}", logAction.ActionNameEN);
                    TemplateHandler.Assign(ref template, "{_txtCH}", logAction.ActionNameCH);
                    TemplateHandler.Assign(ref template, "{_txtCS}", logAction.ActionNameCS);
                    TemplateHandler.Assign(ref template, "{_txtJP}", logAction.ActionNameJP);
                    TemplateHandler.Assign(ref template, "{_txtTH}", logAction.ActionNameTH);
                    TemplateHandler.Assign(ref template, "{_txtKO}", logAction.ActionNameKO);
                    TemplateHandler.Assign(ref template, "{_txtIT}", logAction.ActionNameIT);
                    if (logAction.IsInternal == 1)
                    {
                        TemplateHandler.Assign(ref template, "{rdYesChecked}", "checked='checked'");
                        TemplateHandler.Assign(ref template, "{rdNoChecked}", "");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref template, "{rdYesChecked}", "");
                        TemplateHandler.Assign(ref template, "{rdNoChecked}", "checked='checked'");
                    }
                    TemplateHandler.Assign(ref template, "{_txtSPName}", logAction.SPName);

                    //Get Log Action Type Options
                    List<LogActionType> list = (new LogActionTypeService()).GetAll();

                    if (ReferenceEquals(logAction.ActionTypeID, null) ||
                        CastAide.AsInt32(logAction.ActionTypeID, 0) == 0)
                    {
                        stringBuilder.Append("<option value='0' selected='selected'>N/A</option>");
                        foreach (LogActionType l in list)
                        {
                            stringBuilder.Append("<option value='" + l.ActionTypeID + "'>" + l.ActionTypeNameEN +
                                                 "</option>");
                        }
                    }
                    else
                    {
                        stringBuilder.Append("<option value='0'>N/A</option>");
                        foreach (LogActionType l in list)
                        {
                            if (l.ActionTypeID != logAction.ActionTypeID)
                            {
                                stringBuilder.Append("<option value='" + l.ActionTypeID + "'>" + l.ActionTypeNameEN +
                                                     "</option>");
                            }
                            else
                            {
                                stringBuilder.Append("<option value='" + l.ActionTypeID + "' selected='selected'>" +
                                                     l.ActionTypeNameEN + "</option>");
                            }
                        }
                    }
                    TemplateHandler.Assign(ref template, "{ActionTypeOptions}", stringBuilder.ToString());


                    return template;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return string.Empty;
        }
        public static string GetActionTypeName(List<LogActionType> list, LanguageCode languageCode, int id)
        {
            string logActionTypeName = string.Empty;
            if (id < 1)
                return "N/A";
            else
            {
                foreach (LogActionType l in list)
                {
                    if (id == l.ActionTypeID)
                    {
                        switch (languageCode)
                        {
                            case LanguageCode.EN:
                                logActionTypeName = l.ActionTypeNameEN;
                                break;
                            case LanguageCode.CH:
                                logActionTypeName = l.ActionTypeNameCH;
                                break;
                            case LanguageCode.CS:
                                logActionTypeName = l.ActionTypeNameCS;
                                break;
                            case LanguageCode.JP:
                                logActionTypeName = l.ActionTypeNameJP;
                                break;
                            case LanguageCode.TH:
                                logActionTypeName = l.ActionTypeNameTH;
                                break;
                            case LanguageCode.KO:
                                logActionTypeName = l.ActionTypeNameKO;
                                break;
                            case LanguageCode.IT:
                                logActionTypeName = l.ActionTypeNameIT;
                                break;
                        }
                        break;
                    }
                }
                return logActionTypeName;
            }
            return string.Empty;
        }
        public static LanguageCode ToLanguageCode(string langKey)
        {
            switch (langKey)
            {
                case "E":
                    return LanguageCode.EN;
                case "J":
                    return LanguageCode.JP;
                case "CT":
                    return LanguageCode.CH;
                case "CS":
                    return LanguageCode.CS;
                case "TH":
                    return LanguageCode.TH;
                case "KO":
                    return LanguageCode.KO;
                case "IT":
                    return LanguageCode.IT;
            }
            return LanguageCode.EN;
        }

        public static string EditLogAction(string actionID, string actionType, string txtEN, string txtCH, string txtCS,
                                           string txtJP, string txtTH, string txtKO, string txtIT, string isInternal,
                                           string txtSPName)
        {
            try
            {
                //IBC.Shared.ViewLogLib.Model.LogAction logAction = new IBC.Shared.ViewLogLib.Model.LogAction();
                Sunplus.Shared.ViewLogLib.Model.LogAction logAction =
                    (new LogActionService()).GetById(CastAide.AsInt32(actionID));
                //logAction.ActionID = CastAide.AsInt32(actionID);
                logAction.ActionNameEN = txtEN;
                logAction.ActionNameCH = txtCH;
                logAction.ActionNameCS = txtCS;
                logAction.ActionNameJP = txtJP;
                logAction.ActionNameTH = txtTH;
                logAction.ActionNameKO = txtKO;
                logAction.ActionNameIT = txtIT;
                logAction.ActionTypeID = CastAide.AsInt32(actionType, 0);
                logAction.IsInternal = CastAide.AsByte(isInternal);
                logAction.SPName = txtSPName;
                bool success = (new LogActionService()).Update(logAction);
                if (success)
                    return "";
                else
                    return "err|Not success, no record was updated!";
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return "err|" + ex.Message;
            }
            return string.Empty;
        }

        public static string EditLogActionType(string actionTypeID, string txtEN, string txtCH, string txtCS,
                                               string txtJP, string txtTH, string txtKO, string txtIT)
        {
            try
            {
                //IBC.Shared.ViewLogLib.Model.LogAction logAction = new IBC.Shared.ViewLogLib.Model.LogAction();
                LogActionType logActionType = (new LogActionTypeService()).GetById(CastAide.AsInt32(actionTypeID));
                //logAction.ActionID = CastAide.AsInt32(actionID);
                logActionType.ActionTypeNameEN = txtEN;
                logActionType.ActionTypeNameCH = txtCH;
                logActionType.ActionTypeNameCS = txtCS;
                logActionType.ActionTypeNameJP = txtJP;
                logActionType.ActionTypeNameTH = txtTH;
                logActionType.ActionTypeNameKO = txtKO;
                logActionType.ActionTypeNameIT = txtIT;

                bool success = (new LogActionTypeService()).Update(logActionType);
                if (success)
                    return "";
                else
                    return "err|Not success, no record was updated!";
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return "err|" + ex.Message;
            }
            return string.Empty;
        }
    }
}
