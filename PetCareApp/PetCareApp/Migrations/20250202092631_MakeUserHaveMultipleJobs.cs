using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserHaveMultipleJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationEmployees_AppUserId",
                table: "OrganizationEmployees");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEmployees_AppUserId",
                table: "OrganizationEmployees",
                unique: false,
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationEmployees_AppUserId",
                table: "OrganizationEmployees");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEmployees_AppUserId",
                table: "OrganizationEmployees",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }
    }
}
