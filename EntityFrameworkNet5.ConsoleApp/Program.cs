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
            //SimpleQuery();
            //await SimpleQueryJson();
            
            //NOTE: FILTERING 
            // await QueryFilters();
            // await QueryFiltersInput();
            // await QueryFiltersContains();

            //await AdditionalExecutionMethods();

            //NOTE: LINQ
            //await AlternativeLinqSyntax();
            
            //NOTE: UPDATE
            // await SimpleUpdateLeagueRecord();
            // await SimpleUpdateTeamRecord();

            //NOTE: Delete
            // await SimpleDelete();

            //NOTE: Tracking
            // await TrackingVsNoTracking();

            //NOTE: One to many
            // await GetTeamsFromLeague();

            //NOTE: Inserting related Data
            // await AddNewTeamWithLeague();
            // await AddNewTeamWithLeagueId();
            // await AddNewLeagueWithTeams();
            // await AddNewMaches();
            // await AddNewCoach();

            //NOTE: including related data
            // await QueryRelatedRecords();

            //NOTE: Projections
            // await SelectOneProperty();
            // await AnonymousProjection();
            // await StronglyTypeProjection();

            //NOTE: Filtering with related data
            // await FilteringWithRelatedData();

            //NOTE:: Calling a View
            // await QueryView();

            //NOTE: SQL Raw
            // await RawSQLQuery();

            //NOTE: Sp
            // await ExecStoredProcedure();

            //NOTE: Non Query Raw
            // await DeleteUsingEF();
            // await ExecuteNonQueryCommand();

            //NOTE: Manipulating Entries
            // await SimpleUpdateTeamRecordTestUpdateDate();

            //NOTE: Extending DbContext 
            // await SimpleUpdateTeamRecordWithAuditContext();

            //NOTE: Audit Table
            await TestingAuditTableOnCoach();

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

        private static async Task GetTeamsFromLeague()
        {
            
            
                (await context.Leagues
                    .Include(league => league.Teams)
                    .FirstAsync(l => l.Name == "Serie A"))
                    .Teams
                    .ForEach(t => Console.WriteLine($"Team {t.Id}: {t.Name}"));

            
        }
        static void  SimpleQuery()
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
            var exactMatches = await context.Leagues.
            Where(league => league.Name.Equals(leagueName)).ToListAsync();
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

        private static async Task AlternativeLinqSyntax()
        {
            //NOTE: this would be working too in a non async context
            // var teams = from team in context.Teams select team;

            Console.Write("Enter League Name (or part of): ");
            var leagueName = Console.ReadLine();
            
            var teams = await (from team in context.Teams 
                where EF.Functions.Like(team.Name, $"%{leagueName}%")
                select team).ToListAsync();
            
            foreach (var team in teams)
            {
                Console.WriteLine($"{team.Id} - {team.Name}");
            }
        }
        private static async Task SimpleUpdateLeagueRecord()
        {
            //Retreive Record
            League league  = await GetLeagueRecord(1010);
            // Make record change
            league.Name = "Scottish Premiership";
            await context.SaveChangesAsync();
            
            league= await GetLeagueRecord(1010);
            Console.WriteLine($"{league.Id} - {league.Name}");
        }
        private static async Task<League> GetLeagueRecord(int id)
        {
            return await context.Leagues.FindAsync(id);
        }
        private static async Task<Team> GetTeamRecord(int id)
        {
            return await context.Teams.FindAsync(id);
        }

        private static async Task SimpleUpdateTeamRecord()
        {
            var team = new Team{ Id= 6, Name="Tivoli Gardens", LeagueId=1010 };
            context.Teams.Update(team);
            
            await context.SaveChangesAsync();
            Team teamRecord = await GetTeamRecord(6);
            Console.WriteLine($"{teamRecord.Id} - {teamRecord.Name} - {teamRecord.LeagueId}");

            var teamNoPK = new Team{ Name="Seba United FC", LeagueId=1010};
            context.Teams.Update(teamNoPK);
            await context.SaveChangesAsync();
        }
        private static async Task SimpleUpdateTeamRecordTestUpdateDate()
        {
            var team = new Team{ Id= 20, Name="Date = Eric Carlier - Sample Team ", LeagueId=1010 };
            context.Teams.Update(team);
            await context.SaveChangesAsync();
        }
        private static async Task SimpleUpdateTeamRecordWithAuditContext()
        {
            var team = new Team{ Id= 20, Name="AuditContext = Eric Carlier - Sample Team ", LeagueId=1010 };
            context.Teams.Update(team);
            await context.SaveChangesAsync("Test Team Management user");
        }
        private static async Task SimpleDelete()
        {
            League league  = await context.Leagues.FindAsync(1012);
            context.Leagues.Remove(league);
            await context.SaveChangesAsync();
        }
        private static async Task TrackingVsNoTracking()
        {
            var withTracking = await context.Teams.FirstOrDefaultAsync(team => team.Id == 10);
            var withNoTracking = await context.Teams.AsNoTracking().FirstOrDefaultAsync(team => team.Id == 11);

            withTracking.Name = "TrackingNEW";
            withNoTracking.Name = "NoTrackingNEW";

            var entries = context.ChangeTracker.Entries();
            await context.SaveChangesAsync();
            var entriesAfterSave = context.ChangeTracker.Entries();
        }
        
        private static async Task AddNewTeamWithLeague()
        {
            // var league = new League { Name = "Ligue 1"};
            // var team = new Team { Name = "PSG", League = league};
            // await context.AddAsync(team);
            // await context.SaveChangesAsync();

            var league = new League { Name = "Ligue Check Date"};
            var team = new Team { Name = "PSG Insert Date", League = league};
            await context.AddAsync(team);
            await context.SaveChangesAsync();
        }
        private static async Task AddNewTeamWithLeagueId()
        {
            var team = new Team { Name = "Fiorentina", LeagueId = 1007};
            await context.AddAsync(team);
            await context.SaveChangesAsync();
        }
        private static async Task AddNewLeagueWithTeams()
        {
            var teams = new List<Team>
            {
                new Team { Name = "Rivoli United"},
                new Team { Name = "Waterhouse FC"}
            };

            var league = new League { Name = "CIFA", Teams = teams};
            
            await context.AddAsync(league);
            await context.SaveChangesAsync();
        }
        private static async Task AddNewMaches()
        {
            var matches = new List<Match>
            {
                new Match {HomeTeamId = 1, AwayTeamId = 2, Date = new DateTime(2022,01,01) },
                new Match {HomeTeamId = 2, AwayTeamId = 3, Date = DateTime.Now },
                new Match {HomeTeamId = 1, AwayTeamId = 3, Date = DateTime.Now }
            };
            await context.AddRangeAsync(matches);
            await context.SaveChangesAsync();
        }
        private static async Task AddNewCoach()
        {
            var coach1 = new Coach { Name = "Jose Mourinho", TeamId = 1};
            await context.AddAsync(coach1);
            var coach2 = new Coach { Name = "Antonio Conte"};
            await context.AddAsync(coach2);
            await context.SaveChangesAsync();
        }

        private static async Task QueryRelatedRecords()
        {
            //get many related records: Leagues => Teams
            var leagues = await context.Leagues.Include(t => t.Teams).ToListAsync();

            //get one relating record = Team = > Coach , using the coach navigation property
            var team = await context.Teams
                .Include(t => t.Coach)
                .FirstOrDefaultAsync(t => t.Id ==1);
            
            //Get grand children related record: Team => Matches => Home/Away Team
            var teamsWithMatchesAndOpponents = await context.Teams
                .Include(t => t.AwayMatches).ThenInclude(m => m.HomeTeam).ThenInclude(t => t.Coach)
                .Include(t => t.HomeMatches).ThenInclude(m => m.AwayTeam)
                .FirstOrDefaultAsync(t => t.Id ==2);
            
            //Get Includes with filters
            var teams = await context.Teams
                .Where(t => t.HomeMatches.Count > 0)
                .Include(t => t.Coach)
                .ToListAsync();
        }


        private static async Task SelectOneProperty()
        {
            //Restrict to only 1  property
            List<string> teams = await context.Teams
                .Select(q => q.Name)
                .ToListAsync();   
        }
        private static async Task AnonymousProjection()
        {
            //multiple prop from multiple tables: list of team names and coach names
            //used for on the fly scenario
            var teams = await context.Teams
                .Include(q => q.Coach)
                .Select(t => 
                    new 
                    { 
                        TeamName = t.Name, 
                        CoachName = t.Coach.Name 
                    })
                .ToListAsync();
            teams.ForEach(item => Console.WriteLine($"Team: {item.TeamName} | Coach: {item.CoachName}"));
        }

        public record TeamDetail(
           string TeamName,
           string CoachName,
           string LeagueName 
        );
        private static async Task StronglyTypeProjection()
        {
            var teams = await context.Teams
                .Include(q => q.Coach)
                .Include(q => q.League)
                .Select(t => 
                    new TeamDetail
                    (
                        t.Name, 
                        t.Coach.Name,
                        t.League.Name 
                    )
                ).ToListAsync();
            teams.ForEach(item => Console.WriteLine($"Team: {item.TeamName} | League: {item.LeagueName} | Coach: {item.CoachName}"));
        }

        private static async Task FilteringWithRelatedData()
        {
            var leagues = await context.Leagues.Where(l => l.Teams.Any(x => x.Name.Contains("Bay"))).ToListAsync();
        }

        private static async Task QueryView()
        {
            var details = await context.TeamCoachesLeagues.ToListAsync();
            details.ForEach(item => Console.WriteLine($"Team: {item.Name} | League: {item.LeagueName} | Coach: {item.CoachName}"));
        }

        private static async Task RawSQLQuery()
        {
            var name= "AS Roma";
            var team1 = await context.Teams.FromSqlRaw($"select Id, LeagueId, Name from teams where name = '{name}'")
                .Include(q => q.Coach)
                .ToListAsync();

            var team2 = await context.Teams.FromSqlInterpolated($"select * from teams where name = {name}").ToListAsync();  
        }
        private static async Task ExecStoredProcedure()
        {
            var teamId = 1;
            var result = await context.Coaches.FromSqlRaw("EXEC [dbo].[sp_GetTeamCoach] {0}", teamId).ToListAsync();
        }

        private static async Task DeleteUsingEF()
        {
            using var context =  new FootBallLeagueDbContext();
            var coach  = await context.Coaches.Where(c => c.Name.Contains("zzz")).FirstOrDefaultAsync();
            
            context.Entry(coach).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
        private static async Task ExecuteNonQueryCommand()
        {
            var coachId  = 6;
            // int affectedRows = await  context.Database.ExecuteSqlRawAsync("exec sp_DeleteCoachById {0}", coachId);
            int affectedRows = await  context.Database.ExecuteSqlInterpolatedAsync($"exec sp_DeleteCoachById {coachId}");
        }
        private static async Task TestingAuditTableOnCoach()
        {
            var coach = new Coach{ Name="Francoise Carlier", TeamId=22 };
            context.Coaches.Update(coach);
            await context.SaveChangesAsync("Audit Coach tb");
        }
    }
}
