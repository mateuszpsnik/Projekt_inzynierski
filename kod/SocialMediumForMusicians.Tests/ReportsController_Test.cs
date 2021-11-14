using IdentityServer4.EntityFramework.Options;
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

namespace SocialMediumForMusicians.Tests
{
    public class ReportsController_Test
    {
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
                var controller = new ReportsController(context);

                result = (await controller.GetReports(pageIndex: 0, pageSize: 2)).Value;
            }

            // Assert
            Assert.Equal("bbb", result.Elements[0].Justification);
            Assert.Equal(2, result.Elements.Count);
            Assert.Equal(3, result.TotalPages);
            Assert.Equal(5, result.TotalCount);
        }
    }
}
