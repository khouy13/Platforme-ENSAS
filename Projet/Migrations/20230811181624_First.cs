using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordConfirm = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departements",
                columns: table => new
                {
                    IdDepartement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomDepartementt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.IdDepartement);
                });

            migrationBuilder.CreateTable(
                name: "Jours",
                columns: table => new
                {
                    IdJour = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomJour = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jours", x => x.IdJour);
                });

            migrationBuilder.CreateTable(
                name: "Locals",
                columns: table => new
                {
                    IdLocal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomLocal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapaciteLocal = table.Column<int>(type: "int", nullable: true),
                    DescriptionLocal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locals", x => x.IdLocal);
                });

            migrationBuilder.CreateTable(
                name: "Seances",
                columns: table => new
                {
                    IdSeance = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomSeance = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seances", x => x.IdSeance);
                });

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

            migrationBuilder.CreateTable(
                name: "TypeEnseignement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEnseignement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enseignants",
                columns: table => new
                {
                    IdEnseignant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrenomEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialiteEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDepartement = table.Column<int>(type: "int", nullable: false),
                    GradeEnseignant = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enseignants", x => x.IdEnseignant);
                    table.ForeignKey(
                        name: "FK_Enseignants_Departements_IdDepartement",
                        column: x => x.IdDepartement,
                        principalTable: "Departements",
                        principalColumn: "IdDepartement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filieres",
                columns: table => new
                {
                    IdFiliere = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomFiliere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDepartement = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filieres", x => x.IdFiliere);
                    table.ForeignKey(
                        name: "FK_Filieres_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Filieres_Departements_IdDepartement",
                        column: x => x.IdDepartement,
                        principalTable: "Departements",
                        principalColumn: "IdDepartement");
                });

            migrationBuilder.CreateTable(
                name: "Niveaus",
                columns: table => new
                {
                    IdNiveau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomNiveau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdFiliere = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveaus", x => x.IdNiveau);
                    table.ForeignKey(
                        name: "FK_Niveaus_Filieres_IdFiliere",
                        column: x => x.IdFiliere,
                        principalTable: "Filieres",
                        principalColumn: "IdFiliere");
                });

            migrationBuilder.CreateTable(
                name: "Groupes",
                columns: table => new
                {
                    IdGroupe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNiveau = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupes", x => x.IdGroupe);
                    table.ForeignKey(
                        name: "FK_Groupes_Niveaus_IdNiveau",
                        column: x => x.IdNiveau,
                        principalTable: "Niveaus",
                        principalColumn: "IdNiveau");
                });

            migrationBuilder.CreateTable(
                name: "Matieres",
                columns: table => new
                {
                    IdMatiere = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomMatiere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNiveau = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matieres", x => x.IdMatiere);
                    table.ForeignKey(
                        name: "FK_Matieres_Niveaus_IdNiveau",
                        column: x => x.IdNiveau,
                        principalTable: "Niveaus",
                        principalColumn: "IdNiveau");
                });

            migrationBuilder.CreateTable(
                name: "EmploiTemps",
                columns: table => new
                {
                    IdEmploi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Semaine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdJour = table.Column<int>(type: "int", nullable: false),
                    IdSeance = table.Column<int>(type: "int", nullable: false),
                    IdEnseignant = table.Column<int>(type: "int", nullable: false),
                    IdTypeEnseignement = table.Column<int>(type: "int", nullable: false),
                    IdLocal = table.Column<int>(type: "int", nullable: false),
                    IdGroupe = table.Column<int>(type: "int", nullable: false),
                    IdMatiere = table.Column<int>(type: "int", nullable: false),
                    EnseignantIdEnseignant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploiTemps", x => x.IdEmploi);
                    table.ForeignKey(
                        name: "FK_EmploiTemps_Enseignants_EnseignantIdEnseignant",
                        column: x => x.EnseignantIdEnseignant,
                        principalTable: "Enseignants",
                        principalColumn: "IdEnseignant");
                    table.ForeignKey(
                        name: "FK_EmploiTemps_Groupes_IdGroupe",
                        column: x => x.IdGroupe,
                        principalTable: "Groupes",
                        principalColumn: "IdGroupe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiTemps_Jours_IdJour",
                        column: x => x.IdJour,
                        principalTable: "Jours",
                        principalColumn: "IdJour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiTemps_Locals_IdLocal",
                        column: x => x.IdLocal,
                        principalTable: "Locals",
                        principalColumn: "IdLocal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiTemps_Matieres_IdMatiere",
                        column: x => x.IdMatiere,
                        principalTable: "Matieres",
                        principalColumn: "IdMatiere",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiTemps_Seances_IdSeance",
                        column: x => x.IdSeance,
                        principalTable: "Seances",
                        principalColumn: "IdSeance",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploiTemps_TypeEnseignement_IdTypeEnseignement",
                        column: x => x.IdTypeEnseignement,
                        principalTable: "TypeEnseignement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_EnseignantIdEnseignant",
                table: "EmploiTemps",
                column: "EnseignantIdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdGroupe",
                table: "EmploiTemps",
                column: "IdGroupe");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdJour",
                table: "EmploiTemps",
                column: "IdJour");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdLocal",
                table: "EmploiTemps",
                column: "IdLocal");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdMatiere",
                table: "EmploiTemps",
                column: "IdMatiere");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdSeance",
                table: "EmploiTemps",
                column: "IdSeance");

            migrationBuilder.CreateIndex(
                name: "IX_EmploiTemps_IdTypeEnseignement",
                table: "EmploiTemps",
                column: "IdTypeEnseignement");

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_IdDepartement",
                table: "Enseignants",
                column: "IdDepartement");

            migrationBuilder.CreateIndex(
                name: "IX_Filieres_ApplicationUserId",
                table: "Filieres",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Filieres_IdDepartement",
                table: "Filieres",
                column: "IdDepartement");

            migrationBuilder.CreateIndex(
                name: "IX_Groupes_IdNiveau",
                table: "Groupes",
                column: "IdNiveau");

            migrationBuilder.CreateIndex(
                name: "IX_Matieres_IdNiveau",
                table: "Matieres",
                column: "IdNiveau");

            migrationBuilder.CreateIndex(
                name: "IX_Niveaus_IdFiliere",
                table: "Niveaus",
                column: "IdFiliere");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmploiTemps");

            migrationBuilder.DropTable(
                name: "tets");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Enseignants");

            migrationBuilder.DropTable(
                name: "Groupes");

            migrationBuilder.DropTable(
                name: "Jours");

            migrationBuilder.DropTable(
                name: "Locals");

            migrationBuilder.DropTable(
                name: "Matieres");

            migrationBuilder.DropTable(
                name: "Seances");

            migrationBuilder.DropTable(
                name: "TypeEnseignement");

            migrationBuilder.DropTable(
                name: "Niveaus");

            migrationBuilder.DropTable(
                name: "Filieres");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Departements");
        }
    }
}
