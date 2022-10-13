using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Reservations_ReservationEntityId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_ReservationEntityId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "ReservationEntityId",
                table: "Tables");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_ReservationId",
                table: "Tables",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Reservations_ReservationId",
                table: "Tables",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Reservations_ReservationId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_ReservationId",
                table: "Tables");

            migrationBuilder.AddColumn<int>(
                name: "ReservationEntityId",
                table: "Tables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tables_ReservationEntityId",
                table: "Tables",
                column: "ReservationEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Reservations_ReservationEntityId",
                table: "Tables",
                column: "ReservationEntityId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
