using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using OgameApi.Objects;
using OgameApi.Utilities;
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
            throw new NotImplementedException("Do something sensible");
        }

        private void ProcessData(PlayersContainer model)
        {
            throw new NotImplementedException("Do something sensible");
        }

        private void ProcessData(ServerData model)
        {
            throw new NotImplementedException("Do something sensible");
        }

        private void ProcessData(Universe model)
        {
            throw new NotImplementedException("Do something sensible");
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
    }
}