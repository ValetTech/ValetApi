using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class Removeresidfromtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Tables");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Tables",
                type: "int",
                nullable: true);

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
        }
    }
}
