using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using ScraperClientLib.Engine.Parsing;
using ScraperClientLib.Utilities;

namespace ScraperClientLib.Engine
{
    public class ResponseContainer
    {
        public HttpRequestMessage RequestMessage { get; }

        public HttpResponseMessage ResponseMessage { get; }

        public Lazy<HtmlDocument> ResponseHtml { get; }


        public HttpStatusCode StatusCode => ResponseMessage.StatusCode;

        public bool WasSuccess => ResponseMessage.IsSuccessStatusCode;

        public bool IsHtmlResponse => ResponseHtml.Value != null;


        public List<DataObject> ParsedObjects { get; set; }

        public ResponseContainer(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
        {
            RequestMessage = requestMessage;
            ResponseMessage = responseMessage;

            ParsedObjects = new List<DataObject>();

            ResponseHtml = new Lazy<HtmlDocument>(() =>
            {
                if (ResponseMessage.Content.Headers.ContentType.MediaType != "text/html")
                {
                    // Not HTML
                    return null;
                }

                Stream contentStream = ResponseMessage.Content.ReadAsStream2Async().Sync();
                
                HtmlDocument doc = new HtmlDocument();
                doc.Load(contentStream);

                return doc;
            });
        }
    }
}