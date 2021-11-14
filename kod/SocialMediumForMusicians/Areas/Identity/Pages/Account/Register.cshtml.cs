using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;

namespace SocialMediumForMusicians.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment environment,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Podaj email")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Podaj hasło")]
            [StringLength(100, ErrorMessage = "Hasło musi mieć minimum {2} i maksimum {1} znaków.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła się różnią.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Jesteś muzykiem?")]
            public bool IsMusician { get; set; }

            [Required(ErrorMessage = "Podaj swoje imię i nazwisko")]
            [Display(Name = "Imię i nazwisko")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Napisz krótki opis")]
            [StringLength(150, ErrorMessage = "Opis może mieć maksymalnie 150 znaków.")]
            [Display(Name = "Krótki opis")]
            public string Description { get; set; }

            [Display(Name = "Zdjęcie profilowe")]
            public IFormFile UploadFile { get; set; }

            [Display(Name = "Cena za godzinę (zł)", Prompt = "50.00")]
            [RegularExpression(@"[0-9.]*$", ErrorMessage = "Cena musi być w formacie 0.00.")]
            public decimal? Price { get; set; }

            [StringLength(1500, ErrorMessage = "Opis może mieć maksymalnie 1500 znaków.")]
            [Display(Name = "Długi opis")]
            public string LongDescription { get; set; }

            [Display(Name = "Pierwszy instrument")]
            public string FirstInstrument { get; set; }

            [Display(Name = "Drugi instrument")]
            public string SecondInstrument { get; set; }

            [Display(Name = "Czy chcesz uczyć innych gry na instrumencie?")]
            public bool TeacherSelected { get; set; }

            [Display(Name = "Czy chcesz grać wspólnie z innymi?")]
            public bool JammingSelected { get; set; }

            [Display(Name = "Czy jesteś muzykiem sesyjnym/kontraktowym?")]
            public bool SessionSelected { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var instruments = await _context.Instruments.ToListAsync();
            // no instrument by default
            instruments.Insert(0, new Instrument { Id = -1, Name = " " });
            ViewData["Instruments"] = new SelectList(instruments, "Name", "Name");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Save the profile image
            string imgPath = null;
            if (Input.UploadFile != null)
            {
                imgPath = Path.Combine("profile-img", Input.Email + "_" + 
                    Input.UploadFile.FileName);
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot",
                    imgPath); 
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.UploadFile.CopyToAsync(fileStream);
                }
            }
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                IdentityResult result;
                User user;

                if (Input.IsMusician)
                {
                    // Instruments
                    var instruments = new List<string>();
                    if (!string.IsNullOrWhiteSpace(Input.FirstInstrument))
                        instruments.Add(Input.FirstInstrument);
                    if (!string.IsNullOrWhiteSpace(Input.SecondInstrument))
                        instruments.Add(Input.SecondInstrument);

                    // Musician types
                    var types = new List<MusicianType>();
                    if (Input.TeacherSelected)
                        types.Add(MusicianType.Teacher);
                    if (Input.JammingSelected)
                        types.Add(MusicianType.Jamming);
                    if (Input.SessionSelected)
                        types.Add(MusicianType.Session);

                    var musician = new Musician
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        IsMusician = true,
                        Name = Input.Name,
                        ProfilePicFilename = imgPath,
                        Description = Input.Description,
                        Price = Input.Price ?? 50.00M,
                        LongDescription = Input.LongDescription,
                        Instruments = instruments,
                        Types = types
                    };
                    result = await _userManager.CreateAsync(musician, Input.Password);

                    // Cast to User
                    user = musician;
                }
                else
                {
                    user = new User
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        IsMusician = false,
                        Name = Input.Name,
                        ProfilePicFilename = imgPath,
                        Description = Input.Description
                    };
                    result = await _userManager.CreateAsync(user, Input.Password);
                }
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Potwierdź swój email",
                    $"Potwierdź proszę swoje konto, klikając <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>tutaj</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
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
}
