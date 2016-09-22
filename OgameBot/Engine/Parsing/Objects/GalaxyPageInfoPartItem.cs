using OgameBot.Objects;

namespace OgameBot.Engine.Parsing.Objects
{
    public class GalaxyPageInfoPartItem
    {
        public Coordinate Coordinate { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }

        public SendShipsInfo EspionageLinkInfo { get; set; }
    }
}