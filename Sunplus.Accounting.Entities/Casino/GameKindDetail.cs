 /// <summary>
/// Summary description for GameKindDetail
/// </summary>

namespace Sunplus.Accounting.Entities.Casino
{
    public class GameKindDetail
    {
        private double _MasterMPositionTaking;


        public GameKindDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public GameKindDetail(int _CodeID, string _CodeNo, string _CodeName, string _CodeSName)
        {
            this.CodeID = _CodeID;
            this.CodeNo = _CodeNo;
            this.CodeName = _CodeName;
            this.CodeSName = _CodeSName;
        }

        public int CodeID { get; set; }

        public string CodeNo { get; set; }

        public string CodeName { get; set; }

        public string CodeSName { get; set; }

        public double SPositionTaking { get; set; }

        public double MPositionTaking { get; set; }

        public double APositionTaking { get; set; }

        public double AutoPositionTaking { get; set; }

        public double MinMPositionTaking { get; set; }

        public double MaxPositionTaking1 { get; set; }

        public double MaxPositionTaking { get; set; }

        public double MinPositionTaking { get; set; }

        public double SMAgentRollingRate { get; set; }

        public double MAgentRollingRate { get; set; }

        public double AgentRollingRate { get; set; }

        public double MemberRollingRate { get; set; }

        public bool IsAutoPositionTaking { get; set; }

        //Upline Information
        public double SPositionTakingUpline { get; set; }

        public double MPositionTakingUpline { get; set; }

        public double APositionTakingUpline { get; set; }

        public double AutoPositionTakingUpline { get; set; }

        public double MinMPositionTakingUpline { get; set; }

        public double MaxPositionTakingUpline { get; set; }

        public double SuperMaxPositionTaking { get; set; }

        public double SuperMinPositionTaking { get; set; }

        public double MasterMaxPositionTaking { get; set; }

        public double MasterMinPositionTaking { get; set; }

        public double MinPositionTakingUpline { get; set; }

        public double SMAgentRollingRateUpline { get; set; }

        public double MAgentRollingRateUpline { get; set; }

        public double AgentRollingRateUpline { get; set; }

        public double MemberRollingRateUpline { get; set; }

        public bool IsAutoPositionTakingUpline { get; set; }
    }
}