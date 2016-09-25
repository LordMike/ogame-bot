using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace OgameBot.Db
{
    public class BotDb : DbContext
    {
        public DbSet<GalaxyScan> Scans { get; set; }

        public DbSet<DbPlanet> Planets { get; set; }

        public DbSet<DbPlayer> Players { get; set; }

        public DbSet<DebrisField> DebrisFields { get; set; }

        public override int SaveChanges()
        {
            HandleAuditing();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            HandleAuditing();

            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            HandleAuditing();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleAuditing()
        {
            ChangeTracker.DetectChanges();

            IEnumerable<DbEntityEntry> entries = ChangeTracker.Entries();

            foreach (DbEntityEntry entry in entries)
            {
                if (entry.State == EntityState.Unchanged)
                    continue;

                ICreatedOn asICreatedOn = entry.Entity as ICreatedOn;
                IModifiedOn asIModifiedOn = entry.Entity as IModifiedOn;

                if (entry.State == EntityState.Added)
                {
                    if (asICreatedOn != null)
                        asICreatedOn.CreatedOn = DateTimeOffset.Now;

                    if (asIModifiedOn != null)
                        asIModifiedOn.UpdatedOn = DateTimeOffset.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (asIModifiedOn != null)
                        asIModifiedOn.UpdatedOn = DateTimeOffset.Now;
                }
            }
        }
    }

    public interface ICreatedOn
    {
        DateTimeOffset CreatedOn { get; set; }
    }

    public interface IModifiedOn
    {
        DateTimeOffset UpdatedOn { get; set; }
    }
}
