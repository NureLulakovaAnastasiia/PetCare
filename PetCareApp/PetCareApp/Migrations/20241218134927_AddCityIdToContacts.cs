using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCityIdToContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 703448);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CityId",
                table: "Contacts",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Cities_CityId",
                table: "Contacts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Cities_CityId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_CityId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
