using System;
using DNAide.Core;

namespace Sunplus.Accounting.DataAccess.IBCDLL
{
    public class SystemInfo
    {
        public static DateTime GetDBTime()
        {
            DNAide.Data.DbResult dr = null;
            DateTime loginTime;
            var p = new System.Collections.Hashtable();
            p.Add("logintime", null);
            try
            {
                dr = DNAide.Data.DbHelper.Execute("Age_LoginAdminVerifyTicket", p, DBType.MAIN);
                loginTime = CastAide.AsDateTime(dr.Outputs["logintime"]);
            }
            catch { throw; }

            return loginTime;
        }
    }
}
