using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addIsDeletedToBoking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "bokings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "bokings");
        }
    }
}
