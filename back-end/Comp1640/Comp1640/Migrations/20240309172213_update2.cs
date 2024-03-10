using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FacultyID",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_FacultyID",
                table: "Articles",
                column: "FacultyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Faculties_FacultyID",
                table: "Articles",
                column: "FacultyID",
                principalTable: "Faculties",
                principalColumn: "FacultyID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Faculties_FacultyID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_FacultyID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "FacultyID",
                table: "Articles");
        }
    }
}
