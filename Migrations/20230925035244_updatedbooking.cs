using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_corp.Migrations
{
    /// <inheritdoc />
    public partial class updatedbooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_UserID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Session_SessionID",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_SessionID",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UserID",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Booking",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "CoachName",
                table: "Booking",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Booking",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Booking",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemberID",
                table: "Booking",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoachName",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "MemberID",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Booking",
                newName: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_SessionID",
                table: "Booking",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserID",
                table: "Booking",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_UserID",
                table: "Booking",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Session_SessionID",
                table: "Booking",
                column: "SessionID",
                principalTable: "Session",
                principalColumn: "SessionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
