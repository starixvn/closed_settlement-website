using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities
{
    [DataContract]
    public class ResultBO
    {

        public ResultBO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ResultBO(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        [DataMember]
        public int ErrorCode
        { get; set; }
        [DataMember]
        public string ErrorMessage
        { get; set; }
    }

}

