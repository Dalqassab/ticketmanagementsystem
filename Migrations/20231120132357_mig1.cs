using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_receiverID",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_senderId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "timestampsend",
                table: "Message",
                newName: "Timestampsend");

            migrationBuilder.RenameColumn(
                name: "senderId",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "receiverID",
                table: "Message",
                newName: "ReceiverID");

            migrationBuilder.RenameIndex(
                name: "IX_Message_senderId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_receiverID",
                table: "Message",
                newName: "IX_Message_ReceiverID");

            migrationBuilder.AddColumn<int>(
                name: "TheTicketTicketID",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_TheTicketTicketID",
                table: "Message",
                column: "TheTicketTicketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_ReceiverID",
                table: "Message",
                column: "ReceiverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Ticket_TheTicketTicketID",
                table: "Message",
                column: "TheTicketTicketID",
                principalTable: "Ticket",
                principalColumn: "TicketID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_ReceiverID",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_SenderId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Ticket_TheTicketTicketID",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_TheTicketTicketID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TheTicketTicketID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "Timestampsend",
                table: "Message",
                newName: "timestampsend");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "senderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "Message",
                newName: "receiverID");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                newName: "IX_Message_senderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ReceiverID",
                table: "Message",
                newName: "IX_Message_receiverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_receiverID",
                table: "Message",
                column: "receiverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_senderId",
                table: "Message",
                column: "senderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
