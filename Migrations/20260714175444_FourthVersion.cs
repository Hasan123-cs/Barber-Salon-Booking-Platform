using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberSalon.Migrations
{
    /// <inheritdoc />
    public partial class FourthVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_EmployeeId_AppointmentDate_StartTime",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmployeeId_AppointmentDate_StartTime",
                table: "Appointments",
                columns: new[] { "EmployeeId", "AppointmentDate", "StartTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_EmployeeId_AppointmentDate_StartTime",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EmployeeId_AppointmentDate_StartTime",
                table: "Appointments",
                columns: new[] { "EmployeeId", "AppointmentDate", "StartTime" });
        }
    }
}
