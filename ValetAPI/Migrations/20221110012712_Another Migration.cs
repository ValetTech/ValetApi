using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class AnotherMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Tables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 11,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 12,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 13,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 14,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 15,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 16,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 17,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 18,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 19,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 20,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 21,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 22,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 23,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 24,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 25,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 26,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 27,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 28,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 29,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 30,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 31,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 32,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 33,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 34,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 35,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 36,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 37,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 38,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 39,
                column: "VenueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 40,
                column: "VenueId",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Tables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 11,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 12,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 13,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 14,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 15,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 16,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 17,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 18,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 19,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 20,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 21,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 22,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 23,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 24,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 25,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 26,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 27,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 28,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 29,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 30,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 31,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 32,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 33,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 34,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 35,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 36,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 37,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 38,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 39,
                column: "VenueId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 40,
                column: "VenueId",
                value: 0);
        }
    }
}
