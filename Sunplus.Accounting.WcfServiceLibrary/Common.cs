using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Sunplus.Accounting.WcfServiceLibrary
{
    public class Common
    {
        public static string getClientIP()
        {
            //WebOperationContext webContext = WebOperationContext.Current;

            OperationContext context = OperationContext.Current;

            MessageProperties messageProperties = context.IncomingMessageProperties;

            RemoteEndpointMessageProperty endpointProperty =

                messageProperties[RemoteEndpointMessageProperty.Name]

                as RemoteEndpointMessageProperty;
            return endpointProperty.Address;
        }
    }
}
