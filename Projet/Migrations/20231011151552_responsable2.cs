using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class responsable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdVacataire",
                table: "Matieres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matieres_IdVacataire",
                table: "Matieres",
                column: "IdVacataire");

            migrationBuilder.AddForeignKey(
                name: "FK_Matieres_vacataires_IdVacataire",
                table: "Matieres",
                column: "IdVacataire",
                principalTable: "vacataires",
                principalColumn: "IdVacataire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matieres_vacataires_IdVacataire",
                table: "Matieres");

            migrationBuilder.DropIndex(
                name: "IX_Matieres_IdVacataire",
                table: "Matieres");

            migrationBuilder.DropColumn(
                name: "IdVacataire",
                table: "Matieres");
        }
    }
}
