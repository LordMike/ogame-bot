using System;
using System.Linq;
using System.Net.Http;
using OgameBot.Engine.Commands;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Logging;
using OgameBot.Objects;
using ScraperClientLib.Engine;

namespace OgameBot.Engine.Tasks
{
    public class MessageReaderJob : WorkerBase
    {
        private readonly OGameClient _client;
        private MessageCountObject _lastCount;
        private DateTime _lastCountTime;
        private DateTime _lastRun;

        public MessageReaderJob(OGameClient client)
        {
            _client = client;
            client.OnResponseReceived += ClientOnOnResponseReceived;

            ExecutionInterval = TimeSpan.FromSeconds(10);
        }

        private void ClientOnOnResponseReceived(ResponseContainer responseContainer)
        {
            MessageCountObject count = responseContainer.GetParsedSingle<MessageCountObject>(false);

            if (count == null)
                return;

            _lastCount = count;
            _lastCountTime = DateTime.UtcNow;
        }

        protected override void RunInternal()
        {
            if (_lastCount == null || _lastRun > _lastCountTime)
                return;

            _lastRun = DateTime.UtcNow;
            if (_lastCount.NewMessages <= 0)
                return;

            Logger.Instance.Log(LogLevel.Debug, $"Checking for new messages, {_lastCount.NewMessages:N0} reported at {_lastCountTime}");

            // Read all messages
            ReadAllMessagesCommand cmd = new ReadAllMessagesCommand(_client);
            cmd.Run();

            foreach (MessagesPage messagesPage in cmd.ParsedObjects.OfType<MessagesPage>())
            {
                // Request each message
                foreach (Tuple<int, MessageType> message in messagesPage.MessageIds)
                {
                    if (message.Item2 == MessageType.EspionageReport)
                    {
                        HttpRequestMessage req = _client.RequestBuilder.GetMessagePage(message.Item1, MessageTabType.FleetsEspionage);
                        _client.IssueRequest(req);
                    }
                }
            }
        }
    }
}