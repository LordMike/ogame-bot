using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using OgameBot.Engine;
using OgameBot.Engine.Savers;
using OgameBot.Engine.Tasks;
using OgameBot.Logging;
using OgameBot.Objects;
using ScraperClientLib.Engine.Interventions;
using ScraperClientLib.Engine.Parsing;

namespace OgameBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!File.Exists("config.json"))
            {
                Console.WriteLine("Please copy config.template.json to config.json and fill it out");
                return;
            }

            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

            Logger.Instance.Log(LogLevel.Info, $"Loaded settings, user: {config.Username}, server: {config.Server}");

            // Setup
            OGameStringProvider stringProvider = OGameStringProvider.Load(@"Resources\strings-en.json");
            CultureInfo clientServerCulture = CultureInfo.GetCultureInfo("da-DK");

            // Processing
            OGameClient client = new OGameClient(config.Server, stringProvider, config.Username, config.Password);
            client.Settings.ServerUtcOffset = TimeSpan.FromHours(1);
            client.Settings.Galaxies = 8;
            client.Settings.Systems = 499;
            client.ServerCulture = clientServerCulture;

            Logger.Instance.Log(LogLevel.Debug, "Prepared OGameClient");

            // Savers
            client.RegisterSaver(new GalaxyPageSaver());
            client.RegisterSaver(new EspionageReportSaver());
            client.RegisterSaver(new GalaxyPageDebrisSaver());

            // UA stuff
            client.RegisterDefaultHeader("Accept-Language", "en-GB,en;q=0.8,da;q=0.6");
            client.RegisterDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.RegisterDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");

            Logger.Instance.Log(LogLevel.Debug, "Prepared user agent");

            // Show details
            foreach (IInterventionHandler item in client.GetIntervention())
                Logger.Instance.Log(LogLevel.Info, $"Loaded Intervention Handler: {item.GetType().FullName}");

            foreach (BaseParser item in client.GetParsers())
                Logger.Instance.Log(LogLevel.Info, $"Loaded Parser: {item.GetType().FullName}");

            foreach (SaverBase item in client.GetSavers())
                Logger.Instance.Log(LogLevel.Info, $"Loaded Saver: {item.GetType().FullName}");

            // Start proxy
            OgameClientProxy proxy = new OgameClientProxy("127.0.0.1", 9400, client);
            proxy.SubstituteRoot = new Uri($"https://{config.Server}");
            proxy.Start();

            Logger.Instance.Log(LogLevel.Warning, $"Prepared reverse proxy, visit: {proxy.ListenPrefix}");

            // Kick-off
            client.PerformLogin();

            // Example job
            ApiImporterJob job1 = new ApiImporterJob(client, new DirectoryInfo("temp"));
            job1.Start();

            ScannerJob job = new ScannerJob(client, new SystemCoordinate(6, 60), new SystemCoordinate(6, 100));
            job.Start();

            // Work
            Console.ReadLine();
        }
    }
}
