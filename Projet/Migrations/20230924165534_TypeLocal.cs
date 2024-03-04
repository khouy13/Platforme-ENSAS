using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class TypeLocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTypeLocal",
                table: "Locals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeLocals",
                columns: table => new
                {
                    IdTypeLocal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeLocals", x => x.IdTypeLocal);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locals_IdTypeLocal",
                table: "Locals",
                column: "IdTypeLocal");

            migrationBuilder.AddForeignKey(
                name: "FK_Locals_TypeLocals_IdTypeLocal",
                table: "Locals",
                column: "IdTypeLocal",
                principalTable: "TypeLocals",
                principalColumn: "IdTypeLocal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locals_TypeLocals_IdTypeLocal",
                table: "Locals");

            migrationBuilder.DropTable(
                name: "TypeLocals");

            migrationBuilder.DropIndex(
                name: "IX_Locals_IdTypeLocal",
                table: "Locals");

            migrationBuilder.DropColumn(
                name: "IdTypeLocal",
                table: "Locals");
        }
    }
}
