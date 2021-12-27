using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Controllers;
using SocialMediumForMusicians.Data;
using SocialMediumForMusicians.Data.Models;
using Xunit;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;
using System;

namespace SocialMediumForMusicians.Tests
{
    public class MeetingsController_Test
    {
        [Fact]
        public async void GetMeetings()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SocialMediumForMusicians3").Options;
            var storeOptions = Options.Create(new OperationalStoreOptions());

            var m1Id = Guid.NewGuid().ToString();
            var m2Id = Guid.NewGuid().ToString();

            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var m1 = new Musician
                {
                    Id = m1Id,
                    Email = "example@aaa.com",
                    Name = "Aaa",
                    Price = 10.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming,
                        MusicianType.Session, MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };

                var m2 = new Musician
                {
                    Id = m2Id,
                    Email = "example1@aaa.com",
                    Name = "Aaa2",
                    Price = 10.03M
                };

                context.AddRange(new List<Musician> { m1, m2 });

                var meet1 = new Meeting
                {
                    Id = Guid.NewGuid(),
                    Host = m1,
                    Guest = m2,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };

                var meet2 = new Meeting
                {
                    Id = Guid.NewGuid(),
                    Host = m2,
                    Guest = m1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };

                var meet3 = new Meeting
                {
                    Id = Guid.NewGuid(),
                    Host = m2,
                    Guest = m1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };

                var meet4 = new Meeting
                {
                    Id = Guid.NewGuid(),
                    Host = m2,
                    Guest = m1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };

                var meet5 = new Meeting
                {
                    Id = Guid.NewGuid(),
                    Host = m2,
                    Guest = m1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };

                context.AddRange(new List<Meeting> { meet1, meet2, meet3, meet4, meet5 });

                context.SaveChanges();
            }

            PaginationApiResult<MeetingDTO> resultHost;
            PaginationApiResult<MeetingDTO> resultGuest;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new MeetingsController(context);

                resultHost = (await controller.GetMeetings(hostId: m1Id)).Value;
                resultGuest = (await controller.GetMeetings(guestId: m1Id, pageIndex: 0, pageSize: 2)).Value;
            }

            // Assert 
            Assert.Single(resultHost.Elements);

            Assert.Equal(4, resultGuest.TotalCount);
            Assert.Equal(2, resultGuest.TotalPages);
            Assert.Equal(2, resultGuest.Elements.Count);
        }
    }
}
