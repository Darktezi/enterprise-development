using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Airline.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassportNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: false),
                    FlightRange = table.Column<double>(type: "float", nullable: false),
                    PassengerCapacity = table.Column<int>(type: "int", nullable: false),
                    CargoCapacity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureAirport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalAirport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TravelTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    AircraftModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Models_AircraftModelId",
                        column: x => x.AircraftModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasHandLuggage = table.Column<bool>(type: "bit", nullable: false),
                    BaggageWeight = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Families",
                columns: new[] { "Id", "Manufacturer", "Name" },
                values: new object[,]
                {
                    { 1, "Boeing", "Boeing 737" },
                    { 2, "Airbus", "Airbus A320" },
                    { 3, "Boeing", "Boeing 777" },
                    { 4, "Airbus", "Airbus A330" },
                    { 5, "Embraer", "Embraer E190" },
                    { 6, "Bombardier", "Bombardier CS300" },
                    { 7, "Boeing", "Boeing 787" },
                    { 8, "Airbus", "Airbus A350" },
                    { 9, "Sukhoi", "Sukhoi Superjet 100" },
                    { 10, "Comac", "Comac C919" }
                });

            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "Id", "BirthDate", "FirstName", "LastName", "MiddleName", "PassportNumber" },
                values: new object[,]
                {
                    { 1, new DateOnly(1985, 5, 12), "Иван", "Иванов", "Иванович", "P000001" },
                    { 2, new DateOnly(1990, 7, 22), "Анна", "Петрова", "Сергеевна", "P000002" },
                    { 3, new DateOnly(1982, 11, 2), "Павел", "Сидоров", null, "P000003" },
                    { 4, new DateOnly(1995, 3, 15), "Мария", "Кузнецова", null, "P000004" },
                    { 5, new DateOnly(1988, 12, 5), "Алексей", "Смирнов", "Игоревич", "P000005" },
                    { 6, new DateOnly(1992, 1, 20), "Елена", "Васильева", null, "P000006" },
                    { 7, new DateOnly(1983, 6, 30), "Дмитрий", "Морозов", "Александрович", "P000007" },
                    { 8, new DateOnly(1991, 9, 9), "Ольга", "Федорова", null, "P000008" },
                    { 9, new DateOnly(1987, 4, 18), "Никита", "Попов", "Сергеевич", "P000009" },
                    { 10, new DateOnly(1994, 8, 25), "Анна", "Михайлова", null, "P000010" }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "Id", "CargoCapacity", "FamilyId", "FlightRange", "ModelName", "PassengerCapacity" },
                values: new object[,]
                {
                    { 1, 8000.0, 1, 5000.0, "737-800", 160 },
                    { 2, 7500.0, 2, 6100.0, "A320-200", 150 },
                    { 3, 18000.0, 3, 13600.0, "777-300ER", 300 },
                    { 4, 15000.0, 4, 11700.0, "A330-300", 280 },
                    { 5, 4000.0, 5, 4000.0, "E190", 100 },
                    { 6, 5000.0, 6, 3600.0, "CS300", 130 },
                    { 7, 16000.0, 7, 14100.0, "787-9", 280 },
                    { 8, 17000.0, 8, 15000.0, "A350-900", 300 },
                    { 9, 3500.0, 9, 3000.0, "SSJ100", 98 },
                    { 10, 7000.0, 10, 4000.0, "C919", 156 }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "AircraftModelId", "ArrivalAirport", "ArrivalDate", "Code", "DepartureAirport", "DepartureDate", "TravelTime" },
                values: new object[,]
                {
                    { 1, 1, "JFK", new DateTime(2025, 9, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), "SU1001", "SVO", new DateTime(2025, 9, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 2, 2, "LHR", new DateTime(2025, 9, 2, 16, 0, 0, 0, DateTimeKind.Unspecified), "SU1002", "LED", new DateTime(2025, 9, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 3, 3, "CDG", new DateTime(2025, 9, 3, 13, 0, 0, 0, DateTimeKind.Unspecified), "SU1003", "SVO", new DateTime(2025, 9, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 4, 4, "FRA", new DateTime(2025, 9, 4, 19, 0, 0, 0, DateTimeKind.Unspecified), "SU1004", "LED", new DateTime(2025, 9, 4, 15, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 5, 5, "AMS", new DateTime(2025, 9, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), "SU1005", "SVO", new DateTime(2025, 9, 5, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 6, 6, "JFK", new DateTime(2025, 9, 6, 15, 0, 0, 0, DateTimeKind.Unspecified), "SU1006", "LED", new DateTime(2025, 9, 6, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 7, 7, "LHR", new DateTime(2025, 9, 7, 17, 0, 0, 0, DateTimeKind.Unspecified), "SU1007", "SVO", new DateTime(2025, 9, 7, 13, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 8, 8, "CDG", new DateTime(2025, 9, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), "SU1008", "LED", new DateTime(2025, 9, 8, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 9, 9, "FRA", new DateTime(2025, 9, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), "SU1009", "SVO", new DateTime(2025, 9, 9, 14, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) },
                    { 10, 10, "AMS", new DateTime(2025, 9, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), "SU1010", "LED", new DateTime(2025, 9, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "BaggageWeight", "FlightId", "HasHandLuggage", "PassengerId", "SeatNumber" },
                values: new object[,]
                {
                    { 1, 0.0, 1, true, 1, "12A" },
                    { 2, 1.0, 2, false, 2, "13A" },
                    { 3, 0.0, 3, true, 3, "14A" },
                    { 4, 1.0, 4, false, 4, "15A" },
                    { 5, 0.0, 5, true, 5, "16A" },
                    { 6, 1.0, 6, false, 6, "17A" },
                    { 7, 0.0, 7, true, 7, "18A" },
                    { 8, 1.0, 8, false, 8, "19A" },
                    { 9, 0.0, 9, true, 9, "20A" },
                    { 10, 1.0, 10, false, 10, "21A" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftModelId",
                table: "Flights",
                column: "AircraftModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_FamilyId",
                table: "Models",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FlightId",
                table: "Tickets",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets",
                column: "PassengerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
