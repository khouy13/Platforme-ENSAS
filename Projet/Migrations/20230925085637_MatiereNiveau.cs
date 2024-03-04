using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class MatiereNiveau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matieres_Niveaus_IdNiveau",
                table: "Matieres");

            migrationBuilder.RenameColumn(
                name: "IdNiveau",
                table: "Matieres",
                newName: "niveauIdNiveau");

            migrationBuilder.RenameIndex(
                name: "IX_Matieres_IdNiveau",
                table: "Matieres",
                newName: "IX_Matieres_niveauIdNiveau");

            migrationBuilder.CreateTable(
                name: "MatiereNiveaus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NiveauIdNiveau = table.Column<int>(type: "int", nullable: true),
                    IdMatiere = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatiereNiveaus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatiereNiveaus_Matieres_IdMatiere",
                        column: x => x.IdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere");
                    table.ForeignKey(
                        name: "FK_MatiereNiveaus_Niveaus_NiveauIdNiveau",
                        column: x => x.NiveauIdNiveau,
                        principalTable: "Niveaus",
                        principalColumn: "IdNiveau");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatiereNiveaus_IdMatiere",
                table: "MatiereNiveaus",
                column: "IdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_MatiereNiveaus_NiveauIdNiveau",
                table: "MatiereNiveaus",
                column: "NiveauIdNiveau");

            migrationBuilder.AddForeignKey(
                name: "FK_Matieres_Niveaus_niveauIdNiveau",
                table: "Matieres",
                column: "niveauIdNiveau",
                principalTable: "Niveaus",
                principalColumn: "IdNiveau");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matieres_Niveaus_niveauIdNiveau",
                table: "Matieres");

            migrationBuilder.DropTable(
                name: "MatiereNiveaus");

            migrationBuilder.RenameColumn(
                name: "niveauIdNiveau",
                table: "Matieres",
                newName: "IdNiveau");

            migrationBuilder.RenameIndex(
                name: "IX_Matieres_niveauIdNiveau",
                table: "Matieres",
                newName: "IX_Matieres_IdNiveau");

            migrationBuilder.AddForeignKey(
                name: "FK_Matieres_Niveaus_IdNiveau",
                table: "Matieres",
                column: "IdNiveau",
                principalTable: "Niveaus",
                principalColumn: "IdNiveau");
        }
    }
}
