using System.Collections;


namespace Sunplus.Accounting.Entities
{
    public class Menu
    {
        public enum CSSFloatType
        {
            none = 0, left = 1, right = 2
        }

        public static string GetCSSFloatType(CSSFloatType floattype)
        {
            switch (floattype)
            {
                case CSSFloatType.left:
                    return "left";
                case CSSFloatType.right:
                    return "right";
                default:
                    return "none";
            }
        }
        private readonly ArrayList _listsubmenu = new ArrayList();
        private string _classname = "";
        private string _floatstyle = "none";
        private string _jsfunc = "";
        private string _left = "";
        private string _name = "";
        private string _submenuid = "";
        private string _subwidth = "";
        private string _target = "";
        private string _url = "";
        private string _width = "";

        public Menu(string name, string url, string target, string submenuid, string classname, string width,
                    string subwidth, string left, string floatstyle, string jsfunc)
        {
            _name = name;
            _submenuid = submenuid;
            _classname = classname;
            _floatstyle = floatstyle;
            _width = width;
            _left = left;
            _url = url;
            _target = target;
            _jsfunc = jsfunc;
        }

        public Menu(string name, string url, string target, string submenuid, string classname, string width,
                    string subwidth, string left, CSSFloatType floatstyle, string jsfunc)
        {
            _name = name;
            _submenuid = submenuid;
            _classname = classname;
            _floatstyle = GetCSSFloatType(floatstyle);
            _width = width;
            _subwidth = subwidth;
            _left = left;
            _url = url;
            _target = target;
            _jsfunc = jsfunc;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public string SubMenuId
        {
            get { return _submenuid; }
            set { _submenuid = value; }
        }

        public string ClassName
        {
            get { return _classname; }
            set { _classname = value; }
        }

        public string FloatStyle
        {
            get { return _floatstyle; }
            set { _floatstyle = value; }
        }

        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string SubWidth
        {
            get { return _subwidth; }
            set { _subwidth = value; }
        }

        public string Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public string JSFunc
        {
            get { return _jsfunc; }
            set { _jsfunc = value; }
        }

        public void AddItem(MenuItem item)
        {
            _listsubmenu.Add(item);
        }

        public void AddItems(ArrayList items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                AddItem((MenuItem)items[i]);
            }
        }


        
    }
}