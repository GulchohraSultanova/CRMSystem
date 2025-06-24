using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditPendingCategorys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "IsUpdateRequest",
                table: "PendingCategories");

            migrationBuilder.DropColumn(
                name: "NewUnit",
                table: "PendingCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdateRequest",
                table: "PendingCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NewUnit",
                table: "PendingCategories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
