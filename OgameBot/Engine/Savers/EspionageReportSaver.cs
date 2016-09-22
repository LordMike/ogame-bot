using System.Collections.Generic;
using System.Linq;
using OgameBot.Db;
using OgameBot.Engine.Parsing;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot.Engine.Savers
{
    public class EspionageReportSaver : SaverBase
    {
        public override void Run(List<DataObject> result)
        {
            List<EspionageReport> reports = result.OfType<EspionageReport>().ToList();
            if (!reports.Any())
                return;

            using (BotDb db = new BotDb())
            {
                long[] locIds = reports.Select(s => s.Coordinate.Id).ToArray();
                Dictionary<long, DbPlanet> existing = db.Planets.Where(s => locIds.Contains(s.LocationId)).ToDictionary(s => s.LocationId);

                foreach (EspionageReport report in reports)
                {
                    DbPlanet item;
                    if (!existing.TryGetValue(report.Coordinate, out item))
                    {
                        item = new DbPlanet
                        {
                            Coordinate = report.Coordinate
                        };

                        db.Planets.Add(item);
                    }

                    if (report.Details.HasFlag(ReportDetails.Resources))
                    {
                        item.Resources = report.Resources;
                        item.LastResourcesTime = report.Sent;
                    }

                    if (report.Details.HasFlag(ReportDetails.Buildings))
                    {
                        item.PlanetInfo.Buildings = report.DetectedBuildings;
                        item.LastBuildingsTime = report.Sent;
                    }

                    if (report.Details.HasFlag(ReportDetails.Defense))
                    {
                        item.PlanetInfo.Defences = report.DetectedDefence;
                        item.LastDefencesTime = report.Sent;
                    }

                    if (report.Details.HasFlag(ReportDetails.Ships))
                    {
                        item.PlanetInfo.Ships = report.DetectedShips;
                        item.LastShipsTime = report.Sent;
                    }

                    item.Update();
                }

                db.SaveChanges();
            }
        }
    }
}