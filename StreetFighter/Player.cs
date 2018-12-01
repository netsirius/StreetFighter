using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFighter
{
    class Player
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => string.Format("Player Id: {0} - Player Name {1}", Id, Name);
    }
}
