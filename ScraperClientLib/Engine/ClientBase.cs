using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using ScraperClientLib.Engine.Interventions;
using ScraperClientLib.Engine.Parsing;
using ScraperClientLib.Utilities;

namespace ScraperClientLib.Engine
{
    public abstract class ClientBase
    {
        private readonly object _lockObject = new object();
        private readonly HttpClient _httpClient;
        private readonly List<BaseParser> _parsers;
        private readonly List<IInterventionHandler> _interventionHandlers;
        private readonly Dictionary<string, string> _defaultHeaders;

        public Uri BaseUri { get { return _httpClient.BaseAddress; } set { _httpClient.BaseAddress = value; } }

        public CultureInfo ServerCulture { get; set; }

        protected ClientBase()
        {
            _httpClient = new HttpClient();
            _parsers = new List<BaseParser>();
            _interventionHandlers = new List<IInterventionHandler>();
            _defaultHeaders = new Dictionary<string, string>();

            RegisterDefaultHeader("Accept-Encoding", "gzip, deflate");
        }

        public void RegisterDefaultHeader(string key, string value)
        {
            _defaultHeaders[key] = value;
        }

        public void RegisterParser(BaseParser parser)
        {
            using (EnterExclusive())
                _parsers.Add(parser);
        }

        public void RegisterIntervention(IInterventionHandler handler)
        {
            using (EnterExclusive())
                _interventionHandlers.Add(handler);
        }

        protected virtual void PostRequest(ResponseContainer response)
        {

        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, Uri uri)
        {
            HttpRequestMessage req = new HttpRequestMessage(method, uri);

            foreach (KeyValuePair<string, string> pair in _defaultHeaders)
            {
                req.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
            }

            return req;
        }

        public HttpRequestMessage BuildRequest(Uri uri)
        {
            return PrepareRequest(HttpMethod.Get, uri);
        }

        public HttpRequestMessage BuildPost(Uri uri, NameValueCollection nvc)
        {
            return BuildPost(uri, nvc.AsKeyValues());
        }

        public HttpRequestMessage BuildPost(Uri uri, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            HttpRequestMessage req = PrepareRequest(HttpMethod.Post, uri);

            req.Content = new FormUrlEncodedContent(parameters);

            return req;
        }

        private ResponseContainer IssueRequestInternal(HttpRequestMessage request)
        {
            HttpResponseMessage response = _httpClient.SendAsync(request).Sync();

            ResponseContainer result = new ResponseContainer(request, response);

            return result;
        }

        public ResponseContainer IssueRequest(HttpRequestMessage request)
        {
            using (EnterExclusive())
            {
                ResponseContainer result = IssueRequestInternal(request);

                // Check interventions
                foreach (IInterventionHandler handler in _interventionHandlers)
                {
                    if (handler.DoIntervention(result))
                    {
                        // Woops, intervention needs to be handled
                        InterventionResult interventionResult = handler.Handle(result);

                        if (interventionResult.IntermediateTask != null)
                        {
                            // Process intermediate request
                            // TODO: Should we recurse?
                            IssueRequestInternal(request);
                        }

                        switch (interventionResult.State)
                        {
                            case InterventionResultState.Abort:
                                throw new Exception("Unable to process request");
                            case InterventionResultState.RetryCurrentTask:
                                // Retry same request
                                IssueRequest(request);
                                break;
                            case InterventionResultState.Continue:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                // Process all parsers
                foreach (BaseParser parser in _parsers)
                {
                    if (!parser.ShouldProcess(result))
                        continue;

                    foreach (DataObject dataObject in parser.Process(this, result))
                        result.ParsedObjects.Add(dataObject);
                }

                // Call childs custom logic
                PostRequest(result);

                // Return
                return result;
            }
        }

        public ExclusiveLock EnterExclusive()
        {
            return EnterExclusive(TimeSpan.MinValue);
        }

        public ExclusiveLock EnterExclusive(TimeSpan lockTimeout)
        {
            if (lockTimeout == TimeSpan.MinValue)
            {
                Monitor.Enter(_lockObject);
                return new ExclusiveLock(this);
            }

            bool gotLock = Monitor.TryEnter(_lockObject, lockTimeout);
            if (gotLock)
                return new ExclusiveLock(this);

            return null;
        }

        public class ExclusiveLock : IDisposable
        {
            private readonly ClientBase _client;

            internal ExclusiveLock(ClientBase client)
            {
                _client = client;
            }

            public void Dispose()
            {
                Monitor.Exit(_client._lockObject);
            }
        }
    }
}
