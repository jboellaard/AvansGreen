using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.AG_EF.Migrations
{
    public partial class AddedCanteens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "Id", "City", "HasWarmMeals", "Name" },
                values: new object[,]
                {
                    { 1, "Breda", true, "LA5" },
                    { 2, "Breda", true, "LD1" },
                    { 3, "Breda", true, "HA1" },
                    { 4, "Tilburg", false, "TH1" },
                    { 5, "Tilburg", false, "TH5" },
                    { 6, "Den Bosch", true, "DH1" },
                    { 7, "Den Bosch", true, "DH5" }
                });
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
        }
    }
}
