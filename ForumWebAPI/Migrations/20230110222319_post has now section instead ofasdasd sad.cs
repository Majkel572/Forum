using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumWebAPI.Migrations
{
    public partial class posthasnowsectioninsteadofasdasdsad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserEmail",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostOwnerEmail",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserEmail",
                table: "Posts",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserEmail",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Posts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "PostOwnerEmail",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserEmail",
                table: "Posts",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }
    }
}
