using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class MatierCommun2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatiereCommuns_Matieres_MainMatiereIdMatiere",
                table: "MatiereCommuns");

            migrationBuilder.DropForeignKey(
                name: "FK_MatiereCommuns_Matieres_RelatedMatiereIdMatiere",
                table: "MatiereCommuns");

            migrationBuilder.RenameColumn(
                name: "RelatedMatiereIdMatiere",
                table: "MatiereCommuns",
                newName: "RelatedMatiereId");

            migrationBuilder.RenameColumn(
                name: "MainMatiereIdMatiere",
                table: "MatiereCommuns",
                newName: "MainMatiereId");

            migrationBuilder.RenameIndex(
                name: "IX_MatiereCommuns_RelatedMatiereIdMatiere",
                table: "MatiereCommuns",
                newName: "IX_MatiereCommuns_RelatedMatiereId");

            migrationBuilder.RenameIndex(
                name: "IX_MatiereCommuns_MainMatiereIdMatiere",
                table: "MatiereCommuns",
                newName: "IX_MatiereCommuns_MainMatiereId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatiereCommuns_Matieres_MainMatiereId",
                table: "MatiereCommuns",
                column: "MainMatiereId",
                principalTable: "Matieres",
                principalColumn: "IdMatiere",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatiereCommuns_Matieres_RelatedMatiereId",
                table: "MatiereCommuns",
                column: "RelatedMatiereId",
                principalTable: "Matieres",
                principalColumn: "IdMatiere",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatiereCommuns_Matieres_MainMatiereId",
                table: "MatiereCommuns");

            migrationBuilder.DropForeignKey(
                name: "FK_MatiereCommuns_Matieres_RelatedMatiereId",
                table: "MatiereCommuns");

            migrationBuilder.RenameColumn(
                name: "RelatedMatiereId",
                table: "MatiereCommuns",
                newName: "RelatedMatiereIdMatiere");

            migrationBuilder.RenameColumn(
                name: "MainMatiereId",
                table: "MatiereCommuns",
                newName: "MainMatiereIdMatiere");

            migrationBuilder.RenameIndex(
                name: "IX_MatiereCommuns_RelatedMatiereId",
                table: "MatiereCommuns",
                newName: "IX_MatiereCommuns_RelatedMatiereIdMatiere");

            migrationBuilder.RenameIndex(
                name: "IX_MatiereCommuns_MainMatiereId",
                table: "MatiereCommuns",
                newName: "IX_MatiereCommuns_MainMatiereIdMatiere");

            migrationBuilder.AddForeignKey(
                name: "FK_MatiereCommuns_Matieres_MainMatiereIdMatiere",
                table: "MatiereCommuns",
                column: "MainMatiereIdMatiere",
                principalTable: "Matieres",
                principalColumn: "IdMatiere",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MatiereCommuns_Matieres_RelatedMatiereIdMatiere",
                table: "MatiereCommuns",
                column: "RelatedMatiereIdMatiere",
                principalTable: "Matieres",
                principalColumn: "IdMatiere",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
