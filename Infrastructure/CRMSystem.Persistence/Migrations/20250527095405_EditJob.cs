using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Jobs_JobId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Sections_SectionId",
                table: "Jobs");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionId",
                table: "Jobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Jobs_JobId",
                table: "AspNetUsers",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Sections_SectionId",
                table: "Jobs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Jobs_JobId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Sections_SectionId",
                table: "Jobs");

            migrationBuilder.AlterColumn<Guid>(
                name: "SectionId",
                table: "Jobs",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Jobs_JobId",
                table: "AspNetUsers",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Sections_SectionId",
                table: "Jobs",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
