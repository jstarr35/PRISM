using Microsoft.EntityFrameworkCore;
using PRISM.Api.Entities;

namespace PRISM.Api.Infrastructure;

public class PrismDbContext : DbContext
{
    public PrismDbContext(DbContextOptions<PrismDbContext> options) : base(options) { }

    public DbSet<StravaAthlete> Athletes => Set<StravaAthlete>();
    public DbSet<StravaActivity> Activities => Set<StravaActivity>();
    public DbSet<StravaSyncRun> SyncRuns => Set<StravaSyncRun>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StravaActivity>()
            .HasIndex(a => a.StravaActivityId)
            .IsUnique();

        modelBuilder.Entity<StravaAthlete>()
            .HasIndex(a => a.StravaAthleteId)
            .IsUnique();
    }
}
