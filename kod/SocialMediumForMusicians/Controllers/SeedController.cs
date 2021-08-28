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

namespace SocialMediumForMusicians.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public SeedController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            environment = env;
        }

        [HttpGet]
        public async Task<ActionResult> SendData()
        {
            // do not run this in production environments
            if (!environment.IsDevelopment())
            {
                throw new SecurityException("Seeding not allowed in the prod env.");
            }

            var musician0 = new Musician() { Email = "aaa@bbb.com", Name = "Aaa", Price = 20.00M,
                Instruments = new List<string>() { "Guitar", "Piano" },
                FavouriteMusiciansIds = new List<int>() { 2, 3 },
                ProfilePicFilename = "aaa.png"
            };
            var musician1 = new Musician() { Email = "bbb@ccc.com", Name = "Bbb", Price = 30.00M,
                Instruments = new List<string>() { "Bass Guitar" },
                ProfilePicFilename = "aaa.png"
            };
            var musician2 = new Musician() { Email = "ccc@ddd.com", Name = "Ccc", Price = 40.00M,
                Instruments = new List<string>() { "Drums" }    
            };
            var musician3 = new Musician() { Email = "aaa1@bbb.com", Name = "Aaa", Price = 20.00M,
                Instruments = new List<string>() { "Guitar", "Piano" },
                FavouriteMusiciansIds = new List<int>() { 1, 2, 3 },
                ProfilePicFilename = "aaa.png"
            };
            var musician4 = new Musician() { Email = "bbb1@ccc.com", Name = "Bbb", Price = 30.00M,
                Instruments = new List<string>() { "Bass Guitar" }
            };
            var musician5 = new Musician() { Email = "ccc1@ddd.com", Name = "Ccc", Price = 40.00M,
                Instruments = new List<string>() { "Drums" }
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
            var reviews = new List<Review> { review0, review1, review2 };

            await context.Reviews.AddRangeAsync(reviews);

            await context.SaveChangesAsync();

            return new JsonResult(new
            {
                result = "OK"
            });
        }
    }
}
