using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Data;

public class FootballLeagueDbContext : DbContext
{
    public FootballLeagueDbContext(DbContextOptions<FootballLeagueDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("FootboolLeague");

        builder.Entity<Team>().HasData(
            new Team { TeamId = 1, Name = "수원삼성",  },
            new Team { TeamId = 2, Name = "레알마드리드",  },
            new Team { TeamId = 3, Name = "맨유", }
            );

        builder.Entity<Coach>().HasData(
            new Coach { Id = 1, Name = "무리뉴" },
            new Coach { Id = 2, Name = "과르디올라" }
            ); 
    }

    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<Coach> Coaches { get; set; } = null!;
}
