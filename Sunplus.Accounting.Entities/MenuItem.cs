namespace Sunplus.Accounting.Entities
{
    public class MenuItem
    {
        private string _name = "";
        private string _target = "";
        private string _url = "";

        public MenuItem(string url, string name, string target)
        {
            _url = url;
            _name = name;
            _target = target;
        }

        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }
    }
}