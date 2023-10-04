using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    public partial class HomePage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SKUs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Values",
                table: "AttributeMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SKUAttributes_SKUId",
                table: "SKUAttributes",
                column: "SKUId");

            migrationBuilder.AddForeignKey(
                name: "FK_SKUAttributes_SKUs_SKUId",
                table: "SKUAttributes",
                column: "SKUId",
                principalTable: "SKUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SKUAttributes_SKUs_SKUId",
                table: "SKUAttributes");

            migrationBuilder.DropIndex(
                name: "IX_SKUAttributes_SKUId",
                table: "SKUAttributes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SKUs");

            migrationBuilder.AlterColumn<string>(
                name: "Values",
                table: "AttributeMaster",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
