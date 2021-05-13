using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactTodo.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<long>(type: "bigint", nullable: false),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Todo" },
                    { 2L, "Active" },
                    { 3L, "Blocked" },
                    { 4L, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "BoardId", "DeadLine", "Description", "Priority", "Title" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2021, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "My fist todo", 1, "Todo #1" },
                    { 2L, 1L, new DateTime(2021, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "My second todo", 2, "Todo #2" },
                    { 3L, 1L, new DateTime(2021, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "My third todo", 3, "Todo #3" },
                    { 4L, 1L, new DateTime(2021, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "My fourth todo", 4, "Todo #4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_BoardId",
                table: "TodoItems",
                column: "BoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
