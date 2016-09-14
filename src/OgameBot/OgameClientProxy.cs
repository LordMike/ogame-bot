using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ScraperClientLib.Engine;
using ScraperClientLib.Utilities;

namespace OgameBot
{
    public class OgameClientProxy
    {
        private readonly string _listenHost;
        private readonly int _listenPort;
        private readonly ClientBase _client;
        private readonly HttpListener _listener;
        private bool _isRunning;

        public Uri SubstituteRoot { get; set; }

        public OgameClientProxy(string listenHost, int listenPort, ClientBase client)
        {
            _listenHost = listenHost;
            _listenPort = listenPort;
            _client = client;
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://{listenHost}:{listenPort}/");
        }

        public void Start()
        {
            _isRunning = true;
            _listener.Start();
            _listener.BeginGetContext(Process, null);
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }

        private void Process(IAsyncResult ar)
        {
            HttpListenerContext ctx = null;
            try
            {
                ctx = _listener.EndGetContext(ar);
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 995) // ERROR_OPERATION_ABORTED = 995
            {
                // Request was aborted. Most likely the listener has been stopped
                // Ignore these
            }

            if (_isRunning)
                _listener.BeginGetContext(Process, null);

            if (ctx == null)
                return;

            // Process the current context
            HttpMethod requestedMethod = new HttpMethod(ctx.Request.HttpMethod);
            if (requestedMethod != HttpMethod.Get && requestedMethod != HttpMethod.Post)
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                ctx.Response.OutputStream.WriteString($"Unsupported method: {requestedMethod}");
                return;
            }

            if (ctx.Request.Url.PathAndQuery == "/")
            {
                // Asked for root - send the client to the overview page

                ctx.Response.StatusCode = (int)HttpStatusCode.TemporaryRedirect;
                ctx.Response.RedirectLocation = "/game/index.php?page=overview";

                ctx.Response.OutputStream.WriteString("Redirecting to Overview");

                // Return
                ctx.Response.Close();
                return;
            }

            // Prepare uri
            Uri targetUri = new Uri(SubstituteRoot, ctx.Request.Url.PathAndQuery);

            // NOTE: Enable this to load external ressources through proxy
            //if (targetUri.AbsolutePath.StartsWith("/SPECIAL/"))
            //{
            //    // Request is not for our target. It's for elsewhere
            //    string rest = string.Join("", targetUri.Segments.Skip(2));
            //    rest = rest.Replace(":/", "://");

            //    targetUri = new Uri(rest);
            //}

            // Prepare request
            HttpRequestMessage proxyReq = _client.BuildRequest(targetUri);

            proxyReq.Method = requestedMethod;

            if (requestedMethod == HttpMethod.Post)
            {
                MemoryStream ms = new MemoryStream();
                ctx.Request.InputStream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);

                proxyReq.Content = new StreamContent(ms);
                proxyReq.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(ctx.Request.ContentType);
            }

            // Issue
            ResponseContainer resp = _client.IssueRequest(proxyReq);

            byte[] data = resp.ResponseMessage.Content.ReadAsByteArrayAsync().Sync();

            foreach (string encoding in resp.ResponseMessage.Content.Headers.ContentEncoding)
            {
                if (encoding == "gzip")
                {
                    using (var ms = new MemoryStream(data))
                    using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                    using (var msTarget = new MemoryStream())
                    {
                        gzip.CopyTo(msTarget);

                        data = msTarget.ToArray();
                    }
                }
            }

            // Rewrite html/js
            if (resp.IsHtmlResponse || resp.ResponseMessage.Content.Headers.ContentType.MediaType == "application/x-javascript")
            {
                string str = Encoding.UTF8.GetString(data);

                // NOTE: Enable this to load external ressources through proxy
                //str = Regex.Replace(str, @"(https://gf[\d]+.geo.gfsrv.net/)", $"http://{_listenHost}:{_listenPort}/SPECIAL/$0", RegexOptions.Compiled);

                str = str.Replace(SubstituteRoot.ToString().Replace("/", "\\/"), $@"http:\/\/{_listenHost}:{_listenPort}\/");   // In JS strings
                str = str.Replace(SubstituteRoot.ToString(), $"http://{_listenHost}:{_listenPort}/");   // In links
                str = str.Replace(SubstituteRoot.Host + ":" + SubstituteRoot.Port, $"{_listenHost}:{_listenPort}"); // Without scheme
                str = str.Replace(SubstituteRoot.Host, $"{_listenHost}:{_listenPort}"); // Remainders

                data = Encoding.UTF8.GetBytes(str);
            }

            // Write headers
            ctx.Response.StatusCode = (int)resp.StatusCode;

            foreach (KeyValuePair<string, IEnumerable<string>> header in resp.ResponseMessage.Headers)
                foreach (string value in header.Value)
                    ctx.Response.AddHeader(header.Key, value);

            ctx.Response.ContentType = resp.ResponseMessage.Content.Headers.ContentType.ToString();

            // Write content
            ctx.Response.OutputStream.Write(data, 0, data.Length);

            // Return
            ctx.Response.Close();
        }
    }
}