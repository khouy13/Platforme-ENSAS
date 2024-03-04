using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migratEmploiExam2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdNiveau",
                table: "EmploiExams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExams_IdNiveau",
                table: "EmploiExams",
                column: "IdNiveau");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiExams_Niveaus_IdNiveau",
                table: "EmploiExams",
                column: "IdNiveau",
                principalTable: "Niveaus",
                principalColumn: "IdNiveau");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiExams_Niveaus_IdNiveau",
                table: "EmploiExams");

            migrationBuilder.DropIndex(
                name: "IX_EmploiExams_IdNiveau",
                table: "EmploiExams");

            migrationBuilder.DropColumn(
                name: "IdNiveau",
                table: "EmploiExams");
        }
    }
}
