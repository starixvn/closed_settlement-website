using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sunplus.Data.DataAbstract.DAO;
using Sunplus.Data.DataAbstract.BootStrappers;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.Business
{
    public class IpManager
    {
        private IIpVnDAO _ipDao;
        private List<IpVn> _listIp;

        public IpManager()
        {
            _ipDao = CommonBootStrapper.Locator.GetInstance<IIpVnDAO>();
            _listIp = _ipDao.GetVnIpCollection();
            
        }

        private bool IsVnIp(string iP)
        {
            var ipNumber = Ip2Number(iP);
            var result = _listIp.Exists(ip => Convert.ToDouble(ip.IpFrom) <= ipNumber && Convert.ToDouble(ip.IpTo) >= ipNumber);
            
            return result;
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


    }
}
