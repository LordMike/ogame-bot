using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using ScraperClientLib.Utilities;

namespace ScraperClientLib.Engine
{
    public class ResponseDocument
    {
        public HttpRequestMessage RequestMessage { get; }

        public HttpResponseMessage ResponseMessage { get; }

        public Lazy<HtmlDocument> ResponseHtml { get; }
        
        
        public HttpStatusCode StatusCode => ResponseMessage.StatusCode;

        public bool WasSuccess => ResponseMessage.IsSuccessStatusCode;

        public bool IsHtmlResponse => ResponseHtml.Value != null;
        

        public List<DataObject> ParsedObjects { get; set; }

        public ResponseDocument(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage)
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

                Stream contentStream = ResponseMessage.Content.ReadAsStreamAsync().Sync();

                HtmlDocument doc = new HtmlDocument();
                doc.Load(contentStream);

                return doc;
            });
        }
    }
}