using System;
using System.Net.Http;

namespace OgameBot.Engine.Tasks
{
    public class SessionKeepAliveJob : WorkerBase
    {
        private readonly TimeSpan _sessionAgeLimit = TimeSpan.FromMinutes(8);
        private readonly OGameClient _client;

        public SessionKeepAliveJob(OGameClient client)
        {
            _client = client;

            ExecutionInterval = TimeSpan.FromMinutes(10);
        }

        protected override void RunInternal()
        {
            TimeSpan lastReqAge = DateTime.UtcNow - _client.LastRequestUtc;
            if (lastReqAge < _sessionAgeLimit)
                return;

            HttpRequestMessage req = _client.RequestBuilder.GetOverviewPage();
            _client.IssueRequest(req);
        }
    }
}