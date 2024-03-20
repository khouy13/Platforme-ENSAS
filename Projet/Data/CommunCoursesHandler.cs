using Microsoft.EntityFrameworkCore;
using Projet.Areas.Coordonnateur.Models;
using System.Text.RegularExpressions;

namespace Projet.Data
{
    public class CommunCoursesHandler
    {
        private readonly AppDbContext _context;
        public CommunCoursesHandler(AppDbContext _context) 
        {
            this._context = _context;
        }
        public bool IsMatiereCommun(int? IdMatiere1, int? IdMatiere2)
        {
            if (IdMatiere1 != null && IdMatiere2 != null && IdMatiere1 == IdMatiere2)
            {
                return true;
            }
            var Matiere1 = _context.Matieres.Include(e => e.MatiereGroupeMatieres).Where(e => e.IdMatiere == IdMatiere1).FirstOrDefault();
            var Matiere2 = _context.Matieres.Include(e => e.MatiereGroupeMatieres).Where(e => e.IdMatiere == IdMatiere2).FirstOrDefault();
            if (Matiere1 == null || Matiere2 == null)
            {
                return false;
            }
            //Remove WhiteSpaces
            var matier1Nom = Regex.Replace(Matiere1.NomMatiere, @"\s+", "");
            var matier2Nom = Regex.Replace(Matiere2.NomMatiere, @"\s+", "");
            //Verifier Si Le nom est Le meme
            //Si Deux Matiere On Le meme Nom Ils Sont Considere Comme communes
            if (matier1Nom.Equals(matier2Nom))
            {
                return true;
            }

            //verifier si les deux matiere sont une parties du meme groupe 
            //si c'est le cas il sont considere communs
            var listOfMatiere1Groups = Matiere1?.MatiereGroupeMatieres?.Select(e => e.GroupMatiereId).ToList();
            var listOfMatiere2Groups = Matiere2?.MatiereGroupeMatieres?.Select(e => e.GroupMatiereId).ToList();

            bool isCommun = listOfMatiere1Groups?.AsQueryable().Intersect(listOfMatiere2Groups ?? Enumerable.Empty<int>()).Count() > 0;

            return isCommun;
        }

        public bool CommonCourse(EmploiTemps? e, EmploiTemps? emploi)
        {
            if (e==null || emploi==null)
            {
                return false;
            }
            bool var =
                    //e.SemaineDebut == emploi.SemaineDebut &&
                    //e.SemaineFin == emploi.SemaineFin &&
                    emploi.IdTypeEnseignement != null && e.IdTypeEnseignement == emploi.IdTypeEnseignement &&
                    emploi.IdSeance != null && e.IdSeance == emploi.IdSeance &&
                    emploi.IdJour != null && e.IdJour == emploi.IdJour &&
                    emploi.IdSemestre != null && e.IdSemestre == emploi.IdSemestre &&
                    // Car On specifie juste un , soit enseignant ou vacataire
                    ((emploi.IdEnseignant != null && e.IdEnseignant == emploi.IdEnseignant)
                    || (emploi.IdVacataire != null && e.IdVacataire == emploi.IdVacataire)) &&
                    IsMatiereCommun(emploi.IdMatiere, e.IdMatiere)
                    //&& emploi.IdMatiere != null && e.IdMatiere == emploi.IdMatiere
                    &&
                    (e.IdNiveau != emploi.IdNiveau || e.IdGroupe != emploi.IdGroupe);
            return var;
        }
    }
}
