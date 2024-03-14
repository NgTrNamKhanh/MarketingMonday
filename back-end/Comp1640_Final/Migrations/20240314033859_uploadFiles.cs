using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class uploadFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MarketingCoordinatorId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "MarketingCoordinatorId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
