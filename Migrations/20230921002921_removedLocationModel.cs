using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_corp.Migrations
{
    /// <inheritdoc />
    public partial class removedLocationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_Location_LocationID",
                table: "Session");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Session_LocationID",
                table: "Session");

            migrationBuilder.RenameColumn(
                name: "LocationID",
                table: "Session",
                newName: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Session",
                newName: "LocationID");

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Session_LocationID",
                table: "Session",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Location_LocationID",
                table: "Session",
                column: "LocationID",
                principalTable: "Location",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
