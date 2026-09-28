using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketDesk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentManagerLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AgentId",
                table: "Users",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_AgentId",
                table: "Users",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_AgentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AgentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Users");
        }
    }
}
