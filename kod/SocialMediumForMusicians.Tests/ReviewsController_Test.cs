using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Controllers;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using Xunit;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;

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
            var storeOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var m1 = new Musician()
                {
                    Id = "1",
                    Email = "example@aaa.com",
                    Name = "Aaa",
                    Price = 10.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming,
                        MusicianType.Session, MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };
                var m2 = new Musician()
                {
                    Id = "2",
                    Email = "example1@aaa.com",
                    Name = "Bbb",
                    Price = 20.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming },
                    Instruments = new List<string>() { "Guitar", "Bass Guitar", "Drums" }
                };
                var m3 = new Musician()
                {
                    Id = "3",
                    Email = "example2@aaa.com",
                    Name = "Ccc",
                    Price = 20.03M,
                    Types = new List<MusicianType>() { MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };
                var m4 = new Musician()
                {
                    Id = "4",
                    Email = "example3@aaa.com",
                    Name = "Ddd",
                    Price = 40.03M
                };
                context.AddRange(new List<Musician>() { m1, m2, m3, m4 });

                var r1 = new Review()
                {
                    Author = m4,
                    Target = m1,
                    Rate = 4,
                    SentAt = new System.DateTime(2000, 12, 31, 15, 43, 23)
                };
                var r2 = new Review()
                {
                    Author = m4,
                    Target = m1,
                    Rate = 3,
                    SentAt = new System.DateTime(2001, 12, 31, 15, 43, 23)
                };
                var r3 = new Review()
                {
                    Author = m4,
                    Target = m2,
                    Rate = 5,
                    SentAt = new System.DateTime(2002, 12, 31, 15, 43, 23)
                };
                var r4 = new Review()
                {
                    Author = m1,
                    Target = m2,
                    Rate = 2,
                    SentAt = new System.DateTime(2003, 12, 31, 15, 43, 23)
                };
                var r5 = new Review()
                {
                    Author = m2,
                    Target = m1,
                    Rate = 1,
                    SentAt = new System.DateTime(2004, 12, 31, 15, 43, 23)
                };
                context.AddRange(new List<Review>() { r1, r2, r3, r4, r5 });

                context.SaveChanges();
            }

            PaginationApiResult<ReviewsListDTO> result;
            PaginationApiResult<ReviewsListDTO> resultPagination;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new ReviewsController(context);

                result = (await controller.GetReviews(top: 3)).Value;
                resultPagination = (await controller.GetReviews(id: "1", pageIndex: 0, pageSize: 2)).Value;
            }

            // Assert

            Assert.Equal(3, result.Elements.Count);
            Assert.Equal(1, result.Elements[0].Rate);

            // ID & pagination
            Assert.Equal(2, resultPagination.Elements.Count);
            Assert.Equal(3, resultPagination.TotalCount);
            Assert.Equal(2, resultPagination.TotalPages);
        }
    }
}
