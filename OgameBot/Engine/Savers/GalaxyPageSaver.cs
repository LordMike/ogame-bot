using System;
using System.Collections.Generic;
using System.Linq;
using OgameBot.Db;
using OgameBot.Engine.Parsing;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Utilities;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Savers
{
    public class GalaxyPageSaver : SaverBase
    {
        public override void Run(List<DataObject> result)
        {
            GalaxyPageSystem systemDetails = result.OfType<GalaxyPageSystem>().FirstOrDefault();
            if (systemDetails == null)
                return;

            using (BotDb db = new BotDb())
            {
                int systemId = systemDetails.System;
                GalaxyScan scanInfo = db.Scans.SingleOrDefault(s => s.LocationId == systemId);

                if (scanInfo == null)
                {
                    scanInfo = new GalaxyScan
                    {
                        SystemCoordinate = systemDetails.System
                    };

                    db.Scans.Add(scanInfo);
                }

                scanInfo.LastScan = DateTimeOffset.Now;

                // Individual items
                long systemLower = systemDetails.System.LowerCoordinate;
                long systemUpper = systemDetails.System.UpperCoordinate;

                Dictionary<long, GalaxyItem> toRemove = db.GalaxyItems.Where(s => systemLower <= s.LocationId && s.LocationId <= systemUpper).ToDictionary(s => s.LocationId);

                foreach (GalaxyPageInfoItem row in result.OfType<GalaxyPageInfoItem>())
                {
                    GalaxyItem planet;
                    if (!toRemove.TryRemove(row.Planet.Coordinate, out planet))
                    {
                        planet = new GalaxyItem
                        {
                            Coordinate = row.Planet.Coordinate
                        };

                        db.GalaxyItems.Add(planet);
                    }

                    planet.Name = row.Planet.Name;
                    planet.PlayerId = row.PlayerId;

                    if (row.Moon != null)
                    {
                        GalaxyItem moon;
                        if (!toRemove.TryRemove(row.Moon.Coordinate, out moon))
                        {
                            moon = new GalaxyItem
                            {
                                Coordinate = row.Moon.Coordinate
                            };

                            db.GalaxyItems.Add(moon);
                        }

                        moon.Name = row.Moon.Name;
                        moon.PlayerId = row.PlayerId;
                    }
                }

                db.GalaxyItems.RemoveRange(toRemove.Values);
                toRemove.Clear();

                db.SaveChanges();
            }
        }
    }
}