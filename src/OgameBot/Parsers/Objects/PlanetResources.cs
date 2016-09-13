using OgameBot.Objects;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers.Objects
{
    public class PlanetResources : DataObject
    {
        public Coordinate Coordinate { get; set; }

        public Resources Resources { get; set; }

        public Resources Capacities { get; set; }

        public Resources ProductionPerHour { get; set; }
        
        public override string ToString()
        {
            return $"Resources: {Coordinate}, {Resources}";
        }
    }
}