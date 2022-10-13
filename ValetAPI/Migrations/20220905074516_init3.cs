using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Venues_VenueEntityId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_Venues_VenueEntityId",
                table: "Sittings");

            migrationBuilder.DropIndex(
                name: "IX_Sittings_VenueEntityId",
                table: "Sittings");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VenueEntityId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "VenueEntityId",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "VenueEntityId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Sittings_VenueId",
                table: "Sittings",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VenueId",
                table: "Reservations",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Venues_VenueId",
                table: "Reservations",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Reservations_Venues_VenueId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Sittings_Venues_VenueId",
                table: "Sittings");

            migrationBuilder.DropIndex(
                name: "IX_Sittings_VenueId",
                table: "Sittings");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_VenueId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "VenueEntityId",
                table: "Sittings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VenueEntityId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sittings_VenueEntityId",
                table: "Sittings",
                column: "VenueEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VenueEntityId",
                table: "Reservations",
                column: "VenueEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Venues_VenueEntityId",
                table: "Reservations",
                column: "VenueEntityId",
                principalTable: "Venues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sittings_Venues_VenueEntityId",
                table: "Sittings",
                column: "VenueEntityId",
                principalTable: "Venues",
                principalColumn: "Id");
        }
    }
}
