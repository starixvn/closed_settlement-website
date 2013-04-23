namespace Sunplus.Accounting.Business.Others
{
    /// <summary>
    /// Summary description for MeetingComm
    /// </summary>
    public class MeetingHeader
    {
        private string _CommissionPercent = "Commission (%)";
        private string _Meeting = "Meeting";
        private string _Place = "Place";
        private string _ProgramID = "ProgramID";
        private string _Type = "Comm Type";
        private string _Win = "Win";
        private string _WinPlace = "Win Place";
    

        public string CommissionPercent
        {
            get { return _CommissionPercent; }
            set { _CommissionPercent = value; }
        }

        public string Meeting
        {
            get { return _Meeting; }
            set { _Meeting = value; }
        }

        public string ProgramID
        {
            get { return _ProgramID; }
            set { _ProgramID = value; }
        }

        public string Win
        {
            get { return _Win; }
            set { _Win = value; }
        }

        public string Place
        {
            get { return _Place; }
            set { _Place = value; }
        }

        public string WinPlace
        {
            get { return _WinPlace; }
            set { _WinPlace = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }
    public class MeetingDetail
    {
        public MeetingDetail(int Num, string Meeting, string ProgramID, string PoolCurrency)
        {
            this.Num = Num;
            this.Meeting = Meeting;
            this.ProgramID = ProgramID;
            this.PoolCurrency = PoolCurrency;
        }

        public int Num { get; set; }

        public string Meeting { get; set; }

        public string ProgramID { get; set; }

        public string PoolCurrency { get; set; }
    }

    public class MeetingPartner
    {
        public MeetingPartner(string Partner, double Win, double Place, double WinPlace, double Fees)
        {
            this.Partner = Partner;
            this.Win = Win;
            this.Place = Place;
            this.WinPlace = WinPlace;
            this.Fees = Fees;
        }

        public string Partner { get; set; }

        public double Win { get; set; }

        public double Place { get; set; }

        public double WinPlace { get; set; }

        public double Fees { get; set; }
    }
}