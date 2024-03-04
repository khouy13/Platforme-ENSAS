using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migratEmploiExam3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdJour",
                table: "EmploiExams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdSeance",
                table: "EmploiExams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExams_IdJour",
                table: "EmploiExams",
                column: "IdJour");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExams_IdSeance",
                table: "EmploiExams",
                column: "IdSeance");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiExams_Jours_IdJour",
                table: "EmploiExams",
                column: "IdJour",
                principalTable: "Jours",
                principalColumn: "IdJour");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiExams_Seances_IdSeance",
                table: "EmploiExams",
                column: "IdSeance",
                principalTable: "Seances",
                principalColumn: "IdSeance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiExams_Jours_IdJour",
                table: "EmploiExams");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploiExams_Seances_IdSeance",
                table: "EmploiExams");

            migrationBuilder.DropIndex(
                name: "IX_EmploiExams_IdJour",
                table: "EmploiExams");

            migrationBuilder.DropIndex(
                name: "IX_EmploiExams_IdSeance",
                table: "EmploiExams");

            migrationBuilder.DropColumn(
                name: "IdJour",
                table: "EmploiExams");

            migrationBuilder.DropColumn(
                name: "IdSeance",
                table: "EmploiExams");
        }
    }
}
