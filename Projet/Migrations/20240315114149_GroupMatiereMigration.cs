using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class GroupMatiereMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatiereGroupe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatiereGroupe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatiereGroupeMatieres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatiereId = table.Column<int>(type: "int", nullable: false),
                    GroupMatiereId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatiereGroupeMatieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatiereGroupeMatieres_MatiereGroupe_GroupMatiereId",
                        column: x => x.GroupMatiereId,
                        principalTable: "MatiereGroupe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatiereGroupeMatieres_Matieres_MatiereId",
                        column: x => x.MatiereId,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatiereGroupeMatieres_GroupMatiereId",
                table: "MatiereGroupeMatieres",
                column: "GroupMatiereId");

            migrationBuilder.CreateIndex(
                name: "IX_MatiereGroupeMatieres_MatiereId",
                table: "MatiereGroupeMatieres",
                column: "MatiereId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatiereGroupeMatieres");

            migrationBuilder.DropTable(
                name: "MatiereGroupe");
        }
    }
}
