using System.Threading;
using System.Threading.Tasks;
using OgameBot.Objects;

namespace OgameBot.Engine.Tasks
{
    public class FarmingBot
    {
        private readonly OGameClient _client;
        private readonly SystemCoordinate _startSystem;
        private readonly SystemCoordinate _endSystem;

        public FarmingBot(OGameClient client, SystemCoordinate startSystem, SystemCoordinate endSystem)
        {
            _client = client;
            _startSystem = startSystem;
            _endSystem = endSystem;
        }

        public void Start()
        {
            // Start scanner
            ScannerJob scanner = new ScannerJob(_client, _startSystem, _endSystem);
            scanner.Start();

            // Start worker
            Task.Factory.StartNew(Worker);
        }

        private void Worker()
        {
            while (true)
            {
                // 




                Thread.Sleep(1000);
            }
        }
    }
}