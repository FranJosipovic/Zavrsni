using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zavrsni.TeamOps.Migrations
{
    /// <inheritdoc />
    public partial class eighthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProjectWikis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProjectWikis");
        }
    }
}
