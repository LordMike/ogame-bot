using System.Collections.Generic;
using System.Linq;
using OgameBot.Objects.Types;

namespace OgameBot.Objects
{
    public class FleetComposition
    {
        public Dictionary<ShipType, int> Ships { get; }

        public Resources Resources { get; set; }

        public int TotalShips => Ships.Sum(s => s.Value);

        public FleetComposition()
        {
            Ships = new Dictionary<ShipType, int>();
            Resources = new Resources();
        }
    }
}