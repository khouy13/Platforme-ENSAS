using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class departementVactaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tets");

            migrationBuilder.AddColumn<int>(
                name: "IdDepartement",
                table: "vacataires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_vacataires_IdDepartement",
                table: "vacataires",
                column: "IdDepartement");

            migrationBuilder.AddForeignKey(
                name: "FK_vacataires_Departements_IdDepartement",
                table: "vacataires",
                column: "IdDepartement",
                principalTable: "Departements",
                principalColumn: "IdDepartement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vacataires_Departements_IdDepartement",
                table: "vacataires");

            migrationBuilder.DropIndex(
                name: "IX_vacataires_IdDepartement",
                table: "vacataires");

            migrationBuilder.DropColumn(
                name: "IdDepartement",
                table: "vacataires");

            migrationBuilder.CreateTable(
                name: "tets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tets", x => x.id);
                });
        }
    }
}
