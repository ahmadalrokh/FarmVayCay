using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Farm.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Seed10Banks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "vayCayBanks",
                columns: new[] { "Id", "Balance", "CardName", "CardNumber", "ExpDate" },
                values: new object[,]
                {
                    { 1, 10500.75, "Ali Hasan", "4111 2222 3333 4444", "12/27" },
                    { 2, 8200.0, "Lana Hani", "5500 1111 2222 3333", "06/26" },
                    { 3, 15750.6, "Omar Yasin", "3400 9999 8888 777", "11/28" },
                    { 4, 9100.1000000000004, "Rana Yousef", "6011 0009 9011 2233", "09/29" },
                    { 5, 14200.0, "Mohammad Zaid", "4539 5589 1234 4321", "01/30" },
                    { 6, 13200.9, "Nour Ahmad", "3782 8224 6310 005", "07/27" },
                    { 7, 8800.3999999999996, "Khaled Sami", "2223 4567 7890 1234", "05/26" },
                    { 8, 9700.0, "Mona Saeed", "6011 3456 7890 1111", "10/28" },
                    { 9, 12300.0, "Tariq Fares", "4111 6543 2109 8888", "03/27" },
                    { 10, 10000.0, "Dana Ameen", "5105 1051 0510 5100", "08/29" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "vayCayBanks",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
