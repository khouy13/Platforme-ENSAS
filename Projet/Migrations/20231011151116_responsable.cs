using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class responsable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matieres_Niveaus_niveauIdNiveau",
                table: "Matieres");

            migrationBuilder.RenameColumn(
                name: "niveauIdNiveau",
                table: "Matieres",
                newName: "IdEnseignant");

            migrationBuilder.RenameIndex(
                name: "IX_Matieres_niveauIdNiveau",
                table: "Matieres",
                newName: "IX_Matieres_IdEnseignant");

            migrationBuilder.AddForeignKey(
                name: "FK_Matieres_Enseignants_IdEnseignant",
                table: "Matieres",
                column: "IdEnseignant",
                principalTable: "Enseignants",
                principalColumn: "IdEnseignant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matieres_Enseignants_IdEnseignant",
                table: "Matieres");

            migrationBuilder.RenameColumn(
                name: "IdEnseignant",
                table: "Matieres",
                newName: "niveauIdNiveau");

            migrationBuilder.RenameIndex(
                name: "IX_Matieres_IdEnseignant",
                table: "Matieres",
                newName: "IX_Matieres_niveauIdNiveau");

            migrationBuilder.AddForeignKey(
                name: "FK_Matieres_Niveaus_niveauIdNiveau",
                table: "Matieres",
                column: "niveauIdNiveau",
                principalTable: "Niveaus",
                principalColumn: "IdNiveau");
        }
    }
}
