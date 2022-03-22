using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkNet5.Data.Migrations
{
    public partial class AddingTeamDetailsViewAndEarlyMatchFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[GetEarliestMatch] (@teamId int)
                                    RETURNS datetime
                                    BEGIN
                                        DECLARE @result datetime
                                        SELECT TOP 1 @result = [Date]
                                        FROM [dbo].[Matches]
                                        Order By Date
                                        return @result
                                    END");
            
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[TeamsCoachesLeagues] AS
                                    SELECT t.Name, c.Name AS CoachName, l.Name as LeagueName
                                    FROM [dbo].[Teams] AS t 
                                    LEFT OUTER JOIN [dbo].[Coaches] AS c ON c.TeamId = t.Id
                                    INNER JOIN [dbo].[Leagues] AS l ON l.Id = t.LeagueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCION [dbo].[GetEarliestMatch]");
            migrationBuilder.Sql("DROP VIEW [dbo].[TeamsCoachesLeagues]");
        }
    }
}
