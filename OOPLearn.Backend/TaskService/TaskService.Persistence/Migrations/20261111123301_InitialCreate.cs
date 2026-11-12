using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CodeLanguage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StarterCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "task_options",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_options_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: true),
                    SubmittedCode = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_submissions_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_testcases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Input = table.Column<string>(type: "text", nullable: false),
                    ExpectedOutput = table.Column<string>(type: "text", nullable: false),
                    IsHidden = table.Column<bool>(type: "boolean", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_testcases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_testcases_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_submission_options",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskOptionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_submission_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_submission_options_task_options_TaskOptionId",
                        column: x => x.TaskOptionId,
                        principalTable: "task_options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_submission_options_task_submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "task_submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_options_TaskId_OrderIndex",
                table: "task_options",
                columns: new[] { "TaskId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_task_submission_options_SubmissionId_TaskOptionId",
                table: "task_submission_options",
                columns: new[] { "SubmissionId", "TaskOptionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_submission_options_TaskOptionId",
                table: "task_submission_options",
                column: "TaskOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_task_submissions_TaskId",
                table: "task_submissions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_submissions_UserId_SubmittedAt",
                table: "task_submissions",
                columns: new[] { "UserId", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_task_testcases_TaskId_OrderIndex",
                table: "task_testcases",
                columns: new[] { "TaskId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_LessonId_OrderIndex",
                table: "tasks",
                columns: new[] { "LessonId", "OrderIndex" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_submission_options");

            migrationBuilder.DropTable(
                name: "task_testcases");

            migrationBuilder.DropTable(
                name: "task_options");

            migrationBuilder.DropTable(
                name: "task_submissions");

            migrationBuilder.DropTable(
                name: "tasks");
        }
    }
}
