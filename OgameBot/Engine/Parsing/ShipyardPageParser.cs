using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Objects.Types;
using OgameBot.Utilities;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing
{
    public class ShipyardPageParser : BaseParser
    {
        private static Regex CssRegex = new Regex(@"(?:military[\d]+|civil[\d]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            return container.RequestMessage.RequestUri.Query.Contains("page=shipyard");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNodeCollection imageFields = doc.DocumentNode.SelectNodes("//div[@id='buttonz']/div[@class='content']//div[contains(@class, 'military') or contains(@class, 'civil')]");

            if (imageFields == null)
                yield break;

            foreach (HtmlNode node in imageFields)
            {
                string cssClss = node.GetCssClasses(CssRegex).FirstOrDefault();

                Ship type;
                switch (cssClss)
                {
                    case "military204":
                        type = Ship.LightFighter;
                        break;
                    case "military205":
                        type = Ship.HeavyFighter;
                        break;
                    case "military206":
                        type = Ship.Cruiser;
                        break;
                    case "military207":
                        type = Ship.Battleship;
                        break;
                    case "military215":
                        type = Ship.Battlecruiser;
                        break;
                    case "military211":
                        type = Ship.Bomber;
                        break;
                    case "military213":
                        type = Ship.Destroyer;
                        break;
                    case "military214":
                        type = Ship.Deathstar;
                        break;
                    case "civil202":
                        type = Ship.SmallCargo;
                        break;
                    case "civil203":
                        type = Ship.LargeCargo;
                        break;
                    case "civil208":
                        type = Ship.Colony;
                        break;
                    case "civil209":
                        type = Ship.Recycler;
                        break;
                    case "civil210":
                        type = Ship.EspionageProbe;
                        break;
                    case "civil212":
                        type = Ship.SolarSatellite;
                        break;
                    default:
                        continue;
                }

                string countText = node.SelectSingleNode(".//span[@class='level']").ChildNodes.Last(s => s.NodeType == HtmlNodeType.Text).InnerText;
                int count = int.Parse(countText, NumberStyles.Integer | NumberStyles.AllowThousands, client.ServerCulture);

                yield return new DetectedShip
                {
                    Ship = type,
                    Count = count
                };
            }
        }
    }
}