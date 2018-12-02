using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreetFighter;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = ReadPlayersFromCsv("../../../Data/players.csv");
            List<City> cities = ReadCitiesFromCsv("../../../Data/cities.csv");
            List<Match> matches = ReadMatchesFromCsv("../../../Data/games.csv");

            StreetFighterResult results = new StreetFighterResult(players, cities, matches);
            results.CityWithMoreMatches();
            Console.ReadLine();
        }

        private static List<Player> ReadPlayersFromCsv(string csvPath)
        {
            var reader = new StreamReader(File.OpenRead(csvPath));
            List<Player> players = new List<Player>();

            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Split(';');
                players.Add(new Player(int.Parse(line[0]), line[1]));
            }
            return players;
        }
        private static List<City> ReadCitiesFromCsv(string csvPath)
        {
            var reader = new StreamReader(File.OpenRead(csvPath));
            List<City> cities = new List<City>();

            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Split(';');
                cities.Add(new City(int.Parse(line[0]), line[1]));
            }
            return cities;
        }
        private static List<Match> ReadMatchesFromCsv(string csvPath)
        {
            var reader = new StreamReader(File.OpenRead(csvPath));
            List<Match> matches = new List<Match>();

            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Split(';');
                matches.Add(new Match(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]), line[4]));
            }
            return matches;
        }
    }
}
