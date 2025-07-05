using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorites_farms_FarmId",
                table: "favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_ratings_farms_FarmId",
                table: "ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_images_farms_FarmId",
                table: "images");

            //migrationBuilder.DropIndex(
            //    name: "IX_images_FarmsId",
            //    table: "images");

            //migrationBuilder.DropColumn(
            //    name: "FarmsId",
            //    table: "images");

            migrationBuilder.AddForeignKey(
                name: "FK_favorites_farms_FarmId",
                table: "favorites",
                column: "FarmId",
                principalTable: "farms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_farms_FarmId",
                table: "ratings",
                column: "FarmId",
                principalTable: "farms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_images_farms_FarmId",
                table: "images",
                column: "FarmId",
                principalTable: "farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // ⛔ هون ألغيت الكاسكيد
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorites_farms_FarmId",
                table: "favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_ratings_farms_FarmId",
                table: "ratings");

            migrationBuilder.AddColumn<int>(
                name: "FarmsId",
                table: "images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_images_FarmsId",
                table: "images",
                column: "FarmsId");

            migrationBuilder.AddForeignKey(
                name: "FK_favorites_farms_FarmId",
                table: "favorites",
                column: "FarmId",
                principalTable: "farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_images_farms_FarmsId",
                table: "images",
                column: "FarmsId",
                principalTable: "farms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_farms_FarmId",
                table: "ratings",
                column: "FarmId",
                principalTable: "farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
