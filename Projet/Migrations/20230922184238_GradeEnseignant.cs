using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class GradeEnseignant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_GradeEnseigant_GradeId",
                table: "Enseignants");

            migrationBuilder.DropTable(
                name: "GradeEnseigant");

            migrationBuilder.DropColumn(
                name: "GradeEnseignant",
                table: "Enseignants");

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "vacataires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeNomComplet = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vacataires_GradeId",
                table: "vacataires",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_Grades_GradeId",
                table: "Enseignants",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_vacataires_Grades_GradeId",
                table: "vacataires",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_Grades_GradeId",
                table: "Enseignants");

            migrationBuilder.DropForeignKey(
                name: "FK_vacataires_Grades_GradeId",
                table: "vacataires");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_vacataires_GradeId",
                table: "vacataires");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "vacataires");

            migrationBuilder.AddColumn<string>(
                name: "GradeEnseignant",
                table: "Enseignants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GradeEnseigant",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeNomComplet = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeEnseigant", x => x.GradeId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_GradeEnseigant_GradeId",
                table: "Enseignants",
                column: "GradeId",
                principalTable: "GradeEnseigant",
                principalColumn: "GradeId");
        }
    }
}
