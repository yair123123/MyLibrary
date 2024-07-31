using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdLibrary",
                table: "Shelf");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Shelf",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Shelf");

            migrationBuilder.AddColumn<int>(
                name: "IdLibrary",
                table: "Shelf",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
