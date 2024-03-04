using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migratEmploiExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEmploiExam",
                table: "vacataires",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEmploiExam",
                table: "Locals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEmploiExam",
                table: "Enseignants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Examens",
                columns: table => new
                {
                    IdExamen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateExamen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroExamen = table.Column<int>(type: "int", nullable: false),
                    IdSemestre = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examens", x => x.IdExamen);
                    table.ForeignKey(
                        name: "FK_Examens_semestres_IdSemestre",
                        column: x => x.IdSemestre,
                        principalTable: "semestres",
                        principalColumn: "IdSemestre");
                });

            migrationBuilder.CreateTable(
                name: "EmploiExams",
                columns: table => new
                {
                    IdEmploiExam = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdExamen = table.Column<int>(type: "int", nullable: true),
                    IdMatiere = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploiExams", x => x.IdEmploiExam);
                    table.ForeignKey(
                        name: "FK_EmploiExams_Examens_IdExamen",
                        column: x => x.IdExamen,
                        principalTable: "Examens",
                        principalColumn: "IdExamen");
                    table.ForeignKey(
                        name: "FK_EmploiExams_Matieres_IdMatiere",
                        column: x => x.IdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere");
                });

            migrationBuilder.CreateIndex(
                name: "IX_vacataires_IdEmploiExam",
                table: "vacataires",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_Locals_IdEmploiExam",
                table: "Locals",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_IdEmploiExam",
                table: "Enseignants",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExams_IdExamen",
                table: "EmploiExams",
                column: "IdExamen");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExams_IdMatiere",
                table: "EmploiExams",
                column: "IdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_IdSemestre",
                table: "Examens",
                column: "IdSemestre");

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_EmploiExams_IdEmploiExam",
                table: "Enseignants",
                column: "IdEmploiExam",
                principalTable: "EmploiExams",
                principalColumn: "IdEmploiExam");

            migrationBuilder.AddForeignKey(
                name: "FK_Locals_EmploiExams_IdEmploiExam",
                table: "Locals",
                column: "IdEmploiExam",
                principalTable: "EmploiExams",
                principalColumn: "IdEmploiExam");

            migrationBuilder.AddForeignKey(
                name: "FK_vacataires_EmploiExams_IdEmploiExam",
                table: "vacataires",
                column: "IdEmploiExam",
                principalTable: "EmploiExams",
                principalColumn: "IdEmploiExam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_EmploiExams_IdEmploiExam",
                table: "Enseignants");

            migrationBuilder.DropForeignKey(
                name: "FK_Locals_EmploiExams_IdEmploiExam",
                table: "Locals");

            migrationBuilder.DropForeignKey(
                name: "FK_vacataires_EmploiExams_IdEmploiExam",
                table: "vacataires");

            migrationBuilder.DropTable(
                name: "EmploiExams");

            migrationBuilder.DropTable(
                name: "Examens");

            migrationBuilder.DropIndex(
                name: "IX_vacataires_IdEmploiExam",
                table: "vacataires");

            migrationBuilder.DropIndex(
                name: "IX_Locals_IdEmploiExam",
                table: "Locals");

            migrationBuilder.DropIndex(
                name: "IX_Enseignants_IdEmploiExam",
                table: "Enseignants");

            migrationBuilder.DropColumn(
                name: "IdEmploiExam",
                table: "vacataires");

            migrationBuilder.DropColumn(
                name: "IdEmploiExam",
                table: "Locals");

            migrationBuilder.DropColumn(
                name: "IdEmploiExam",
                table: "Enseignants");
        }
    }
}
