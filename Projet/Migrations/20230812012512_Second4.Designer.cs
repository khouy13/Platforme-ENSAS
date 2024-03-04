﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projet.Data;

#nullable disable

namespace Projet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230812012512_Second4")]
    partial class Second4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Projet.Areas.Admin.Models.Departement", b =>
                {
                    b.Property<int>("IdDepartement")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDepartement"));

                    b.Property<string>("NomDepartementt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdDepartement");

                    b.ToTable("Departements");
                });

            modelBuilder.Entity("Projet.Areas.Admin.Models.Enseignant", b =>
                {
                    b.Property<int>("IdEnseignant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEnseignant"));

                    b.Property<string>("GradeEnseignant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdDepartement")
                        .HasColumnType("int");

                    b.Property<string>("NomEnseignant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrenomEnseignant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialiteEnseignant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEnseignant");

                    b.HasIndex("IdDepartement");

                    b.ToTable("Enseignants");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Filiere", b =>
                {
                    b.Property<int>("IdFiliere")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFiliere"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("IdDepartement")
                        .HasColumnType("int");

                    b.Property<string>("NomFiliere")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdFiliere");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("IdDepartement");

                    b.ToTable("Filieres");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Groupe", b =>
                {
                    b.Property<int>("IdGroupe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdGroupe"));

                    b.Property<int?>("IdNiveau")
                        .HasColumnType("int");

                    b.Property<string>("NomGroup")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdGroupe");

                    b.HasIndex("IdNiveau");

                    b.ToTable("Groupes");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Matiere", b =>
                {
                    b.Property<int>("IdMatiere")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdMatiere"));

                    b.Property<int?>("IdNiveau")
                        .HasColumnType("int");

                    b.Property<string>("NomMatiere")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdMatiere");

                    b.HasIndex("IdNiveau");

                    b.ToTable("Matieres");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Niveau", b =>
                {
                    b.Property<int>("IdNiveau")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdNiveau"));

                    b.Property<int?>("IdFiliere")
                        .HasColumnType("int");

                    b.Property<string>("NomNiveau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdNiveau");

                    b.HasIndex("IdFiliere");

                    b.ToTable("Niveaus");
                });

            modelBuilder.Entity("Projet.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Projet.Models.EmploiTemps", b =>
                {
                    b.Property<int>("IdEmploi")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEmploi"));

                    b.Property<DateTime?>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EnseignantIdEnseignant")
                        .HasColumnType("int");

                    b.Property<int?>("IdEnseignant")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdGroupe")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdJour")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdLocal")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdMatiere")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdSeance")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdTypeEnseignement")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Semaine")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEmploi");

                    b.HasIndex("EnseignantIdEnseignant");

                    b.HasIndex("IdGroupe");

                    b.HasIndex("IdJour");

                    b.HasIndex("IdLocal");

                    b.HasIndex("IdMatiere");

                    b.HasIndex("IdSeance");

                    b.HasIndex("IdTypeEnseignement");

                    b.ToTable("EmploiTemps");
                });

            modelBuilder.Entity("Projet.Models.RegisterViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordConfirm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Projet.Models.test", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("tets");
                });

            modelBuilder.Entity("ProjetStage.Models.Jour", b =>
                {
                    b.Property<int>("IdJour")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdJour"));

                    b.Property<string>("NomJour")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdJour");

                    b.ToTable("Jours");
                });

            modelBuilder.Entity("ProjetStage.Models.Local", b =>
                {
                    b.Property<int>("IdLocal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLocal"));

                    b.Property<int?>("CapaciteLocal")
                        .HasColumnType("int");

                    b.Property<string>("DescriptionLocal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomLocal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLocal");

                    b.ToTable("Locals");
                });

            modelBuilder.Entity("ProjetStage.Models.Seance", b =>
                {
                    b.Property<int>("IdSeance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSeance"));

                    b.Property<string>("NomSeance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdSeance");

                    b.ToTable("Seances");
                });

            modelBuilder.Entity("ProjetStage.Models.TypeEnseignement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NomEn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TypeEnseignement");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Projet.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Projet.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Projet.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Projet.Areas.Admin.Models.Enseignant", b =>
                {
                    b.HasOne("Projet.Areas.Admin.Models.Departement", "departement")
                        .WithMany("Enseignants")
                        .HasForeignKey("IdDepartement");

                    b.Navigation("departement");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Filiere", b =>
                {
                    b.HasOne("Projet.Data.ApplicationUser", "ApplicationUser")
                        .WithMany("filiers")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Projet.Areas.Admin.Models.Departement", "Departement")
                        .WithMany("Filieres")
                        .HasForeignKey("IdDepartement");

                    b.Navigation("ApplicationUser");

                    b.Navigation("Departement");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Groupe", b =>
                {
                    b.HasOne("Projet.Areas.Responsable.Models.Niveau", "niveau")
                        .WithMany("Groupes")
                        .HasForeignKey("IdNiveau");

                    b.Navigation("niveau");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Matiere", b =>
                {
                    b.HasOne("Projet.Areas.Responsable.Models.Niveau", "niveau")
                        .WithMany("Matieres")
                        .HasForeignKey("IdNiveau");

                    b.Navigation("niveau");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Niveau", b =>
                {
                    b.HasOne("Projet.Areas.Responsable.Models.Filiere", "filiere")
                        .WithMany("Niveaus")
                        .HasForeignKey("IdFiliere");

                    b.Navigation("filiere");
                });

            modelBuilder.Entity("Projet.Models.EmploiTemps", b =>
                {
                    b.HasOne("Projet.Areas.Admin.Models.Enseignant", "Enseignant")
                        .WithMany("Emplois")
                        .HasForeignKey("EnseignantIdEnseignant");

                    b.HasOne("Projet.Areas.Responsable.Models.Groupe", "Groupe")
                        .WithMany("Emplois")
                        .HasForeignKey("IdGroupe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetStage.Models.Jour", "Jour")
                        .WithMany("Emplois")
                        .HasForeignKey("IdJour")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetStage.Models.Local", "Local")
                        .WithMany("Emplois")
                        .HasForeignKey("IdLocal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projet.Areas.Responsable.Models.Matiere", "Matiere")
                        .WithMany("Emplois")
                        .HasForeignKey("IdMatiere")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetStage.Models.Seance", "Seance")
                        .WithMany("Emplois")
                        .HasForeignKey("IdSeance")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetStage.Models.TypeEnseignement", "TypeEnseignement")
                        .WithMany("Emplois")
                        .HasForeignKey("IdTypeEnseignement")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enseignant");

                    b.Navigation("Groupe");

                    b.Navigation("Jour");

                    b.Navigation("Local");

                    b.Navigation("Matiere");

                    b.Navigation("Seance");

                    b.Navigation("TypeEnseignement");
                });

            modelBuilder.Entity("Projet.Areas.Admin.Models.Departement", b =>
                {
                    b.Navigation("Enseignants");

                    b.Navigation("Filieres");
                });

            modelBuilder.Entity("Projet.Areas.Admin.Models.Enseignant", b =>
                {
                    b.Navigation("Emplois");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Filiere", b =>
                {
                    b.Navigation("Niveaus");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Groupe", b =>
                {
                    b.Navigation("Emplois");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Matiere", b =>
                {
                    b.Navigation("Emplois");
                });

            modelBuilder.Entity("Projet.Areas.Responsable.Models.Niveau", b =>
                {
                    b.Navigation("Groupes");

                    b.Navigation("Matieres");
                });

            modelBuilder.Entity("Projet.Data.ApplicationUser", b =>
                {
                    b.Navigation("filiers");
                });

            modelBuilder.Entity("ProjetStage.Models.Jour", b =>
                {
                    b.Navigation("Emplois");
                });

            modelBuilder.Entity("ProjetStage.Models.Local", b =>
                {
                    b.Navigation("Emplois");
                });

            modelBuilder.Entity("ProjetStage.Models.Seance", b =>
                {
                    b.Navigation("Emplois");
                });

            modelBuilder.Entity("ProjetStage.Models.TypeEnseignement", b =>
                {
                    b.Navigation("Emplois");
                });
#pragma warning restore 612, 618
        }
    }
}
