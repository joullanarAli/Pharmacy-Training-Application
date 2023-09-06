using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDB.Migrations
{
    public partial class deleteAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestion_UserExam_UserExamId",
                table: "UserQuestion");

            migrationBuilder.DropTable(
                name: "Answer");

            /*migrationBuilder.DropColumn(
                name: "ExamDate",
                table: "Exam");*/

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestion_UserExam_UserExamId",
                table: "UserQuestion",
                column: "UserExamId",
                principalTable: "UserExam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestion_UserExam_UserExamId",
                table: "UserQuestion");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExamDate",
                table: "Exam",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChoiceId = table.Column<int>(type: "int", nullable: true),
                    UserQuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_UserQuestion_UserQuestionId",
                        column: x => x.UserQuestionId,
                        principalTable: "UserQuestion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Choices",
                        column: x => x.ChoiceId,
                        principalTable: "Choice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ChoiceId",
                table: "Answer",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserQuestionId",
                table: "Answer",
                column: "UserQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestion_UserExam_UserExamId",
                table: "UserQuestion",
                column: "UserExamId",
                principalTable: "UserExam",
                principalColumn: "Id");
        }
    }
}
