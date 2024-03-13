using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp1640_Final.Migrations
{
    /// <inheritdoc />
    public partial class updateArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Articles",
                newName: "UploadDate");

            migrationBuilder.AddColumn<string>(
                name: "CoordinatorComment",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingCoordinatorId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PublishStatusId",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_EventId",
                table: "Articles",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_MarketingCoordinatorId",
                table: "Articles",
                column: "MarketingCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_StudentId",
                table: "Articles",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_MarketingCoordinatorId",
                table: "Articles",
                column: "MarketingCoordinatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_StudentId",
                table: "Articles",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Events_EventId",
                table: "Articles",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_MarketingCoordinatorId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_StudentId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Events_EventId",
                table: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_Articles_EventId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_MarketingCoordinatorId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_StudentId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CoordinatorComment",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "MarketingCoordinatorId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PublishStatusId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "Articles",
                newName: "Date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });
        }
    }
}
