using System.Collections.Generic;
using HtmlAgilityPack;
using OgameBot.Engine.Parsing.Objects;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing
{
    public class MessageCountParser : BaseParser
    {
        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            // Always parse this (if it's html)
            return container.IsHtmlResponse;
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNode item = doc.DocumentNode.SelectSingleNode("//*[@data-new-messages]");

            if (item == null)
                yield break;

            int newMessages = item.GetAttributeValue("data-new-messages", 0);

            yield return new MessageCountObject { NewMessages = newMessages };
        }
    }
}