using OgameBot.Objects;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
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