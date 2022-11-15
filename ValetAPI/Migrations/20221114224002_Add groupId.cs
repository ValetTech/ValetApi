using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class AddgroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Sittings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Sittings",
                columns: new[] { "Id", "Capacity", "EndTime", "GroupId", "StartTime", "Type", "VenueId" },
                values: new object[,]
                {
                    { 11, 50, new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc), "Breakfast", 1 },
                    { 12, 50, new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc), "Lunch", 1 },
                    { 13, 50, new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc), "Dinner", 1 },
                    { 21, 50, new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc), "Breakfast", 1 },
                    { 22, 50, new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc), "Lunch", 1 },
                    { 23, 50, new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc), "Dinner", 1 },
                    { 31, 50, new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc), "Breakfast", 1 },
                    { 32, 50, new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc), "Lunch", 1 },
                    { 33, 50, new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc), "Dinner", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Sittings");

            migrationBuilder.InsertData(
                table: "Sittings",
                columns: new[] { "Id", "Capacity", "EndTime", "StartTime", "Type", "VenueId" },
                values: new object[,]
                {
                    { 11, 50, new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc), "Breakfast", 1 },
                    { 12, 50, new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc), "Lunch", 1 },
                    { 13, 50, new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc), "Dinner", 1 },
                    { 21, 50, new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc), "Breakfast", 1 },
                    { 22, 50, new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc), "Lunch", 1 },
                    { 23, 50, new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc), "Dinner", 1 },
                    { 31, 50, new DateTime(2022, 12, 25, 1, 30, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 24, 23, 30, 0, 0, DateTimeKind.Utc), "Breakfast", 1 },
                    { 32, 50, new DateTime(2022, 12, 25, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 2, 0, 0, 0, DateTimeKind.Utc), "Lunch", 1 },
                    { 33, 50, new DateTime(2022, 12, 25, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 12, 25, 6, 30, 0, 0, DateTimeKind.Utc), "Dinner", 1 }
                });
        }
    }
}
