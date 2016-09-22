using System;
using System.Text.RegularExpressions;
using OgameBot.Objects;
using OgameBot.Objects.Types;

namespace OgameBot.Engine.Parsing.UtilityParsers
{
    public static class SendShipsLinkParser
    {
        private static Regex _regex = new Regex(@"sendShips\((?:[^0-9]*([0-9]+)[^0-9]*){6}\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses a sendships link
        ///     sendShips(8,6,73,6,2,1);return false                        Recycle 6:73:6 with 1 recycler
        ///     sendShips(6,6,73,6,3,40);return false;                      Espionage 6:73:6-moon with 40 probes
        /// sendShips(
        ///                                                6,
        ///                                                6,
        ///                                                73,
        ///                                                6,
        ///                                                1,
        ///                                                40
        ///                                            ); return false;     Espionage 6:73:6-planet with 40 probes
        /// </summary>
        /// <param name="linkText"></param>
        public static Tuple<MissionType, Coordinate, int> ParseSendLink(string linkText)
        {
            var match = _regex.Match(linkText);

            if (!match.Success)
                return null;

            MissionType mission = (MissionType)int.Parse(match.Groups[1].Value);
            byte galaxy = (byte)int.Parse(match.Groups[2].Value);
            short system = (short)int.Parse(match.Groups[3].Value);
            byte planet = (byte)int.Parse(match.Groups[4].Value);
            CoordinateType type = (CoordinateType)int.Parse(match.Groups[5].Value);
            int shipCount = int.Parse(match.Groups[6].Value);

            Coordinate coord = new Coordinate(galaxy, system, planet, type);

            return Tuple.Create(mission, coord, shipCount);
        }
    }
}