using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_corp.Migrations
{
    /// <inheritdoc />
    public partial class changedsessionmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_AspNetUsers_UserID",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_UserID",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Session");

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "Session",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Session");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Session",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Session_UserID",
                table: "Session",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_AspNetUsers_UserID",
                table: "Session",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
