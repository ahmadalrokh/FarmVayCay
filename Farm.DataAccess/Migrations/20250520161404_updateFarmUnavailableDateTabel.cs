using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateFarmUnavailableDateTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnavailableDate",
                table: "UnavailableDates",
                newName: "UnavailableDateTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "UnavailableDateFrom",
                table: "UnavailableDates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnavailableDateFrom",
                table: "UnavailableDates");

            migrationBuilder.RenameColumn(
                name: "UnavailableDateTo",
                table: "UnavailableDates",
                newName: "UnavailableDate");
        }
    }
}
