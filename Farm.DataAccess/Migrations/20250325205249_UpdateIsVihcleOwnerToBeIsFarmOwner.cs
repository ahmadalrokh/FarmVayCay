using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIsVihcleOwnerToBeIsFarmOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsVehicleOwner",
                table: "AspNetUsers",
                newName: "IsFarmOwner");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFarmOwner",
                table: "AspNetUsers",
                newName: "IsVehicleOwner");
        }
    }
}
