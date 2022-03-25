using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityFrameworkCore.Domain;
using EntityFrameworkNet5.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Data
{
    public class FootBallLeagueDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=(localdb)\\MSSQLLocalDb; Initial Catalog=FootballLeague_EFCore")
                .LogTo(Console.WriteLine, new [] { DbLoggerCategory.Database.Command.Name}, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasMany(m => m.HomeMatches)
                .WithOne(m => m.HomeTeam)
                .HasForeignKey(m => m.HomeTeamId)
                .IsRequired()
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(m => m.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .IsRequired()
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            
            modelBuilder.Entity<TeamCoachesLeaguesView>().HasNoKey().ToView("TeamsCoachesLeagues");

            modelBuilder.ApplyConfiguration(new LeagueSeedConfiguration());
            modelBuilder.ApplyConfiguration(new TeamSeedConfiguration());
            modelBuilder.ApplyConfiguration(new CoachSeedConfiguration());
        } 

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken =default)
        {
            var entries = ChangeTracker.Entries().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified).ToList();
            entries.ForEach(entry => 
            {
                var auditableObject = (BaseDomainObject)entry.Entity;
                auditableObject.ModifiedDate = DateTime.Now;
                if (entry.State == EntityState.Added)
                {
                    auditableObject.CreatedDate = DateTime.Now;
                }
            });
            return await base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Team> Teams {get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<TeamCoachesLeaguesView> TeamCoachesLeagues { get; set; }

        // public DbSet<Test> test { get; set; }
    }  
}