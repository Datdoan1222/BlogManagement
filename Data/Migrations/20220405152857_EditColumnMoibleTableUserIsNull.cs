using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.Migrations
{
    public partial class EditColumnMoibleTableUserIsNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
  

            migrationBuilder.DropIndex(
                name: "uq_mobile",
                table: "Users");



            migrationBuilder.CreateIndex(
                name: "uq_mobile",
                table: "Users",
                column: "Mobile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "uq_mobile",
                table: "Users");


            migrationBuilder.CreateIndex(
                name: "uq_mobile",
                table: "Users",
                column: "Mobile",
                unique: true);
        }
    }
}
