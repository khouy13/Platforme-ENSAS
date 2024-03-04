using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class Semestre7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSemestre",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdSemestre",
                table: "EmploiTemps",
                column: "IdSemestre");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_semestres_IdSemestre",
                table: "EmploiTemps",
                column: "IdSemestre",
                principalTable: "semestres",
                principalColumn: "IdSemestre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_semestres_IdSemestre",
                table: "EmploiTemps");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdSemestre",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "IdSemestre",
                table: "EmploiTemps");
        }
    }
}
