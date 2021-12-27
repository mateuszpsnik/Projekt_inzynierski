using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialMediumForMusicians.Controllers;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace SocialMediumForMusicians.Tests
{
    public class ReportsController_Test
    {
        // Based on code from: https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
        public static Mock<UserManager<TUser>> MockUserManager<TUser>()
            where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var manager = new Mock<UserManager<TUser>>(store.Object,
                null, null, null, null, null, null, null, null);

            var user = new Mock<TUser>();
            manager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
               .ReturnsAsync(user.Object);
            manager.Setup(x => x.IsInRoleAsync(It.IsAny<TUser>(), It.IsAny<string>()))
               .ReturnsAsync(true);

            return manager;
        }

        [Fact]
        public async void GetReports()
        {
            // Arrange 
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("SocialMediumForMusicians4").Options;
            var storeOptions = Options.Create(new OperationalStoreOptions());

            var id = Guid.NewGuid().ToString();

            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var m = new Musician
                {
                    Id = id,
                    Email = "example@aaa.com",
                    Name = "Aaa",
                    Price = 10.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming,
                        MusicianType.Session, MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };

                context.Add(m);

                var r1 = new Report
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    Justification = "aaa",
                    SentAt = DateTime.Now
                };
                var r2 = new Report
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    Justification = "aaa",
                    SentAt = DateTime.Now
                };
                var r3 = new Report
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    Justification = "aaa",
                    SentAt = DateTime.Now
                };
                var r4 = new Report
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    Justification = "aaa",
                    SentAt = DateTime.Now
                };
                var r5 = new Report
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    Justification = "bbb",
                    SentAt = DateTime.Now
                };

                context.AddRange(new List<Report> { r1, r2, r3, r4, r5 });

                context.SaveChanges();
            }

            PaginationApiResult<ReportDTO> result;

            // Act 
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var userManager = MockUserManager<User>();
                var controller = new ReportsController(context, userManager.Object);

                result = (await controller.GetReports(userId: "", pageIndex: 0, pageSize: 2)).Value;
            }

            // Assert
            Assert.Equal("bbb", result.Elements[0].Justification);
            Assert.Equal(2, result.Elements.Count);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.TotalCount);
        }
    }
}
