using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingIdTOFarmsTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "farms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_farms_RatingId",
                table: "farms",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_farms_ratings_RatingId",
                table: "farms",
                column: "RatingId",
                principalTable: "ratings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_farms_ratings_RatingId",
                table: "farms");

            migrationBuilder.DropIndex(
                name: "IX_farms_RatingId",
                table: "farms");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "farms");
        }
    }
}
