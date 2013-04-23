using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.User.Entities
{
    public class UserLog
    {
        public int LogId { get; set; }
        public int TypeId { get; set; }
        public string LogDate { get; set; }
        public int AccId { get; set; }
        public string AccName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string Doing { get; set; }
        public LogGroupInfo GroupInfo { get; set; }
        public LogAdminPolicy AdminPolicy { get; set; }
        public LogGroupPolicy GroupPolicy { get; set; }
        public LogAccountInfo AccountInfo { get; set; }
        public string ToGroup { get; set; }
        public int PagesTotal { get; set; }
        public string Status { get; set; }
        public string GroupDescription { get; set; }
        public object NewAccountInfo { get; set; }
    }
}
