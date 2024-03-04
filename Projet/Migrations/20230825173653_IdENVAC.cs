using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class IdENVAC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEnseignant",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdVacataire",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdEnseignant",
                table: "AspNetUsers",
                column: "IdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdVacataire",
                table: "AspNetUsers",
                column: "IdVacataire");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Enseignants_IdEnseignant",
                table: "AspNetUsers",
                column: "IdEnseignant",
                principalTable: "Enseignants",
                principalColumn: "IdEnseignant");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_vacataires_IdVacataire",
                table: "AspNetUsers",
                column: "IdVacataire",
                principalTable: "vacataires",
                principalColumn: "IdVacataire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Enseignants_IdEnseignant",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_vacataires_IdVacataire",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdEnseignant",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdVacataire",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdEnseignant",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdVacataire",
                table: "AspNetUsers");
        }
    }
}
