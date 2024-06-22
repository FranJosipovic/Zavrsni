using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zavrsni.TeamOps.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGithubUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubUser",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GitHubUser",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
