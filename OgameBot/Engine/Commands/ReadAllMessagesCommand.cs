using System.Net.Http;
using OgameBot.Objects;

namespace OgameBot.Engine.Commands
{
    public class ReadAllMessagesCommand : CommandBase
    {
        public ReadAllMessagesCommand(OGameClient client)
            : base(client)
        {
        }

        public override void Run()
        {
            // Read first page of all message types
            MessageTabType[] types = { MessageTabType.FleetsEspionage };

            foreach (MessageTabType type in types)
            {
                HttpRequestMessage req = Client.RequestBuilder.GetMessagePageRequest(type, 1);
                AssistedIssue(req);
            }
        }
    }
}