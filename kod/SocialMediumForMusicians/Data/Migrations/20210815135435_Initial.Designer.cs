﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialMediumForMusicians.Data;

namespace SocialMediumForMusicians.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210815135435_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("HostId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Read")
                        .HasColumnType("bit");

                    b.Property<int>("RecipentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RecipentId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.Property<int>("TargetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("TargetId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FavouriteMusiciansIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicFilename")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Musician", b =>
                {
                    b.HasBaseType("SocialMediumForMusicians.Data.Models.User");

                    b.Property<string>("Instruments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LongDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(6,2)");

                    b.ToTable("Musicians");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Meeting", b =>
                {
                    b.HasOne("SocialMediumForMusicians.Data.Models.User", "Guest")
                        .WithMany("GuestMeetings")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SocialMediumForMusicians.Data.Models.User", "Host")
                        .WithMany("HostedMeetings")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Host");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Message", b =>
                {
                    b.HasOne("SocialMediumForMusicians.Data.Models.User", "Author")
                        .WithMany("OutMessages")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SocialMediumForMusicians.Data.Models.User", "Recipent")
                        .WithMany("InMessages")
                        .HasForeignKey("RecipentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Recipent");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Review", b =>
                {
                    b.HasOne("SocialMediumForMusicians.Data.Models.User", "Author")
                        .WithMany("MyReviews")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SocialMediumForMusicians.Data.Models.Musician", "Target")
                        .WithMany("Reviews")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Musician", b =>
                {
                    b.HasOne("SocialMediumForMusicians.Data.Models.User", null)
                        .WithOne()
                        .HasForeignKey("SocialMediumForMusicians.Data.Models.Musician", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.User", b =>
                {
                    b.Navigation("GuestMeetings");

                    b.Navigation("HostedMeetings");

                    b.Navigation("InMessages");

                    b.Navigation("MyReviews");

                    b.Navigation("OutMessages");
                });

            modelBuilder.Entity("SocialMediumForMusicians.Data.Models.Musician", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
