using System.Collections.Generic;
using OgameBot.Objects;
using OgameBot.Objects.Types;

namespace OgameBot.Engine.Parsing.Objects
{
    public class EspionageReport : MessageBase
    {
        public Coordinate Coordinate { get; set; }

        public ReportDetails Details { get; set; }

        public Resources Resources { get; set; }

        public Dictionary<BuildingType, int> DetectedBuildings { get; set; }

        public Dictionary<ShipType, int> DetectedShips { get; set; }

        public Dictionary<DefenceType, int> DetectedDefence { get; set; }

        public Dictionary<ResearchType, int> DetectedResearch { get; set; }

        public override string ToString()
        {
            return $"Report {Coordinate}, level: {Details}";
        }
    }
}