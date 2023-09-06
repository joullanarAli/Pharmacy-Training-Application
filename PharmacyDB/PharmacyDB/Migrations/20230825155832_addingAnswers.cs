using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDB.Migrations
{
    public partial class addingAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.RenameColumn(
                name: "UserQuestionsId",
                table: "Answer",
                newName: "UserQuestionId");*/

            migrationBuilder.CreateTable(
                name: "UserExam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExam_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserExam_Exam_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamQuestionId = table.Column<int>(type: "int", nullable: false),
                    UserExamId = table.Column<int>(type: "int", nullable: false),
                    Mark = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestion_ExamQuestion_ExamQuestionId",
                        column: x => x.ExamQuestionId,
                        principalTable: "ExamQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestion_UserExam_UserExamId",
                        column: x => x.UserExamId,
                        principalTable: "UserExam",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserQuestionId",
                table: "Answer",
                column: "UserQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_ExamId",
                table: "UserExam",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExam_UserId",
                table: "UserExam",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestion_ExamQuestionId",
                table: "UserQuestion",
                column: "ExamQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestion_UserExamId",
                table: "UserQuestion",
                column: "UserExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_UserQuestion_UserQuestionId",
                table: "Answer",
                column: "UserQuestionId",
                principalTable: "UserQuestion",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_UserQuestion_UserQuestionId",
                table: "Answer");

            migrationBuilder.DropTable(
                name: "UserQuestion");

            migrationBuilder.DropTable(
                name: "UserExam");

            migrationBuilder.DropIndex(
                name: "IX_Answer_UserQuestionId",
                table: "Answer");

            migrationBuilder.RenameColumn(
                name: "UserQuestionId",
                table: "Answer",
                newName: "UserQuestionsId");
        }
    }
}
