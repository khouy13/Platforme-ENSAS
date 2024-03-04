using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGradeEnseignantProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Enseignants",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_GradeId",
                table: "Enseignants",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_GradeEnseigant_GradeId",
                table: "Enseignants",
                column: "GradeId",
                principalTable: "GradeEnseigant",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_GradeEnseigant_GradeId",
                table: "Enseignants");

            migrationBuilder.DropTable(
                name: "GradeEnseigant");

            migrationBuilder.DropIndex(
                name: "IX_Enseignants_GradeId",
                table: "Enseignants");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Enseignants");
        }
    }
}
