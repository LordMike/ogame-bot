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
    public class DefencesPageParser : BaseParser
    {
        private static Regex CssRegex = new Regex(@"defense[\d]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            return container.RequestMessage.RequestUri.Query.Contains("page=defense");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNodeCollection imageFields = doc.DocumentNode.SelectNodes("//div[@id='buttonz']/div[@class='content']//div[contains(@class, 'defense')]");

            if (imageFields == null)
                yield break;

            foreach (HtmlNode node in imageFields)
            {
                string cssClss = node.GetCssClasses(CssRegex).FirstOrDefault();

                Defence type;
                switch (cssClss)
                {
                    case "defense401":
                        type = Defence.RocketLauncher;
                        break;
                    case "defense402":
                        type = Defence.LightLaser;
                        break;
                    case "defense403":
                        type = Defence.HeavyLaser;
                        break;
                    case "defense404":
                        type = Defence.GaussCannon;
                        break;
                    case "defense405":
                        type = Defence.IonCannon;
                        break;
                    case "defense406":
                        type = Defence.PlasmaTurret;
                        break;
                    case "defense407":
                        type = Defence.SmallShieldDome;
                        break;
                    case "defense408":
                        type = Defence.LargeShieldDome;
                        break;
                    //case "defense502":
                    //    type = Defence.AntiBallisticMissile;
                    //    break;
                    //case "defense503":
                    //    type = Defence.InterplanetaryMissile;
                    //    break;
                    default:
                        continue;
                }

                int count = node.SelectSingleNode(".//span[@class='level']").GetFirstNumberChildNode(client.ServerCulture);

                yield return new DetectedDefence
                {
                    Building = type,
                    Count = count
                };
            }
        }
    }
}