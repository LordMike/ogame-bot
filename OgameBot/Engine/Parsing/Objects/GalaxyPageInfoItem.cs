using System.Text;
using OgameBot.Objects;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public class GalaxyPageInfoItem : DataObject
    {
        public GalaxyPageInfoPartItem Planet { get; set; }

        public GalaxyPageInfoPartItem Moon { get; set; }

        public Resources DebrisField { get; set; }

        public string PlayerName { get; set; }

        public int PlayerId { get; set; }

        public PlayerStatus PlayerStatus { get; set; }

        public string AllyName { get; set; }

        public int AllyId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Planet.Coordinate} ({Planet.Id}: {Planet.Name}), player ({PlayerId}) {PlayerName}");

            if (DebrisField.Total > 0)
                sb.Append($"-- DM: {DebrisField.Metal:N0}, DC: {DebrisField.Crystal:N0}");

            if (Moon != null)
                sb.Append(" -- W/MOON");

            return sb.ToString();
        }
    }
}