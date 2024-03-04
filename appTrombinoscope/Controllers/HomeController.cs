using appTrombinoscope.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Rotativa.AspNetCore;
using System.Diagnostics;
using System.Globalization;

namespace appTrombinoscope.Controllers
{
    public class HomeController : Controller
    {

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<string> niveaux = getAllNiveau();
            ViewBag.NiveauList = niveaux;
            return View();
        }

        [HttpPost, ActionName("CreatePdf")]
         public IActionResult CreatePdf(Niveau niveau)
        {
            var data =getEtudiantOfNiveau(niveau.Name);
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            niveau.Name = niveau.Name.Replace("é", "e");
            niveau.Name = niveau.Name.Replace("è", "e");
            niveau.Name = textInfo.ToTitleCase(niveau.Name);
            niveau.Name = niveau.Name.Replace(" ", "");


            return new ViewAsPdf("Etudiants", data)
            {
                FileName =niveau.Name+".pdf"
            };
        }

        public IActionResult Etudiants()
        {
            var niveaux = getAllNiveau();
            var data = getEtudiantOfNiveau(niveaux.FirstOrDefault());
            return View(data);
        
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<string> getAllNiveau()
        {
            string cheminFichier = Path.Combine(_hostingEnvironment.WebRootPath, "folderFechierExcel/student.csv");

            if (Path.Exists(cheminFichier))
            {
                int colonneIndex = 3;
                var niveaux = new List<string>() { "choisi le niveau"};

                using (TextFieldParser parser = new TextFieldParser(cheminFichier))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] ligne = parser.ReadFields();

                        if (ligne.Length > colonneIndex)
                        {
                            string valeurDeLaColonne = ligne[colonneIndex];
                            niveaux.Add(valeurDeLaColonne);
                        }
                    }
                }
                niveaux.RemoveAt(1);
                return niveaux.Distinct().ToList();
            }
            return new List<string>();
        }

        public Dictionary<string, List<string[]>> getEtudiantOfNiveau(string niveau)
        {
            string cheminFichier = Path.Combine(_hostingEnvironment.WebRootPath, "folderFechierExcel/student.csv");

            Dictionary<string, List<string[]>> data = new Dictionary<string, List<string[]>>();

            data.Add(niveau, new List<string[]>());

            using (TextFieldParser parser = new TextFieldParser(cheminFichier))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] ligne = parser.ReadFields();
                    if(niveau.Equals(ligne[3]))
                        data[niveau].Add(ligne);
                }
            }
            return data;
        }
    }
}