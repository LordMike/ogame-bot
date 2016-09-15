using System.Data.Entity;

namespace OgameBot.Db
{
    public class BotDb : DbContext
    {
        public DbSet<GalaxyScan> Scans { get; set; }

        public DbSet<GalaxyItem> GalaxyItems { get; set; }

        public DbSet<DebrisField> DebrisFields { get; set; }
    }
}
