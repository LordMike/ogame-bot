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
    public class GalaxyPageDebrisSaver : SaverBase
    {
        public override void Run(List<DataObject> result)
        {
            GalaxyPageSystem systemDetails = result.OfType<GalaxyPageSystem>().FirstOrDefault();
            if (systemDetails == null)
                return;

            using (BotDb db = new BotDb())
            {
                long lower = systemDetails.System.LowerCoordinate;
                long upper = systemDetails.System.UpperCoordinate;

                Dictionary<long, DebrisField> toRemove = db.DebrisFields.Where(s => lower <= s.LocationId && s.LocationId <= upper).ToDictionary(s => s.LocationId);

                foreach (GalaxyPageInfoItem item in result.OfType<GalaxyPageInfoItem>())
                {
                    if (item.DebrisField.Total <= 0)
                        continue;

                    DebrisField field;
                    if (!toRemove.TryRemove(item.Planet.Coordinate, out field))
                    {
                        field = new DebrisField
                        {
                            Coordinate = item.Planet.Coordinate
                        };

                        db.DebrisFields.Add(field);
                    }

                    field.LastSeen = DateTimeOffset.Now;
                    field.Resources = item.DebrisField;
                }

                db.DebrisFields.RemoveRange(toRemove.Values);
                db.SaveChanges();
            }
        }
    }
}