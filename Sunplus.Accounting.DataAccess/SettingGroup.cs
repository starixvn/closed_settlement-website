using System.Collections;
using DNAide.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;

namespace Sunplus.Accounting.DataAccess
{
    /// <summary>
    /// Summary description for SettingGroup
    /// </summary>
    public class SettingGroup
    {
        private ArrayList settinggroup = new ArrayList();
        public void Add(SettingControl st)
        {
            settinggroup.Add(st);
        }
        public ArrayList ShowSettingControl()
        {
            ArrayList _st = new ArrayList();
            foreach (SettingControl s in settinggroup)
            {
                if (!s.Hidden)
                    _st.Add(s);
            }
            return _st;
        }
        public bool IsDisplay()
        {
            bool check = false;
            foreach (SettingControl s in settinggroup)
            {
                if (!s.Hidden)
                    check = true;
            }
            return check;
        }

        public string GetHTMLSettingControl()
        {

            string strsetting = "";
            string htmctl = "";
            string _htlctl = "";
            string lblagenttype = "";
            ArrayList arr = new ArrayList();
            htmctl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate(Globals.CUSTINFO_HTM_TEMPLATE);
            arr = ShowSettingControl();

            for (int i = 0; i < arr.Count; i += 2)
            {
                SettingControl control = (SettingControl)arr[i];

                _htlctl = (string)htmctl.Clone();
                TemplateHandler.Assign(ref _htlctl, "{lblL}", control.Lable);

                if (Utils.IsBooleanType(control.Value))
                {
                    //TemplateHandler.Assign(ref _htlctl, "{L}", (bool)control.Value == true ? "No" : "Yes");
                    TemplateHandler.Assign(ref _htlctl, "{L}", "");
                    TemplateHandler.Assign(ref _htlctl, "{classnameL}", (bool)control.Value == true ? "chkchecked" : "chkuncheck");
                }
                else
                {
                    TemplateHandler.Assign(ref _htlctl, "{L}", control.Value.ToString());
                    TemplateHandler.Assign(ref _htlctl, "{classnameL}", control.CSSClass);
                }

                TemplateHandler.Assign(ref _htlctl, "{jsonclickL}", control.JSOnClick);
                TemplateHandler.Assign(ref _htlctl, "{jsonmouseoverL}", control.JSOnMouseOver);
                TemplateHandler.Assign(ref _htlctl, "{jsonmouseoutL}", control.JSOnMouseOut);
                //TemplateHandler.Assign(ref _htlctl, "{classnameL}", control.CSSClass);
                TemplateHandler.Assign(ref _htlctl, "{lblidL}", control.StatusName.Trim().Replace(" ", ""));
                TemplateHandler.Assign(ref _htlctl, "{lblPopUpTitleL}", control.Lable);
                TemplateHandler.Assign(ref _htlctl, "{typeL}", control.Type);
                if (Utils.IsBooleanType(control.Value))
                {
                    TemplateHandler.Assign(ref _htlctl, "{statusL}", (bool)control.Value == true ? "checked='true'" : "");
                    TemplateHandler.Assign(ref _htlctl, "{oldvalueL}", (bool)control.Value == true ? "1" : "0");
                }

                if (control.Type == "SelectedBetting")
                {
                    if ((Utils.IsBooleanType(control.Value) && (bool)control.Value))
                    {
                        TemplateHandler.Assign(ref _htlctl, "{oldvalueDLL}", "1");
                        TemplateHandler.Assign(ref _htlctl, "{statusDLL}", "checked");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _htlctl, "{oldvalueDLL}", "0");
                        TemplateHandler.Assign(ref _htlctl, "{statusDLL}", "");
                    }

                    if (control.RoleID < 3)
                    {
                        TemplateHandler.RemoveBlock(ref _htlctl, "chkdownlineL");
                    }
                }
                else
                {
                    if ((Utils.IsBooleanType(control.Value) && !(bool)control.Value) || !control.IsDisableDownlines)
                    {
                        TemplateHandler.Assign(ref _htlctl, "{oldvalueDLL}", "0");
                        TemplateHandler.Assign(ref _htlctl, "{statusDLL}", "");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _htlctl, "{oldvalueDLL}", "1");
                        TemplateHandler.Assign(ref _htlctl, "{statusDLL}", "checked='true'");
                    }

                    if (control.RoleID == 1)
                    {
                        TemplateHandler.RemoveBlock(ref _htlctl, "chkdownlineL");
                    }
                }



                //TemplateHandler.Assign(ref _htlctl, "{statusDLL}", control.IsDisableDownlines ? "checked='true'" : "");
                lblagenttype = Sunplus.Accounting.Core.Common.Constants.GetNameByRoleId(control.RoleID);

                TemplateHandler.Assign(ref _htlctl, "{lblagenttypeL}", lblagenttype);
                TemplateHandler.Assign(ref _htlctl, "{lblagenttypeDLL}", (control.Type == "CS" ? "Casino" : lblagenttype) + "'s downlines");




                if (i + 1 < arr.Count)
                {
                    SettingControl nextControl = (SettingControl)arr[i + 1];

                    if (Utils.IsBooleanType(nextControl.Value))
                        TemplateHandler.Assign(ref _htlctl, "{statusR}", (bool)nextControl.Value == true ? "checked='true'" : "");
                    TemplateHandler.Assign(ref _htlctl, "{typeR}", nextControl.Type);
                    lblagenttype = Sunplus.Accounting.Core.Common.Constants.GetNameByRoleId(nextControl.RoleID);

                    TemplateHandler.Assign(ref _htlctl, "{lblagenttypeR}", lblagenttype);
                    TemplateHandler.Assign(ref _htlctl, "{lblagenttypeDLR}", (nextControl.Type == "CS" ? "Casino" : lblagenttype) + "'s downlines");

                    TemplateHandler.Assign(ref _htlctl, "{lblR}", nextControl.Lable);
                    if (Utils.IsBooleanType(nextControl.Value))
                    {
                        TemplateHandler.Assign(ref _htlctl, "{R}", "");
                        TemplateHandler.Assign(ref _htlctl, "{classnameR}", (bool)nextControl.Value == true ? "chkchecked" : "chkuncheck");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref _htlctl, "{R}", nextControl.Value.ToString());
                        TemplateHandler.Assign(ref _htlctl, "{classnameR}", nextControl.CSSClass);
                    }


                    TemplateHandler.Assign(ref _htlctl, "{jsonclickR}", nextControl.JSOnClick);
                    TemplateHandler.Assign(ref _htlctl, "{jsonmouseoverR}", nextControl.JSOnMouseOver);
                    TemplateHandler.Assign(ref _htlctl, "{jsonmouseoutR}", nextControl.JSOnMouseOut);
                    //TemplateHandler.Assign(ref _htlctl, "{classnameR}", nextControl.CSSClass);
                    TemplateHandler.Assign(ref _htlctl, "{lblidR}", nextControl.StatusName.Trim().Replace(" ", ""));
                    TemplateHandler.Assign(ref _htlctl, "{lblPopUpTitleR}", nextControl.Lable);
                    if (Utils.IsBooleanType(nextControl.Value))
                        TemplateHandler.Assign(ref _htlctl, "{oldvalueR}", (bool)nextControl.Value == true ? "1" : "0");

                    if (nextControl.Type == "SelectedBetting")
                    {
                        if ((Utils.IsBooleanType(nextControl.Value) && (bool)nextControl.Value))
                        {
                            TemplateHandler.Assign(ref _htlctl, "{oldvalueDLR}", "1");
                            TemplateHandler.Assign(ref _htlctl, "{statusDLR}", "checked");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _htlctl, "{oldvalueDLR}", "0");
                            TemplateHandler.Assign(ref _htlctl, "{statusDLR}", "");
                        }

                        if (nextControl.RoleID < 3)
                        {
                            TemplateHandler.RemoveBlock(ref _htlctl, "chkdownlineR");
                        }
                    }
                    else
                    {

                        if ((Utils.IsBooleanType(nextControl.Value) && !(bool)nextControl.Value) || !nextControl.IsDisableDownlines)
                        {
                            TemplateHandler.Assign(ref _htlctl, "{oldvalueDLR}", "0");
                            TemplateHandler.Assign(ref _htlctl, "{statusDLR}", "");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _htlctl, "{oldvalueDLR}", "1");
                            TemplateHandler.Assign(ref _htlctl, "{statusDLR}", "checked='true'");
                        }

                        if (nextControl.RoleID == 1)
                        {
                            TemplateHandler.RemoveBlock(ref _htlctl, "chkdownlineR");
                        }
                    }



                }
                else
                {
                    TemplateHandler.Assign(ref _htlctl, "{lblR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{R}", "");
                    TemplateHandler.Assign(ref _htlctl, "{jsonclickR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{jsonmouseoverR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{jsonmouseoutR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{classnameR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{lblidR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{lblPopUpTitleR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{lblagenttypeR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{lblagenttypeDLR}}", "");
                    TemplateHandler.Assign(ref _htlctl, "{statusR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{oldvalueR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{oldvalueDLR}", "");
                    TemplateHandler.Assign(ref _htlctl, "{statusDLR}", "");
                    TemplateHandler.RemoveBlock(ref _htlctl, "removeable");

                }
                strsetting += _htlctl;
            }
            return strsetting;

        }

        //string GetSelectBettingPopupHTML(SettingControl control)
        //{
        //    string s = IBC.Accounting.Web.WebUtils.LoadTemplate("SettingSelectedBetting.htm");

        //    TemplateHandler.Assign(ref s, "{onmouseout}", control.JSOnMouseOut);
        //    TemplateHandler.Assign(ref s, "{onmouseover}", control.JSOnMouseOver);
        //    TemplateHandler.Assign(ref s, "{classname}", (control.Value) ? "chkchecked" : "chkuncheck");
        //    TemplateHandler.Assign(ref s, "{onclick}", control.JSOnClick );
        //    TemplateHandler.Assign(ref s, "{lblSelectedBetting}", control.Lable );

        //    string lblagenttype = Sunplus.Accounting.Common.Constants.GetNameByRoleId(control.RoleID);

        //    TemplateHandler.Assign(ref s, "{lblSelectedBettingCurrent}", lblagenttype);
        //    TemplateHandler.Assign(ref s, "{lblSelectedBettingDL}", lblagenttype + "'s downline");
        //    if (control.Value)
        //    {
        //        TemplateHandler.Assign(ref s, "{checked}", "checked='false'");
        //        TemplateHandler.Assign(ref s, "{disabledDL}", "");
        //    }
        //    else
        //    { 
        //        TemplateHandler.Assign(ref s, "{checked}", "checked='true'");
        //        TemplateHandler.Assign(ref s, "{disabledDL}", "disabled='disabled'");

        //    }
        //    TemplateHandler.Assign(ref s, "{btonclick}", "return updateSelectedBetting();");
        //    return s;
        //}
    }
}
