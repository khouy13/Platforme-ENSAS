using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class plusieursCordonn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filieres_ApplicationUserId",
                table: "Filieres");

            migrationBuilder.CreateIndex(
                name: "IX_Filieres_ApplicationUserId",
                table: "Filieres",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filieres_ApplicationUserId",
                table: "Filieres");

            migrationBuilder.CreateIndex(
                name: "IX_Filieres_ApplicationUserId",
                table: "Filieres",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");
        }
    }
}
