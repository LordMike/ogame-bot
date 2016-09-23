using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using OgameBot.Engine.Interventions;
using OgameBot.Engine.Parsing;
using OgameBot.Engine.Savers;
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

        public OGameStringProvider StringProvider { get; }

        public OGameSettings Settings { get; }

        public OGameClient(string server, OGameStringProvider stringProvider, string username, string password)
        {
            _server = server;
            _username = username;
            _password = password;

            _savers = new List<SaverBase>();

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

            RegisterIntervention(new OGameAutoLoginner(this));
        }

        public void RegisterSaver(SaverBase saver)
        {
            using (EnterExclusive())
                _savers.Add(saver);
        }

        protected override void PostRequest(ResponseContainer response)
        {
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