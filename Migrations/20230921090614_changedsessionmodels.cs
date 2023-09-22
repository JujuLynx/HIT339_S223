using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_corp.Migrations
{
    /// <inheritdoc />
    public partial class changedsessionmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Session_CoachId",
                table: "Session",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_AspNetUsers_CoachId",
                table: "Session",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_AspNetUsers_CoachId",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_CoachId",
                table: "Session");
        }
    }
}
