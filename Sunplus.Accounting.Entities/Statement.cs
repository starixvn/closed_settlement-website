using System;

namespace Sunplus.Accounting.Entities
{
    /// <summary>
    /// Summary description for Statement
    /// </summary>
    public class Statement
    {
        public DateTime CreatedDate { get; set; }

        public double TotalAmount { get; set; }

        public int TransTypeId { get; set; }
        public string TransType { get; set; }

        public string TransDescription { get; set; }

        public User Creator { get; set; }

        public User Checker { get; set; }

        public bool IsAllowedVerified { get; set; }

        public DateTime? CheckedTime { get; set; }

        public long RefNo { get; set; }

        public bool IsVerified
        {
            get { return !ReferenceEquals(null, Checker); }
        }
    }
}
