using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d92c112-73f6-40b8-a6c8-a8f1402299d3", null, "Admin", "ADMIN" },
                    { "7eaea8cc-4288-4000-8814-9c7895656290", null, "Organization", "ORGANIZATION" },
                    { "ae928d37-2ae5-4e99-ae82-2e3377741e0c", null, "Master", "MASTER" },
                    { "cb6ad40c-1e33-4d05-900b-cc69e83b6917", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d92c112-73f6-40b8-a6c8-a8f1402299d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eaea8cc-4288-4000-8814-9c7895656290");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae928d37-2ae5-4e99-ae82-2e3377741e0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb6ad40c-1e33-4d05-900b-cc69e83b6917");
        }
    }
}
