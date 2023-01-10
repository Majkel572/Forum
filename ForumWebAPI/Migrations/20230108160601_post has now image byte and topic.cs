using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumWebAPI.Migrations
{
    public partial class posthasnowimagebyteandtopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "postOwnerEmail",
                table: "Posts",
                newName: "PostOwnerEmail");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Posts",
                newName: "Content");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PostOwnerEmail",
                table: "Posts",
                newName: "postOwnerEmail");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "content");
        }
    }
}
