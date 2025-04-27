using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenamedPhotoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Photos",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "PhotoPublicUrl",
                table: "Photos",
                newName: "PublicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Photos",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "Photos",
                newName: "PhotoPublicUrl");
        }
    }
}
