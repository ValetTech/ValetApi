using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_Venues_VenueId",
                table: "Sittings");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Sittings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Name", "VenueId" },
                values: new object[] { "Gorgeous Main Dining Area", "Main Dining", 1 });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Name", "VenueId" },
                values: new object[] { "Outside with a view", "Outside", 1 });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Description", "Name", "VenueId" },
                values: new object[,]
                {
                    { 13, "Upstairs away from the noise", "Upstairs", 1 },
                    { 21, "Gorgeous Main Dining Area", "Main Dining", 2 },
                    { 22, "Outside with a view", "Outside", 2 },
                    { 23, "Upstairs away from the noise", "Upstairs", 2 },
                    { 31, "Gorgeous Main Dining Area", "Main Dining", 3 },
                    { 32, "Outside with a view", "Outside", 3 },
                    { 33, "Upstairs away from the noise", "Upstairs", 3 },
                    { 41, "Gorgeous Main Dining Area", "Main Dining", 4 },
                    { 42, "Outside with a view", "Outside", 4 },
                    { 43, "Upstairs away from the noise", "Upstairs", 4 }
                });

            migrationBuilder.InsertData(
                table: "Sittings",
                columns: new[] { "Id", "AreaId", "EndTime", "StartTime", "Type", "VenueId" },
                values: new object[,]
                {
                    { 111, 11, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 1 },
                    { 112, 11, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 1 },
                    { 113, 11, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 1 },
                    { 121, 12, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 1 },
                    { 122, 12, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 1 },
                    { 123, 12, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 1 }
                });

            migrationBuilder.InsertData(
                table: "Sittings",
                columns: new[] { "Id", "AreaId", "EndTime", "StartTime", "Type", "VenueId" },
                values: new object[,]
                {
                    { 131, 13, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 1 },
                    { 132, 13, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 1 },
                    { 133, 13, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 1 },
                    { 211, 21, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 2 },
                    { 212, 21, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 2 },
                    { 213, 21, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 2 },
                    { 221, 22, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 2 },
                    { 222, 22, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 2 },
                    { 223, 22, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 2 },
                    { 231, 23, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 2 },
                    { 232, 23, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 2 },
                    { 233, 23, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 2 },
                    { 311, 31, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 3 },
                    { 312, 31, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 3 },
                    { 313, 31, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 3 },
                    { 321, 32, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 3 },
                    { 322, 32, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 3 },
                    { 323, 32, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 3 },
                    { 331, 33, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 3 },
                    { 332, 33, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 3 },
                    { 333, 33, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 3 },
                    { 411, 41, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 4 },
                    { 412, 41, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 4 },
                    { 413, 41, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 4 },
                    { 421, 42, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 4 },
                    { 422, 42, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 4 },
                    { 423, 42, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 4 },
                    { 431, 43, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", 4 },
                    { 432, 43, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", 4 },
                    { 433, 43, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Sittings_Venues_VenueId",
                table: "Sittings",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_Venues_VenueId",
                table: "Sittings");

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 411);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 412);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 413);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 421);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 422);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 423);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 431);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 432);

            migrationBuilder.DeleteData(
                table: "Sittings",
                keyColumn: "Id",
                keyValue: 433);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Sittings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Name", "VenueId" },
                values: new object[] { "Outside with a view", "Outside", 4 });

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Name", "VenueId" },
                values: new object[] { "Upstairs away from the noise", "Upstairs", 4 });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Description", "Name", "VenueId" },
                values: new object[,]
                {
                    { 1, "Gorgeous Main Dining Area", "Main Dining", 1 },
                    { 2, "Outside with a view", "Outside", 1 },
                    { 3, "Upstairs away from the noise", "Upstairs", 1 },
                    { 4, "Gorgeous Main Dining Area", "Main Dining", 2 },
                    { 5, "Outside with a view", "Outside", 2 },
                    { 6, "Upstairs away from the noise", "Upstairs", 2 },
                    { 7, "Gorgeous Main Dining Area", "Main Dining", 3 },
                    { 8, "Outside with a view", "Outside", 3 },
                    { 9, "Upstairs away from the noise", "Upstairs", 3 },
                    { 10, "Gorgeous Main Dining Area", "Main Dining", 4 }
                });

            migrationBuilder.InsertData(
                table: "Sittings",
                columns: new[] { "Id", "AreaId", "EndTime", "StartTime", "Type", "VenueId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", null },
                    { 2, 1, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", null },
                    { 3, 1, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", null },
                    { 4, 2, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", null },
                    { 5, 2, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", null },
                    { 6, 2, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", null },
                    { 7, 3, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", null },
                    { 8, 3, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", null },
                    { 9, 3, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", null },
                    { 10, 4, new DateTime(2022, 12, 25, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 10, 30, 0, 0, DateTimeKind.Unspecified), "Breakfast", null },
                    { 11, 4, new DateTime(2022, 12, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 13, 0, 0, 0, DateTimeKind.Unspecified), "Lunch", null },
                    { 12, 4, new DateTime(2022, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 12, 25, 17, 30, 0, 0, DateTimeKind.Unspecified), "Dinner", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Sittings_Venues_VenueId",
                table: "Sittings",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }
    }
}
