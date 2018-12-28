using StreetFighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class StreetFighterResult
    {
        public List<Player> playersList { get; set; }
        public List<City> citiesList { get; set; }
        public List<Match> matchesList { get; set; }
        public StreetFighterResult(List<Player> playersList, List<City> citiesList, List<Match> matchesList)
        {
            this.playersList = playersList;
            this.citiesList = citiesList;
            this.matchesList = matchesList;
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Count and group matches by city and print the city where more mathes have been played.
        public void CityWithMoreMatches()
        {
            var cityWithMoreMatches = (from match in matchesList
                                       join city in citiesList
                                       on match.CityId equals city.Id
                                       group match by city into cityGroup
                                       let matchCount = cityGroup.Count()
                                       orderby matchCount descending
                                       select new
                                       {
                                           CityId = cityGroup.Key.Id,
                                           CityName = cityGroup.Key.Name,
                                           Matches = matchCount
                                       }).First();

            Console.WriteLine("City where more matches have been played:");
            Console.WriteLine("CityInfo -> Id: {0} Name: {1} with {2} matches \n", cityWithMoreMatches.CityId, cityWithMoreMatches.CityName,
                cityWithMoreMatches.Matches);
        }

        // Count and group matches by city where the match result is a draw and print the cities info.
        public void CitiesWithMoreDrawMatches()
        {
            var citiesWithMoreDrawMatches = from match in matchesList
                                            join city in citiesList
                                            on match.CityId equals city.Id
                                            where match.Winner.Equals("Draw")
                                            group match by city into cityGroup
                                            let matchesDrawCount = cityGroup.Count()
                                            orderby matchesDrawCount descending
                                            select new
                                            {
                                                CityId = cityGroup.Key.Id,
                                                CityName = cityGroup.Key.Name,
                                                MatchesDraw = matchesDrawCount
                                            };

            int maxDrawMatches = citiesWithMoreDrawMatches.First().MatchesDraw;
            Console.WriteLine("Cities with more draws:");
            foreach (var result in citiesWithMoreDrawMatches)
            {
                if (result.MatchesDraw == maxDrawMatches)
                {
                    Console.WriteLine("CityInfo -> Id: {0} Name: {1} with {2} matches draw \n", result.CityId, result.CityName,
                    result.MatchesDraw);
                }
            }
        }

        // Compute the best player with best win ratio
        public void BestPlayerByWinRatio()
        {
            var playerMatchInfo = from player in playersList
                                  from match in matchesList
                                  where player.Id == match.PlayerOneId || player.Id == match.PlayerTwoId
                                  group match by player into playerMatchGroup
                                  let winRatio = (float)playerMatchGroup.Count(m => PlayerWin(playerMatchGroup.Key, m)) / (float)playerMatchGroup.Count()
                                  orderby winRatio descending
                                  select new
                                  {
                                      Player = playerMatchGroup.Key,
                                      WinRatio = winRatio,
                                  };
            var results = playerMatchInfo.Where(player => player.WinRatio == playerMatchInfo.First().WinRatio);
            foreach (var result in results)
            {
                Console.WriteLine("Player with the highest win ratio -> Id: {0} Name: {1} with {2} win ratio \n", result.Player.Id, result.Player.Name,
                    result.WinRatio);
            }
        }

        // Compute the players classification taking into account wined matches as 1 point, draw as 0.5
        public void ComputePlayersClassification()
        {
            var classification = from player in playersList
                                 from match in matchesList
                                 where player.Id == match.PlayerOneId || player.Id == match.PlayerTwoId
                                 group match by player into playerMatchGroup
                                 let playerPoints = (float)playerMatchGroup.Count(m => PlayerWin(playerMatchGroup.Key, m)) * 1.0 +
                                                       (float)playerMatchGroup.Count(m => m.Winner.Equals("Draw")) * 0.5
                                 orderby playerPoints descending
                                 select new
                                 {
                                     Player = playerMatchGroup.Key,
                                     Points = playerPoints,
                                 };
            int counter = 1;
            foreach (var result in classification)
            {
                Console.WriteLine("{0}.- Id: {1} Player name: {2} Points: {3}", counter++, result.Player.Id, result.Player.Name, result.Points);
            }
        }

        // Compute the best player with more matches wined
        public void PlayerWithMoreWins()
        {
            var playerMatchInfo = from player in playersList
                                  from match in matchesList
                                  where player.Id == match.PlayerOneId || player.Id == match.PlayerTwoId
                                  group match by player into playerMatchGroup
                                  let playerWins = (float)playerMatchGroup.Count(m => PlayerWin(playerMatchGroup.Key, m))
                                  orderby playerWins descending
                                  select new
                                  {
                                      Player = playerMatchGroup.Key,
                                      Wins = playerWins,
                                  };
            var results = playerMatchInfo.Where(player => player.Wins == playerMatchInfo.First().Wins);
            foreach (var result in results)
            {
                Console.WriteLine("\nPlayer with more matches wined -> Id: {0} Name: {1} with {2} matches wined\n", result.Player.Id, result.Player.Name,
                    result.Wins);
            }
        }

        // Compute the cities classification
        public void ComputeCitiesClassification()
        {
            var classification = from city in citiesList
                                 join match in matchesList
                                 on city.Id equals match.CityId
                                 group match by city into matchesGroup
                                 let cityPoints = matchesGroup.Count(m => (m.Winner.Equals("Player1") || m.Winner.Equals("Player2"))) * 1.0 +
                                                   matchesGroup.Count(m => m.Winner.Equals("Draw"))
                                 orderby cityPoints descending
                                 select new
                                 {
                                     City = matchesGroup.Key,
                                     Points = cityPoints,
                                };

            int counter = 1;
            foreach (var result in classification)
            {
                Console.WriteLine("{0}.- Id: {1} City name: {2} Points: {3}", counter++, result.City.Id, result.City.Name, result.Points);
            }
        }

        //Compute the worst streak
        public void PlayerWithWorseStreak()
        {
            var results = from player in playersList
                           from match in matchesList
                           where player.Id == match.PlayerOneId || player.Id == match.PlayerTwoId
                           group match by player into playerMatchGroup
                           let loseMatchesStreak = GetStreak(playerMatchGroup.Key, playerMatchGroup.AsEnumerable())
                           orderby loseMatchesStreak descending
                           select new
                           {
                               Player = playerMatchGroup.Key,
                               Streak = loseMatchesStreak,
                           };

                Console.WriteLine("\nPlayer with worst streak -> Id: {0} Name: {1} with {2} matches lost streak", results.First().Player.Id, results.First().Player.Name,
                    results.First().Streak);
        }
        //Compute the player/s that win 5 or more consecutive matches.
        public void PlayerWithNConsecuitiveWins()
        {
            var results = from player in playersList
                          from match in matchesList
                          where player.Id == match.PlayerOneId || player.Id == match.PlayerTwoId
                          group match by player into playerMatchGroup
                          where PlayerHasConsecutiveWins(playerMatchGroup, 5)
                          select playerMatchGroup.Key;

            Console.WriteLine("Player with more that 5 consecutive wins:");
            foreach (var result in results)
            {
                Console.WriteLine("Id: {0} Name: {1} with matches wined", result.Id, result.Name);
            }
        }

        //Compute for each city the unbeaten players
        public void UnbeatenPlayersForCIty()
        {
            var results = from match in matchesList
                          join city in citiesList
                          on match.CityId equals city.Id
                          group match by city into cityMatchesGroup
                          from matchesByPlayer in (
                            from player in playersList
                            from match in cityMatchesGroup
                            where player.Id == match.PlayerOneId || player.Id == match.PlayerTwoId
                            orderby player.Id
                            group match by player
                          )
                          group matchesByPlayer by cityMatchesGroup.Key into playerMatchesByCity
                          orderby playerMatchesByCity.Key.Id ascending
                          select new
                          {
                              City = playerMatchesByCity.Key,
                              UnbeatenPlayers = GetUnbeatenPlayers(playerMatchesByCity),
                          };

            Console.WriteLine("\n");
            foreach (var result in results)
            {
                Console.WriteLine("Unbeaten players in {0}:", result.City.Name);
                foreach (var player in result.UnbeatenPlayers)
                {
                    Console.WriteLine("Id: {0} Name: {1}", player.Id, player.Name);
                }
            }

        }

        /** Auxiliar methods **/

        private bool PlayerWin(Player player, Match match)
        {
            return player.Id == match.PlayerOneId && match.Winner.Equals("Player1") ||
                player.Id == match.PlayerTwoId && match.Winner.Equals("Player2");
        }

        private int GetStreak(Player player, IEnumerable<Match> matches)
        {
            int streak = 0;
            int lostMatchesCount = 0;

            foreach (var match in matches)
            {
                if (!PlayerWin(player, match))
                {
                    lostMatchesCount++;
                }
                else
                {
                    lostMatchesCount = 0;
                }
                streak = Math.Max(streak, lostMatchesCount);
            }
            return streak;
        }

        private bool PlayerHasConsecutiveWins(IGrouping<Player, Match> playerMatchGroup, int v)
        {
            int winsCount = 0;

            foreach (var group in playerMatchGroup)
            {
                if (PlayerWin(playerMatchGroup.Key, group))
                {
                    winsCount++;
                    if (winsCount >= 5)
                    {
                        return true;
                    }
                }
                else
                {
                    winsCount = 0;
                }
            }
            return false;
        }


        private IEnumerable<Player> GetUnbeatenPlayers(IGrouping<City, IGrouping<Player, Match>> playerMatchesByCity)
        {
            return playerMatchesByCity.Where(playerMatches => playerMatches.All(match => PlayerWin(playerMatches.Key, match) || match.Winner.Equals("Draw")))
                                                              .Select(player => player.Key);
        }

    }
}
