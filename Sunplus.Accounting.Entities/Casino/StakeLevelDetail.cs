 /// <summary>
/// Summary description for StakeLevelDetail
/// </summary>

namespace Sunplus.Accounting.Entities.Casino
{
    public class StakeLevelDetail
    {
        public StakeLevelDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public StakeLevelDetail(int _StakeLevelID, string _StakeLevelName, bool _IsChecked)
        {
            this.StakeLevelID = _StakeLevelID;
            this.StakeLevelName = _StakeLevelName;
            this.IsChecked = _IsChecked;
        }

        public int StakeLevelID { get; set; }

        public string StakeLevelName { get; set; }

        public bool IsChecked { get; set; }
    }
}