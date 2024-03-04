using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migrationAssoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_EmploiExams_emploiExamIdEmploiExam",
                table: "Enseignants");

            migrationBuilder.DropIndex(
                name: "IX_Enseignants_emploiExamIdEmploiExam",
                table: "Enseignants");

            migrationBuilder.DropColumn(
                name: "emploiExamIdEmploiExam",
                table: "Enseignants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "emploiExamIdEmploiExam",
                table: "Enseignants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_emploiExamIdEmploiExam",
                table: "Enseignants",
                column: "emploiExamIdEmploiExam");

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_EmploiExams_emploiExamIdEmploiExam",
                table: "Enseignants",
                column: "emploiExamIdEmploiExam",
                principalTable: "EmploiExams",
                principalColumn: "IdEmploiExam");
        }
    }
}
