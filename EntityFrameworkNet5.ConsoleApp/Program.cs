using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Newtonsoft.Json;

namespace EntityFrameworkNet5.ConsoleApp
{
    public class Program
    {
        private static readonly FootBallLeagueDbContext context = new();
        static async Task Main(string[] args)
        {
            //NOTE:  INSERTS
            // await InitialInsert();
            //await InsertWithTeam();
            //await AddTeamLeagueTogether();

            //NOTE: SELECTING
            //await SimpleQuery();
            await SimpleQueryJson();
            
            

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

        static async Task SimpleQuery()
        {
            var leagues = context.Leagues.Distinct().ToList();
            leagues.ForEach(league => Console.WriteLine($"{league.Id} - {league.Name}"));
        }
        static async Task SimpleQueryJson()
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Error = (sender, args) => { args.ErrorContext.Handled = true;}
            };
            
            using var context =  new FootBallLeagueDbContext();
            var myEntity = context.Leagues.Select(l => new { Id = l.Id, Name = l.Name, TestResultOnName = !string.IsNullOrWhiteSpace(l.Name) } );
            var leagueJson=  await Task.Run(() => JsonConvert.SerializeObject(myEntity, settings)) ;
            Console.WriteLine(leagueJson);
        }
    }
}
