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
    public class MessagesController_Test
    {
        [Fact]
        public async void GetMessages()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SocialMediumForMusicians3").Options;
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
                context.AddRange(new List<Musician>() { m1, m2, m3 });

                var mes1 = new Message()
                {
                    Author = m3,
                    Recipent = m1,
                    Content = "aaa",
                    SentAt = new System.DateTime(2020, 02, 02)
                };
                var mes2 = new Message()
                {
                    Author = m3,
                    Recipent = m1,
                    Content = "bbb",
                    SentAt = new System.DateTime(2020, 02, 03)
                };
                var mes3 = new Message()
                {
                    Author = m2,
                    Recipent = m1,
                    Content = "ccc",
                    SentAt = new System.DateTime(2020, 02, 04)
                };
                var mes4 = new Message()
                {
                    Author = m1,
                    Recipent = m3,
                    Content = "ddd",
                    SentAt = new System.DateTime(2020, 02, 05)
                };
                context.AddRange(new List<Message>() { mes1, mes2, mes3, mes4 });

                context.SaveChanges();
            }

            PaginationApiResult<MessageDTO> resultAllM1;
            PaginationApiResult<MessageDTO> resultThreadM1M3;

            // Act 
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new MessagesController(context);

                resultAllM1 = (await controller.GetMessages(id: "1", pageIndex: 0, pageSize: 2)).Value;
                resultThreadM1M3 = (await controller.GetMessages(id: "1", authorId: "3")).Value;
            }

            // Assert 
            Assert.Equal(3, resultAllM1.TotalCount);
            Assert.Equal(2, resultAllM1.Elements.Count);
            Assert.Equal("ccc", resultAllM1.Elements[0].Content);

            Assert.Equal(3, resultThreadM1M3.TotalCount);
            Assert.Equal(3, resultThreadM1M3.Elements.Count);
            Assert.Equal("aaa", resultThreadM1M3.Elements[0].Content);
        }
    }
}
