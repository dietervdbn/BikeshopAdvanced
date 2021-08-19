using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeshopAdvanced.Migrations
{
    public partial class ShopDbV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "shoppingItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "shoppingItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
