using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Configurations;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace Sunplus.Accounting.Core.Helper.IP
{
    public class IpManager
    {

        private List<IPVNBean> _listVnIp;

        public IpManager()
        {
            
        }

        public IpManager(List<IPVNBean> listVnIp)
        {
            _listVnIp = listVnIp;
            
        }

        private bool IsVnIp(string iP)
        {
            var ipNumber = Ip2Number(iP);
            var result = _listVnIp.Where(ip => Convert.ToDouble(ip.IPFrom) <= ipNumber && Convert.ToDouble(ip.IPTo) >= ipNumber);
            
            return result.Count() > 0;
        }

        //Convert IP to nummber
        private double Ip2Number(string iP)
        {
            try
            {
                double longIP = 0;
                if ((IsValidIp(iP)))
                {
                    var sIP = iP.Split('.');
                    longIP = double.Parse(sIP[0]) * 16777216 + double.Parse(sIP[1]) * 65536 + double.Parse(sIP[2]) * 256 +
                             double.Parse(sIP[3]);
                }
                return longIP;
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }

        //Replace VN IP with other one or empty string
        public string ReplaceVnIp(string iP, string replaceString)
        {
            return IsVnIp(iP) ? replaceString : iP;
        }


        //Check input string is a valid IP4 format
        private  bool IsValidIp(string addr)
        {
            const string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            var check = new Regex(pattern);
            bool valid = false;
            valid = addr != "" && check.IsMatch(addr, 0);
            return valid;
        }

        public string GetCountryNameByIp(string IpAddress)
        {
            string countryName = "";
            try
            {
                string url = (WebSettings.getCountryName.StartsWith("http") ? WebSettings.getCountryName : "http://" + WebSettings.getCountryName) + "?ip=" + IpAddress;
                /*var webClient = new System.Net.WebClient();
                string r = webClient.DownloadString(url);
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(r);
                var xElement = xmlDoc.GetElementsByTagName("countryName");
                countryName = xElement[0].InnerText;*/
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string responseContent = reader.ReadToEnd();
                StringReader sr = new StringReader(responseContent);
                XElement respElement = XElement.Load(sr);
                countryName = (string)respElement.Element("CountryName");

               
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return countryName;
        }


    }
}
