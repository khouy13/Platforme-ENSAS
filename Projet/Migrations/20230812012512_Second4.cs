using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class Second4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_Departements_IdDepartement",
                table: "Enseignants");

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartement",
                table: "Enseignants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_Departements_IdDepartement",
                table: "Enseignants",
                column: "IdDepartement",
                principalTable: "Departements",
                principalColumn: "IdDepartement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enseignants_Departements_IdDepartement",
                table: "Enseignants");

            migrationBuilder.AlterColumn<int>(
                name: "IdDepartement",
                table: "Enseignants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enseignants_Departements_IdDepartement",
                table: "Enseignants",
                column: "IdDepartement",
                principalTable: "Departements",
                principalColumn: "IdDepartement",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
