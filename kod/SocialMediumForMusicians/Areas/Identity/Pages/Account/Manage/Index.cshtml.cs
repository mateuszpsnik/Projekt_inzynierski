using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;

namespace SocialMediumForMusicians.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
 
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _environment = environment;
        }

        [Display(Name = "Nazwa użytkownika (adres email)")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool IsMusician { get; set; }

        public class InputModel
        {
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

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Name = user.Name,
                Description = user.Description
            };
        }

        private async Task LoadMusicianAsync(Musician musician)
        {
            Username = await _userManager.GetUserNameAsync(musician);

            Input = new InputModel
            {
                Name = musician.Name,
                Description = musician.Description,
                Price = musician.Price,
                LongDescription = musician.LongDescription,
                FirstInstrument = musician.Instruments[0] ?? "",
                SecondInstrument = musician.Instruments[1] ?? "",
                TeacherSelected = musician.Types != null ? 
                                    musician.Types.Contains(MusicianType.Teacher) : false,
                JammingSelected = musician.Types != null ?
                                    musician.Types.Contains(MusicianType.Jamming) : false,
                SessionSelected = musician.Types != null ?
                                    musician.Types.Contains(MusicianType.Session) : false
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie udało się załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            var instruments = await _context.Instruments.ToListAsync();
            // no instrument by default
            instruments.Insert(0, new Instrument { Id = -1, Name = " " });
            ViewData["Instruments"] = new SelectList(instruments, "Name", "Name");

            IsMusician = user.IsMusician;

            if (IsMusician)
            {
                var musician = _context.Musicians.Single(m => m.Id == user.Id);
                await LoadMusicianAsync(musician);
            }
            else
            {
                await LoadAsync(user);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie udało się załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                var instruments = await _context.Instruments.ToListAsync();
                // no instrument by default
                instruments.Insert(0, new Instrument { Id = -1, Name = " " });
                ViewData["Instruments"] = new SelectList(instruments, "Name", "Name");

                IsMusician = user.IsMusician;

                if (IsMusician)
                {
                    var musician = _context.Musicians.Single(m => m.Id == user.Id);
                    await LoadMusicianAsync(musician);
                }
                else
                {
                    await LoadAsync(user);
                }

                return Page();
            }

            // Save the profile image
            string imgPath = null;
            if (Input.UploadFile != null)
            {
                imgPath = Path.Combine("profile-img", user.Email + "_" +
                    Input.UploadFile.FileName);
                var filePath = Path.Combine(_environment.ContentRootPath, "wwwroot",
                    imgPath);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.UploadFile.CopyToAsync(fileStream);
                }
            }

            if (user.IsMusician)
            {
                var musician = _context.Musicians.Single(m => m.Id == user.Id);
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

                musician.Name = Input.Name;
                musician.Description = Input.Description;
                musician.ProfilePicFilename = imgPath ?? musician.ProfilePicFilename;
                musician.Price = Input.Price ?? 50.00M;
                musician.LongDescription = Input.LongDescription;
                musician.Instruments = instruments;
                musician.Types = types;

                await _userManager.UpdateAsync(musician);
            } 
            else
            {
                user.Name = Input.Name;
                user.Description = Input.Description;
                user.ProfilePicFilename = imgPath ?? user.ProfilePicFilename;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twój profil został zaktualizowany.";
            return RedirectToPage();
        }
    }
}
