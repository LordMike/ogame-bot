using System;
using System.Text.RegularExpressions;
using OgameBot.Engine.Parsing.Objects;
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
        public static SendShipsInfo ParseSendLink(string linkText)
        {
            Match match = _regex.Match(linkText);

            if (!match.Success)
                return null;

            Group groupInfo = match.Groups[1];

            MissionType mission = (MissionType)int.Parse(groupInfo.Captures[0].Value);
            byte galaxy = (byte)int.Parse(groupInfo.Captures[1].Value);
            short system = (short)int.Parse(groupInfo.Captures[2].Value);
            byte planet = (byte)int.Parse(groupInfo.Captures[3].Value);
            CoordinateType type = (CoordinateType)int.Parse(groupInfo.Captures[4].Value);
            int shipCount = int.Parse(groupInfo.Captures[5].Value);

            Coordinate coord = new Coordinate(galaxy, system, planet, type);

            return new SendShipsInfo(mission, coord, shipCount);
        }
    }
}