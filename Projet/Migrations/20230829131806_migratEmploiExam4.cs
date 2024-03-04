using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migratEmploiExam4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdSemestre",
                table: "EmploiExams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExams_IdSemestre",
                table: "EmploiExams",
                column: "IdSemestre");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiExams_semestres_IdSemestre",
                table: "EmploiExams",
                column: "IdSemestre",
                principalTable: "semestres",
                principalColumn: "IdSemestre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiExams_semestres_IdSemestre",
                table: "EmploiExams");

            migrationBuilder.DropIndex(
                name: "IX_EmploiExams_IdSemestre",
                table: "EmploiExams");

            migrationBuilder.DropColumn(
                name: "IdSemestre",
                table: "EmploiExams");
        }
    }
}
