using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using OgameBot.Utilities;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers
{
    public class GalaxyPageParser : BaseParser
    {
        private static readonly Regex CssPlayerStatus = new Regex("status_abbr_([a-z]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex LinkRelRegex = new Regex(@"(?:alliance|player)([\d]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override bool ShouldProcessInternal(ResponseDocument document)
        {
            return document.RequestMessage.RequestUri.Query.Contains("page=galaxyContent");
        }

        public override IEnumerable<DataObject> ProcessInternal(ClientBase client, ResponseDocument document)
        {
            HtmlDocument doc = document.ResponseHtml.Value;
            HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//table[@id='galaxytable']");
            HtmlNodeCollection rows = tableNode?.SelectNodes("./tbody/tr");

            if (rows == null)
                yield break;

            int galaxy = tableNode.GetAttributeValue("data-galaxy", 0);
            int system = tableNode.GetAttributeValue("data-system", 0);

            SystemCoordinate systemCoordinate = new SystemCoordinate((byte)galaxy, (short)system);

            GalaxyPageSystem systemResult = new GalaxyPageSystem();
            systemResult.System = systemCoordinate;

            foreach (HtmlNode row in rows)
            {
                string positionText = row.SelectSingleNode("./td[contains(@class, 'position')]").InnerText;
                byte position = byte.Parse(positionText, NumberStyles.Integer, client.ServerCulture);

                HtmlNode planetNode = row.SelectSingleNode("./td[@data-planet-id]");
                HtmlNode moonNode = row.SelectSingleNode("./td[@data-moon-id]");

                if (planetNode == null)
                    systemResult.AbsentItems.Add(Coordinate.Create(systemCoordinate, position, CoordinateType.Planet));

                if (moonNode == null)
                    systemResult.AbsentItems.Add(Coordinate.Create(systemCoordinate, position, CoordinateType.Moon));

                if (planetNode == null && moonNode == null)
                    // Nothing to do here
                    continue;

                GalaxyPageInfoItem item = new GalaxyPageInfoItem();

                // Process planet
                if (planetNode != null)
                {
                    systemResult.PresentItems.Add(Coordinate.Create(systemCoordinate, position, CoordinateType.Planet));

                    int planetId = planetNode.GetAttributeValue("data-planet-id", 0);
                    string planetName = row.SelectSingleNode("./td[contains(@class, 'planetname')]").InnerText.Trim();

                    item.Planet = new GalaxyPageInfoPartItem
                    {
                        Coordinate = Coordinate.Create(systemCoordinate, position, CoordinateType.Planet),
                        Id = planetId,
                        Name = planetName
                    };
                }

                // Process moon
                if (moonNode != null)
                {
                    systemResult.PresentItems.Add(Coordinate.Create(systemCoordinate, position, CoordinateType.Moon));

                    int moonId = moonNode.GetAttributeValue("data-moon-id", 0);
                    string moonName = row.SelectSingleNode(".//span[@class='textNormal']").InnerText.Trim();

                    item.Moon = new GalaxyPageInfoPartItem
                    {
                        Coordinate = Coordinate.Create(systemCoordinate, position, CoordinateType.Moon),
                        Id = moonId,
                        Name = moonName
                    };
                }

                // Process debris field
                {
                    HtmlNode debrisNode = row.SelectSingleNode(".//td[contains(@class, 'debris')]");
                    HtmlNodeCollection debrisContents = debrisNode.SelectNodes(".//li[@class='debris-content']");
                    int debrisMetal = 0, debrisCrystal = 0;

                    if (debrisContents?.Count == 2)
                    {
                        string metalText = debrisContents[0].InnerText;
                        string crystalText = debrisContents[1].InnerText;

                        metalText = metalText.Split(' ').Last();
                        crystalText = crystalText.Split(' ').Last();

                        debrisMetal = int.Parse(metalText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture);
                        debrisCrystal = int.Parse(crystalText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture);
                    }

                    item.DebrisField = new Resources { Metal = debrisMetal, Crystal = debrisCrystal };
                }

                // Process player
                if (planetNode != null)
                {
                    string playerName;
                    int playerId;
                    PlayerStatus playerStatus = PlayerStatus.None;

                    HtmlNode planetOwnerNode = row.SelectSingleNode(".//td[contains(@class, 'playername')]");
                    HtmlNode playerLinkNode = planetOwnerNode.SelectSingleNode(".//a[starts-with(@rel, 'player')]");

                    if (playerLinkNode == null)
                    {
                        // Own planet?
                        Debug.Assert(planetOwnerNode.SelectNodes(".//a") == null);
                        playerLinkNode = planetOwnerNode.SelectSingleNode(".//span[@class='status_abbr_active']");

                        playerName = playerLinkNode.InnerText.Trim();
                        playerId = -1;
                        playerStatus = PlayerStatus.Active;
                    }
                    else
                    {
                        // Some users planet
                        playerName = playerLinkNode.InnerText.Trim();
                        playerId = planetOwnerNode.SelectSingleNode(".//a[@data-playerid]").GetAttributeValue("data-playerid", 0);

                        HtmlNodeCollection playerStatusSpans = planetOwnerNode.SelectNodes("./span[@class='status']/span");

                        if (playerStatusSpans != null)
                        {
                            foreach (HtmlNode playerStatusSpan in playerStatusSpans)
                            {
                                string @class = playerStatusSpan.GetCssClasses(CssPlayerStatus).FirstOrDefault();

                                switch (@class)
                                {
                                    case "status_abbr_noob":
                                        playerStatus |= PlayerStatus.Noob;
                                        break;
                                    case "status_abbr_vacation":
                                        playerStatus |= PlayerStatus.Vacation;
                                        break;
                                    case "status_abbr_strong":
                                        playerStatus |= PlayerStatus.Strong;
                                        break;
                                    case "status_abbr_banned":
                                        playerStatus |= PlayerStatus.Banned;
                                        break;
                                    case "status_abbr_active":
                                        playerStatus |= PlayerStatus.Active;
                                        break;
                                    case "status_abbr_inactive":
                                        playerStatus |= PlayerStatus.Inactive;
                                        break;
                                    case "status_abbr_longinactive":
                                        playerStatus |= PlayerStatus.LongInactive;
                                        break;
                                    case "status_abbr_outlaw":
                                        playerStatus |= PlayerStatus.Outlaw;
                                        break;
                                    case "status_abbr_honorableTarget":
                                        playerStatus |= PlayerStatus.HonorableTarget;
                                        break;
                                    case "status_abbr_ally_own":
                                        playerStatus |= PlayerStatus.AllyOwn;
                                        break;
                                    case "status_abbr_ally_war":
                                        playerStatus |= PlayerStatus.AllyWar;
                                        break;
                                    case "status_abbr_buddy":
                                        playerStatus |= PlayerStatus.Buddy;
                                        break;
                                    case "status_abbr_admin":
                                        playerStatus |= PlayerStatus.Admin;
                                        break;
                                }
                            }
                        }
                    }

                    item.PlayerId = playerId;
                    item.PlayerName = playerName;
                    item.PlayerStatus = playerStatus;
                }

                // Process ally
                if (planetNode != null)
                {
                    string playerAllyName = null;
                    int playerAllyId = 0;

                    HtmlNode planetOwnerAllyNode = row.SelectSingleNode("./td[contains(@class, 'allytag')]");
                    HtmlNode playerAllyInfoNode = planetOwnerAllyNode.SelectSingleNode(".//span[starts-with(@rel, 'alliance')]");

                    if (playerAllyInfoNode != null)
                    {
                        string playerAllyIdText = LinkRelRegex.Match(playerAllyInfoNode.GetAttributeValue("rel", "")).Groups[1].Value;

                        playerAllyName = playerAllyInfoNode.SelectSingleNode(".//h1").InnerText.Trim();
                        playerAllyId = int.Parse(playerAllyIdText);
                    }

                    item.AllyId = playerAllyId;
                    item.AllyName = playerAllyName;
                }

                // TODO: Rankings

                yield return item;
            }

            yield return systemResult;
        }
    }

    [Flags]
    public enum PlayerStatus
    {
        None = 0,
        Noob = 1 << 0,
        Vacation = 1 << 1,
        Strong = 1 << 2,
        Banned = 1 << 3,
        Active = 1 << 4,
        Inactive = 1 << 5,
        LongInactive = 1 << 6,
        Outlaw = 1 << 7,
        HonorableTarget = 1 << 8,
        AllyOwn = 1 << 9,
        AllyWar = 1 << 10,
        Buddy = 1 << 11,
        Admin = 1 << 12
    }

    public class GalaxyPageSystem : DataObject
    {
        public SystemCoordinate System { get; set; }

        public List<Coordinate> PresentItems { get; set; }

        public List<Coordinate> AbsentItems { get; set; }

        public GalaxyPageSystem()
        {
            PresentItems = new List<Coordinate>();
            AbsentItems = new List<Coordinate>();
        }

        public override string ToString()
        {
            return $"{System}. Coordinates: {PresentItems.Count + AbsentItems.Count:N0}, present: {PresentItems.Count:N0}, absent: {AbsentItems.Count:N0}";
        }
    }

    public class GalaxyPageInfoItem : DataObject
    {
        public GalaxyPageInfoPartItem Planet { get; set; }

        public GalaxyPageInfoPartItem Moon { get; set; }

        public Resources DebrisField { get; set; }

        public string PlayerName { get; set; }

        public int PlayerId { get; set; }

        public PlayerStatus PlayerStatus { get; set; }

        public string AllyName { get; set; }

        public int AllyId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Planet.Coordinate} ({Planet.Id}: {Planet.Name}), player ({PlayerId}) {PlayerName}");

            if (DebrisField.Total > 0)
                sb.Append($"-- DM: {DebrisField.Metal:N0}, DC: {DebrisField.Crystal:N0}");

            if (Moon != null)
                sb.Append(" -- W/MOON");

            return sb.ToString();
        }
    }

    public class GalaxyPageInfoPartItem
    {
        public Coordinate Coordinate { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}