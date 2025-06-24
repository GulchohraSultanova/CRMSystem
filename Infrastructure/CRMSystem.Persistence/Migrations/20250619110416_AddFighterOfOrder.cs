using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFighterOfOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AdminId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "FighterId",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FighterId",
                table: "Orders",
                column: "FighterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AdminId",
                table: "Orders",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_FighterId",
                table: "Orders",
                column: "FighterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AdminId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_FighterId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FighterId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FighterId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AdminId",
                table: "Orders",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
