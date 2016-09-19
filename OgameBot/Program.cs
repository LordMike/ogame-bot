using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using OgameBot.Engine;
using OgameBot.Engine.Savers;
using OgameBot.Engine.Tasks;
using OgameBot.Objects;

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

            // Setup
            OGameStringProvider stringProvider = OGameStringProvider.Load(@"Resources\strings-en.json");
            
            CultureInfo clientServerCulture = CultureInfo.GetCultureInfo("da-DK");
            
            // Processing
            OGameClient client = new OGameClient(config.Server, stringProvider, config.Username, config.Password);
            client.Settings.ServerUtcOffset = TimeSpan.FromHours(1);
            client.Settings.Galaxies = 8;
            client.Settings.Systems = 499;
            client.ServerCulture = clientServerCulture;

            client.RegisterSaver(new GalaxyPageSaver());
            client.RegisterSaver(new EspionageReportSaver());
            client.RegisterSaver(new GalaxyPageDebrisSaver());

            OgameClientProxy xx = new OgameClientProxy("127.0.0.1", 9400, client);
            xx.SubstituteRoot = new Uri($"https://{config.Server}");
            xx.Start();

            client.RegisterDefaultHeader("Accept-Language", "en-GB,en;q=0.8,da;q=0.6");
            client.RegisterDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.RegisterDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");

            client.PerformLogin();

            // Preparing
            ScannerJob job = new ScannerJob(client, new SystemCoordinate(6, 60), new SystemCoordinate(6, 100));
            job.Start();

            // Work
            Console.ReadLine();
        }
    }
}
