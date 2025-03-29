using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseExampleMove_CourseExampleMove_ExampleId",
                table: "CourseExampleMove");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseExampleMove_CourseExamples_CourseExampleId",
                table: "CourseExampleMove");

            migrationBuilder.DropIndex(
                name: "IX_CourseExampleMove_CourseExampleId",
                table: "CourseExampleMove");

            migrationBuilder.DropColumn(
                name: "CourseExampleId",
                table: "CourseExampleMove");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseExampleMove_CourseExamples_ExampleId",
                table: "CourseExampleMove",
                column: "ExampleId",
                principalTable: "CourseExamples",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseExampleMove_CourseExamples_ExampleId",
                table: "CourseExampleMove");

            migrationBuilder.AddColumn<int>(
                name: "CourseExampleId",
                table: "CourseExampleMove",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseExampleMove_CourseExampleId",
                table: "CourseExampleMove",
                column: "CourseExampleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseExampleMove_CourseExampleMove_ExampleId",
                table: "CourseExampleMove",
                column: "ExampleId",
                principalTable: "CourseExampleMove",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseExampleMove_CourseExamples_CourseExampleId",
                table: "CourseExampleMove",
                column: "CourseExampleId",
                principalTable: "CourseExamples",
                principalColumn: "Id");
        }
    }
}
