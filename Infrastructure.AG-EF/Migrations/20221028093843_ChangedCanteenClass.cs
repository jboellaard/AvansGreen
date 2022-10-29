using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.AG_EF.Migrations
{
    public partial class ChangedCanteenClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Canteens");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Canteens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "Id", "City", "Discriminator", "HasWarmMeals", "Name" },
                values: new object[,]
                {
                    { 1, "Breda", "BredaCanteen", true, "LA5" },
                    { 2, "Breda", "BredaCanteen", true, "LD1" },
                    { 3, "Breda", "BredaCanteen", true, "HA1" },
                    { 6, "Den Bosch", "DenBoschCanteen", true, "DH1" },
                    { 7, "Den Bosch", "DenBoschCanteen", true, "DH5" },
                    { 4, "Tilburg", "TilburgCanteen", false, "TH1" },
                    { 5, "Tilburg", "TilburgCanteen", false, "TH5" }
                });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 27, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 27, 17, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 28, 17, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 28, 13, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 29, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 29, 17, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 28, 15, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 28, 11, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 28, 12, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 28, 8, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Packets",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PickUpTimeEnd", "PickUpTimeStart" },
                values: new object[] { new DateTime(2022, 10, 28, 14, 0, 0, 0, DateTimeKind.Local), new DateTime(2022, 10, 28, 10, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
