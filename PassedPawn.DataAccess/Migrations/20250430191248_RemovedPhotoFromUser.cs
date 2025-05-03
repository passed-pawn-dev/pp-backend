using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPhotoFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Photos_PhotoId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PhotoId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Students",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PhotoId",
                table: "Students",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Photos_PhotoId",
                table: "Students",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }
    }
}
