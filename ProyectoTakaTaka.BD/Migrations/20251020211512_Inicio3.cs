using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTakaTaka.BD.Migrations
{
    /// <inheritdoc />
    public partial class Inicio3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Cumpleaneros_CumplaneroId",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "CumplaneroId",
                table: "Eventos",
                newName: "CumpleaneroId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_CumplaneroId",
                table: "Eventos",
                newName: "IX_Eventos_CumpleaneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Cumpleaneros_CumpleaneroId",
                table: "Eventos",
                column: "CumpleaneroId",
                principalTable: "Cumpleaneros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Cumpleaneros_CumpleaneroId",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "CumpleaneroId",
                table: "Eventos",
                newName: "CumplaneroId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_CumpleaneroId",
                table: "Eventos",
                newName: "IX_Eventos_CumplaneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Cumpleaneros_CumplaneroId",
                table: "Eventos",
                column: "CumplaneroId",
                principalTable: "Cumpleaneros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
