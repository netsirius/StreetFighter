using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFighter
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return string.Format("City Id: {0} - City Name {1}", Id, Name);
        }
    }
}
