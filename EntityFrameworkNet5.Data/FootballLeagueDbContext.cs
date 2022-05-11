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
    public class FootBallLeagueDbContext : AuditableFootballLeagueDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                    connectionString: "Data source=(localdb)\\MSSQLLocalDb; Initial Catalog=FootballLeague_EFCore", 
                    sqlServerOptionsAction: sqlOptions =>
                    { 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    })
                .LogTo(Console.WriteLine, new [] { DbLoggerCategory.Database.Command.Name}, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                       
            modelBuilder.Entity<TeamCoachesLeaguesView>().HasNoKey().ToView("TeamsCoachesLeagues");

            modelBuilder.ApplyConfiguration(new LeagueConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new CoachConfiguration());

            //Set all FK to restrict 
            modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys())
                .Where(x => !x.IsOwnership && x.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList().ForEach(fk => 
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                });
            //Indicate which tables has history/temporal tables
            modelBuilder
                .Entity<Team>()
                .ToTable("Teams", b => b.IsTemporal());
        } 

        // protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        // {
        //     configurationBuilder.Properties<string>()
        //         // .AreUnicode(false)
        //         .HaveMaxLength(50);
        // }
        public DbSet<Team> Teams {get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<TeamCoachesLeaguesView> TeamCoachesLeagues { get; set; }

        public DbSet<TestModel> TestModels { get; set; }
    }  
}