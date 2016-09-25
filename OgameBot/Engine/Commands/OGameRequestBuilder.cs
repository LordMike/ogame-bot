using System;
using System.Net.Http;
using OgameBot.Objects;
using OgameBot.Utilities;

namespace OgameBot.Engine.Commands
{
    public class OGameRequestBuilder
    {
        private readonly OGameClient _client;

        public OGameRequestBuilder(OGameClient client)
        {
            _client = client;
        }

        public HttpRequestMessage GetGalaxyContent(SystemCoordinate system)
        {
            return _client.BuildPost(new Uri("/game/index.php?page=galaxyContent&ajax=1", UriKind.Relative), new[]
            {
                KeyValuePair.Create("galaxy", system.Galaxy.ToString()),
                KeyValuePair.Create("system", system.System.ToString())
            });
        }

        public HttpRequestMessage GetMessagePageRequest(MessageTabType tab, int page)
        {
            return _client.BuildPost(new Uri("/game/index.php?page=messages", UriKind.Relative), new[]
            {
                KeyValuePair.Create("messageId", "-1"),
                KeyValuePair.Create("tabid", ((int)tab).ToString()),
                KeyValuePair.Create("action", "107"),
                KeyValuePair.Create("pagination", page.ToString()),
                KeyValuePair.Create("ajax", "1")
            });
        }

        public HttpRequestMessage GetMessagePage(int messageId, MessageTabType tabType)
        {
            return _client.BuildRequest(new Uri($"/game/index.php?page=messages&messageId={messageId}&tabid={(int)tabType}&ajax=1", UriKind.Relative));
        }

        public HttpRequestMessage GetOverviewPage()
        {
            return _client.BuildRequest(new Uri($"/game/index.php?page=overview", UriKind.Relative));
        }
    }
}