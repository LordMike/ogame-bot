using System;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers.Objects
{
    public class DetectedBuilding : DataObject
    {
        public BuildingType Building { get; set; }

        public int Level { get; set; }

        public Uri UpgradeUri { get; set; }

        public override string ToString()
        {
            return $"{Building}, lvl {Level:N0}";
        }
    }
}