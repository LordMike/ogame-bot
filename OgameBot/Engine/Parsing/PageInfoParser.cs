using System.Collections.Generic;
using HtmlAgilityPack;
using OgameBot.Engine.Parsing.Objects;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing
{
    public class PageInfoParser : BaseParser
    {
        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            return container.IsHtmlResponse;
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNodeCollection metaFields = doc.DocumentNode.SelectNodes("/html/head/meta[@name and @content]");

            if (metaFields == null)
                yield break;

            OgamePageInfo res = new OgamePageInfo();

            foreach (HtmlNode field in metaFields)
            {
                string key = field.GetAttributeValue("name", string.Empty);
                string value = field.GetAttributeValue("content", string.Empty);

                res.Fields[key] = value;
            }

            yield return res;
        }
    }
}