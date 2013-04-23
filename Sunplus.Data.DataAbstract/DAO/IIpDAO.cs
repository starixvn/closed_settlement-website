using System.Collections.Generic;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.DAO
{
    public interface IIpVnDAO
    {
        List<IpVn> GetVnIpCollection();
        bool InsertIp(IpVn ipDetail);
        IpVn GetIp(string ipId);
        bool UpdateIp(IpVn ipDetail);
        bool DeleteIp(string ipId);
    }
}