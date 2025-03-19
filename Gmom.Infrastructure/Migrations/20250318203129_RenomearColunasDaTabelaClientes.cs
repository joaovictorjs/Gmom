using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenomearColunasDaTabelaClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "clientes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "clientes",
                newName: "nome");
            
            migrationBuilder.RenameIndex("clientes_Id_seq", "clientes_id_seq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "clientes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "clientes",
                newName: "Name");
            
            migrationBuilder.RenameIndex("clientes_id_seq" , "clientes_Id_seq");
        }
    }
}
