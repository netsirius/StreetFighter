using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFighter
{
    public class Match
    {
        public int MatchId { get; set; }
        public int CityId { get; set; }
        public int PlayerOneId { get; set; }
        public int PlayerTwoId { get; set; }
        public string Winner { get; set; }

        public Match(int matchId, int cityId, int playerOneId, int playerTwoId, string winner)
        {
            MatchId = matchId;
            CityId = cityId;
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            Winner = winner;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
