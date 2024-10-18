using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POE_p2_s4.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
          name: "Claims",
          columns: table => new
          {
              Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
              ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: false),
              Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
              ClaimDate = table.Column<DateTime>(type: "datetime2", nullable: false),
              HoursWorked = table.Column<double>(type: "float", nullable: false),
              ClaimExpenses = table.Column<double>(type: "float", nullable: true),
              ClaimStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
              UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
             

          },
          constraints: table =>
          {
              table.PrimaryKey("PK_Claims", x => x.Id);
              table.ForeignKey(
                  name: "FK_Claims_AspNetUsers_UserId",
                  column: x => x.UserId,
                  principalTable: "AspNetUsers",
                  principalColumn: "Id");
          });

            migrationBuilder.CreateIndex(
             name: "IX_Claims_UserId",
             table: "Claims",
             column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_AspNetUsers_UserId",
                table: "Claims");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Claims",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_AspNetUsers_UserId",
                table: "Claims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
