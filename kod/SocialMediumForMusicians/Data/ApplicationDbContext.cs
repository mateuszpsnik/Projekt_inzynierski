using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using SocialMediumForMusicians.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace SocialMediumForMusicians.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var splitStringConverter = new ValueConverter<List<string>, string>(
                v => string.Join(";", v),
                v => v.Split(new[] { ';' }).ToList());
            var splitEnumConverter = new ValueConverter<List<MusicianType>, string>(
                v => string.Join(";", v.Select(e => e.ToString("D"))),
                v => v.Split(new[] { ';' }).Select(e => Enum.Parse(typeof(MusicianType), e))
                      .Cast<MusicianType>().ToList());

            var stringComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            var enumComparer = new ValueComparer<List<MusicianType>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            modelBuilder.Entity<User>().Property(nameof(User.FavouriteMusiciansIds))
                .HasConversion(splitStringConverter, stringComparer);
            modelBuilder.Entity<Musician>().Property(nameof(Musician.Instruments))
                .HasConversion(splitStringConverter, stringComparer);
            modelBuilder.Entity<Musician>().Property(nameof(Musician.Types))
                .HasConversion(splitEnumConverter, enumComparer);

            modelBuilder.Entity<User>().HasIndex(u => u.Email)
                                       .IsUnique();

            modelBuilder.Entity<Meeting>().HasOne(m => m.Host)
                                          .WithMany(u => u.HostedMeetings)
                                          .HasForeignKey(m => m.HostId)
                                          .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Meeting>().HasOne(m => m.Guest)
                                          .WithMany(u => u.GuestMeetings)
                                          .HasForeignKey(m => m.GuestId)
                                          .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>().HasOne(m => m.Recipent)
                                          .WithMany(u => u.InMessages)
                                          .HasForeignKey(m => m.RecipentId)
                                          .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Message>().HasOne(m => m.Author)
                                          .WithMany(u => u.OutMessages)
                                          .HasForeignKey(m => m.AuthorId)
                                          .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmailMessage>().HasOne(mes => mes.Recipent)
                                               .WithMany(mus => mus.EmailMessages)
                                               .HasForeignKey(mes => mes.RecipentId)
                                               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Review>().HasOne(r => r.Author)
                                         .WithMany(u => u.MyReviews)
                                         .HasForeignKey(r => r.AuthorId)
                                         .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Review>().HasOne(r => r.Target)
                                         .WithMany(m => m.Reviews)
                                         .HasForeignKey(r => r.TargetId)
                                         .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public override DbSet<User> Users { get; set; }
        public DbSet<EmailMessage> EmailMessage { get; set; }
    }
}
