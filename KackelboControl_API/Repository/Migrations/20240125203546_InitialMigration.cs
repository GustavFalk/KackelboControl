using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KackelboControl_API.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelayChangeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Relay = table.Column<int>(type: "INTEGER", nullable: false),
                    ArduinoTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    RelayOn = table.Column<bool>(type: "INTEGER", nullable: false),
                    TempIndoors = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelayChangeLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorTriggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LightOnTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    LightOffTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    MinTemp = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxTemp = table.Column<decimal>(type: "TEXT", nullable: false),
                    UseSunLight = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSynced = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorTriggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorValueLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArduinoTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TempIndoors = table.Column<decimal>(type: "TEXT", nullable: false),
                    TempOutdoor = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorValueLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SunRiseSunSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Sunrise = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Sunset = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SunRiseSunSet", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelayChangeLog");

            migrationBuilder.DropTable(
                name: "SensorTriggers");

            migrationBuilder.DropTable(
                name: "SensorValueLog");

            migrationBuilder.DropTable(
                name: "SunRiseSunSet");
        }
    }
}
