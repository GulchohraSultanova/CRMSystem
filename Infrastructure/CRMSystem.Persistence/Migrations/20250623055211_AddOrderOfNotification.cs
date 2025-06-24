using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderOfNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "AdminNotifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_OrderId",
                table: "AdminNotifications",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminNotifications_Orders_OrderId",
                table: "AdminNotifications",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminNotifications_Orders_OrderId",
                table: "AdminNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_OrderId",
                table: "AdminNotifications");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AdminNotifications");
        }
    }
}
