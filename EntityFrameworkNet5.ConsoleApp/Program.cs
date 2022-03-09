using System;
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
            //Console.WriteLine("Hello World!");
            context.Leagues.Add(new League {Name = "Red Stripe Premiere League"});
            await context.SaveChangesAsync();
 
        }
    }
}
