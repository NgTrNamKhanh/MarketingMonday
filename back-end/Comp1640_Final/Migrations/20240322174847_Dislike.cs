using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class Dislike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the primary key constraint that depends on the Id column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Dislikes",
                table: "Dislikes");

            // Drop the Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Dislikes");

            // Add a new Id column with the appropriate data type
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Dislikes",
                type: "uniqueidentifier",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add back the Id column
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Dislikes",
                type: "int",
                nullable: false);

            // Recreate the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_Dislikes",
                table: "Dislikes",
                column: "Id");
        }

    }
}
