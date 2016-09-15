using System;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers.Objects
{
    public class DetectedResearch : DataObject
    {
        public ResearchType Research { get; set; }

        public int Level { get; set; }

        public Uri UpgradeUri { get; set; }

        public override string ToString()
        {
            return $"{Research}, lvl {Level:N0}";
        }
    }
}