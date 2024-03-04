using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class isComuncours4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isComuncours",
                table: "EmploiTemps",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isComuncours",
                table: "EmploiTemps");
        }
    }
}
