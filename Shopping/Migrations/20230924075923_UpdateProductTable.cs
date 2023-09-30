using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    public partial class UpdateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "AspectRatioCssClass");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "CartItems",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "CartItems",
                newName: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AspectRatioCssClass",
                table: "Products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CartItems",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CartItems",
                newName: "CreatedDate");
        }
    }
}
