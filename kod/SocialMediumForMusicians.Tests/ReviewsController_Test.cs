using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Controllers;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using Xunit;

namespace SocialMediumForMusicians.Tests
{
    public class ReviewsController_Test
    {
        [Fact]
        public async void GetReviews()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SocialMediumForMusicians2").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var m1 = new Musician()
                {
                    Email = "example@aaa.com",
                    Name = "Aaa",
                    Price = 10.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming,
                        MusicianType.Session, MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };
                var m2 = new Musician()
                {
                    Email = "example1@aaa.com",
                    Name = "Bbb",
                    Price = 20.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming },
                    Instruments = new List<string>() { "Guitar", "Bass Guitar", "Drums" }
                };
                var m3 = new Musician()
                {
                    Email = "example2@aaa.com",
                    Name = "Ccc",
                    Price = 20.03M,
                    Types = new List<MusicianType>() { MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };
                var m4 = new Musician()
                {
                    Email = "example3@aaa.com",
                    Name = "Ddd",
                    Price = 40.03M
                };
                context.AddRange(new List<Musician>() { m1, m2, m3, m4 });

                var r1 = new Review()
                {
                    Author = m4,
                    Target = m1,
                    Rate = 4
                };
                var r2 = new Review()
                {
                    Author = m4,
                    Target = m1,
                    Rate = 3
                };
                var r3 = new Review()
                {
                    Author = m4,
                    Target = m2,
                    Rate = 5
                };
                var r4 = new Review()
                {
                    Author = m1,
                    Target = m2,
                    Rate = 2
                };
                context.AddRange(new List<Review>() { r1, r2, r3, r4 });

                context.SaveChanges();
            }

            List<ReviewsListDTO> result;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ReviewsController(context);

                result = (await controller.GetReviews(top: 3)).Value.ToList();
            }

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result[0].Rate);
        }
    }
}
