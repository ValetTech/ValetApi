using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ValetAPI.Migrations
{
    public partial class Addreferencetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SittingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SittingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SittingTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 0, "Breakfast" },
                    { 1, "Lunch" },
                    { 2, "Dinner" },
                    { 3, "Special" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Source" },
                values: new object[,]
                {
                    { 0, "Website" },
                    { 1, "InPerson" },
                    { 2, "Email" },
                    { 3, "Phone" }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "State" },
                values: new object[,]
                {
                    { 0, "Pending" },
                    { 1, "Confirmed" },
                    { 2, "Cancelled" },
                    { 3, "Assigned" },
                    { 4, "Seated" },
                    { 5, "Completed" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SittingTypes");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
