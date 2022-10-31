using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.AG_EF.Migrations
{
    public partial class ChangedPathImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packets_CanteenEmployees_CanteenEmployeeId",
                table: "Packets");

            migrationBuilder.DropIndex(
                name: "IX_Packets_CanteenEmployeeId",
                table: "Packets");

            migrationBuilder.DropColumn(
                name: "CanteenEmployeeId",
                table: "Packets");

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 30, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 30, 17, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 31, 17, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 31, 13, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 11, 1, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 11, 1, 17, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 31, 15, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 31, 11, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 31, 12, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 31, 8, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 31, 14, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 31, 10, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CanteenEmployeeId",
                table: "Packets",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 28, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 28, 17, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 29, 17, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 29, 13, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 30, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 30, 17, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 29, 15, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 29, 11, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 29, 12, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 29, 8, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 29, 14, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 29, 10, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_Packets_CanteenEmployeeId",
                table: "Packets",
                column: "CanteenEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packets_CanteenEmployees_CanteenEmployeeId",
                table: "Packets",
                column: "CanteenEmployeeId",
                principalTable: "CanteenEmployees",
                principalColumn: "Id");
        }
    }
}
