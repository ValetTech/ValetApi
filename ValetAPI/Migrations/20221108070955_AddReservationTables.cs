using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class AddReservationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Reservations_ReservationId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_ReservationId",
                table: "Tables");

            migrationBuilder.AddColumn<bool>(
                name: "IsVip",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReservationEntityTableEntity",
                columns: table => new
                {
                    ReservationsId = table.Column<int>(type: "int", nullable: false),
                    TablesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationEntityTableEntity", x => new { x.ReservationsId, x.TablesId });
                    table.ForeignKey(
                        name: "FK_ReservationEntityTableEntity_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationEntityTableEntity_Tables_TablesId",
                        column: x => x.TablesId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationTable",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTable", x => new { x.ReservationId, x.TableId });
                    table.ForeignKey(
                        name: "FK_ReservationTable_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationTable_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ReservationEntityTableEntity_TablesId",
                table: "ReservationEntityTableEntity",
                column: "TablesId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTable_TableId",
                table: "ReservationTable",
                column: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationEntityTableEntity");

            migrationBuilder.DropTable(
                name: "ReservationTable");

            migrationBuilder.DropColumn(
                name: "IsVip",
                table: "Customers");

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

            migrationBuilder.CreateIndex(
                name: "IX_Tables_ReservationId",
                table: "Tables",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Reservations_ReservationId",
                table: "Tables",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
