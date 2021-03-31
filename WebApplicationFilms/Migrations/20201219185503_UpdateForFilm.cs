using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationFilms.Migrations
{
    public partial class UpdateForFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Films",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GreatRoles",
                table: "Films",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlTrailer",
                table: "Films",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Director",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "GreatRoles",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "UrlTrailer",
                table: "Films");
        }
    }
}
