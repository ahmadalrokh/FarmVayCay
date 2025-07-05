using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seed2newData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "vayCayBanks",
                columns: new[] { "Id", "Balance", "CardName", "CardNumber", "ExpDate" },
                values: new object[,]
                {
                    { 11, 1E+17, "Mohanad Salameh", "2000 7070 1000 1000", "03/29" },
                    { 12, 9E+17, "Qusai Salameh", "2000 7070 1000 1000", "09/29" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
