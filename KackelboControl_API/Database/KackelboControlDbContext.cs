using KackelboControl_API.Database.Entities;
using KackelboControl_API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KackelboControl_API.Database;

public class KackelboControlDbContext : DbContext
{

    public KackelboControlDbContext(DbContextOptions<KackelboControlDbContext> options) : base(options) { }

    public DbSet<SensorTriggers> SensorTriggers { get; set; }
    public DbSet<SensorValue> SensorValueLog { get; set; }
    public DbSet<RelayChange> RelayChangeLog { get; set; }
    public DbSet<Sunlight> SunRiseSunSet { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var sunlightPosts = LocalJsonReader.GetSunlightFromJson();
        modelBuilder.Entity<Sunlight>().HasData(sunlightPosts);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=./Database/KackelboControlDb.db");
    }
}





