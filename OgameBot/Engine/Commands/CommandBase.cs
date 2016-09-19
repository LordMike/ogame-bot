using System.Collections.Generic;
using System.Net.Http;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Commands
{
    public abstract class CommandBase
    {
        protected OGameClient Client { get; }

        public List<DataObject> ParsedObjects { get; }

        protected CommandBase(OGameClient client)
        {
            Client = client;
            ParsedObjects = new List<DataObject>();
        }

        protected ResponseContainer AssistedIssue(HttpRequestMessage request)
        {
            ResponseContainer result = Client.IssueRequest(request);

            ParsedObjects.AddRange(result.ParsedObjects);

            return result;
        }

        public abstract void Run();
    }
}
