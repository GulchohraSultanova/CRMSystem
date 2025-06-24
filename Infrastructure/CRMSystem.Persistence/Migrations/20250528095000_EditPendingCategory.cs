using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditPendingCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingCategories_Categories_OriginalCategoryId",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PendingCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "PendingCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "PendingCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NewName",
                table: "PendingCategories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewUnit",
                table: "PendingCategories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingCategories_Categories_OriginalCategoryId",
                table: "PendingCategories",
                column: "OriginalCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingCategories_Categories_OriginalCategoryId",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "NewName",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "NewUnit",
                table: "PendingCategories");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PendingCategories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingCategories_Categories_OriginalCategoryId",
                table: "PendingCategories",
                column: "OriginalCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
