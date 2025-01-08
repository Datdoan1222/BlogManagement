using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.Migrations
{
    public partial class RelationshipTablePostCommentwithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "PostComments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);


            migrationBuilder.CreateIndex(
                name: "IX_PostComments_UserId",
                table: "PostComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "fk_comment_user",
                table: "PostComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comment_user",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_UserId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostComments");


        }
    }
}
