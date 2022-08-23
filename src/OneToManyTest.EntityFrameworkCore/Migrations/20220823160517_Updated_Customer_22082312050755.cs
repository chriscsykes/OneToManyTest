using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneToManyTest.Migrations
{
    public partial class Updated_Customer_22082312050755 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppCustomers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppCustomers");
        }
    }
}
