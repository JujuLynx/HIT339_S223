using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_corp.Migrations
{
    /// <inheritdoc />
    public partial class updatedbookingsessionname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Booking",
                newName: "SessionName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionName",
                table: "Booking",
                newName: "Name");
        }
    }
}
