using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using OgameBot.Engine.Commands;
using OgameBot.Engine.Interventions;
using OgameBot.Engine.Parsing;
using OgameBot.Engine.Savers;
using OgameBot.Logging;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine
{
    public class OGameClient : ClientBase
    {
        private readonly Uri _loginUri = new Uri("https://en.ogame.gameforge.com/main/login");

        private readonly string _server;
        private readonly string _username;
        private readonly string _password;

        private readonly List<SaverBase> _savers;

        public event Action<ResponseContainer> OnResponseReceived;

        public OGameStringProvider StringProvider { get; }

        public OGameSettings Settings { get; }

        public OGameRequestBuilder RequestBuilder { get; }

        public OGameClient(string server, OGameStringProvider stringProvider, string username, string password)
        {
            _server = server;
            _username = username;
            _password = password;

            _savers = new List<SaverBase>();

            RequestBuilder = new OGameRequestBuilder(this);

            StringProvider = stringProvider;
            BaseUri = new Uri($"https://{server}/");

            Settings = new OGameSettings();

            RegisterParser(new PageInfoParser());
            RegisterParser(new DefencesPageParser());
            RegisterParser(new FacilitiesPageParser());
            RegisterParser(new FleetMovementPageParser());
            RegisterParser(new GalaxyPageParser());
            RegisterParser(new PlanetListParser());
            RegisterParser(new PlanetResourcesParser());
            RegisterParser(new ResearchPageParser());
            RegisterParser(new ResourcesPageParser());
            RegisterParser(new ShipyardPageParser());
            RegisterParser(new MessagesPageParser());
            RegisterParser(new EspionageDetailsParser());
            RegisterParser(new MessageCountParser());

            RegisterIntervention(new OGameAutoLoginner(this));
        }

        public void RegisterSaver(SaverBase saver)
        {
            using (EnterExclusive())
                _savers.Add(saver);
        }

        public IReadOnlyList<SaverBase> GetSavers()
        {
            return _savers.AsReadOnly();
        }

        protected override void PostRequest(ResponseContainer response)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Got {response.StatusCode} to {response.RequestMessage.RequestUri}, ({response.ParsedObjects.Count:N0} parsed objects)");

            Debug.WriteLine($"Response to {response.RequestMessage.RequestUri}, ({response.ParsedObjects.Count:N0} parsed objects)");
            foreach (DataObject dataObject in response.ParsedObjects)
            {
                Debug.WriteLine($"Parsed object by {dataObject.ParserType}: {dataObject}");
            }

            // Save to DB
            foreach (SaverBase saver in _savers)
            {
                saver.Run(response.ParsedObjects);
            }

            // Execute other interests
            OnResponseReceived?.Invoke(response);
        }

        internal HttpRequestMessage PrepareLogin()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc["kid"] = "";
            nvc["uni"] = _server;
            nvc["login"] = _username;
            nvc["pass"] = _password;

            return BuildPost(_loginUri, nvc);
        }

        public void PerformLogin()
        {
            using (EnterExclusive())
            {
                HttpRequestMessage loginReq = PrepareLogin();
                IssueRequest(loginReq);
            }
        }
    }
}