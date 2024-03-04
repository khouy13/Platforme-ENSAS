using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class migrationEmploie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semaine",
                table: "EmploiTemps");

            migrationBuilder.AddColumn<int>(
                name: "SemaineDebut",
                table: "EmploiTemps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SemaineFin",
                table: "EmploiTemps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SemaineDebut",
                table: "EmploiTemps");

            migrationBuilder.DropColumn(
                name: "SemaineFin",
                table: "EmploiTemps");

            migrationBuilder.AddColumn<string>(
                name: "Semaine",
                table: "EmploiTemps",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
