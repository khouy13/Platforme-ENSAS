using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class semestreSFD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomSemestre",
                table: "semestres");

            migrationBuilder.RenameColumn(
                name: "NumeroSemestre",
                table: "semestres",
                newName: "SemaineFin");

            migrationBuilder.AddColumn<int>(
                name: "SemaineDebut",
                table: "semestres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SemaineDebut",
                table: "semestres");

            migrationBuilder.RenameColumn(
                name: "SemaineFin",
                table: "semestres",
                newName: "NumeroSemestre");

            migrationBuilder.AddColumn<string>(
                name: "NomSemestre",
                table: "semestres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
