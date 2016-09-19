using System;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
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