using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_corp.Migrations
{
    /// <inheritdoc />
    public partial class coachimagefunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CoachProfile",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CoachProfile");
        }
    }
}
