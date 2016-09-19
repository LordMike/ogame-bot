using OgameBot.Objects.Types;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public class DetectedShip : DataObject
    {
        public ShipType Ship { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Ship}, lvl {Count:N0}";
        }
    }
}