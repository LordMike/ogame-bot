using System.Collections.Generic;
using OgameBot.Objects.Types;

namespace OgameBot.Db
{
    public class PlanetInfo
    {
        public Dictionary<ShipType, int> Ships { get; set; }

        public Dictionary<BuildingType, int> Buildings { get; set; }

        public Dictionary<DefenceType, int> Defences { get; set; }

        public PlanetInfo()
        {
            Ships = new Dictionary<ShipType, int>();
            Buildings = new Dictionary<BuildingType, int>();
            Defences = new Dictionary<DefenceType, int>();
        }
    }
}