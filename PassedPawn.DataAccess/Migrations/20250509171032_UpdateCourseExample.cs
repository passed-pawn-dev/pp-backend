using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseExample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialFen",
                table: "CourseExamples");

            migrationBuilder.RenameColumn(
                name: "AlgebraicNotation",
                table: "CourseExampleMove",
                newName: "Fen");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CourseExampleMove",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "CourseExampleMove");

            migrationBuilder.RenameColumn(
                name: "Fen",
                table: "CourseExampleMove",
                newName: "AlgebraicNotation");

            migrationBuilder.AddColumn<string>(
                name: "InitialFen",
                table: "CourseExamples",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
