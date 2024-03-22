using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDislike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            // Drop the existing column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Likes");

            // Add a new column with type Guid
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Likes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            // Make the new column the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the existing primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            // Drop the existing column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Likes");

            // Add a new column with type int
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Likes",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Make the new column the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "Id");
        }
    }
}
