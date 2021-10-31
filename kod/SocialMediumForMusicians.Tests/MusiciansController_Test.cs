using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

            var storeOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var m1 = new Musician()
                {
                    Id = "aaa",
                    Email = "example@aaa.com",
                    Name = "Aaa",
                    Price = 10.03M,
                    Types = new List<MusicianType>() { MusicianType.Jamming,
                        MusicianType.Session, MusicianType.Teacher },
                    Instruments = new List<string>() { "Drums" }
                };
                var m4 = new Musician()
                {
                    Id = "bbb",
                    Email = "example3@aaa.com",
                    Name = "Ddd",
                    Price = 40.03M
                };
                context.AddRange(new List<Musician>() { m1, m4 });

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
                    Rate = 4
                };
                var r3 = new Review()
                {
                    Author = m4,
                    Target = m1,
                    Rate = 5
                };
                context.AddRange(new List<Review>() { r1, r2, r3 });
                context.SaveChanges();
            }

            MusicianDTO musician;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new MusiciansController(context);
                musician = (await controller.GetMusician(id: "aaa")).Value;
            }

            // Assert
            Assert.Equal(4, musician.FullStars);
            Assert.True(musician.HalfStar);
        }

        [Fact]
        public async void GetMusicians()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SocialMediumForMusicians").Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());

            using (var context = new ApplicationDbContext(options, storeOptions))
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
                context.AddRange(new List<Review>() { r1, r2, r3 });

                context.SaveChanges();
            }

            PaginationApiResult<MusiciansListDTO> result;

            PaginationApiResult<MusiciansListDTO> resultSession;
            PaginationApiResult<MusiciansListDTO> resultJamming;
            PaginationApiResult<MusiciansListDTO> resultTeacher;
            PaginationApiResult<MusiciansListDTO> resultDrums;
            PaginationApiResult<MusiciansListDTO> resultLessThan15;
            PaginationApiResult<MusiciansListDTO> resultMoreThan15;
            PaginationApiResult<MusiciansListDTO> resultBetween15And25;
            PaginationApiResult<MusiciansListDTO> resultAvgAbove45;
            PaginationApiResult<MusiciansListDTO> resultTeacherDrumsMoreThan15;

            PaginationApiResult<MusiciansListDTO> resultAlphabetically;
            PaginationApiResult<MusiciansListDTO> resultScoreAsc;
            PaginationApiResult<MusiciansListDTO> resultScoreDsc;
            PaginationApiResult<MusiciansListDTO> resultPriceAsc;
            PaginationApiResult<MusiciansListDTO> resultPriceDsc;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new MusiciansController(context);

                result = (await controller.GetMusicians(pageIndex: 0, pageSize: 2)).Value;

                resultSession = (await controller.GetMusicians(pageIndex: 0, pageSize: 3, type: 2)).Value;
                resultJamming = (await controller.GetMusicians(type: 1)).Value;
                resultTeacher = (await controller.GetMusicians(type: 0)).Value;
                resultDrums = (await controller.GetMusicians(instrument: "Drums")).Value;
                resultLessThan15 = (await controller.GetMusicians(maxPrice: 15.01M)).Value;
                resultMoreThan15 = (await controller.GetMusicians(minPrice: 15.01M)).Value;
                resultBetween15And25 = (await controller.GetMusicians(minPrice: 15.01M,
                    maxPrice: 25.01M)).Value;
                resultAvgAbove45 = (await controller.GetMusicians(minAvgScore: 4.5)).Value;
                resultTeacherDrumsMoreThan15 = (await controller.GetMusicians(minPrice: 15,
                    instrument: "Drums", type: 0)).Value;

                resultAlphabetically = (await controller.GetMusicians(sort: 4)).Value;
                resultScoreAsc = (await controller.GetMusicians(sort: 1)).Value;
                resultScoreDsc = (await controller.GetMusicians(sort: 0)).Value;
                resultPriceAsc = (await controller.GetMusicians(sort: 2)).Value;
                resultPriceDsc = (await controller.GetMusicians(sort: 3)).Value;
            }

            // Assert

            // Pagination
            Assert.Equal(2, result.Elements.Count);
            Assert.Equal(4, result.TotalCount);
            Assert.Equal(2, result.TotalPages);

            // Filtering
            Assert.NotEmpty(resultSession.Elements);
            Assert.Equal(2, resultJamming.Elements.Count);
            Assert.Equal(2, resultTeacher.Elements.Count);
            Assert.Equal(3, resultDrums.Elements.Count);
            Assert.Single(resultLessThan15.Elements);
            Assert.Equal(3, resultMoreThan15.Elements.Count);
            Assert.Equal(2, resultBetween15And25.Elements.Count);
            Assert.Single(resultAvgAbove45.Elements);
            Assert.Single(resultTeacherDrumsMoreThan15.Elements);

            // Sorting
            Assert.Equal("Aaa", resultAlphabetically.Elements[0].Name);
            Assert.Equal(0, resultScoreAsc.Elements[0].AvgScore);
            Assert.Equal(5, resultScoreDsc.Elements[0].AvgScore);
            Assert.Equal(10.03M, resultPriceAsc.Elements[0].Price);
            Assert.Equal(40.03M, resultPriceDsc.Elements[0].Price);
        }
    }
}
