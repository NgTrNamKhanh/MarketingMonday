using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class notification2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ArticleId",
                table: "Notifications",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CommentId",
                table: "Notifications",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Articles_ArticleId",
                table: "Notifications",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Comments_CommentId",
                table: "Notifications",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Articles_ArticleId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Comments_CommentId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ArticleId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CommentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Notifications");
        }
    }
}
