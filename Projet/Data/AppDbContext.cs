using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projet.Areas.Admin.Models;
using Projet.Areas.Coordonnateur.Models;
using Projet.Areas.Responsable.Models;
using Projet.Models;

namespace Projet.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //Grade TypeLocal
        public DbSet<Grade> Grades { get; set; }
        public DbSet<TypeLocal> TypeLocals { get; set; }
        public DbSet<EmploiTemps> EmploiTemps { get; set; }
        public DbSet<MatiereNiveau> MatiereNiveaus { get; set; }
        public  DbSet<Departement> Departements { get; set; }
        public DbSet<Semestre> semestres { get; set; }
        public DbSet<Vacataire> vacataires { get; set; }
        public DbSet<EmploiExamLocal> EmploiExamLocals { get; set; }
        public DbSet<EmploiExamEnseignant> EmploiExamEnseignants { get; set; }
        public DbSet<EmploiExamVacataire> EmploiExamVacataires { get; set; }
        //Examen
        public DbSet<Examen> Examens { get; set; }
        public DbSet<EmploiExam> EmploiExams { get; set; }

        public DbSet<TypeEnseignement> TypeEnseignements { get; set; }
        public  DbSet<Enseignant> Enseignants { get; set; }

        public  DbSet<Filiere> Filieres { get; set; }

        public  DbSet<Groupe> Groupes { get; set; }

        public  DbSet<Jour> Jours { get; set; }

        public  DbSet<Local> Locals { get; set; }

        public  DbSet<Matiere> Matieres { get; set; }

        public  DbSet<Niveau> Niveaus { get; set; }
        

        public  DbSet<Seance> Seances { get; set; }
        public DbSet<RegisterViewModel> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

       



    }
}
       
    }

   