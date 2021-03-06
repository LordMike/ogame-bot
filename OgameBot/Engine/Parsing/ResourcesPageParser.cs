using System;
using System.Collections.Generic;
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
    public class ResourcesPageParser : BaseParser
    {
        private static readonly Regex CssRegex = new Regex(@"supply[\d]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            return container.RequestMessage.RequestUri.Query.Contains("page=resources");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNodeCollection imageFields = doc.DocumentNode.SelectNodes("//div[@id='buttonz']/div[@class='content']//div[starts-with(@class, 'supply')]");

            if (imageFields == null)
                yield break;

            foreach (HtmlNode node in imageFields)
            {
                string cssClass = node.GetCssClasses(CssRegex).FirstOrDefault();

                Building type;
                switch (cssClass)
                {
                    case "supply1":
                        type = Building.MetalMine;
                        break;
                    case "supply2":
                        type = Building.CrystalMine;
                        break;
                    case "supply3":
                        type = Building.DeuteriumSynthesizer;
                        break;
                    case "supply4":
                        type = Building.SolarPlant;
                        break;
                    case "supply12":
                        type = Building.FusionReactor;
                        break;
                    case "supply23":
                        type = Building.MetalStorage;
                        break;
                    case "supply24":
                        type = Building.CrystalStorage;
                        break;
                    case "supply25":
                        type = Building.DeuteriumTank;
                        break;
                    default:
                        continue;
                }

                int level = node.SelectSingleNode(".//span[@class='level']").GetFirstNumberChildNode(client.ServerCulture);

                HtmlNode fastBuildLinkNode = node.SelectSingleNode(".//a[contains(@class, 'fastBuild')]");
                string fastBuildLink = fastBuildLinkNode?.GetAttributeValue("onclick", null).Split('\'')[1];

                yield return new DetectedBuilding
                {
                    Building = type,
                    Level = level,
                    UpgradeUri = fastBuildLink == null ? null : new Uri(fastBuildLink)
                };
            }
        }
    }
}