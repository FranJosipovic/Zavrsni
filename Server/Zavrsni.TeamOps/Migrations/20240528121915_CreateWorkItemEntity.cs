using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zavrsni.TeamOps.Migrations
{
    /// <inheritdoc />
    public partial class CreateWorkItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IterationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkItem_Iterrations_IterationId",
                        column: x => x.IterationId,
                        principalTable: "Iterrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkItem_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkItem_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkItem_WorkItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "WorkItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_AssignedToId",
                table: "WorkItem",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_CreatedById",
                table: "WorkItem",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_IterationId",
                table: "WorkItem",
                column: "IterationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_ParentId",
                table: "WorkItem",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItem");
        }
    }
}
