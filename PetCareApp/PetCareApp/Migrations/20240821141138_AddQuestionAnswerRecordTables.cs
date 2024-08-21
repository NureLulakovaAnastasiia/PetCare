using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionAnswerRecordTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_AppUserId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_AspNetUsers_AppUserId",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pets",
                table: "Pets");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.RenameTable(
                name: "Pets",
                newName: "Pet");

            migrationBuilder.RenameColumn(
                name: "MaxmumTime",
                table: "Services",
                newName: "MaximumTime");

            migrationBuilder.RenameColumn(
                name: "MaxmumPrice",
                table: "Services",
                newName: "MaximumPrice");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_AppUserId",
                table: "Schedules",
                newName: "IX_Schedules_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_AppUserId",
                table: "Pet",
                newName: "IX_Pet_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pet",
                table: "Pet",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HasAnswerWithFixedTime = table.Column<bool>(type: "bit", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    PetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Records_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Records_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    IsTimeMinimum = table.Column<bool>(type: "bit", nullable: false),
                    IsTimeMaximum = table.Column<bool>(type: "bit", nullable: false),
                    IsTimeFixed = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecordCancels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecordId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordCancels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordCancels_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecordCancels_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ServiceId",
                table: "Questions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordCancels_AppUserId",
                table: "RecordCancels",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordCancels_RecordId",
                table: "RecordCancels",
                column: "RecordId",
                unique: true,
                filter: "[RecordId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Records_AppUserId",
                table: "Records",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_PetId",
                table: "Records",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ServiceId",
                table: "Records",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_AspNetUsers_AppUserId",
                table: "Pet",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_AppUserId",
                table: "Schedules",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_AppUserId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "RecordCancels");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pet",
                table: "Pet");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.RenameTable(
                name: "Pet",
                newName: "Pets");

            migrationBuilder.RenameColumn(
                name: "MaximumTime",
                table: "Services",
                newName: "MaxmumTime");

            migrationBuilder.RenameColumn(
                name: "MaximumPrice",
                table: "Services",
                newName: "MaxmumPrice");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_AppUserId",
                table: "Schedule",
                newName: "IX_Schedule_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Pet_AppUserId",
                table: "Pets",
                newName: "IX_Pets_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pets",
                table: "Pets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_AppUserId",
                table: "Pets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_AspNetUsers_AppUserId",
                table: "Schedule",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
