using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialMediumForMusicians.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediumForMusicians.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var splitStringConverter = new ValueConverter<List<string>, string>(
                v => string.Join(";", v),
                v => v.Split(new[] { ';' }).ToList());
            var splitIntConverter = new ValueConverter<List<int>, string>(
                v => string.Join(";", v),
                v => v.Split(new[] { ';' }).Select(int.Parse).ToList());

            modelBuilder.Entity<User>().Property(nameof(User.FavouriteMusiciansIds))
                .HasConversion(splitIntConverter);
            modelBuilder.Entity<Musician>().Property(nameof(Musician.Instruments))
                .HasConversion(splitStringConverter);

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

            modelBuilder.Entity<Review>().HasOne(r => r.Author)
                                         .WithMany(u => u.MyReviews)
                                         .HasForeignKey(r => r.AuthorId)
                                         .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Review>().HasOne(r => r.Target)
                                         .WithMany(m => m.Reviews)
                                         .HasForeignKey(r => r.TargetId)
                                         .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Musician> Musicians { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
