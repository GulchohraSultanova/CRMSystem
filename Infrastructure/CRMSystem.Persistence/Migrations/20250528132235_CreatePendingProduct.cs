using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreatePendingProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NewMeasure = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingProducts_Products_OriginalProductId",
                        column: x => x.OriginalProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingProducts_OriginalProductId",
                table: "PendingProducts",
                column: "OriginalProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingProducts");
        }
    }
}
