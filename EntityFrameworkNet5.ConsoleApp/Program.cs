using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;

namespace EntityFrameworkNet5.ConsoleApp
{
    public class Program
    {
        private static readonly FootBallLeagueDbContext context = new();
        static async Task Main(string[] args)
        {
            // await InitialInsert();
            //await InsertWithTeam();
            await AddTeamLeagueTogether();
        

            Console.WriteLine("Press any key to end ...");
            Console.ReadKey();
        }

        static async Task InitialInsert()
        {
            await context.Leagues.AddAsync(new League {Name = "Red Stripe Premiere League"});
            
            var league = new League{ Name= "La Liga"};
            await context.AddAsync(league);
            
            await context.SaveChangesAsync();
        }

        static async Task InsertWithTeam()
        {
            var league  = new League{ Name = "Serie A"};
            await context.Leagues.AddAsync(league);
            await context.SaveChangesAsync();

            await AddTeamsWithLeague(league);
            await context.SaveChangesAsync();
        }
        static async Task AddTeamsWithLeague(League league)
        {
            var teams = new List<Team>
            {
                new Team
                {
                    Name = "Juventus",
                    LeagueId = league.Id
                },
                new Team
                {
                    Name = "AC Milan",
                    LeagueId = league.Id
                },
                new Team
                {
                    Name = "AS Roma",
                    League = league
                }
            };

            await context.AddRangeAsync(teams);

        }
        static async Task AddTeamLeagueTogether()
        {
            var league = new League { Name = "Bundesliga"};
            var team = new Team{ Name= "Bayern Munich", League= league};
            await context.AddAsync(team);

            await context.SaveChangesAsync();

        }
    }
}
