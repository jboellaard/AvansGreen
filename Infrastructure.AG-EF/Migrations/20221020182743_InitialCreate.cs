using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.AG_EF.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canteens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canteens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAlcoholic = table.Column<bool>(type: "bit", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityOfSchool = table.Column<int>(type: "int", nullable: false),
                    PhoneNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CanteenEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanteenId = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteenEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanteenEmployees_Canteens_CanteenId",
                        column: x => x.CanteenId,
                        principalTable: "Canteens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanteenId = table.Column<int>(type: "int", nullable: false),
                    PickUpTimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickUpTimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOfPickUpByStudent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAlcoholic = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TypeOfMeal = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packets_Canteens_CanteenId",
                        column: x => x.CanteenId,
                        principalTable: "Canteens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Packets_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductsInPacket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PacketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsInPacket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsInPacket_Packets_PacketId",
                        column: x => x.PacketId,
                        principalTable: "Packets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsInPacket_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "Id", "City", "Location" },
                values: new object[,]
                {
                    { 1, 0, "LA5" },
                    { 2, 0, "H1" },
                    { 3, 0, "LD1" },
                    { 4, 1, "TH1" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ImageData", "IsAlcoholic", "Name" },
                values: new object[,]
                {
                    { 1, null, true, "Bottle of vodka" },
                    { 2, null, false, "Panini" },
                    { 3, null, false, "Sandwich" },
                    { 4, null, false, "Apple" },
                    { 5, null, false, "Soup" },
                    { 6, null, false, "Baguette" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "CityOfSchool", "DateOfBirth", "EmailAddress", "Name", "PhoneNr", "StudentNr" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(1998, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "je.boellaard@student.avans.nl", "Joy Boellaard", "0612345678", "2182556" },
                    { 2, 0, new DateTime(2000, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "em.degroot@student.avans.nl", "Emma de Groot", "0623456789", "2192233" },
                    { 3, 1, new DateTime(2001, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "b.dejong@student.avans.nl", "Ben de Jong", "0634567890", "2192344" },
                    { 4, 0, new DateTime(1999, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "d.li@student.avans.nl", "Diana Li", "0645678901", "2184399" }
                });

            migrationBuilder.InsertData(
                table: "CanteenEmployees",
                columns: new[] { "Id", "CanteenId", "EmailAddress", "EmployeeNr", "Name" },
                values: new object[,]
                {
                    { 1, 1, "n.devries@avans.nl", "1234567", "n.devries" },
                    { 2, 2, "p.smit@avans.nl", "1111111", "p.smit" },
                    { 3, 4, "l.degroot@avans.nl", "2222222", "l.degroot" }
                });

            migrationBuilder.InsertData(
                table: "Packets",
                columns: new[] { "Id", "CanteenId", "IsAlcoholic", "Name", "PickUpTimeEnd", "PickUpTimeStart", "Price", "StudentId", "TimeOfPickUpByStudent", "TypeOfMeal" },
                values: new object[,]
                {
                    { 1, 1, false, "Alcoholic beverage and snack", new DateTime(2022, 10, 20, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 20, 17, 0, 0, 0, DateTimeKind.Unspecified), 5.0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, 2, false, "Lunch with two sandwiches", new DateTime(2022, 10, 21, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 21, 13, 0, 0, 0, DateTimeKind.Unspecified), 5.5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 }
                });

            migrationBuilder.InsertData(
                table: "ProductsInPacket",
                columns: new[] { "Id", "PacketId", "ProductId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "ProductsInPacket",
                columns: new[] { "Id", "PacketId", "ProductId" },
                values: new object[] { 2, 1, 4 });

            migrationBuilder.InsertData(
                table: "ProductsInPacket",
                columns: new[] { "Id", "PacketId", "ProductId" },
                values: new object[] { 3, 2, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_CanteenEmployees_CanteenId",
                table: "CanteenEmployees",
                column: "CanteenId");

            migrationBuilder.CreateIndex(
                name: "IX_Packets_CanteenId",
                table: "Packets",
                column: "CanteenId");

            migrationBuilder.CreateIndex(
                name: "IX_Packets_StudentId",
                table: "Packets",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInPacket_PacketId",
                table: "ProductsInPacket",
                column: "PacketId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsInPacket_ProductId",
                table: "ProductsInPacket",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanteenEmployees");

            migrationBuilder.DropTable(
                name: "ProductsInPacket");

            migrationBuilder.DropTable(
                name: "Packets");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Canteens");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
