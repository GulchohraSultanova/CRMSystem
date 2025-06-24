using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderAndOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Buyed",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Finished",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OutDatedOrder",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "FullFlied",
                table: "Orders",
                newName: "FighterConfirm");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "OrderItems",
                newName: "SuppliedQuantity");

            migrationBuilder.RenameColumn(
                name: "OrderDateTimeLimit",
                table: "OrderItems",
                newName: "OrderLimitTime");

            migrationBuilder.AddColumn<bool>(
                name: "EmployeeDelivery",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDeliveryTime",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "RequiredQuantity",
                table: "OrderItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeDelivery",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderDeliveryTime",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "RequiredQuantity",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "FighterConfirm",
                table: "Orders",
                newName: "FullFlied");

            migrationBuilder.RenameColumn(
                name: "SuppliedQuantity",
                table: "OrderItems",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "OrderLimitTime",
                table: "OrderItems",
                newName: "OrderDateTimeLimit");

            migrationBuilder.AddColumn<bool>(
                name: "Buyed",
                table: "OrderItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "OrderItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "OrderItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OutDatedOrder",
                table: "OrderItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
