// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Projet.Areas.Admin.Models;
using Projet.Areas.Responsable.Models;
using Projet.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace Projet.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _host;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            AppDbContext context,
            IHostingEnvironment host,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
          
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _host = host;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

       
        [BindProperty]
        public InputModel Input { get; set; }

     
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Display(Name = "FirstName")]
            public string? FirstName { get; set; }


            [Display(Name = "LastName")]
            public string? LastName { get; set; }
            public string? ImagePath { get; set; }

            // IFormFile nous a permet d obtenir des informations sur le fichier quon aploader 
            [NotMapped] // Mark this property as not mapped to the database
            public IFormFile UserFile { get; set; }

            [NotMapped]
            public int? VacataireOrEnseignant{get;set;}
        
            public string? Password { get; set; }


            public string? ConfirmPassword { get; set; }
            [Required]
            public String? Role { get; set; }
            [ValidateNever]

            public int? IdEnseignant { get; set; }
            [ForeignKey(nameof(IdEnseignant))]
            [InverseProperty("CompteEnseignat")]
            public Enseignant? Enseignant { get; set; }

            public int? IdVacataire { get; set; }
            [ForeignKey(nameof(IdVacataire))]
            [InverseProperty("CompteVacataire")]
            public Vacataire? Vacataire { get; set; }
            public IEnumerable<SelectListItem> RolesList { get; set; }
           
            [ValidateNever]
            public IEnumerable<SelectListItem> EnseignantList { get; set; }
             
            
            public IEnumerable<SelectListItem>? VacataireList { get; set; }
            public string NomComplet
            {
                get { return $"{LastName}  {FirstName}"; }
            }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input = new InputModel()
            {
                RolesList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),
                EnseignantList = _context.Enseignants
                .Select(x => new SelectListItem
                {
                    Text = x.PrenomEnseignant + " " + x.NomEnseignant,
                    Value = x.IdEnseignant.ToString()
                }),
                VacataireList = _context.vacataires
                .Select(x => new SelectListItem
                {
                    Text = x.Prenom + " " + x.Nom,
                    Value = x.IdVacataire.ToString()
                })

            };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            if((Input.IdVacataire.HasValue || Input.IdEnseignant.HasValue) && (Input.VacataireOrEnseignant==1 || Input.VacataireOrEnseignant==0))
            {

                if (Input.VacataireOrEnseignant==0)
                {
                    var user = _context.vacataires.FirstOrDefault(p => p.IdVacataire == Input.IdVacataire);
                    Input.FirstName = user.Nom;
                    Input.LastName = user.Prenom;
                    Input.Email = user.Email;
                    Input.IdVacataire = 0;
               
                }
                if(Input.VacataireOrEnseignant==1)
                {
                    var user = _context.Enseignants.FirstOrDefault(p => p.IdEnseignant == Input.IdEnseignant);
                    Input.FirstName = user.NomEnseignant;
                    Input.LastName = user.PrenomEnseignant;
                    Input.Email = user.Email;
                    Input.IdEnseignant = 0;
                }
             
                Input.RolesList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }
                );

                Input.EnseignantList = _context.Enseignants
                .Select(x => new SelectListItem
                {
                    Text = x.PrenomEnseignant + " " + x.NomEnseignant,
                    Value = x.IdEnseignant.ToString()
                });

                Input.VacataireOrEnseignant = -1;

                Input.VacataireList = _context.vacataires
                 .Select(x => new SelectListItem
                 {
                     Text = x.Prenom + " " + x.Nom,
                     Value = x.IdVacataire.ToString()
                 });
                 return Page();
            }
            else
            {

                //returnUrl ??= Url.Content("~/responsable");
                returnUrl ??= Url.Page("/Account/Register");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (ModelState.IsValid)
                {
                    var user = CreateUser();
                    string generatedPassword = string.Empty;
                    if (Input.IdVacataire.HasValue)
                    {
                        var vacataire = await _context.vacataires.FindAsync(Input.IdVacataire);
                        if (vacataire != null)
                        {
                            user.FirstName = vacataire.Prenom;  // Mettez à jour FirstName en premier
                            user.LastName = vacataire.Nom;
                            generatedPassword = $"{char.ToUpper(vacataire.Prenom[0])}{vacataire.Prenom.Substring(1).ToLower()}123@";
                        }
                    }

                    else if (Input.IdEnseignant.HasValue)
                    {
                        var enseignant = await _context.Enseignants.FindAsync(Input.IdEnseignant);
                        if (enseignant != null)
                        {
                            user.FirstName = enseignant.PrenomEnseignant;
                            user.LastName = enseignant.NomEnseignant;

                            generatedPassword = $"{char.ToUpper(enseignant.PrenomEnseignant[0])}{enseignant.PrenomEnseignant.Substring(1).ToLower()}123@";
                        }

                    }
                    user.IdVacataire = Input.IdVacataire;
                    user.IdEnseignant = Input.IdEnseignant;
                    if (!Input.IdEnseignant.HasValue && !Input.IdVacataire.HasValue)
                    {
                        user.FirstName = Input.FirstName;
                        user.LastName = Input.LastName;
                        generatedPassword = $"{char.ToUpper(Input.FirstName[0])}{Input.FirstName.Substring(1).ToLower()}123@";
                    }

                    if (Input.UserFile != null)
                    {
                        string uploadsFolder = Path.Combine(_host.WebRootPath, "asset", "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Input.UserFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Gérer la copie du fichier
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Input.UserFile.CopyToAsync(stream);
                        }

                        user.ImagePath = Path.Combine("asset", "images", uniqueFileName); // Stockez le chemin relatif dans la base de données
                    }
                    else
                    {
                        user.ImagePath = "asset/images/profilNexistePas.jpg";
                    }

                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, generatedPassword);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        await _userManager.AddToRoleAsync(user, Input.Role);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            //await _signInManager.SignInAsync(user, isPersistent: false);




                            // Rediriger vers la page cible
                            //return RedirectToPage("/Account/Login");
                            return RedirectToAction("Index", "ApplicationUsers", new { area = "Responsable" });


                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                // If we got this far, something failed, redisplay form
                return Page();
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
