using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.IO;
using System.Web.Mvc.Razor;
using System.Xml.Linq;

using IHostingEnvironment= Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace appTrombinoscope.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            bool cdt = HttpContext.Session.GetInt32("userId") != null && HttpContext.Session.GetString("role") != null;
            bool isAdmin =  cdt && HttpContext.Session.GetString("role") == "admin";
            bool isUser = cdt && HttpContext.Session.GetString("role") == "user";
            if (isAdmin)
            {
                return View();
            }
            else if (isUser)
            {
				TempData["error"] = "Accès refusé!";
				return RedirectToAction("Index", "Home");
            }
			TempData["error"] = "Accès refusé!";
			return RedirectToAction("Login", "Account");
        }

        public AdminController(IHostingEnvironment host)
        {
            _host = host;  
        }

        private readonly IHostingEnvironment _host;

        [HttpPost,ActionName("UploadDoussierImages")]
        public async Task<IActionResult> UploadDoussierImages(List<IFormFile> imageFolder)
        {
           if (imageFolder!=null && imageFolder.Count > 0 && isImages(imageFolder))
            {
                string FolderImages = Path.Combine(_host.WebRootPath,"imageFolder");

                if (!Directory.Exists(FolderImages))
                {
                    Directory.CreateDirectory(FolderImages);
                }
                foreach (var file in imageFolder)
                {
                    string fileName = file.FileName;
                    for (int i = 0; i < fileName.Length; i++)
                    {
                        if (fileName[i] == '/')
                            
                            fileName = fileName.Substring(i + 1);
                    }
                    
                    string filename = Path.Combine(FolderImages, fileName);
                    filename = Path.ChangeExtension(filename, "png");

                    using (var stream=new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                TempData["success"] = "les images des etudiants est enregister avec success";
                return RedirectToAction("Index");
            }

            TempData["error"] = "le dossier choisi est vide ou contient des fechiers autres que des images";
            return RedirectToAction("Index");
        }

       
        [HttpPost, ActionName("UploadFechierExcel")]
        public async Task<IActionResult> UploadFechierExcel(IFormFile excelFile)
        {
            if (excelFile.Length > 0 && excelFile!=null )
            {

                if(Path.GetExtension(excelFile.FileName).ToLower()==".xlsx" ||
                   Path.GetExtension(excelFile.FileName).ToLower() == ".xls" ||
                   Path.GetExtension(excelFile.FileName).ToLower() == ".csv")
                {
                         string uploadFolder = Path.Combine(_host.WebRootPath,"folderFechierExcel");

                         if (!Directory.Exists(uploadFolder))
                         {
                            Directory.CreateDirectory(uploadFolder);
                          }
                    string filename = Path.Combine(uploadFolder, "student"+Path.GetExtension(excelFile.FileName).ToLower());
                    using (var stream = new FileStream(filename, FileMode.Create))
                    {
                        await excelFile.CopyToAsync(stream);
                    }

                    TempData["success"] = "le fechier excel est enregistrer avec success";
                    return RedirectToAction("Index");

                }
                TempData["error"] = "l'extension n'est compatible tu doit selectionner un fechier excel";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    
        private bool isImages(List<IFormFile> imageFolder)
        {
            foreach(var image in imageFolder)
            {
             if (Path.GetExtension(image.FileName).ToLower() != ".png"  &&
                    Path.GetExtension(image.FileName).ToLower() != ".jpg"  &&
                    Path.GetExtension(image.FileName).ToLower() != ".jpeg" &&
                    Path.GetExtension(image.FileName).ToLower() != ".jpe" 
                    )
                {
                    return false;
                }
            }
            return true;
        }

        public IActionResult DeleteFechierExcel()
        {
            string uploadFolder = Path.Combine(_host.WebRootPath, "folderFechierExcel");

            if (Directory.Exists(uploadFolder) && Directory.GetFiles(uploadFolder).Count() > 0)
            {
                DirectoryInfo directory = new DirectoryInfo(uploadFolder);
                foreach(var file in directory.GetFiles())
                {
                    file.Delete();
                }
                TempData["success"] = "le fechier excel est supprimer avec success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "le fechier excel est deja supprimer";
                return RedirectToAction("Index");
            }
        }


        public IActionResult DeleteImageEtudiants()
        {
            string uploadFolder = Path.Combine(_host.WebRootPath, "imageFolder");

            if (Directory.Exists(uploadFolder) && Directory.GetFiles(uploadFolder).Count()>0)
            {
                DirectoryInfo directory = new DirectoryInfo(uploadFolder);
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
                TempData["success"] = "tous les images des etudiants sont supprimer avec success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "les images des etudiants sont deja supprimer";
                return RedirectToAction("Index");
            }
        }
    }
}

