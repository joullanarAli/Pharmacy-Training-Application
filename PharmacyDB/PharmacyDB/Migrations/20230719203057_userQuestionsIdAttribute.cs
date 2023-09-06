using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDB.Migrations
{
    public partial class userQuestionsIdAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserQuestionIsd",
                table: "Answer",
                newName: "UserQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_UserQuestionIsd",
                table: "Answer",
                newName: "IX_Answer_UserQuestionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserQuestionsId",
                table: "Answer",
                newName: "UserQuestionIsd");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_UserQuestionsId",
                table: "Answer",
                newName: "IX_Answer_UserQuestionIsd");
        }
    }
}
