using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOrderRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 10, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8750), new DateTime(2024, 9, 8, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8732) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 9, 10, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8752), new DateTime(2024, 9, 8, 18, 58, 58, 604, DateTimeKind.Local).AddTicks(8751) });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

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
    }
}
