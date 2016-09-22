using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Engine.Parsing.UtilityParsers;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Parsing
{
    public class FleetMovementPageParser : BaseParser
    {
        private static readonly Regex FleetIdRegex = new Regex(@"fleet([\d]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseContainer container)
        {
            return container.RequestMessage.RequestUri.Query.Contains("page=movement");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseContainer container)
        {
            HtmlDocument doc = container.ResponseHtml.Value;
            HtmlNodeCollection imageFields = doc.DocumentNode.SelectNodes("//div[starts-with(@id, 'fleet')]");

            if (imageFields == null)
                yield break;

            foreach (HtmlNode node in imageFields)
            {
                string idText = node.GetAttributeValue("id", null);
                int id = int.Parse(FleetIdRegex.Match(idText).Groups[1].Value, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture);

                MissionType missionType = (MissionType)node.GetAttributeValue("data-mission-type", 0);
                bool isReturn = node.GetAttributeValue("data-return-flight", false);
                int arrivalSec = node.GetAttributeValue("data-arrival-time", 0);
                DateTimeOffset arrival = DateTimeOffset.FromUnixTimeSeconds(arrivalSec);

                HtmlNode fleetInfo = node.SelectSingleNode(".//span[@class='starStreak']");
                FleetComposition composition = FleetCompositionParser.ParseFleetInfoTable((OGameClient) client, fleetInfo);

                FleetEndpointInfo endpointOrigin = ParseEndpoint(node.SelectSingleNode("./span[@class='originData']"));
                FleetEndpointInfo endpointDestination = ParseEndpoint(node.SelectSingleNode("./span[@class='destinationData']"));

                yield return new FleetInfo
                {
                    Id = id,
                    ArrivalTime = arrival,
                    IsReturning = isReturn,
                    MissionType = missionType,
                    Origin = endpointOrigin,
                    Destination = endpointDestination,
                    Composition = composition
                };
            }
        }

        private FleetEndpointInfo ParseEndpoint(HtmlNode node)
        {
            // destinationCoords
            HtmlNode coordsNode = node.SelectSingleNode(".//span[contains(@class, 'originCoords') or contains(@class, 'destinationCoords')]");
            HtmlNode planetNode = node.SelectSingleNode(".//span[contains(@class, 'originPlanet') or contains(@class, 'destinationPlanet')]");

            string coordsText = coordsNode.InnerText;
            string playerName = coordsNode.GetAttributeValue("title", "");
            string planetName = planetNode.InnerText;

            return new FleetEndpointInfo
            {
                // TODO: planet always
                Coordinate = Coordinate.Parse(coordsText, CoordinateType.Planet),
                EndpointName = planetName,
                Playername = playerName
            };
        }
    }
}