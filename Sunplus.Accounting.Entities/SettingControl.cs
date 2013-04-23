using System;

namespace Sunplus.Accounting.Entities
{
    [Serializable]
    public class SettingControl
    {

        private string _cssclass = "";
        private bool _hidden = true;
        private bool _isdisabledownlines = true;
        private string _jsonclick = "";
        private string _jsonmouseout = "";
        private string _jsonmouseover = "";
        private string _lable = "";
        private int _roleid = 1;
        private string _statusname;
        private string _type = "";
        private object _value = false;


        public SettingControl(string lable, bool value, bool hidden, string jsonclick, string jsonmouseout,
                              string jsonmouseover, string cssclass, string statusname, int roleid, bool isdisabledownlines,
                              string type)
        {
            _lable = lable;
            _value = value;
            _hidden = hidden;
            _jsonclick = jsonclick;
            _jsonmouseout = jsonmouseout;
            _jsonmouseover = jsonmouseover;
            _cssclass = cssclass;
            _statusname = statusname;
            _roleid = roleid;
            _isdisabledownlines = isdisabledownlines;
            _type = type;
        }

        public SettingControl(string lable, string value, bool hidden, string jsonclick, string jsonmouseout,
                              string jsonmouseover, string cssclass, string statusname, int roleid, bool isdisabledownlines,
                              string type)
        {
            _lable = lable;
            _value = value;
            _hidden = hidden;
            _jsonclick = jsonclick;
            _jsonmouseout = jsonmouseout;
            _jsonmouseover = jsonmouseover;
            _cssclass = cssclass;
            _statusname = statusname;
            _roleid = roleid;
            _isdisabledownlines = isdisabledownlines;
            _type = type;
        }

        public string Lable
        {
            get { return _lable; }
            set { _lable = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        public string JSOnClick
        {
            get { return _jsonclick; }
            set { _jsonclick = value; }
        }

        public string JSOnMouseOut
        {
            get { return _jsonmouseout; }
            set { _jsonmouseout = value; }
        }

        public string JSOnMouseOver
        {
            get { return _jsonmouseover; }
            set { _jsonmouseover = value; }
        }

        public string CSSClass
        {
            get { return _cssclass; }
            set { _cssclass = value; }
        }

        public string StatusName
        {
            get { return _statusname; }
            set { _statusname = value; }
        }

        public int RoleID
        {
            get { return _roleid; }
            set { _roleid = value; }
        }

        public bool IsDisableDownlines
        {
            get { return _isdisabledownlines; }
            set { _isdisabledownlines = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}