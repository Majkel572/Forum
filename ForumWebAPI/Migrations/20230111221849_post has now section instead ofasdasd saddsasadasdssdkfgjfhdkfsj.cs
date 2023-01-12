using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumWebAPI.Migrations
{
    public partial class posthasnowsectioninsteadofasdasdsaddsasadasdssdkfgjfhdkfsj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Posts");
        }
    }
}
