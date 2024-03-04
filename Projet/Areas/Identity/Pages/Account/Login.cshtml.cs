// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Projet.Data;
using Projet.Areas.Admin.Models;
using Projet.Controllers;

namespace Projet.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
     

        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

      
        public string ReturnUrl { get; set; }

      
        [TempData]
        public string ErrorMessage { get; set; }

      
        public class InputModel
        {
           
            [Required]
            [EmailAddress]
            public string Email { get; set; }

          
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

         
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
             
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) {
                    //_logger.LogInformation("User logged in.");

                var user = await _userManager.FindByEmailAsync(Input.Email);
                var roles = await _userManager.GetRolesAsync(user);
              _logger.LogInformation($"Utilisateur connecté:{user.NomComplet} Adresse e-mail : {0}, Rôles : {1}", user.Email, string.Join(", ", roles));

                    // Store user ID in the session
                    HttpContext.Session.SetString("UserId", user.Id);
                    if (user.IdEnseignant != null)
                    {
                        HttpContext.Session.SetInt32("IdEnseignant", (int)user.IdEnseignant);
                    }else if (user.IdVacataire != null)
                    {
                        HttpContext.Session.SetInt32("IdVacataire", (int)user.IdVacataire);
                    }
                  
                   

                    if (roles.Contains("Admin")) 
                {
                    // Rediriger l'utilisateur vers la page spécifique pour le rôle Admin
                    return RedirectToAction("Index", "Responsable", new { area = "Responsable" });
                }
                else if (roles.Contains("Coordonnateur"))
                {
                    // Rediriger l'utilisateur vers la page spécifique pour le rôle User
                    return RedirectToAction("Index", "Coordonnateur", new { area = "Coordonnateur" });
                }
                    
                    else if (roles.Contains("Directeur"))
                    {
                        // Rediriger l'utilisateur vers la page spécifique pour le rôle User
                        return RedirectToAction("Index", "Directeur", new { area = "Directeur" });
                    }

                    else if (roles.Contains("Enseignant"))
                    {
                       
                        // Rediriger l'utilisateur vers la page spécifique pour le rôle User
                        return RedirectToAction("Index", "Prof", new { area = "Professeur" });
                    }
                    else if (roles.Contains("Chef"))
                    {
                        return RedirectToAction("Index", "Chef", new { area ="Chef" });
                    }
                    else if (roles.Contains("Secritaire"))
                    {
                        return RedirectToAction("Index","Secritaires", new { area = "Secritaire" });
                    }

                    else
                {
                    // Si l'utilisateur n'appartient à aucun rôle spécifique, vous pouvez le rediriger vers une page par défaut
                    return LocalRedirect(returnUrl);
                }
            }
            if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
