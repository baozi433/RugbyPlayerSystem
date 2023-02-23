using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RugbyPlayerSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoundedYear = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Unions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unions", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "BirthDate", "Height", "Name", "PlaceOfBirth", "TeamName", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 190.0, "Player 1", "Auckland", "Team 1", 90.0 },
                    { 2, new DateTime(1994, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 180.0, "Player 2", "Christchurch", "Team 2", 100.0 },
                    { 3, new DateTime(1998, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 185.0, "Player 3", "Otago", "Team 3", 79.0 }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Name", "Coach", "FoundedYear", "Ground", "Region", "UnionName" },
                values: new object[,]
                {
                    { "Team 1", "Default", 1988, "Default", "Default", "Nationl man's Union" },
                    { "Team 2", "Default", 1975, "Default", "Default", "Nationl man's Union" },
                    { "Team 3", "Default", 1963, "Default", "Default", "Nationl man's Union" }
                });

            migrationBuilder.InsertData(
                table: "Unions",
                column: "Name",
                value: "Nationl man's Union");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Unions");
        }
    }
}
