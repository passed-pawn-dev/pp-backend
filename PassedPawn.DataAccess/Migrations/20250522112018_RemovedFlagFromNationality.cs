using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFlagFromNationality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nationalities_Photos_FlagId",
                table: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Nationalities_FlagId",
                table: "Nationalities");

            migrationBuilder.DropColumn(
                name: "FlagId",
                table: "Nationalities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlagId",
                table: "Nationalities",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 1,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 2,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 3,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 4,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 5,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 6,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 7,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 8,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 9,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 10,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 11,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 12,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 13,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 14,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 15,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 16,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 17,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 18,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 19,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 20,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 21,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 22,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 23,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 24,
                column: "FlagId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 25,
                column: "FlagId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_FlagId",
                table: "Nationalities",
                column: "FlagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nationalities_Photos_FlagId",
                table: "Nationalities",
                column: "FlagId",
                principalTable: "Photos",
                principalColumn: "Id");
        }
    }
}
