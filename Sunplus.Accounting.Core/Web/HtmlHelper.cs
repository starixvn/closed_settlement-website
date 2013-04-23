using System;
using System.Text;
using DNAide.Core;
using DNAide.Web;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Web
{
    /// <summary>
    /// Summary description for HtmlHelper
    /// </summary>
    public class HtmlHelper
    {
        
        public static string GenSelectOption(double maxValue, double currentVal, int interval, bool disabled, string id,
                                             string onChangeFunc)
        {
            try
            {
                string tplSelect = WebUtils.LoadTemplate(Globals.TPL_CTR_SELECTOPTION);
                var options = new StringBuilder();
                //int groupValue = Convert.ToInt32(maxValue * 100000);
                int groupValue = Convert.ToInt32(maxValue*10000);
                currentVal = Math.Round(currentVal*10000)/10000;
                maxValue = Math.Round(maxValue*10000)/10000;
                if (currentVal > maxValue)
                {
                    //currentVal = maxValue;
                    options.Append("<option value='" + currentVal + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                   Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + currentVal +
                                   "' selected='true'>" + currentVal + "</option>");
                }
                while (groupValue >= 0)
                {
                    //double selValue = Convert.ToDouble(groupValue) / 100000;
                    double selValue = Convert.ToDouble(groupValue)/10000;
                    if (currentVal == selValue)
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue +
                                       "' selected='true'>" + selValue + "</option>");
                    else
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "'>" +
                                       selValue + "</option>");
                    //groupValue = groupValue - 25;
                    groupValue = groupValue - interval;
                }

                TemplateHandler.Assign(ref tplSelect, "ddl-Id", id);
                TemplateHandler.Assign(ref tplSelect, "onchange", onChangeFunc);
                TemplateHandler.Assign(ref tplSelect, "options-val", options.ToString());
                TemplateHandler.Assign(ref tplSelect, "disabled", (disabled) ? "disabled='disabled'" : "");
                return tplSelect;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenSelectOption(double maxValue, double minValue, double currentVal, int interval,
                                             bool disabled, string id, string onChangeFunc)
        {
            try
            {
                string tplSelect = WebUtils.LoadTemplate(Globals.TPL_CTR_SELECTOPTION);
                var options = new StringBuilder();
                minValue = Math.Min(maxValue, minValue);
                //int groupValue = Convert.ToInt32(maxValue * 100000);
                int groupValue = Convert.ToInt32(maxValue*10000);
                int min = CastAide.AsInt32(minValue*10000);
                currentVal = Math.Max(currentVal, 0);
                maxValue = Math.Max(maxValue, 0);
                minValue = Math.Max(minValue, 0);
                currentVal = Math.Round(currentVal*10000)/10000;
                maxValue = Math.Round(maxValue*10000)/10000;
                minValue = Math.Round(minValue*10000)/10000;
                if (currentVal > maxValue)
                {
                    options.Append("<option value='" + currentVal + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                   Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + currentVal +
                                   "' selected='true'>" + currentVal + "</option>");
                }
                while (groupValue >= min)
                {
                    //double selValue = Convert.ToDouble(groupValue) / 100000;
                    double selValue = Convert.ToDouble(groupValue)/10000;
                    if (currentVal == selValue)
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue +
                                       "' selected='true'>" + selValue + "</option>");
                    else
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "'>" +
                                       selValue + "</option>");
                    //groupValue = groupValue - 25;
                    groupValue = groupValue - interval;
                }

                TemplateHandler.Assign(ref tplSelect, "ddl-Id", id);
                TemplateHandler.Assign(ref tplSelect, "onchange", onChangeFunc);
                TemplateHandler.Assign(ref tplSelect, "class", "casino");
                TemplateHandler.Assign(ref tplSelect, "options-val", options.ToString());
                TemplateHandler.Assign(ref tplSelect, "disabled", (disabled) ? "disabled='disabled'" : "");
                return tplSelect;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenSelectOption(double maxValue, double minValue, double currentVal, int interval,
                                             bool disabled, string id, string onChangeFunc, string classStyle)
        {
            try
            {
                string tplSelect = WebUtils.LoadTemplate(Globals.TPL_CTR_SELECTOPTION);
                var options = new StringBuilder();
                minValue = Math.Min(maxValue, minValue);
                //int groupValue = Convert.ToInt32(maxValue * 100000);
                int groupValue = Convert.ToInt32(maxValue*10000);
                int min = CastAide.AsInt32(minValue*10000);
                currentVal = Math.Max(currentVal, 0);
                maxValue = Math.Max(maxValue, 0);
                minValue = Math.Max(minValue, 0);
                currentVal = Math.Round(currentVal*10000)/10000;
                maxValue = Math.Round(maxValue*10000)/10000;
                minValue = Math.Round(minValue*10000)/10000;
                //if (currentVal > maxValue)
                //{
                //    currentVal = maxValue;
                //}
                if (currentVal > maxValue)
                {
                    //double selValue = Convert.ToInt32(currentVal * 10000) / 10000;
                    options.Append("<option value='" + currentVal + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                   Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + currentVal +
                                   "' selected='true'>" + currentVal + "</option>");
                }
                while (groupValue >= min)
                {
                    //double selValue = Convert.ToDouble(groupValue) / 100000;
                    double selValue = Convert.ToDouble(groupValue)/10000;
                    if (currentVal == selValue)
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue +
                                       "' selected='true'>" + selValue + "</option>");
                    else
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "'>" +
                                       selValue + "</option>");
                    //groupValue = groupValue - 25;
                    groupValue = groupValue - interval;
                }

                TemplateHandler.Assign(ref tplSelect, "ddl-Id", id);
                TemplateHandler.Assign(ref tplSelect, "onchange", onChangeFunc);
                TemplateHandler.Assign(ref tplSelect, "class", classStyle);
                TemplateHandler.Assign(ref tplSelect, "options-val", options.ToString());
                TemplateHandler.Assign(ref tplSelect, "disabled", (disabled) ? "disabled='disabled'" : "");
                return tplSelect;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        public static string GenSelectOption(double maxValue, double currentVal, int interval, bool disabled, string id,
                                             string onChangeFunc, string classStyle)
        {
            try
            {
                string tplSelect = WebUtils.LoadTemplate(Globals.TPL_CTR_SELECTOPTION);
                var options = new StringBuilder();
                //int groupValue = Convert.ToInt32(maxValue * 100000);
                int groupValue = Convert.ToInt32(maxValue*10000);
                currentVal = Math.Max(currentVal, 0);
                if (currentVal > maxValue)
                {
                    currentVal = maxValue;
                }
                while (groupValue >= 0)
                {
                    //double selValue = Convert.ToDouble(groupValue) / 100000;
                    double selValue = Convert.ToDouble(groupValue)/10000;
                    if (currentVal == selValue)
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue +
                                       "' selected='true'>" + selValue + "</option>");
                    else
                        options.Append("<option value='" + selValue + "' id='" + id + Globals.SEP_GAMEKIND_GAMETYPE +
                                       Globals.CTR_OPTIONS_PREFIX + Globals.SEP_GAMEKIND_GAMETYPE + selValue + "'>" +
                                       selValue + "</option>");
                    //groupValue = groupValue - 25;
                    groupValue = groupValue - interval;
                }

                TemplateHandler.Assign(ref tplSelect, "ddl-Id", id);
                TemplateHandler.Assign(ref tplSelect, "class", classStyle);
                TemplateHandler.Assign(ref tplSelect, "onchange", onChangeFunc);
                TemplateHandler.Assign(ref tplSelect, "options-val", options.ToString());
                TemplateHandler.Assign(ref tplSelect, "disabled", (disabled) ? "disabled='disabled'" : "");
                return tplSelect;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenCheckBox(string id, bool disabled, bool check, string funcClick)
        {
            try
            {
                /*string tplCheckBox = Sunplus.Accounting.Web.WebUtils.LoadTemplate(Globals.TPL_CTR_CHECKBOX);
            TemplateHandler.Assign(ref tplCheckBox, "chkId", id);
            TemplateHandler.Assign(ref tplCheckBox, "onclick", funcClick);
            TemplateHandler.Assign(ref tplCheckBox, "disabled", (disabled) ? "disabled='disabled'" : "");
            TemplateHandler.Assign(ref tplCheckBox, "checked", (check) ? "checked='checked'" : "");
            */

                return GenInput(Globals.InputType.CheckBox, id, "", "", disabled, check, funcClick);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Recommend to use this function
        /// </summary>
        /// <param name="id"></param>
        /// <param name="disabled"></param>
        /// <param name="check"></param>
        /// <param name="funcClick"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string GenCheckBox(string id, bool disabled, bool check, string funcClick, string style)
        {
            try
            {
                string tplCheckBox = WebUtils.LoadTemplate(Globals.TPL_CTR_CHECKBOX);
                TemplateHandler.Assign(ref tplCheckBox, "chkId", id);
                TemplateHandler.Assign(ref tplCheckBox, "onclick", funcClick);
                TemplateHandler.Assign(ref tplCheckBox, "disabled", (disabled) ? "disabled='disabled'" : "");
                TemplateHandler.Assign(ref tplCheckBox, "checked", (check) ? "checked='checked'" : "");
                TemplateHandler.Assign(ref tplCheckBox, "class", style);
                return tplCheckBox;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        public static string GenHidden(string id, string value)
        {
            try
            {
                /*string tplHidden = Sunplus.Accounting.Web.WebUtils.LoadTemplate(Globals.TPL_CTR_HIDDEN);
            TemplateHandler.Assign(ref tplHidden, "name", id);
            TemplateHandler.Assign(ref tplHidden, "strValue", value);
            */

                return GenInput(Globals.InputType.Hidden, id, value, "", false, false, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GenButton(string id, string value, string classStyle, bool disabled, string funcClick)
        {
            try
            {
                /*string tplButton = Sunplus.Accounting.Web.WebUtils.LoadTemplate(Globals.TPL_CTR_BUTTON);
            TemplateHandler.Assign(ref tplButton, "btnId", id);
            TemplateHandler.Assign(ref tplButton, "onclick", funcClick);
            TemplateHandler.Assign(ref tplButton, "class", classStyle);
            TemplateHandler.Assign(ref tplButton, "value", value);
            TemplateHandler.Assign(ref tplButton, "disabled", (disabled) ? "disabled='disabled'" : "");
            */
                return GenInput(Globals.InputType.Button, id, value, classStyle, disabled, false, funcClick);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenTextBox(string id, string value, string classStyle, bool disabled, string funcClick)
        {
            try
            {
                /*string tplButton = Sunplus.Accounting.Web.WebUtils.LoadTemplate(Globals.TPL_CTR_BUTTON);
            TemplateHandler.Assign(ref tplButton, "btnId", id);
            TemplateHandler.Assign(ref tplButton, "onclick", funcClick);
            TemplateHandler.Assign(ref tplButton, "class", classStyle);
            TemplateHandler.Assign(ref tplButton, "value", value);
            TemplateHandler.Assign(ref tplButton, "disabled", (disabled) ? "disabled='disabled'" : "");
            */
                return GenInput(Globals.InputType.Text, id, value, classStyle, disabled, false, funcClick);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenTextBox(string id, string name, string value, string classStyle, bool disabled,
                                        string funcClick)
        {
            try
            {
                /*string tplButton = Sunplus.Accounting.Web.WebUtils.LoadTemplate(Globals.TPL_CTR_BUTTON);
            TemplateHandler.Assign(ref tplButton, "btnId", id);
            TemplateHandler.Assign(ref tplButton, "onclick", funcClick);
            TemplateHandler.Assign(ref tplButton, "class", classStyle);
            TemplateHandler.Assign(ref tplButton, "value", value);
            TemplateHandler.Assign(ref tplButton, "disabled", (disabled) ? "disabled='disabled'" : "");
            */
                return GenInput(Globals.InputType.Text, id, name, value, classStyle, disabled, false, funcClick);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenTextBox(string id, string value, string classStyle, bool disabled, string funcClick,
                                        string funcKeydown)
        {
            try
            {
                return GenInput(Globals.InputType.Text, id, value, classStyle, disabled, false, funcClick, funcKeydown);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenChangePTButtons(string id, bool disabled)
        {
            try
            {
                string btn = "&nbsp;";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC1_SUFFIX, "",
                                 (disabled) ? Globals.BTN_DEC1_DISABLE_CLASS : Globals.BTN_DEC1_CLASS, disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_1));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC1_SUFFIX, "",
                                 (disabled) ? Globals.BTN_INC1_DISABLE_CLASS : Globals.BTN_INC1_CLASS, disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_1));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC10_SUFFIX, "",
                                 (disabled) ? Globals.BTN_DEC10_DISABLE_CLASS : Globals.BTN_DEC10_CLASS, disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_10));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC10_SUFFIX, "",
                                 (disabled) ? Globals.BTN_INC10_DISABLE_CLASS : Globals.BTN_INC10_CLASS, disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_10));
                return btn;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenChangePTButtonsHR(string id, bool disabled)
        {
            try
            {
                string btn = "&nbsp;";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC10_SUFFIX, "",
                                 (disabled) ? Globals.BTN_DEC10_DISABLE_CLASS : Globals.BTN_DEC10_CLASS, disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_10));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC10_SUFFIX, "",
                                 (disabled) ? Globals.BTN_INC10_DISABLE_CLASS : Globals.BTN_INC10_CLASS, disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_10));
                return btn;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenChangePTButtonsHR(string id, bool disabled, bool isFI)
        {
            try
            {
                string btn = "&nbsp;";

                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC10_SUFFIX, "",
                                 (disabled)
                                     ? Globals.BTN_DEC10_DISABLE_CLASS
                                     : Globals.BTN_DEC10_DISABLE_CLASS + " btn_hand", disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_10));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC10_SUFFIX, "",
                                 (disabled)
                                     ? Globals.BTN_INC10_DISABLE_CLASS
                                     : Globals.BTN_INC10_DISABLE_CLASS + " btn_hand", disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_10));
                return btn;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        public static string GenChangePTButtons(string id, bool disabled, bool isFI)
        {
            try
            {
                string btn = "&nbsp;";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC1_SUFFIX, "",
                                 (disabled) ? Globals.BTN_DEC1_DISABLE_CLASS : Globals.BTN_DEC1_DISABLE_CLASS + " btn_hand",
                                 disabled, string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_1));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC1_SUFFIX, "",
                                 (disabled) ? Globals.BTN_INC1_DISABLE_CLASS : Globals.BTN_INC1_DISABLE_CLASS + " btn_hand",
                                 disabled, string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_1));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_DEC10_SUFFIX, "",
                                 (disabled)
                                     ? Globals.BTN_DEC10_DISABLE_CLASS
                                     : Globals.BTN_DEC10_DISABLE_CLASS + " btn_hand", disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", -Globals.CONST_CHANGEPT_10));
                btn += "&nbsp";
                btn += GenButton(id + Globals.SEP_GAMEKIND_GAMETYPE + Globals.BTN_INC10_SUFFIX, "",
                                 (disabled)
                                     ? Globals.BTN_INC10_DISABLE_CLASS
                                     : Globals.BTN_INC10_DISABLE_CLASS + " btn_hand", disabled,
                                 string.Format(Globals.BTN_CHANGEPT_CLICK, "this", Globals.CONST_CHANGEPT_10));
                return btn;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenInput(Globals.InputType input, string id, string value, string classStyle, bool disabled,
                                      bool check, string funcClick, string onkeydown)
        {
            try
            {
                string tplInput = WebUtils.LoadTemplate(Globals.TPL_CTR_INPUT);

                switch (input)
                {
                    case Globals.InputType.Button:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Button));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        break;
                    case Globals.InputType.Hidden:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Hidden));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "class", "");
                        break;
                    case Globals.InputType.Text:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Text));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "onkeydown", onkeydown);
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        break;
                    case Globals.InputType.CheckBox:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.CheckBox));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", "");
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
                        break;
                }

                return tplInput;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenInput(Globals.InputType input, string id, string value, string classStyle, bool disabled,
                                      bool check, string funcClick)
        {
            try
            {
                string tplInput = WebUtils.LoadTemplate(Globals.TPL_CTR_INPUT);

                switch (input)
                {
                    case Globals.InputType.Button:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Button));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        break;
                    case Globals.InputType.Hidden:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Hidden));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "class", "");
                        break;
                    case Globals.InputType.Text:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Text));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        break;
                    case Globals.InputType.CheckBox:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.CheckBox));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", id);
                        TemplateHandler.Assign(ref tplInput, "value", "");
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
                        break;
                }

                return tplInput;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenInput(Globals.InputType input, string id, string name, string value, string classStyle,
                                      bool disabled, bool check, string funcClick)
        {
            try
            {
                string tplInput = WebUtils.LoadTemplate(Globals.TPL_CTR_INPUT);

                switch (input)
                {
                    case Globals.InputType.Button:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Button));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", name);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        break;
                    case Globals.InputType.Hidden:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Hidden));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", name);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "class", "");
                        break;
                    case Globals.InputType.Text:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.Text));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", name);
                        TemplateHandler.Assign(ref tplInput, "value", value);
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "checked", "");
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        break;
                    case Globals.InputType.CheckBox:
                        TemplateHandler.Assign(ref tplInput, "type", Globals.GetInputType(Globals.InputType.CheckBox));
                        TemplateHandler.Assign(ref tplInput, "inputId", id);
                        TemplateHandler.Assign(ref tplInput, "inputName", name);
                        TemplateHandler.Assign(ref tplInput, "value", "");
                        TemplateHandler.Assign(ref tplInput, "disabled", (disabled) ? "disabled='disabled'" : "");
                        TemplateHandler.Assign(ref tplInput, "onclick", funcClick);
                        TemplateHandler.Assign(ref tplInput, "class", classStyle);
                        TemplateHandler.Assign(ref tplInput, "checked", (check) ? "checked='checked'" : "");
                        break;
                }

                return tplInput;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenLabel(string id, string value, string classStyle)
        {
            try
            {
                string tplLabel = WebUtils.LoadTemplate(Globals.TPL_CTR_LABEL);
                TemplateHandler.Assign(ref tplLabel, "lblid", id);
                TemplateHandler.Assign(ref tplLabel, "class", classStyle);
                TemplateHandler.Assign(ref tplLabel, "value", value);

                return tplLabel;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenLink(string label, string href, string classStyle, string funcOnclick)
        {
            try
            {
                string tplLink = WebUtils.LoadTemplate(Globals.TPL_CTR_LINK);
                TemplateHandler.Assign(ref tplLink, "label", label);
                TemplateHandler.Assign(ref tplLink, "href", href);
                TemplateHandler.Assign(ref tplLink, "class", classStyle);
                TemplateHandler.Assign(ref tplLink, "onclick", funcOnclick);
                return tplLink;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenTD(int totalTD, int totalColSpan, string styleClass)
        {
            try
            {
                string tplTD = WebUtils.LoadTemplate("Controls/td.htm");
                string result = "";
                if (totalTD <= 1)
                {
                    result = tplTD;
                    TemplateHandler.Assign(ref result, "colspan", totalColSpan);
                    TemplateHandler.Assign(ref result, "class", styleClass);
                }
                else
                {
                    string tmpTD = tplTD;
                    for (int i = 0; i < totalTD; i++)
                    {
                        result += tmpTD;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}