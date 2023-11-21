using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentMessageId",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ParentMessageId",
                table: "Message",
                column: "ParentMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_ParentMessageId",
                table: "Message",
                column: "ParentMessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // Optional: Set cascade behavior


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_ParentMessageId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ParentMessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ParentMessageId",
                table: "Message");
        }
    }
}
