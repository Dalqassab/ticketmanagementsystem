using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Technician_TechnicianId",
                table: "Ticket");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Technician_TechnicianId",
                table: "Ticket",
                column: "TechnicianId",
                principalTable: "Technician",
                principalColumn: "TechnicianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Technician_TechnicianId",
                table: "Ticket");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Technician_TechnicianId",
                table: "Ticket",
                column: "TechnicianId",
                principalTable: "Technician",
                principalColumn: "TechnicianId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
