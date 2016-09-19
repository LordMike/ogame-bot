using System;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
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