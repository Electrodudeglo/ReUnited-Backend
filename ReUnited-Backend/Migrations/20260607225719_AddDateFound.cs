using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReUnited_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDateFound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateFound",
                table: "FoundItems",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFound",
                table: "FoundItems");
        }
    }
}
