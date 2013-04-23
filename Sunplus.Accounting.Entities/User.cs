namespace Sunplus.Accounting.Entities
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User
    {
        private string _userName;
        public int Id { get; set; }

        public string Username
        {
            get
            {
                if (_userName == "0")
                    return string.Empty;

                return _userName;
            }
            set { _userName = value; }
        }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public string LastLoginIP { get; set; }
    }
}