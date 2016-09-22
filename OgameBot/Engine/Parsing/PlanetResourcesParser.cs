using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing
{
    public class PlanetResourcesParser : BaseParser
    {
        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            // Always parse this (if it's html)
            return container.IsHtmlResponse;
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNode infoBox = doc.DocumentNode.SelectSingleNode("//div[@id='info']");

            if (infoBox == null)
                // Requirement for this parser
                yield break;

            Coordinate coord = container.GetParsedSingle<OgamePageInfo>().PlanetCoord;

            string metBoxHtml = infoBox.SelectSingleNode(".//li[@id='metal_box']").GetAttributeValue("title", null).Split('|').Last();
            string cryBoxHtml = infoBox.SelectSingleNode(".//li[@id='crystal_box']").GetAttributeValue("title", null).Split('|').Last();
            string deuBoxHtml = infoBox.SelectSingleNode(".//li[@id='deuterium_box']").GetAttributeValue("title", null).Split('|').Last();
            string eneBoxHtml = infoBox.SelectSingleNode(".//li[@id='energy_box']").GetAttributeValue("title", null).Split('|').Last();

            HtmlDocument metBox = new HtmlDocument();
            metBox.LoadHtml(WebUtility.HtmlDecode(metBoxHtml));
            HtmlDocument cryBox = new HtmlDocument();
            cryBox.LoadHtml(WebUtility.HtmlDecode(cryBoxHtml));
            HtmlDocument deuBox = new HtmlDocument();
            deuBox.LoadHtml(WebUtility.HtmlDecode(deuBoxHtml));
            HtmlDocument eneBox = new HtmlDocument();
            eneBox.LoadHtml(WebUtility.HtmlDecode(eneBoxHtml));

            HtmlNodeCollection metTds = metBox.DocumentNode.SelectNodes("//td");
            HtmlNodeCollection cryTds = cryBox.DocumentNode.SelectNodes("//td");
            HtmlNodeCollection deuTds = deuBox.DocumentNode.SelectNodes("//td");
            HtmlNodeCollection eneTds = eneBox.DocumentNode.SelectNodes("//td");

            PlanetResources result = new PlanetResources();

            result.Coordinate = coord;
            result.Resources = new Resources
            {
                Metal = int.Parse(metTds[0].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Crystal = int.Parse(cryTds[0].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Deuterium = int.Parse(deuTds[0].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Energy = int.Parse(eneTds[0].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture)
            };

            result.Capacities = new Resources
            {
                Metal = int.Parse(metTds[1].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Crystal = int.Parse(cryTds[1].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Deuterium = int.Parse(deuTds[1].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Energy = int.Parse(eneTds[1].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture)
            };

            result.ProductionPerHour = new Resources
            {
                Metal = int.Parse(metTds[2].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Crystal = int.Parse(cryTds[2].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Deuterium = int.Parse(deuTds[2].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture),
                Energy = int.Parse(eneTds[2].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture)
            };

            yield return result;
        }
    }
}