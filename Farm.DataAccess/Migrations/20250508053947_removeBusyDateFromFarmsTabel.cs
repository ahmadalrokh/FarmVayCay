using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeBusyDateFromFarmsTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusyDate",
                table: "farms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BusyDate",
                table: "farms",
                type: "datetime2",
                nullable: true);
        }
    }
}
