using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TechnicianId",
                table: "Ticket",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "areaofexpertise",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "yearsofexperience",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TechnicianId",
                table: "Ticket",
                column: "TechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AspNetUsers_TechnicianId",
                table: "Ticket",
                column: "TechnicianId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AspNetUsers_TechnicianId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TechnicianId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "areaofexpertise",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "yearsofexperience",
                table: "AspNetUsers");
        }
    }
}
