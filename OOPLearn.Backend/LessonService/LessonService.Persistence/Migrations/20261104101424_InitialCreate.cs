using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LessonService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    EstimatedMinutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lessons_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lesson_sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContentMarkdown = table.Column<string>(type: "text", nullable: false),
                    CodeExample = table.Column<string>(type: "text", nullable: true),
                    CodeLanguage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lesson_sections_lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lesson_media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonSectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    S3Key = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MediaType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lesson_media_lesson_sections_LessonSectionId",
                        column: x => x.LessonSectionId,
                        principalTable: "lesson_sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_OrderIndex",
                table: "categories",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_media_LessonSectionId",
                table: "lesson_media",
                column: "LessonSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_media_S3Key",
                table: "lesson_media",
                column: "S3Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_sections_LessonId",
                table: "lesson_sections",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_sections_LessonId_OrderIndex",
                table: "lesson_sections",
                columns: new[] { "LessonId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_lessons_CategoryId",
                table: "lessons",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_lessons_CategoryId_OrderIndex",
                table: "lessons",
                columns: new[] { "CategoryId", "OrderIndex" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lesson_media");

            migrationBuilder.DropTable(
                name: "lesson_sections");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
