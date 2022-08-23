using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneToManyTest.Migrations
{
    public partial class Updated_Hobby_22082315252407 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppHobbyCustomer",
                columns: table => new
                {
                    HobbyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppHobbyCustomer", x => new { x.HobbyId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_AppHobbyCustomer_AppCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AppCustomers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppHobbyCustomer_AppHobbies_HobbyId",
                        column: x => x.HobbyId,
                        principalTable: "AppHobbies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppHobbyCustomer_CustomerId",
                table: "AppHobbyCustomer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppHobbyCustomer_HobbyId_CustomerId",
                table: "AppHobbyCustomer",
                columns: new[] { "HobbyId", "CustomerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppHobbyCustomer");
        }
    }
}
