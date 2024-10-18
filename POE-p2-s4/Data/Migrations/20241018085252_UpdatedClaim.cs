using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_p2_s4.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DocumentBinary",
                table: "Claims",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentBinary",
                table: "Claims");
        }
    }
}
