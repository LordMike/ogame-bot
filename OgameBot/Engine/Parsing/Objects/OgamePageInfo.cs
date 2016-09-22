using System;
using System.Collections.Specialized;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing.Objects
{
    public class OgamePageInfo : DataObject
    {
        public NameValueCollection Fields { get; set; }

        public string Session => TryGet("ogame-session", string.Empty);

        public string Version => TryGet("ogame-version", string.Empty);

        public DateTimeOffset ServerTime => DateTimeOffset.FromUnixTimeSeconds(TryGet("ogame-timestamp", 0));

        public string Universe => TryGet("ogame-universe", string.Empty);

        public int UniverseSpeed => TryGet("ogame-universe-speed", 0);

        public string UniverseLanguage => TryGet("ogame-language", string.Empty);

        public int PlayerId => TryGet("ogame-player-id", 0);

        public string PlayerName => TryGet("ogame-player-name", string.Empty);

        public int PlanetId => TryGet("ogame-planet-id", 0);

        public string PlanetName => TryGet("ogame-planet-name", string.Empty);

        public Coordinate PlanetCoord
        {
            get
            {
                string coords = TryGet("ogame-planet-coordinates", string.Empty);
                string type = TryGet("ogame-planet-type", string.Empty);

                if (type == "planet")
                    return Coordinate.Parse(coords, CoordinateType.Planet);

                return Coordinate.Parse(coords, CoordinateType.Moon);
            }
        }

        public string MiniFleetToken { get; set; }

        private string TryGet(string key, string @default)
        {
            string val = Fields[key];
            if (string.IsNullOrEmpty(val))
                return @default;

            return val;
        }

        private int TryGet(string key, int @default)
        {
            string val = TryGet(key, null);
            if (string.IsNullOrEmpty(val))
                return @default;

            return int.Parse(val);
        }

        public OgamePageInfo()
        {
            Fields = new NameValueCollection();
        }

        public override string ToString()
        {
            return $"OgameInfo, Fields: {Fields.Count:N0}. Planet: {PlanetCoord} ({PlayerName}) Server: {Universe} ({UniverseSpeed}x)";
        }
    }
}