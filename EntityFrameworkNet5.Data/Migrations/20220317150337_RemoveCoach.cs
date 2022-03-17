using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkNet5.Data.Migrations
{
    public partial class RemoveCoach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Teams_TeamId",
                table: "Coaches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coaches",
                table: "Coaches");

            migrationBuilder.RenameTable(
                name: "Coaches",
                newName: "Coach");

            migrationBuilder.RenameIndex(
                name: "IX_Coaches_TeamId",
                table: "Coach",
                newName: "IX_Coach_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coach",
                table: "Coach",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coach_Teams_TeamId",
                table: "Coach",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coach_Teams_TeamId",
                table: "Coach");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coach",
                table: "Coach");

            migrationBuilder.RenameTable(
                name: "Coach",
                newName: "Coaches");

            migrationBuilder.RenameIndex(
                name: "IX_Coach_TeamId",
                table: "Coaches",
                newName: "IX_Coaches_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coaches",
                table: "Coaches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Teams_TeamId",
                table: "Coaches",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
