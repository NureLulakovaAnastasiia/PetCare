using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetTableAddCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
