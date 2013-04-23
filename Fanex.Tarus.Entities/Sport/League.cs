
using System.Collections.Generic;

namespace Fanex.Tarus.Entities.Sport
{
    public class League : BaseModel
    {       
        public List<Match> Matches { get; set; }
    }
}
