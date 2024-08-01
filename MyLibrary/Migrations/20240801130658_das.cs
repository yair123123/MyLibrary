using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLibrary.Migrations
{
    /// <inheritdoc />
    public partial class das : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_SetBook_SetBookId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "SetBook");

            migrationBuilder.DropIndex(
                name: "IX_Book_SetBookId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "SetBookId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "setId",
                table: "Book");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SetBookId",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "setId",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SetBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetBook", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_SetBookId",
                table: "Book",
                column: "SetBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_SetBook_SetBookId",
                table: "Book",
                column: "SetBookId",
                principalTable: "SetBook",
                principalColumn: "Id");
        }
    }
}
