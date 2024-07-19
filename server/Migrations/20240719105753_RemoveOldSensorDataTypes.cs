using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldSensorDataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgGas",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "AvgLux",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "AvgUvs",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "MaxGas",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "MaxLux",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "MaxUvs",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "MinGas",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "MinLux",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "MinUvs",
                table: "SensorReadingsHourly");

            migrationBuilder.DropColumn(
                name: "AvgGas",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "AvgLux",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "AvgUvs",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "MaxGas",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "MaxLux",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "MaxUvs",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "MinGas",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "MinLux",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "MinUvs",
                table: "SensorReadingsDaily");

            migrationBuilder.DropColumn(
                name: "Gas",
                table: "SensorReadings");

            migrationBuilder.DropColumn(
                name: "Lux",
                table: "SensorReadings");

            migrationBuilder.DropColumn(
                name: "Uvs",
                table: "SensorReadings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AvgGas",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgLux",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgUvs",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxGas",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxLux",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxUvs",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinGas",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinLux",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinUvs",
                table: "SensorReadingsHourly",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgGas",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgLux",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgUvs",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxGas",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxLux",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxUvs",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinGas",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinLux",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinUvs",
                table: "SensorReadingsDaily",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "Gas",
                table: "SensorReadings",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Lux",
                table: "SensorReadings",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Uvs",
                table: "SensorReadings",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
