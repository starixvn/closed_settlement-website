

namespace Sunplus.Accounting.Entities
{
    public class PositionTakingDetail
    {
        public PositionTakingDetail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public PositionTakingDetail(string ptName, object value)
        {
            PTName = ptName;
            PTValue = value;
        }

        public string PTName { get; set; }

        public object PTValue { get; set; }
    }
}