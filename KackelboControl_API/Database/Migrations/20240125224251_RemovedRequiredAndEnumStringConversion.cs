using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KackelboControl_API.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequiredAndEnumStringConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Relay",
                table: "RelayChangeLog",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Relay",
                table: "RelayChangeLog",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
