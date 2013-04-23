using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities
{
    public class ResultNG
    {
        public ResultNG()
        {
            ErrorCode = "";
            ErrorMessage = "Successful";
        }

        public ResultNG(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
