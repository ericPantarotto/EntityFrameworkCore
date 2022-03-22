using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkNet5.Data.Migrations
{
    public partial class AddDeleteCoachByIdSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE sp_DeleteCoachById
                                    @coachId int
                                    AS
                                    BEGIN
                                        DELETE FROM [dbo].[Coaches] WHERE Id = @coachId
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_DeleteCoachById");
        }
    }
}
