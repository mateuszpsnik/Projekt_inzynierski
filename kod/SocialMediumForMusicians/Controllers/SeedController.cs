using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediumForMusicians.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Security;
using SocialMediumForMusicians.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SocialMediumForMusicians.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public SeedController(ApplicationDbContext context, IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.context = context;
            environment = env;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> SendData()
        {
            // do not run this in production environments
            if (!environment.IsDevelopment())
            {
                throw new SecurityException("Seeding not allowed in the prod env.");
            }

            var musician0 = new Musician() { Email = "aaa@bbb.com", Name = "Adam Przykładowy", Price = 20.00M,
                Instruments = new List<string>() { "Guitar", "Piano" },
                FavouriteMusiciansIds = new List<int>() { 2, 3 },
                ProfilePicFilename = "aaa.png",
                Types = new List<MusicianType> { MusicianType.Jamming },
                IsMusician = true
            };
            var musician1 = new Musician() { Email = "bbb@ccc.com", Name = "Lorem Ipsum", Price = 30.00M,
                Instruments = new List<string>() { "Bass Guitar" },
                ProfilePicFilename = "aaa.png",
                Types = new List<MusicianType> { MusicianType.Jamming, MusicianType.Session },
                IsMusician = true
            };
            var musician2 = new Musician() { Email = "ccc@ddd.com", Name = "Adam Pierwszy", Price = 40.00M,
                Instruments = new List<string>() { "Drums" }  ,
                Types = new List<MusicianType> { MusicianType.Teacher, MusicianType.Session },
                IsMusician = true
            };
            var musician3 = new Musician() { Email = "aaa1@bbb.com", Name = "Adam Drugi", Price = 20.00M,
                Instruments = new List<string>() { "Guitar", "Piano" },
                FavouriteMusiciansIds = new List<int>() { 1, 2, 3 },
                ProfilePicFilename = "aaa.png",
                IsMusician = true
            };
            var musician4 = new Musician() { Email = "bbb1@ccc.com", Name = "Adam Trzeci", Price = 30.00M,
                Instruments = new List<string>() { "Bass Guitar" },
                IsMusician = true
            };
            var musician5 = new Musician() { Email = "ccc1@ddd.com", Name = "Adam Żyżyński", Price = 40.00M,
                Instruments = new List<string>() { "Drums" },
                IsMusician = true
            };
            var musicians = new List<Musician>() { musician0, musician1, musician2, musician3,
                    musician4, musician5};

            await context.Musicians.AddRangeAsync(musicians);

            var guitar = new Instrument
            {
                Name = "Guitar"
            };
            var piano = new Instrument
            {
                Name = "Piano"
            };
            var bass = new Instrument
            {
                Name = "Bass Guitar"
            };
            var drums = new Instrument
            {
                Name = "Drums"
            };
            var instruments = new List<Instrument> { guitar, piano, bass, drums };

            await context.Instruments.AddRangeAsync(instruments);

            var review0 = new Review
            {
                Author = musician4,
                Target = musician0,
                Rate = 5
            };
            var review1 = new Review
            {
                Author = musician4,
                Target = musician1,
                Rate = 4
            };
            var review2 = new Review
            {
                Author = musician4,
                Target = musician2,
                Rate = 3
            };
            var review3 = new Review
            {
                Author = musician4,
                Target = musician1,
                Rate = 3
            };
            var reviews = new List<Review> { review0, review1, review2, review3 };

            await context.Reviews.AddRangeAsync(reviews);

            await context.SaveChangesAsync();

            return new JsonResult(new
            {
                result = "OK"
            });
        }

        [HttpGet]
        public async Task<ActionResult> CreateSampleAuthUsers()
        {
            // default role names
            var role_RegisteredUser = "RegisteredUser";
            var role_Musician = "Musician";
            var role_Admin = "Admin";

            // create the roles, if they don't exist yet
            if (await roleManager.FindByIdAsync(role_RegisteredUser) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role_RegisteredUser));
            }
            if (await roleManager.FindByIdAsync(role_Musician) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role_Musician));
            }
            if (await roleManager.FindByIdAsync(role_Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role_Admin));
            }

            var usersCount = 0;

            var adminEmail = "admin@sample.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = adminEmail,
                    UserName = adminEmail
                };

                await userManager.CreateAsync(admin, "!admin12");
                await userManager.AddToRolesAsync(admin, 
                    new List<string> { role_RegisteredUser, role_Admin });
                admin.EmailConfirmed = true;
                admin.LockoutEnabled = false;

                usersCount++;
            }

            var musicianEmail = "musician@sample.com";
            if (await userManager.FindByEmailAsync(musicianEmail) == null)
            {
                var musician = new Musician()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = musicianEmail,
                    UserName = musicianEmail,
                    Name = "Aaa Bbb",
                    Price = 0,
                    IsMusician = true
                };

                await userManager.CreateAsync(musician, "!musician1");
                await userManager.AddToRolesAsync(musician,
                    new List<string> { role_RegisteredUser, role_Musician });
                musician.EmailConfirmed = true;
                musician.LockoutEnabled = false;

                usersCount++;
            }

            var userEmail = "user@sample.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new User()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = userEmail,
                    UserName = userEmail
                };

                await userManager.CreateAsync(user, "!user123");
                await userManager.AddToRoleAsync(user, role_RegisteredUser);
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;

                usersCount++;
            }

            if (usersCount > 0)
            {
                await context.SaveChangesAsync();
            }

            return new JsonResult(new
            {
                AddedUsersCount = usersCount
            });
        }
    }
}
