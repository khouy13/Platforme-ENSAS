using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class chef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Departements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departements_ApplicationUserId",
                table: "Departements",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departements_AspNetUsers_ApplicationUserId",
                table: "Departements",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departements_AspNetUsers_ApplicationUserId",
                table: "Departements");

            migrationBuilder.DropIndex(
                name: "IX_Departements_ApplicationUserId",
                table: "Departements");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Departements");
        }
    }
}
