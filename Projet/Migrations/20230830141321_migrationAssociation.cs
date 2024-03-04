using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migrationAssociation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_vacataires_IdEmploiExam",
                table: "vacataires");

            migrationBuilder.DropIndex(
                name: "IX_Locals_IdEmploiExam",
                table: "Locals");

            migrationBuilder.DropColumn(
                name: "IdEmploiExam",
                table: "vacataires");

            migrationBuilder.DropColumn(
                name: "IdEmploiExam",
                table: "Locals");

            migrationBuilder.RenameColumn(
                name: "IdEmploiExam",
                table: "Enseignants",
                newName: "emploiExamIdEmploiExam");

            migrationBuilder.RenameIndex(
                name: "IX_Enseignants_IdEmploiExam",
                table: "Enseignants",
                newName: "IX_Enseignants_emploiExamIdEmploiExam");

            migrationBuilder.CreateTable(
                name: "EmploiExamEnseignants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmploiExam = table.Column<int>(type: "int", nullable: false),
                    IdEnseignant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploiExamEnseignants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploiExamEnseignants_EmploiExams_IdEmploiExam",
                        column: x => x.IdEmploiExam,
                        principalTable: "EmploiExams",
                        principalColumn: "IdEmploiExam",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiExamEnseignants_Enseignants_IdEnseignant",
                        column: x => x.IdEnseignant,
                        principalTable: "Enseignants",
                        principalColumn: "IdEnseignant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmploiExamLocals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmploiExam = table.Column<int>(type: "int", nullable: false),
                    IdLocal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploiExamLocals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploiExamLocals_EmploiExams_IdEmploiExam",
                        column: x => x.IdEmploiExam,
                        principalTable: "EmploiExams",
                        principalColumn: "IdEmploiExam",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiExamLocals_Locals_IdLocal",
                        column: x => x.IdLocal,
                        principalTable: "Locals",
                        principalColumn: "IdLocal",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmploiExamVacataires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmploiExam = table.Column<int>(type: "int", nullable: false),
                    IdVacataire = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploiExamVacataires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploiExamVacataires_EmploiExams_IdEmploiExam",
                        column: x => x.IdEmploiExam,
                        principalTable: "EmploiExams",
                        principalColumn: "IdEmploiExam",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiExamVacataires_vacataires_IdVacataire",
                        column: x => x.IdVacataire,
                        principalTable: "vacataires",
                        principalColumn: "IdVacataire",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExamEnseignants_IdEmploiExam",
                table: "EmploiExamEnseignants",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExamEnseignants_IdEnseignant",
                table: "EmploiExamEnseignants",
                column: "IdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExamLocals_IdEmploiExam",
                table: "EmploiExamLocals",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExamLocals_IdLocal",
                table: "EmploiExamLocals",
                column: "IdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExamVacataires_IdEmploiExam",
                table: "EmploiExamVacataires",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiExamVacataires_IdVacataire",
                table: "EmploiExamVacataires",
                column: "IdVacataire");

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_EmploiExams_emploiExamIdEmploiExam",
                table: "Enseignants",
                column: "emploiExamIdEmploiExam",
                principalTable: "EmploiExams",
                principalColumn: "IdEmploiExam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_EmploiExams_emploiExamIdEmploiExam",
                table: "Enseignants");

            migrationBuilder.DropTable(
                name: "EmploiExamEnseignants");

            migrationBuilder.DropTable(
                name: "EmploiExamLocals");

            migrationBuilder.DropTable(
                name: "EmploiExamVacataires");

            migrationBuilder.RenameColumn(
                name: "emploiExamIdEmploiExam",
                table: "Enseignants",
                newName: "IdEmploiExam");

            migrationBuilder.RenameIndex(
                name: "IX_Enseignants_emploiExamIdEmploiExam",
                table: "Enseignants",
                newName: "IX_Enseignants_IdEmploiExam");

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

            migrationBuilder.CreateIndex(
                name: "IX_vacataires_IdEmploiExam",
                table: "vacataires",
                column: "IdEmploiExam");

            migrationBuilder.CreateIndex(
                name: "IX_Locals_IdEmploiExam",
                table: "Locals",
                column: "IdEmploiExam");

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
    }
}
