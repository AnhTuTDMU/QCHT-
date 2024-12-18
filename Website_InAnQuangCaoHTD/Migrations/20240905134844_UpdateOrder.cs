using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 6, 20, 48, 43, 628, DateTimeKind.Local).AddTicks(2229), new DateTime(2024, 9, 4, 20, 48, 43, 628, DateTimeKind.Local).AddTicks(2215) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 6, 20, 48, 43, 628, DateTimeKind.Local).AddTicks(2232), new DateTime(2024, 9, 4, 20, 48, 43, 628, DateTimeKind.Local).AddTicks(2232) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 6, 19, 7, 17, 601, DateTimeKind.Local).AddTicks(1385), new DateTime(2024, 9, 4, 19, 7, 17, 601, DateTimeKind.Local).AddTicks(1365) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 6, 19, 7, 17, 601, DateTimeKind.Local).AddTicks(1388), new DateTime(2024, 9, 4, 19, 7, 17, 601, DateTimeKind.Local).AddTicks(1387) });
        }
    }
}
