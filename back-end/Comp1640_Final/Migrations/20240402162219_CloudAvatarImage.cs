using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class CloudAvatarImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocPath",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "CloudAvatarImagePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudAvatarImagePath",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "DocPath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
