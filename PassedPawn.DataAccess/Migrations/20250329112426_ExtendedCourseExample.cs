using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedCourseExample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pgn",
                table: "CourseExamples",
                newName: "InitialFen");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CourseExamples",
                newName: "InitialDescription");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CourseExamples",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseExampleMove",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlgebraicNotation = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExampleId = table.Column<int>(type: "integer", nullable: false),
                    CourseExampleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseExampleMove", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseExampleMove_CourseExampleMove_ExampleId",
                        column: x => x.ExampleId,
                        principalTable: "CourseExampleMove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseExampleMove_CourseExamples_CourseExampleId",
                        column: x => x.CourseExampleId,
                        principalTable: "CourseExamples",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseExampleMoveArrow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    ExampleMoveId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseExampleMoveArrow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseExampleMoveArrow_CourseExampleMove_ExampleMoveId",
                        column: x => x.ExampleMoveId,
                        principalTable: "CourseExampleMove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseExampleMoveHighlight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    ExampleMoveId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseExampleMoveHighlight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseExampleMoveHighlight_CourseExampleMove_ExampleMoveId",
                        column: x => x.ExampleMoveId,
                        principalTable: "CourseExampleMove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseExamples_CourseId",
                table: "CourseExamples",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExampleMove_CourseExampleId",
                table: "CourseExampleMove",
                column: "CourseExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExampleMove_ExampleId",
                table: "CourseExampleMove",
                column: "ExampleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExampleMoveArrow_ExampleMoveId",
                table: "CourseExampleMoveArrow",
                column: "ExampleMoveId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseExampleMoveHighlight_ExampleMoveId",
                table: "CourseExampleMoveHighlight",
                column: "ExampleMoveId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseExamples_Courses_CourseId",
                table: "CourseExamples",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseExamples_Courses_CourseId",
                table: "CourseExamples");

            migrationBuilder.DropTable(
                name: "CourseExampleMoveArrow");

            migrationBuilder.DropTable(
                name: "CourseExampleMoveHighlight");

            migrationBuilder.DropTable(
                name: "CourseExampleMove");

            migrationBuilder.DropIndex(
                name: "IX_CourseExamples_CourseId",
                table: "CourseExamples");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseExamples");

            migrationBuilder.RenameColumn(
                name: "InitialFen",
                table: "CourseExamples",
                newName: "Pgn");

            migrationBuilder.RenameColumn(
                name: "InitialDescription",
                table: "CourseExamples",
                newName: "Description");
        }
    }
}
