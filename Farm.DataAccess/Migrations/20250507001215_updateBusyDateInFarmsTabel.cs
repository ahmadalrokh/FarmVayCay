﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateBusyDateInFarmsTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BusyDate",
                table: "farms",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "BusyDate",
                table: "farms",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
