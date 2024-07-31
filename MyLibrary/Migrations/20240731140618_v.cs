using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Migrations
{
    /// <inheritdoc />
    public partial class v : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Shelf_shelfId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "IdShelf",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "shelfId",
                table: "Book",
                newName: "ShelfId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_shelfId",
                table: "Book",
                newName: "IX_Book_ShelfId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Shelf_ShelfId",
                table: "Book",
                column: "ShelfId",
                principalTable: "Shelf",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Shelf_ShelfId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "ShelfId",
                table: "Book",
                newName: "shelfId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ShelfId",
                table: "Book",
                newName: "IX_Book_shelfId");

            migrationBuilder.AddColumn<int>(
                name: "IdShelf",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Shelf_shelfId",
                table: "Book",
                column: "shelfId",
                principalTable: "Shelf",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
