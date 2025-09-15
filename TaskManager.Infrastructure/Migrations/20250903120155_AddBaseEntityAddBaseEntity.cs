using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityAddBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskLables");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TaskItem",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TaskItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "IsDeleted",
                table: "TaskItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TaskItem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TaskItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Label",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Label",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "IsDeleted",
                table: "Label",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Label",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Label",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TaskLabel",
                columns: table => new
                {
                    LabelId = table.Column<int>(type: "integer", nullable: false),
                    TaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLabel", x => new { x.LabelId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskLabel_Label_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskLabel_TaskItem_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin", "ADMIN" },
                    { "2", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLabel_TaskId",
                table: "TaskLabel",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskLabel");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Label");

            migrationBuilder.CreateTable(
                name: "TaskLables",
                columns: table => new
                {
                    LabelsId = table.Column<int>(type: "integer", nullable: false),
                    TasksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLables", x => new { x.LabelsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_TaskLables_Label_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskLables_TaskItem_TasksId",
                        column: x => x.TasksId,
                        principalTable: "TaskItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLables_TasksId",
                table: "TaskLables",
                column: "TasksId");
        }
    }
}
