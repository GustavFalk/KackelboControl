using KackelboControl_API.Controllers;
using KackelboControl_API.Helpers;
using KackelboControl_API.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace KackelboControl_API.Repository;

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
        optionsBuilder.UseSqlite($"Data Source=./Repository/KackelboControlDb.db");
    }
}





