using System;
using EntityFrameworkCore.Data;
using EntityFrameworkCore.Domain;

namespace EntityFrameworkNet5.ConsoleApp
{
    public class Program
    {
        private static readonly FootBallLeagueDbContext context = new();
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            context.Leagues.Add(new League {Name = "Red Stripe Premiere League"});
            context.SaveChangesAsync();
 
        }
    }
}
