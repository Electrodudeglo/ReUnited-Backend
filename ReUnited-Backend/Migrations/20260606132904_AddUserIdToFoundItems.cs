using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReUnited_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToFoundItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FoundItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FoundItems");
        }
    }
}
