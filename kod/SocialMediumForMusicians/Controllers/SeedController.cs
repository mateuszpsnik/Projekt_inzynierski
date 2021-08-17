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

            int numOfMusiciansAdded = 0;
            int numOfUsersAdded = 0;

            // dictionary of musicians already existing in the database
            var musiciansByEmail = context.Musicians.AsNoTracking()
                                        .ToDictionary(m => m.Email,
                                            StringComparer.OrdinalIgnoreCase);

            var usersByEmail = context.Users.AsNoTracking()
                                        .ToDictionary(u => u.Email,
                                            StringComparer.OrdinalIgnoreCase);

            var musicianA = new Musician() { Email = "aaa@bbb.com", Name = "Aaa", Price = 20.00M,
                Instruments = new List<string>() { "Guitar", "Piano" }
            };
            var musicianB = new Musician() { Email = "bbb@ccc.com", Name = "Bbb", Price = 30.00M,
                Instruments = new List<string>() { "Bass Guitar" }
            };
            var musicianC = new Musician() { Email = "ccc@ddd.com", Name = "Ccc", Price = 40.00M,
                Instruments = new List<string>() { "Drums" }    
            };
            var musicians = new List<Musician>() { musicianA, musicianB, musicianC };

            foreach (var musician in musicians)
            {
                // skip if already in the database
                if (musiciansByEmail.ContainsKey(musician.Email))
                {
                    continue;
                }
                // add the musician to the DB context
                await context.Musicians.AddAsync(musician);

                musiciansByEmail.Add(musician.Email, musician);
                numOfMusiciansAdded++;
            }
            // save the musicians in the DB
            if (numOfMusiciansAdded > 0)
            {
                await context.SaveChangesAsync();
            }

            var userD = new User() { Email = "ddd@eee.com", Name = "user",
                    FavouriteMusiciansIds = new List<int>() { 1, 2 }
            };
            var users = new List<User>() { userD };

            foreach (var user in users)
            {
                // skip if already in the database
                if (usersByEmail.ContainsKey(user.Email))
                {
                    continue;
                }
                // add the user to the DB context
                await context.Users.AddAsync(user);

                usersByEmail.Add(user.Email, user);
                numOfUsersAdded++;
            }
            // save the users in the DB
            if (numOfUsersAdded > 0)
            {
                await context.SaveChangesAsync();
            }

            // retrieve Ids of musicians and user
            int mAId = musiciansByEmail[musicianA.Email].Id;
            int mBId = musiciansByEmail[musicianB.Email].Id;
            int mCId = musiciansByEmail[musicianC.Email].Id;
            int uDId = usersByEmail[userD.Email].Id;

            var meeting1 = new Meeting()
            {
                Host = musicianA,
                //HostId = mAId,
                Guest = userD,
                //GuestId = uDId,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2),
                Notes = "aaa"
            };
            var meetings = new List<Meeting>() { meeting1 };

            await context.Meetings.AddRangeAsync(meetings);

            var message1 = new Message()
            {
                Author = musicianB,
                //AuthorId = mBId,
                Recipent = musicianC,
                //RecipentId = mCId,
                Content = "Hej",
                Read = false,
                SentAt = DateTime.Now
            };
            var messages = new List<Message>() { message1 };

            await context.Messages.AddRangeAsync(messages);

            var review1 = new Review()
            {
                Author = userD,
                //AuthorId = uDId,
                Target = musicianA,
                //TargetId = mAId,
                Rate = 5,
                Description = "asdas"
            };
            var review2 = new Review()
            {
                Author = userD,
                //AuthorId = uDId,
                Target = musicianB,
                //TargetId = mBId,
                Rate = 4,
                Description = "asda"
            };
            var reviews = new List<Review>() { review1, review2 };

            await context.Reviews.AddRangeAsync(reviews);

            await context.SaveChangesAsync();

            return new JsonResult(new
            {
                num_users = numOfUsersAdded + numOfMusiciansAdded,
                num_musicians = numOfMusiciansAdded
            });
        }
    }
}
