using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 0, 28, 559, DateTimeKind.Local).AddTicks(9410), new DateTime(2024, 9, 18, 14, 0, 28, 559, DateTimeKind.Local).AddTicks(9392) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 20, 14, 0, 28, 559, DateTimeKind.Local).AddTicks(9414), new DateTime(2024, 9, 18, 14, 0, 28, 559, DateTimeKind.Local).AddTicks(9412) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 11, 18, 50, 41, 227, DateTimeKind.Local).AddTicks(473), new DateTime(2024, 9, 9, 18, 50, 41, 227, DateTimeKind.Local).AddTicks(456) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 11, 18, 50, 41, 227, DateTimeKind.Local).AddTicks(476), new DateTime(2024, 9, 9, 18, 50, 41, 227, DateTimeKind.Local).AddTicks(475) });
        }
    }
}
