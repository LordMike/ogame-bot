using OgameBot.Engine;
using OgameBot.Objects;

namespace OgameBot.Commands
{
    public class ScanGalaxyCommand : CommandBase
    {
        private readonly SystemCoordinate _coordinate;

        public ScanGalaxyCommand(OGameClient client, SystemCoordinate coordinate)
            : base(client)
        {
            _coordinate = coordinate;
        }

        public override void Run()
        {
            //Client.BuildPost()
        }
    }
}