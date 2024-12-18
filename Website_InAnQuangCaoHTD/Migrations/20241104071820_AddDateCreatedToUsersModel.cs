using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class AddDateCreatedToUsersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 11, 5, 14, 18, 17, 426, DateTimeKind.Local).AddTicks(5328), new DateTime(2024, 11, 3, 14, 18, 17, 426, DateTimeKind.Local).AddTicks(5318) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 11, 5, 14, 18, 17, 426, DateTimeKind.Local).AddTicks(5335), new DateTime(2024, 11, 3, 14, 18, 17, 426, DateTimeKind.Local).AddTicks(5333) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 11, 4, 14, 18, 17, 426, DateTimeKind.Local).AddTicks(3112));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Users");

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
        }
    }
}
