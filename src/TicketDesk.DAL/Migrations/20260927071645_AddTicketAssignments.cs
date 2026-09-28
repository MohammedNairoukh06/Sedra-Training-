using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketDesk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketAgents",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAgents", x => new { x.TicketId, x.AgentId });
                    table.ForeignKey(
                        name: "FK_TicketAgents_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAgents_Users_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketManagers",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketManagers", x => new { x.TicketId, x.ManagerId });
                    table.ForeignKey(
                        name: "FK_TicketManagers_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketManagers_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketAgents_AgentId",
                table: "TicketAgents",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketManagers_ManagerId",
                table: "TicketManagers",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketAgents");

            migrationBuilder.DropTable(
                name: "TicketManagers");
        }
    }
}
