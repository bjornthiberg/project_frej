using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pressure = table.Column<float>(type: "REAL", nullable: false),
                    Temperature = table.Column<float>(type: "REAL", nullable: false),
                    Humidity = table.Column<float>(type: "REAL", nullable: false),
                    Lux = table.Column<float>(type: "REAL", nullable: false),
                    Uvs = table.Column<float>(type: "REAL", nullable: false),
                    Gas = table.Column<float>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorReadingsDaily",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AvgPressure = table.Column<double>(type: "REAL", nullable: false),
                    MinPressure = table.Column<double>(type: "REAL", nullable: false),
                    MaxPressure = table.Column<double>(type: "REAL", nullable: false),
                    AvgTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MinTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MaxTemperature = table.Column<double>(type: "REAL", nullable: false),
                    AvgHumidity = table.Column<double>(type: "REAL", nullable: false),
                    MinHumidity = table.Column<double>(type: "REAL", nullable: false),
                    MaxHumidity = table.Column<double>(type: "REAL", nullable: false),
                    AvgLux = table.Column<double>(type: "REAL", nullable: false),
                    MinLux = table.Column<double>(type: "REAL", nullable: false),
                    MaxLux = table.Column<double>(type: "REAL", nullable: false),
                    AvgUvs = table.Column<double>(type: "REAL", nullable: false),
                    MinUvs = table.Column<double>(type: "REAL", nullable: false),
                    MaxUvs = table.Column<double>(type: "REAL", nullable: false),
                    AvgGas = table.Column<double>(type: "REAL", nullable: false),
                    MinGas = table.Column<double>(type: "REAL", nullable: false),
                    MaxGas = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadingsDaily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorReadingsHourly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AvgPressure = table.Column<double>(type: "REAL", nullable: false),
                    MinPressure = table.Column<double>(type: "REAL", nullable: false),
                    MaxPressure = table.Column<double>(type: "REAL", nullable: false),
                    AvgTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MinTemperature = table.Column<double>(type: "REAL", nullable: false),
                    MaxTemperature = table.Column<double>(type: "REAL", nullable: false),
                    AvgHumidity = table.Column<double>(type: "REAL", nullable: false),
                    MinHumidity = table.Column<double>(type: "REAL", nullable: false),
                    MaxHumidity = table.Column<double>(type: "REAL", nullable: false),
                    AvgLux = table.Column<double>(type: "REAL", nullable: false),
                    MinLux = table.Column<double>(type: "REAL", nullable: false),
                    MaxLux = table.Column<double>(type: "REAL", nullable: false),
                    AvgUvs = table.Column<double>(type: "REAL", nullable: false),
                    MinUvs = table.Column<double>(type: "REAL", nullable: false),
                    MaxUvs = table.Column<double>(type: "REAL", nullable: false),
                    AvgGas = table.Column<double>(type: "REAL", nullable: false),
                    MinGas = table.Column<double>(type: "REAL", nullable: false),
                    MaxGas = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadingsHourly", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "SensorReadingsDaily");

            migrationBuilder.DropTable(
                name: "SensorReadingsHourly");
        }
    }
}
