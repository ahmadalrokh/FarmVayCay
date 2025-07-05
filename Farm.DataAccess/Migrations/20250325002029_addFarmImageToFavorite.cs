using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addFarmImageToFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavoriteId",
                table: "images",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "farms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_images_FavoriteId",
                table: "images",
                column: "FavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_images_favorites_FavoriteId",
                table: "images",
                column: "FavoriteId",
                principalTable: "favorites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_images_favorites_FavoriteId",
                table: "images");

            migrationBuilder.DropIndex(
                name: "IX_images_FavoriteId",
                table: "images");

            migrationBuilder.DropColumn(
                name: "FavoriteId",
                table: "images");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "farms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
