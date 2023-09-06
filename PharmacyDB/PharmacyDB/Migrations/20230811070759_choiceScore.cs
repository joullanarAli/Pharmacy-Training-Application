using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDB.Migrations
{
    public partial class choiceScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Choice");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerMark",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Score",
                table: "Choice",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswerMark",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Choice");

            migrationBuilder.AddColumn<float>(
                name: "Mark",
                table: "Choice",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
