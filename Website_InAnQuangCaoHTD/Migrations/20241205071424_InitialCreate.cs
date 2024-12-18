using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_InAnQuangCaoHTD.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ConstructionBookings",
                columns: table => new
                {
                    ConstructionBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsConfirmed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionBookings", x => x.ConstructionBookingId);
                    table.ForeignKey(
                        name: "FK_ConstructionBookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 1,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 12, 6, 14, 14, 23, 375, DateTimeKind.Local).AddTicks(5322), new DateTime(2024, 12, 4, 14, 14, 23, 375, DateTimeKind.Local).AddTicks(5316) });

            migrationBuilder.UpdateData(
                table: "FlashSales",
                keyColumn: "FlashSaleId",
                keyValue: 2,
                columns: new[] { "FlashSaleEndTime", "FlashSaleStartTime" },
                values: new object[] { new DateTime(2024, 12, 6, 14, 14, 23, 375, DateTimeKind.Local).AddTicks(5325), new DateTime(2024, 12, 4, 14, 14, 23, 375, DateTimeKind.Local).AddTicks(5324) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 12, 5, 14, 14, 23, 375, DateTimeKind.Local).AddTicks(4086));

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionBookings_UserId",
                table: "ConstructionBookings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConstructionBookings");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

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
    }
}
