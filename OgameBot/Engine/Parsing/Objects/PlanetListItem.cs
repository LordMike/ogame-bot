using OgameBot.Objects;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public class PlanetListItem : DataObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Coordinate Coordinate { get; set; }

        public override string ToString()
        {
            return $"Planet {Name}: {Coordinate}";
        }
    }
}