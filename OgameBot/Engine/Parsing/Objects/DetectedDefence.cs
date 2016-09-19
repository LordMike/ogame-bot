using OgameBot.Objects.Types;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
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