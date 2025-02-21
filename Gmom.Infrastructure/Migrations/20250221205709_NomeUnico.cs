using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NomeUnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_usuarios_nome",
                table: "usuarios",
                column: "nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuarios_nome",
                table: "usuarios");
        }
    }
}
