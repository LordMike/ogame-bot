using System;
using System.Net.Http;
using OgameBot.Engine;
using OgameBot.Objects;
using OgameBot.Utilities;

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
            HttpRequestMessage req = Client.BuildPost(new Uri("/game/index.php?page=galaxyContent&ajax=1", UriKind.Relative), new[]
            {
                KeyValuePair.Create("galaxy", _coordinate.Galaxy.ToString()),
                KeyValuePair.Create("system", _coordinate.System.ToString())
            });

            AssistedIssue(req);
        }
    }
}