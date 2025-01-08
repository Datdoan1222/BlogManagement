using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.Migrations
{
    public partial class AdddeleteCascadeforTablePostCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pc_category",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "fk_pc_post",
                table: "PostCategories");


            migrationBuilder.AddForeignKey(
                name: "fk_pc_category",
                table: "PostCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pc_post",
                table: "PostCategories",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pc_category",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "fk_pc_post",
                table: "PostCategories");

            

            migrationBuilder.AddForeignKey(
                name: "fk_pc_category",
                table: "PostCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "fk_pc_post",
                table: "PostCategories",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
