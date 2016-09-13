using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using OgameBot.Parsers.Objects;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers
{
    public class PlanetListParser : BaseParser
    {
        private static readonly Regex PlanetIdRegex = new Regex(@"planet-([\d]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseDocument document)
        {
            // Always parse this (if it's html)
            return document.IsHtmlResponse;
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseDocument document)
        {
            HtmlDocument doc = document.ResponseHtml.Value;
            HtmlNodeCollection worldNodes = doc.DocumentNode.SelectNodes("//div[@id='myWorlds']/div[@id='planetList']/div[starts-with(@id, 'planet-')]");

            if (worldNodes == null)
                yield break;

            foreach (HtmlNode node in worldNodes)
            {
                int id = int.Parse(PlanetIdRegex.Match(node.GetAttributeValue("id", null)).Groups[1].Value, client.ServerCulture);
                string name = node.SelectSingleNode(".//span[contains(@class, 'planet-name')]").InnerText;
                string coordinate = node.SelectSingleNode(".//span[contains(@class, 'planet-koords')]").InnerText;

                PlanetListItem item = new PlanetListItem
                {
                    Id = id,
                    Name = name,
                    Coordinate = Coordinate.Parse(coordinate, CoordinateType.Planet)
                };

                yield return item;
            }
        }
    }
}