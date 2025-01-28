using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPublishOptsToVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublishOptionsId",
                table: "Videos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 6, 2, 8, 54, 41, 972, DateTimeKind.Utc).AddTicks(8140));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 6, 2, 8, 54, 41, 972, DateTimeKind.Utc).AddTicks(8160));

            migrationBuilder.CreateIndex(
                name: "IX_Videos_PublishOptionsId",
                table: "Videos",
                column: "PublishOptionsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_PublishOptionsEnumerable_PublishOptionsId",
                table: "Videos",
                column: "PublishOptionsId",
                principalTable: "PublishOptionsEnumerable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_PublishOptionsEnumerable_PublishOptionsId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_PublishOptionsId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "PublishOptionsId",
                table: "Videos");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 6, 2, 7, 44, 23, 153, DateTimeKind.Utc).AddTicks(440));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 6, 2, 7, 44, 23, 153, DateTimeKind.Utc).AddTicks(460));
        }
    }
}
