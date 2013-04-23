using System.Collections.Generic;

namespace Fanex.Tarus.Entities.Customer.DepositCustomer
{
    public class NewLicenseePostParam
    {
        public int SuperId { get; set; }
        public string SuperName { get; set; }

        public int MasterId { get; set; }
        public string MasterName { get; set; }

        public int AgentId { get; set; }
        public string AgentName { get; set; }

        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public List<PostParam> PostParams { get; set; }
    }
}
