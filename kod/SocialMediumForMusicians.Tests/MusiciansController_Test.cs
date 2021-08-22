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
    }
}
