using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassedPawn.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MadePositionNumericalInArrowsAndHighlights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""CourseExampleMoveHighlight""
                ALTER COLUMN ""Position"" TYPE integer USING ""Position""::integer;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""CourseExampleMoveArrow""
                ALTER COLUMN ""Source"" TYPE integer USING ""Source""::integer;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""CourseExampleMoveArrow""
                ALTER COLUMN ""Destination"" TYPE integer USING ""Destination""::integer;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""CourseExampleMoveHighlight""
                ALTER COLUMN ""Position"" TYPE text USING ""Position""::text;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""CourseExampleMoveArrow""
                ALTER COLUMN ""Source"" TYPE text USING ""Source""::text;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""CourseExampleMoveArrow""
                ALTER COLUMN ""Destination"" TYPE text USING ""Destination""::text;
            ");
        }
    }
}