using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketDesk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResolvedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedAt",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedByUserId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ClosedByUserId",
                table: "Tickets",
                column: "ClosedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ResolvedByUserId",
                table: "Tickets",
                column: "ResolvedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReturnedByUserId",
                table: "Tickets",
                column: "ReturnedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ClosedByUserId",
                table: "Tickets",
                column: "ClosedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ResolvedByUserId",
                table: "Tickets",
                column: "ResolvedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_ReturnedByUserId",
                table: "Tickets",
                column: "ReturnedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_ClosedByUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_ResolvedByUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_ReturnedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ClosedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ResolvedByUserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReturnedByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ClosedByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolvedByUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReturnedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReturnedByUserId",
                table: "Tickets");
        }
    }
}
