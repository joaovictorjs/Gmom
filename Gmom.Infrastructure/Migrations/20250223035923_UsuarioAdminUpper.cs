using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioAdminUpper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "id",
                keyValue: 1,
                column: "nome",
                value: "ADMIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "id",
                keyValue: 1,
                column: "nome",
                value: "admin");
        }
    }
}
