﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class ViewCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Articles");
        }
    }
}
