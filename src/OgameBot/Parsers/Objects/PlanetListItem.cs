using OgameBot.Objects;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers.Objects
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