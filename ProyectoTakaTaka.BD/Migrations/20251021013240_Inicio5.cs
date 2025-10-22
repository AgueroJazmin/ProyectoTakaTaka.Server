using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTakaTaka.BD.Migrations
{
    /// <inheritdoc />
    public partial class Inicio5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Meses_MesId",
                table: "Horarios");

            migrationBuilder.DropIndex(
                name: "IX_Horarios_MesId",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "MesId",
                table: "Horarios");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "MesId",
                table: "Horarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_MesId",
                table: "Horarios",
                column: "MesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Meses_MesId",
                table: "Horarios",
                column: "MesId",
                principalTable: "Meses",
                principalColumn: "Id");
        }
    }
}
