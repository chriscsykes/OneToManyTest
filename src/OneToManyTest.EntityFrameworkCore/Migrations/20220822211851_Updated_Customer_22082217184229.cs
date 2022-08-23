using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneToManyTest.Migrations
{
    public partial class Updated_Customer_22082217184229 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "AppCustomers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_OrderId",
                table: "AppCustomers",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCustomers_AppOrders_OrderId",
                table: "AppCustomers",
                column: "OrderId",
                principalTable: "AppOrders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCustomers_AppOrders_OrderId",
                table: "AppCustomers");

            migrationBuilder.DropIndex(
                name: "IX_AppCustomers_OrderId",
                table: "AppCustomers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AppCustomers");
        }
    }
}
