using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class MatiereCommunAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatiereCommuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainMatiereIdMatiere = table.Column<int>(type: "int", nullable: false),
                    RelatedMatiereIdMatiere = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatiereCommuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatiereCommuns_Matieres_MainMatiereIdMatiere",
                        column: x => x.MainMatiereIdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatiereCommuns_Matieres_RelatedMatiereIdMatiere",
                        column: x => x.RelatedMatiereIdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatiereCommuns_MainMatiereIdMatiere",
                table: "MatiereCommuns",
                column: "MainMatiereIdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_MatiereCommuns_RelatedMatiereIdMatiere",
                table: "MatiereCommuns",
                column: "RelatedMatiereIdMatiere");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatiereCommuns");
        }
    }
}
