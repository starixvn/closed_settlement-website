
namespace Fanex.Tarus.Entities.Sport
{
    public class Match : BaseModel
    {
        /// <summary>
        /// Matchcode
        /// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        public string HomeId { get; set; }
        public string AwayId { get; set; }

        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        public string ShowTime { get; set; }

        public int SportTypeId { get; set; }
        public string SportTypeName { get; set; }
    }
}
