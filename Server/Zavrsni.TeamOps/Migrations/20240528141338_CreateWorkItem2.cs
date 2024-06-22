using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zavrsni.TeamOps.Migrations
{
    /// <inheritdoc />
    public partial class CreateWorkItem2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iterrations_Projects_ProjectId",
                table: "Iterrations");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItem_Iterrations_IterationId",
                table: "WorkItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItem_Users_AssignedToId",
                table: "WorkItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItem_Users_CreatedById",
                table: "WorkItem");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItem_WorkItem_ParentId",
                table: "WorkItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkItem",
                table: "WorkItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Iterrations",
                table: "Iterrations");

            migrationBuilder.RenameTable(
                name: "WorkItem",
                newName: "WorkItems");

            migrationBuilder.RenameTable(
                name: "Iterrations",
                newName: "Iterations");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItem_ParentId",
                table: "WorkItems",
                newName: "IX_WorkItems_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItem_IterationId",
                table: "WorkItems",
                newName: "IX_WorkItems_IterationId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItem_CreatedById",
                table: "WorkItems",
                newName: "IX_WorkItems_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItem_AssignedToId",
                table: "WorkItems",
                newName: "IX_WorkItems_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Iterrations_ProjectId",
                table: "Iterations",
                newName: "IX_Iterations_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkItems",
                table: "WorkItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Iterations",
                table: "Iterations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Iterations_Projects_ProjectId",
                table: "Iterations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Iterations_IterationId",
                table: "WorkItems",
                column: "IterationId",
                principalTable: "Iterations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_AssignedToId",
                table: "WorkItems",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_CreatedById",
                table: "WorkItems",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItems_ParentId",
                table: "WorkItems",
                column: "ParentId",
                principalTable: "WorkItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iterations_Projects_ProjectId",
                table: "Iterations");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Iterations_IterationId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_AssignedToId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_CreatedById",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItems_ParentId",
                table: "WorkItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkItems",
                table: "WorkItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Iterations",
                table: "Iterations");

            migrationBuilder.RenameTable(
                name: "WorkItems",
                newName: "WorkItem");

            migrationBuilder.RenameTable(
                name: "Iterations",
                newName: "Iterrations");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ParentId",
                table: "WorkItem",
                newName: "IX_WorkItem_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_IterationId",
                table: "WorkItem",
                newName: "IX_WorkItem_IterationId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_CreatedById",
                table: "WorkItem",
                newName: "IX_WorkItem_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_AssignedToId",
                table: "WorkItem",
                newName: "IX_WorkItem_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Iterations_ProjectId",
                table: "Iterrations",
                newName: "IX_Iterrations_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkItem",
                table: "WorkItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Iterrations",
                table: "Iterrations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Iterrations_Projects_ProjectId",
                table: "Iterrations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItem_Iterrations_IterationId",
                table: "WorkItem",
                column: "IterationId",
                principalTable: "Iterrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItem_Users_AssignedToId",
                table: "WorkItem",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItem_Users_CreatedById",
                table: "WorkItem",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItem_WorkItem_ParentId",
                table: "WorkItem",
                column: "ParentId",
                principalTable: "WorkItem",
                principalColumn: "Id");
        }
    }
}
