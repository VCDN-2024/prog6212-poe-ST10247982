using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_p2_s4.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedClaimsFInal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KilometersTravelled",
                table: "Claims",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KilometersTravelled",
                table: "Claims");
        }
    }
}
