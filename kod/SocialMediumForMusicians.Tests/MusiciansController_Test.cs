using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Controllers;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using Xunit;

namespace SocialMediumForMusicians.Tests
{
    public class MusiciansController_Test
    {
        [Fact]
        public async void GetMusician()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SocialMediumForMusicians").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Add(new Musician()
                {
                    Id = 11,
                    Email = "example@aaa.com",
                    Name = "Example",
                    Price = 20.03M
                });
                context.SaveChanges();
            }

            Musician existing = null;
            Musician notExisting = null;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new MusiciansController(context);
                existing = (await controller.GetMusician(11)).Value;
                notExisting = (await controller.GetMusician(0)).Value;
            }

            // Assert
            Assert.NotNull(existing);
            Assert.Null(notExisting);
        }

        [Fact]
        public async void GetMusicians()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SocialMediumForMusicians").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.AddRange(new List<Musician>()
                {
                    new Musician() 
                    {
                        Email = "example@aaa.com",
                        Name = "Example",
                        Price = 20.03M
                    },
                    new Musician()
                    {
                        Email = "example1@aaa.com",
                        Name = "Example1",
                        Price = 20.03M
                    },
                    new Musician()
                    {
                        Email = "example2@aaa.com",
                        Name = "Example2",
                        Price = 20.03M
                    },
                    new Musician()
                    {
                        Email = "example3@aaa.com",
                        Name = "Example3",
                        Price = 20.03M
                    },
                });
                context.SaveChanges();
            }

            PaginationApiResult<Musician> result;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new MusiciansController(context);
                result = (await controller.GetMusicians(pageIndex: 0, pageSize: 2)).Value;
            }

            // Assert
            Assert.Equal(2, result.Elements.Count);
            Assert.Equal(4, result.TotalCount);
            Assert.Equal(2, result.TotalPages);
        }
    }
}
