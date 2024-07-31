using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updateLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelf_Library_libraryId",
                table: "Shelf");

            migrationBuilder.RenameColumn(
                name: "libraryId",
                table: "Shelf",
                newName: "LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Shelf_libraryId",
                table: "Shelf",
                newName: "IX_Shelf_LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shelf_Library_LibraryId",
                table: "Shelf",
                column: "LibraryId",
                principalTable: "Library",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelf_Library_LibraryId",
                table: "Shelf");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Shelf",
                newName: "libraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Shelf_LibraryId",
                table: "Shelf",
                newName: "IX_Shelf_libraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shelf_Library_libraryId",
                table: "Shelf",
                column: "libraryId",
                principalTable: "Library",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
