using OgameBot.Objects.Types;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers.Objects
{
    public class DetectedDefence : DataObject
    {
        public DefenceType Building { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Building}, lvl {Count:N0}";
        }
    }
}