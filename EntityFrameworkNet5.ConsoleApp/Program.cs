using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
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
            //await SimpleQueryJson();
            
            //NOTE: FILTERING 
            // await QueryFilters();
            // await QueryFiltersInput();
            // await QueryFiltersContains();

            //NOTE:
            await AdditionalExecutionMethods();

            Console.WriteLine("Press any key to end ...");
            Console.Read();
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

        private static async Task QueryFilters()
        {
            var leagues = await context.Leagues.Where(league => league.Name == "Serie A").ToListAsync();
            leagues.ForEach(league => Console.WriteLine($"{league.Id} - {league.Name}"));
        }
        private static async Task QueryFiltersInput()
        {
            Console.Write("Enter League Name (or part of): ");
            var leagueName = Console.ReadLine();
            var leagues = await context.Leagues.Where(league => league.Name.Equals(leagueName)).ToListAsync();
            leagues.ForEach(league => Console.WriteLine($"{league.Id} - {league.Name}"));
        }
        private static async Task QueryFiltersContains()
        {
            Console.Write("Enter League Name (or part of): ");
            var leagueName = Console.ReadLine();
            var exactMatches = await context.Leagues.Where(league => league.Name.Equals(leagueName)).ToListAsync();
            exactMatches.ForEach(league => Console.WriteLine($"{league.Id} - {league.Name}"));

            var partialMatches = await context.Leagues.Where(league => league.Name.Contains(leagueName)).ToListAsync();
            partialMatches.ForEach(league => Console.WriteLine($"{league.Id} - {league.Name}"));

            var partialMatchesEF = await context.Leagues.Where(league => EF.Functions.Like(league.Name, $"%{leagueName}%")).ToListAsync();
            partialMatchesEF.ForEach(league => Console.WriteLine($"{league.Id} - {league.Name}"));
        }

        private static async Task AdditionalExecutionMethods()
        {
            //var league = context.Leagues.Where(x => x.Name.Contains("a")).FirstOrDefault();
            //HACK: Executing method
            var league = await context.Leagues.FirstOrDefaultAsync(x => x.Name.Contains("a"));
            if (league is not null)
            {
                Console.WriteLine($"{league?.Id} - {league?.Name}");
            }

            var leagues = context.Leagues;
            var list = await leagues.ToListAsync();
            var first = await leagues.FirstAsync(); // it is expecting a list and will take the first
            var firstOrDefault = await leagues.FirstOrDefaultAsync();
                Console.WriteLine($"{firstOrDefault.Id} - {firstOrDefault.Name}");
            //NOTE: single expects only 1 record to be returned , if it sees more that one it will throw an exception
            var single = await leagues.SingleAsync(x => x.Name.Equals("Serie A")); 
            //FIXME: this SINGLE statement will throw an error as it would return more than 1 element
            // var singleOrDefault = await leagues.SingleOrDefaultAsync();
            var count = await leagues.CountAsync();
            var longCount = await leagues.LongCountAsync();
            var min = await leagues.MinAsync(x => x.Id);
            Console.WriteLine(new List<int>{1 , 2, 3}.Min()) ;

            //DbSet methods
            var leagueFind = await leagues.FindAsync(min); //pass the Primary Key !
            
        }
    }
}
