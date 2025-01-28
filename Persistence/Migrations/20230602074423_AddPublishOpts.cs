using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPublishOpts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublishOptionsId",
                table: "Nfts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PublishOptionsEnumerable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentType = table.Column<int>(type: "integer", nullable: false),
                    LikeStyle = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishOptionsEnumerable", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Nfts_PublishOptionsId",
                table: "Nfts",
                column: "PublishOptionsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Nfts_PublishOptionsEnumerable_PublishOptionsId",
                table: "Nfts",
                column: "PublishOptionsId",
                principalTable: "PublishOptionsEnumerable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nfts_PublishOptionsEnumerable_PublishOptionsId",
                table: "Nfts");

            migrationBuilder.DropTable(
                name: "PublishOptionsEnumerable");

            migrationBuilder.DropIndex(
                name: "IX_Nfts_PublishOptionsId",
                table: "Nfts");

            migrationBuilder.DropColumn(
                name: "PublishOptionsId",
                table: "Nfts");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 5, 24, 7, 51, 19, 200, DateTimeKind.Utc).AddTicks(8870));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 5, 24, 7, 51, 19, 200, DateTimeKind.Utc).AddTicks(8880));
        }
    }
}
