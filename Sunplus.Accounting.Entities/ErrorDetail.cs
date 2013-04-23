namespace Sunplus.Accounting.Entities
{
    public class ErrorDetail
    {
        private string _errCode = "";
        private int _errorCode = -99;
        private string _errorMessage = "Unknow Error";

        public ErrorDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ErrorDetail(int errorCode, string errorMsg)
        {
            _errorCode = errorCode;
            _errorMessage = errorMsg;
        }

        public ErrorDetail(string errCode, string errorMsg)
        {
            _errCode = errCode;
            _errorMessage = errorMsg;
        }

        public int ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public string ErrCode
        {
            get { return _errCode; }
            set { _errCode = value; }
        }
    }
}