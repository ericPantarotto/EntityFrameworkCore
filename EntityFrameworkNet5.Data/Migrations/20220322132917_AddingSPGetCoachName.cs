using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkNet5.Data.Migrations
{
    public partial class AddingSPGetCoachName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"CREATE PROCEDURE sp_GetTeamCoach
                                    @teamId int
                                    AS
                                    BEGIN
                                        SELECT * FROM [dbo].[Coaches] WHERE TeamId = @teamId
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_GetTeamCoach");
        }
    }
}
