using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using OgameBot.Objects;
using OgameBot.Parsers.Objects;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers
{
    public class MessagesPageParser : BaseParser
    {
        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            return container.RequestMessage.RequestUri.Query.Contains("page=messages") &&
                    !container.RequestMessage.RequestUri.Query.Contains("messageId=");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNodeCollection messageList = doc.DocumentNode.SelectNodes("//li[@data-msg-id]");
            HtmlNodeCollection paginationItems = doc.DocumentNode.SelectNodes("//li[@data-tab and @data-page]");

            if (messageList == null || paginationItems == null)
                yield break;

            MessagesPage result = new MessagesPage();

            HtmlNode currentPageNode = doc.DocumentNode.SelectSingleNode("//li[@data-tab and not(@data-page)]");
            string currentPageText = currentPageNode.InnerText.Split('/').First();

            result.Page = int.Parse(currentPageText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture);

            foreach (HtmlNode item in paginationItems)
            {
                result.TabType = (MessageTabType)item.GetAttributeValue("data-tab", 0);

                int page = item.GetAttributeValue("data-page", 0);
                if (page > result.MaxPage)
                    result.MaxPage = page;
            }

            foreach (HtmlNode node in messageList)
            {
                int msgId = node.GetAttributeValue("data-msg-id", 0);

                MessageType type = MessageType.Unknown;

                if (result.TabType == MessageTabType.FleetsEspionage)
                {
                    bool hasDefText = node.SelectSingleNode(".//span[@class='espionageDefText']") != null;

                    if (hasDefText)
                        type = MessageType.EspionageAction;
                    else
                        type = MessageType.EspionageReport;
                }

                result.MessageIds.Add(Tuple.Create(msgId, type));
            }

            yield return result;
        }
    }
}