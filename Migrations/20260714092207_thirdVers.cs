using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberSalon.Migrations
{
    /// <inheritdoc />
    public partial class thirdVers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Payments",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "PaymentId");
        }
    }
}
