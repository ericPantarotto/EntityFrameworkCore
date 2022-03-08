using System;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Data
{
    public class FootBallLeagueDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=(localdb)\\MSSQLLocalDb; Initial Catalog= FootballLeague_EFCore")
                .LogTo(Console.WriteLine, new [] { DbLoggerCategory.Database.Command.Name}, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging();
        }
        public DbSet<Team> Teams {get; set; }
        public DbSet<League> Leagues { get; set; }
        // public DbSet<Test> test { get; set; }
    }  
}