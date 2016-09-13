using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OgameBot.Objects.Types;
using OgameBot.Parsers.Objects;
using OgameBot.Utilities;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers
{
    public class ResearchPageParser : BaseParser
    {
        private static readonly Regex CssRegex = new Regex(@"research[\d]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseDocument document)
        {
            return document.RequestMessage.RequestUri.Query.Contains("page=research");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseDocument document)
        {
            HtmlDocument doc = document.ResponseHtml.Value;
            HtmlNodeCollection imageFields = doc.DocumentNode.SelectNodes("//div[@id='buttonz']//div[contains(@class, 'research')]");

            if (imageFields == null)
                yield break;

            foreach (HtmlNode node in imageFields)
            {
                string cssClss = node.GetCssClasses(CssRegex).FirstOrDefault();

                Research type;
                switch (cssClss)
                {
                    case "research113":
                        type = Research.EnergyTechnology;
                        break;
                    case "research120":
                        type = Research.LaserTechnology;
                        break;
                    case "research121":
                        type = Research.IonTechnology;
                        break;
                    case "research114":
                        type = Research.HyperspaceTechnology;
                        break;
                    case "research122":
                        type = Research.PlasmaTechnology;
                        break;
                    case "research115":
                        type = Research.CombustionDrive;
                        break;
                    case "research117":
                        type = Research.ImpulseDrive;
                        break;
                    case "research118":
                        type = Research.HyperspaceDrive;
                        break;
                    case "research106":
                        type = Research.EspionageTechnology;
                        break;
                    case "research108":
                        type = Research.ComputerTechnology;
                        break;
                    case "research124":
                        type = Research.Astrophysics;
                        break;
                    case "research123":
                        type = Research.IntergalacticResearchNetwork;
                        break;
                    case "research199":
                        type = Research.GravitonTechnology;
                        break;
                    case "research109":
                        type = Research.WeaponsTechnology;
                        break;
                    case "research110":
                        type = Research.ShieldingTechnology;
                        break;
                    case "research111":
                        type = Research.ArmourTechnology;
                        break;
                    default:
                        continue;
                }

                string levelText = node.SelectSingleNode(".//span[@class='level']").ChildNodes.Where(s => s.NodeType == HtmlNodeType.Text).Skip(1).First().InnerText;
                int level = int.Parse(levelText, NumberStyles.Integer | NumberStyles.AllowThousands, client.ServerCulture);

                HtmlNode fastBuildLinkNode = node.SelectSingleNode(".//a[contains(@class, 'fastBuild')]");
                string fastBuildLink = fastBuildLinkNode?.GetAttributeValue("onclick", null).Split('\'')[1];

                yield return new DetectedResearch()
                {
                    Research = type,
                    Level = level,
                    UpgradeUri = fastBuildLink == null ? null : new Uri(fastBuildLink)
                };
            }
        }
    }
}