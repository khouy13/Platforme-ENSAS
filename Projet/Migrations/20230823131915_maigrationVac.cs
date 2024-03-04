using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class maigrationVac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdVacataire",
                table: "EmploiTemps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vacataires",
                columns: table => new
                {
                    IdVacataire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specialité = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vacataires", x => x.IdVacataire);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdVacataire",
                table: "EmploiTemps",
                column: "IdVacataire");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploiTemps_vacataires_IdVacataire",
                table: "EmploiTemps",
                column: "IdVacataire",
                principalTable: "vacataires",
                principalColumn: "IdVacataire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploiTemps_vacataires_IdVacataire",
                table: "EmploiTemps");

            migrationBuilder.DropTable(
                name: "vacataires");

            migrationBuilder.DropIndex(
                name: "IX_EmploiTemps_IdVacataire",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "IdVacataire",
                table: "EmploiTemps");
        }
    }
}
