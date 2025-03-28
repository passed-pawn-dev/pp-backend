using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NonRequiredMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Nationalities_NationalityId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Nationalities_NationalityId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "Students",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "Coaches",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Nationalities_NationalityId",
                table: "Coaches",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Nationalities_NationalityId",
                table: "Students",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Nationalities_NationalityId",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Nationalities_NationalityId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "Coaches",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Nationalities_NationalityId",
                table: "Coaches",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Nationalities_NationalityId",
                table: "Students",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
