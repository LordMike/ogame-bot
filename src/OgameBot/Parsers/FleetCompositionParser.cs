using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using HtmlAgilityPack;
using OgameBot.Engine;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using OgameBot.Utilities;

namespace OgameBot.Parsers
{
    public static class FleetCompositionParser
    {
        public static FleetComposition ParseFleetInfoTable(OGameClient client, HtmlNode fleetInfoContainer)
        {
            HtmlNode fleetInfoTable = fleetInfoContainer.SelectSingleNode(".//table[@class='fleetinfo']");
            Debug.Assert(fleetInfoTable != null);

            Dictionary<string, ShipType> revMapShips = client.StringProvider.GetReverseMap<ShipType>();
            Dictionary<string, ResourceType> revMapResources = client.StringProvider.GetReverseMap<ResourceType>();

            FleetComposition res = new FleetComposition();
            Resources resources = new Resources();

            HtmlNodeCollection rows = fleetInfoTable.SelectNodes(".//tr");

            foreach (HtmlNode row in rows)
            {
                HtmlNodeCollection tds = row.SelectNodes(".//td");

                if (tds?.Count != 2)
                    continue;

                string name = tds[0].InnerText.Trim().TrimEnd(':');
                int count = int.Parse(tds[1].InnerText, NumberStyles.AllowThousands | NumberStyles.Integer, client.ServerCulture);

                ShipType asShip;
                ResourceType asResource;

                if (revMapShips.TryGetValue(name, out asShip))
                {
                    res.Ships.AddOrUpdate(asShip, () => count, (type, i) => i + count);
                }
                else if (revMapResources.TryGetValue(name, out asResource))
                {
                    if (asResource == ResourceType.Metal)
                    {
                        resources.Metal += count;
                    }
                    else if (asResource == ResourceType.Crystal)
                    {
                        resources.Crystal += count;
                    }
                }
            }

            res.Resources = resources;

            return res;
        }
    }
}