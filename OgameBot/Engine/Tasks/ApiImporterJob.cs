using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using OgameApi.Objects;
using OgameApi.Utilities;
using OgameBot.Db;
using OgameBot.Engine.Parsing.Objects;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using ScraperClientLib.Utilities;

namespace OgameBot.Engine.Tasks
{
    public class ApiImporterJob : WorkerBase
    {
        private readonly OGameClient _client;
        private readonly DirectoryInfo _baseDir;

        public ApiImporterJob(OGameClient client, DirectoryInfo baseDir)
        {
            _client = client;
            _baseDir = baseDir;

            if (!_baseDir.Exists)
            {
                _baseDir.Create();
                _baseDir.Refresh();
            }

            ExecutionInterval = TimeSpan.FromHours(1);
        }

        protected override void RunInternal()
        {
            Uri universeUri = new Uri(_client.BaseUri, "/api/universe.xml");
            Uri alliancesUri = new Uri(_client.BaseUri, "/api/alliances.xml");
            Uri playersUri = new Uri(_client.BaseUri, "/api/players.xml");
            Uri serverDataUri = new Uri(_client.BaseUri, "/api/serverData.xml");

            FileInfo universeFile = new FileInfo(Path.Combine(_baseDir.FullName, _client.BaseUri.Host + "-universe.xml"));
            FileInfo alliancesFile = new FileInfo(Path.Combine(_baseDir.FullName, _client.BaseUri.Host + "-alliances.xml"));
            FileInfo playersFile = new FileInfo(Path.Combine(_baseDir.FullName, _client.BaseUri.Host + "-players.xml"));
            FileInfo serverDataFile = new FileInfo(Path.Combine(_baseDir.FullName, _client.BaseUri.Host + "-serverData.xml"));

            if (NeedUpdate(universeUri, universeFile, "universe").Sync())
            {
                Update(universeUri, universeFile).Sync();

                Universe model = XmlModelSerializer.Deserialize<Universe>(universeFile);
                ProcessData(model);
            }

            if (NeedUpdate(alliancesUri, alliancesFile, "alliances").Sync())
            {
                Update(alliancesUri, alliancesFile).Sync();

                AlliancesContainer model = XmlModelSerializer.Deserialize<AlliancesContainer>(alliancesFile);
                ProcessData(model);
            }

            if (NeedUpdate(playersUri, playersFile, "players").Sync())
            {
                Update(playersUri, playersFile).Sync();

                PlayersContainer model = XmlModelSerializer.Deserialize<PlayersContainer>(playersFile);
                ProcessData(model);
            }

            if (NeedUpdate(serverDataUri, serverDataFile, "serverData").Sync())
            {
                Update(serverDataUri, serverDataFile).Sync();

                ServerData model = XmlModelSerializer.Deserialize<ServerData>(serverDataFile);
                ProcessData(model);
            }
        }

        private void ProcessData(AlliancesContainer model)
        {

        }

        private void ProcessData(PlayersContainer model)
        {
            using (BotDb db = new BotDb())
            {
                Dictionary<int, DbPlayer> allPlayers = db.Players.ToDictionary(s => s.PlayerId);
                List<DbPlayer> newPlayers = new List<DbPlayer>();

                for (int i = 0; i < model.Players.Length; i++)
                {
                    var player = model.Players[i];
                    DbPlayer dbPlayer;
                    if (!allPlayers.TryGetValue(player.Id, out dbPlayer))
                    {
                        dbPlayer = new DbPlayer
                        {
                            PlayerId = player.Id
                        };

                        newPlayers.Add(dbPlayer);
                        allPlayers[player.Id] = dbPlayer;
                    }

                    dbPlayer.Name = player.Name;
                    dbPlayer.Status = ParseStatus(player.Status);

                    if (i % 250 == 0)
                    {
                        db.Players.AddRange(newPlayers);

                        db.SaveChanges();

                        newPlayers.Clear();
                    }
                }

                db.Players.AddRange(newPlayers);

                db.SaveChanges();
            }
        }

        private void ProcessData(ServerData model)
        {

        }

        private void ProcessData(Universe model)
        {
            using (BotDb db = new BotDb())
            {
                Dictionary<long, DbPlanet> allPlanets = db.Planets.ToDictionary(s => s.LocationId);
                Dictionary<int, DbPlayer> allPlayers = db.Players.ToDictionary(s => s.PlayerId);

                List<DbPlanet> newPlanets = new List<DbPlanet>();
                List<DbPlayer> newPlayers = new List<DbPlayer>();

                for (int i = 0; i < model.Planets.Length; i++)
                {
                    Planet planet = model.Planets[i];
                    Coordinate planetCoords = Coordinate.Parse(planet.Coords, CoordinateType.Planet);

                    DbPlanet dbPlanet;
                    if (!allPlanets.TryGetValue(planetCoords.Id, out dbPlanet))
                    {
                        dbPlanet = new DbPlanet
                        {
                            Coordinate = planetCoords
                        };

                        newPlanets.Add(dbPlanet);
                        allPlanets[planetCoords.Id] = dbPlanet;
                    }

                    dbPlanet.Name = planet.Name;
                    dbPlanet.PlayerId = planet.Player;

                    if (planet.Moon != null)
                    {
                        Coordinate moonCoords = Coordinate.Create(planetCoords, CoordinateType.Moon);

                        DbPlanet dbMoon;
                        if (!allPlanets.TryGetValue(moonCoords.Id, out dbMoon))
                        {
                            dbMoon = new DbPlanet
                            {
                                Coordinate = moonCoords
                            };

                            newPlanets.Add(dbMoon);
                            allPlanets[moonCoords.Id] = dbMoon;
                        }

                        dbMoon.Name = planet.Moon.Name;
                    }

                    DbPlayer dbPlayer;
                    if (!allPlayers.TryGetValue(planet.Player, out dbPlayer))
                    {
                        dbPlayer = new DbPlayer
                        {
                            PlayerId = planet.Player
                        };

                        newPlayers.Add(dbPlayer);
                        allPlayers[dbPlayer.PlayerId] = dbPlayer;
                    }

                    if (i % 250 == 0)
                    {
                        db.Planets.AddRange(newPlanets);
                        db.Players.AddRange(newPlayers);

                        db.SaveChanges();

                        newPlanets.Clear();
                        newPlayers.Clear();
                    }
                }

                db.Planets.AddRange(newPlanets);
                db.Players.AddRange(newPlayers);

                db.SaveChanges();
            }
        }

        private static async Task Update(Uri uri, FileInfo file)
        {
            using (HttpClient wc = new HttpClient())
            using (Stream ws = await wc.GetStreamAsync(uri))
            using (FileStream fs = file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                await ws.CopyToAsync(fs);
        }

        private static async Task<bool> NeedUpdate(Uri uri, FileInfo file, string rootElement)
        {
            if (!file.Exists)
                return true;

            if (DateTime.UtcNow - file.LastWriteTimeUtc < TimeSpan.FromMinutes(5))
                // File was last written to less than 5 minutes ago
                return false;

            DateTime serverTime = await GetServerDetails(uri);
            DateTime current = GetXmlTimestamp(file, rootElement);

            if (serverTime - current > TimeSpan.FromHours(1))
            {
                // Update
                return true;
            }

            return false;
        }

        private static DateTime GetXmlTimestamp(FileInfo file, string rootElement)
        {
            if (!file.Exists)
                return DateTime.MinValue;

            try
            {
                using (FileStream fs = file.OpenRead())
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    reader.ReadToFollowing(rootElement);
                    long timestamp = long.Parse(reader.GetAttribute("timestamp"));

                    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to fetch timestamp for {file.FullName}: {ex.Message}");
                return DateTime.MinValue;
            }
        }

        private static async Task<DateTime> GetServerDetails(Uri uri)
        {
            try
            {
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Head, uri);

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage resp = await client.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead))
                    {
                        DateTimeOffset? lastModified = resp.Content.Headers.LastModified;

                        return lastModified?.UtcDateTime ?? DateTime.MaxValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to HEAD {uri} (General): {ex.Message}");
            }

            return DateTime.MaxValue;
        }

        private static PlayerStatus ParseStatus(string status)
        {
            PlayerStatus result = PlayerStatus.None;

            if (string.IsNullOrEmpty(status))
                return result;

            foreach (char part in status)
            {
                switch (part)
                {
                    case 'v':
                        result |= PlayerStatus.Vacation;
                        break;
                    case 'b':
                        result |= PlayerStatus.Banned;
                        break;
                    case 'i':
                        result |= PlayerStatus.Inactive;
                        break;
                    case 'I':
                        result |= PlayerStatus.LongInactive;
                        break;
                    case 'o':
                        result |= PlayerStatus.Outlaw;
                        break;
                    case 'a':
                        result |= PlayerStatus.Admin;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
    }
}