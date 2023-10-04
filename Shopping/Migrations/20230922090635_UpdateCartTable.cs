using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    public partial class UpdateCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CartItems");

            // Rename the 'ProductId' column to 'SKUId'
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartItems",
                newName: "SKUId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CartItems",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CartItems");

            // Rename the 'SKUId' column back to 'ProductId'
            migrationBuilder.RenameColumn(
                name: "SKUId",
                table: "CartItems",
                newName: "ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false);

        }
    }
}
