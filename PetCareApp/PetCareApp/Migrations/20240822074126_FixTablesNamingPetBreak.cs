using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class FixTablesNamingPetBreak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Break_AspNetUsers_AppUserId",
                table: "Break");

            migrationBuilder.DropForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolio_AspNetUsers_AppUserId",
                table: "Portfolio");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Pet_PetId",
                table: "Records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolio",
                table: "Portfolio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pet",
                table: "Pet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Break",
                table: "Break");

            migrationBuilder.RenameTable(
                name: "Portfolio",
                newName: "Portfolios");

            migrationBuilder.RenameTable(
                name: "Pet",
                newName: "Pets");

            migrationBuilder.RenameTable(
                name: "Break",
                newName: "Breaks");

            migrationBuilder.RenameIndex(
                name: "IX_Portfolio_AppUserId",
                table: "Portfolios",
                newName: "IX_Portfolios_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pet_AppUserId",
                table: "Pets",
                newName: "IX_Pets_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Break_AppUserId",
                table: "Breaks",
                newName: "IX_Breaks_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pets",
                table: "Pets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Breaks",
                table: "Breaks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Breaks_AspNetUsers_AppUserId",
                table: "Breaks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_AppUserId",
                table: "Pets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_AspNetUsers_AppUserId",
                table: "Portfolios",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Pets_PetId",
                table: "Records",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breaks_AspNetUsers_AppUserId",
                table: "Breaks");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_AppUserId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_AspNetUsers_AppUserId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_Pets_PetId",
                table: "Records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pets",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Breaks",
                table: "Breaks");

            migrationBuilder.RenameTable(
                name: "Portfolios",
                newName: "Portfolio");

            migrationBuilder.RenameTable(
                name: "Pets",
                newName: "Pet");

            migrationBuilder.RenameTable(
                name: "Breaks",
                newName: "Break");

            migrationBuilder.RenameIndex(
                name: "IX_Portfolios_AppUserId",
                table: "Portfolio",
                newName: "IX_Portfolio_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_AppUserId",
                table: "Pet",
                newName: "IX_Pet_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Breaks_AppUserId",
                table: "Break",
                newName: "IX_Break_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolio",
                table: "Portfolio",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pet",
                table: "Pet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Break",
                table: "Break",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Break_AspNetUsers_AppUserId",
                table: "Break",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolio_AspNetUsers_AppUserId",
                table: "Portfolio",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Pet_PetId",
                table: "Records",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");
        }
    }
}
