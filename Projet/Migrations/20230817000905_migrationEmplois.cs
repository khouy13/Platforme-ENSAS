using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migrationEmplois : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Enseignants_EnseignantIdEnseignant",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Groupes_GroupeIdGroupe",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Jours_JourIdJour",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Locals_LocalIdLocal",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Matieres_MatiereIdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Seances_SeanceIdSeance",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_TypeEnseignementId",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_EnseignantIdEnseignant",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_GroupeIdGroupe",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_JourIdJour",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_LocalIdLocal",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_MatiereIdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_SeanceIdSeance",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_TypeEnseignementId",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "EnseignantIdEnseignant",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "GroupeIdGroupe",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "JourIdJour",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "LocalIdLocal",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "MatiereIdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "SeanceIdSeance",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "TypeEnseignementId",
                table: "EmploiTemps");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdEnseignant",
                table: "EmploiTemps",
                column: "IdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdGroupe",
                table: "EmploiTemps",
                column: "IdGroupe");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdJour",
                table: "EmploiTemps",
                column: "IdJour");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdLocal",
                table: "EmploiTemps",
                column: "IdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdMatiere",
                table: "EmploiTemps",
                column: "IdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdSeance",
                table: "EmploiTemps",
                column: "IdSeance");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdTypeEnseignement",
                table: "EmploiTemps",
                column: "IdTypeEnseignement");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Enseignants_IdEnseignant",
                table: "EmploiTemps",
                column: "IdEnseignant",
                principalTable: "Enseignants",
                principalColumn: "IdEnseignant");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Groupes_IdGroupe",
                table: "EmploiTemps",
                column: "IdGroupe",
                principalTable: "Groupes",
                principalColumn: "IdGroupe");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Jours_IdJour",
                table: "EmploiTemps",
                column: "IdJour",
                principalTable: "Jours",
                principalColumn: "IdJour");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Locals_IdLocal",
                table: "EmploiTemps",
                column: "IdLocal",
                principalTable: "Locals",
                principalColumn: "IdLocal");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Matieres_IdMatiere",
                table: "EmploiTemps",
                column: "IdMatiere",
                principalTable: "Matieres",
                principalColumn: "IdMatiere");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Seances_IdSeance",
                table: "EmploiTemps",
                column: "IdSeance",
                principalTable: "Seances",
                principalColumn: "IdSeance");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_IdTypeEnseignement",
                table: "EmploiTemps",
                column: "IdTypeEnseignement",
                principalTable: "TypeEnseignements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Enseignants_IdEnseignant",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Groupes_IdGroupe",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Jours_IdJour",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Locals_IdLocal",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Matieres_IdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Seances_IdSeance",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_IdTypeEnseignement",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdEnseignant",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdGroupe",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdJour",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdLocal",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdSeance",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdTypeEnseignement",
                table: "EmploiTemps");

            migrationBuilder.AddColumn<int>(
                name: "EnseignantIdEnseignant",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupeIdGroupe",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JourIdJour",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalIdLocal",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatiereIdMatiere",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeanceIdSeance",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeEnseignementId",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_EnseignantIdEnseignant",
                table: "EmploiTemps",
                column: "EnseignantIdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_GroupeIdGroupe",
                table: "EmploiTemps",
                column: "GroupeIdGroupe");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_JourIdJour",
                table: "EmploiTemps",
                column: "JourIdJour");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_LocalIdLocal",
                table: "EmploiTemps",
                column: "LocalIdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_MatiereIdMatiere",
                table: "EmploiTemps",
                column: "MatiereIdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_SeanceIdSeance",
                table: "EmploiTemps",
                column: "SeanceIdSeance");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_TypeEnseignementId",
                table: "EmploiTemps",
                column: "TypeEnseignementId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Enseignants_EnseignantIdEnseignant",
                table: "EmploiTemps",
                column: "EnseignantIdEnseignant",
                principalTable: "Enseignants",
                principalColumn: "IdEnseignant");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Groupes_GroupeIdGroupe",
                table: "EmploiTemps",
                column: "GroupeIdGroupe",
                principalTable: "Groupes",
                principalColumn: "IdGroupe");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Jours_JourIdJour",
                table: "EmploiTemps",
                column: "JourIdJour",
                principalTable: "Jours",
                principalColumn: "IdJour");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Locals_LocalIdLocal",
                table: "EmploiTemps",
                column: "LocalIdLocal",
                principalTable: "Locals",
                principalColumn: "IdLocal");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Matieres_MatiereIdMatiere",
                table: "EmploiTemps",
                column: "MatiereIdMatiere",
                principalTable: "Matieres",
                principalColumn: "IdMatiere");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Seances_SeanceIdSeance",
                table: "EmploiTemps",
                column: "SeanceIdSeance",
                principalTable: "Seances",
                principalColumn: "IdSeance");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_TypeEnseignementId",
                table: "EmploiTemps",
                column: "TypeEnseignementId",
                principalTable: "TypeEnseignements",
                principalColumn: "Id");
        }
    }
}
