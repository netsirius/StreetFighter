using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFighter
{
    class Match
    {
        public int MatchId { get; private set; }
        public int CityId { get; private set; }
        public int PlayerOneId { get; private set; }
        public int PlayerTwoId { get; private set; }
        public string Winner { get; private set; }
    }
}
