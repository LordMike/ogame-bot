using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using OgameBot.Db;
using OgameBot.Objects;

namespace OgameBot.Engine.Tasks
{
    public class ScannerJob : WorkerBase
    {
        private static readonly TimeSpan RescanInterval = TimeSpan.FromHours(8);

        private readonly OGameClient _client;
        private readonly SystemCoordinate _from;
        private readonly SystemCoordinate _to;

        public ScannerJob(OGameClient client, SystemCoordinate from, SystemCoordinate to)
        {
            _client = client;
            _from = from;
            _to = to;

            if (_to < _from)
            {
                _from = to;
                _to = from;
            }

            ExecutionInterval = TimeSpan.FromMinutes(15);
        }

        protected override void RunInternal()
        {
            // Get existing scan infoes
            Dictionary<int, GalaxyScan> existing;
            using (BotDb db = new BotDb())
            {
                int from = _from;
                int to = _to;

                existing = db.Scans.Where(s => from <= s.LocationId && s.LocationId <= to).ToDictionary(s => s.LocationId);
            }

            for (byte gal = _from.Galaxy; gal <= _to.Galaxy; gal++)
            {
                short sFrom = gal == _from.Galaxy ? _from.System : (short)0;
                short sTo = gal == _to.Galaxy ? _to.System : _client.Settings.Systems;

                for (short sys = sFrom; sys <= sTo; sys++)
                {
                    SystemCoordinate coord = new SystemCoordinate(gal, sys);

                    GalaxyScan exists;
                    if (existing.TryGetValue(coord, out exists))
                    {
                        if (DateTimeOffset.Now - exists.LastScan < RescanInterval)
                            // Ignore
                            continue;
                    }

                    // Scan
                    HttpRequestMessage req = _client.RequestBuilder.GetGalaxyContent(coord);
                    _client.IssueRequest(req);
                }
            }
        }
    }
}
