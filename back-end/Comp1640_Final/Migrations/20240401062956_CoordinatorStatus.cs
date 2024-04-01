using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class CoordinatorStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CoordinatorStatus",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatorStatus",
                table: "Articles");
        }
    }
}
