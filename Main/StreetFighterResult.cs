﻿using StreetFighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class StreetFighterResult
    {
        public List<Player> playersList {get; set;}
        public List<City> citiesList { get; set;}
        public List<Match> matchesList {get; set;}
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
            Console.WriteLine("CityInfo -> Id: {0} Name: {1} with {2} matches", cityWithMoreMatches.CityId, cityWithMoreMatches.CityName,
                cityWithMoreMatches.Matches);
        }
    }
}