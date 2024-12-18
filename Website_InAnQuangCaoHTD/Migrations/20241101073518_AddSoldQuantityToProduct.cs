using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class AddSoldQuantityToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldQuantity",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 11, 2, 14, 35, 15, 407, DateTimeKind.Local).AddTicks(6328), new DateTime(2024, 10, 31, 14, 35, 15, 407, DateTimeKind.Local).AddTicks(6302) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 11, 2, 14, 35, 15, 407, DateTimeKind.Local).AddTicks(6336), new DateTime(2024, 10, 31, 14, 35, 15, 407, DateTimeKind.Local).AddTicks(6334) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 1,
                column: "SoldQuantity",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 2,
                column: "SoldQuantity",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 3,
                column: "SoldQuantity",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 4,
                column: "SoldQuantity",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 5,
                column: "SoldQuantity",
                value: null);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: 6,
                column: "SoldQuantity",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldQuantity",
                table: "Products");

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
    }
}
