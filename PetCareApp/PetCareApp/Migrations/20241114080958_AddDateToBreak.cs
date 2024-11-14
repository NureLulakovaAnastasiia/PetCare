using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDateToBreak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Breaks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Breaks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Breaks");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Breaks");
        }
    }
}
