using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_p2_s4.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentBinary = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ClaimId",
                table: "Documents",
                column: "ClaimId",
                unique: true,
                filter: "[ClaimId] IS NOT NULL");
        }
    }
}
