using System.Collections.Generic;
using OgameBot.Objects;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing
{
    public class GalaxyPageSystem : DataObject
    {
        public SystemCoordinate System { get; set; }

        public List<Coordinate> PresentItems { get; set; }

        public List<Coordinate> AbsentItems { get; set; }

        public GalaxyPageSystem()
        {
            PresentItems = new List<Coordinate>();
            AbsentItems = new List<Coordinate>();
        }

        public override string ToString()
        {
            return $"{System}. Coordinates: {PresentItems.Count + AbsentItems.Count:N0}, present: {PresentItems.Count:N0}, absent: {AbsentItems.Count:N0}";
        }
    }
}