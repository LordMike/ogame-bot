using OgameBot.Objects;
using OgameBot.Objects.Types;

namespace OgameBot.Engine.Parsing.Objects
{
    public class SendShipsInfo
    {
        public MissionType Mission { get; set; }

        public Coordinate Coord { get; set; }

        public int ShipCount { get; set; }

        public SendShipsInfo(MissionType mission, Coordinate coord, int shipCount)
        {
            Mission = mission;
            Coord = coord;
            ShipCount = shipCount;
        }
    }
}