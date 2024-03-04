// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Projet.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Projet.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> _logger;
        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<ForgotPasswordModel> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Tentative d'envoi d'e-mail de réinitialisation de mot de passe.");

                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    _logger.LogWarning("L'utilisateur n'existe pas ou n'est pas confirmé.");
                    // Ne pas révéler que l'utilisateur n'existe pas ou n'est pas confirmé
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                string fromName = "rajaekhouy4@gmail.com";
                string fromEmail = "rajaekhouy4@gmail.com";
                string toEmail = Input.Email;
                string subject = "Réinitialisation de mot de passe";

                // Générez le code de réinitialisation
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Créez l'URL de réinitialisation
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                // Créez le corps du message avec le lien et le code
                string messageBody = $"<html><body> Hello User! Hope You Doing Great (*_*) </body></html>";
                messageBody += $"<p>Use this code for password reset: {code}</p>";
                messageBody += $"<p>Or click the following link to reset your password: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reset Password</a></p>";

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "rajaekhouy4@gmail.com";
                string smtpPassword = "twpdkdwmlsuudncf";
                bool enableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail, fromName);
                message.Subject = subject;
                message.To.Add(toEmail);
                message.Body = messageBody;
                message.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(smtpServer);
                smtpClient.Port = smtpPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;

                try
                {
                    _logger.LogInformation("Tentative d'envoi d'e-mail...");
                    smtpClient.Send(message);
                    _logger.LogInformation("E-mail envoyé avec succès.");
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de l'envoi de l'e-mail.");
                    ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'envoi de l'e-mail.");
                    return Page();
                }
            }

            return Page();
        }

    }
}