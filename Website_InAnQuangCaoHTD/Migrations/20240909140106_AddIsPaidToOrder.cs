using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPaidToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 10, 21, 1, 4, 520, DateTimeKind.Local).AddTicks(4316), new DateTime(2024, 9, 8, 21, 1, 4, 520, DateTimeKind.Local).AddTicks(4297) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 10, 21, 1, 4, 520, DateTimeKind.Local).AddTicks(4319), new DateTime(2024, 9, 8, 21, 1, 4, 520, DateTimeKind.Local).AddTicks(4318) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 10, 20, 28, 7, 857, DateTimeKind.Local).AddTicks(4406), new DateTime(2024, 9, 8, 20, 28, 7, 857, DateTimeKind.Local).AddTicks(4390) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 10, 20, 28, 7, 857, DateTimeKind.Local).AddTicks(4409), new DateTime(2024, 9, 8, 20, 28, 7, 857, DateTimeKind.Local).AddTicks(4408) });
        }
    }
}
