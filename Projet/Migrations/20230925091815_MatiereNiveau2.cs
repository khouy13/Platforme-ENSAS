using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class MatiereNiveau2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatiereNiveaus_Niveaus_NiveauIdNiveau",
                table: "MatiereNiveaus");

            migrationBuilder.RenameColumn(
                name: "NiveauIdNiveau",
                table: "MatiereNiveaus",
                newName: "IdNiveau");

            migrationBuilder.RenameIndex(
                name: "IX_MatiereNiveaus_NiveauIdNiveau",
                table: "MatiereNiveaus",
                newName: "IX_MatiereNiveaus_IdNiveau");

            migrationBuilder.AddForeignKey(
                name: "FK_MatiereNiveaus_Niveaus_IdNiveau",
                table: "MatiereNiveaus",
                column: "IdNiveau",
                principalTable: "Niveaus",
                principalColumn: "IdNiveau");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatiereNiveaus_Niveaus_IdNiveau",
                table: "MatiereNiveaus");

            migrationBuilder.RenameColumn(
                name: "IdNiveau",
                table: "MatiereNiveaus",
                newName: "NiveauIdNiveau");

            migrationBuilder.RenameIndex(
                name: "IX_MatiereNiveaus_IdNiveau",
                table: "MatiereNiveaus",
                newName: "IX_MatiereNiveaus_NiveauIdNiveau");

            migrationBuilder.AddForeignKey(
                name: "FK_MatiereNiveaus_Niveaus_NiveauIdNiveau",
                table: "MatiereNiveaus",
                column: "NiveauIdNiveau",
                principalTable: "Niveaus",
                principalColumn: "IdNiveau");
        }
    }
}
