using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class addValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Matieres_IdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_IdTypeEnseignement",
                table: "EmploiTemps");

            migrationBuilder.AlterColumn<int>(
                name: "IdTypeEnseignement",
                table: "EmploiTemps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdMatiere",
                table: "EmploiTemps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Matieres_IdMatiere",
                table: "EmploiTemps",
                column: "IdMatiere",
                principalTable: "Matieres",
                principalColumn: "IdMatiere",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_IdTypeEnseignement",
                table: "EmploiTemps",
                column: "IdTypeEnseignement",
                principalTable: "TypeEnseignements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_Matieres_IdMatiere",
                table: "EmploiTemps");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_IdTypeEnseignement",
                table: "EmploiTemps");

            migrationBuilder.AlterColumn<int>(
                name: "IdTypeEnseignement",
                table: "EmploiTemps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdMatiere",
                table: "EmploiTemps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_Matieres_IdMatiere",
                table: "EmploiTemps",
                column: "IdMatiere",
                principalTable: "Matieres",
                principalColumn: "IdMatiere");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_TypeEnseignements_IdTypeEnseignement",
                table: "EmploiTemps",
                column: "IdTypeEnseignement",
                principalTable: "TypeEnseignements",
                principalColumn: "Id");
        }
    }
}
