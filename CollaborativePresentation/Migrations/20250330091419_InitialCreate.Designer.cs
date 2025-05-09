﻿// <auto-generated />
using System;
using CollaborativePresentation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CollaborativePresentation.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250330091419_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CollaborativePresentation.Models.Presentation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Presentations");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.Slide", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("PresentationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PresentationId");

                    b.ToTable("Slides");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.TextElement", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("PositionX")
                        .HasColumnType("int");

                    b.Property<int>("PositionY")
                        .HasColumnType("int");

                    b.Property<string>("SlideId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SlideId");

                    b.ToTable("TextElements");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.UserConnection", b =>
                {
                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsEditor")
                        .HasColumnType("bit");

                    b.Property<string>("PresentationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ConnectionId");

                    b.HasIndex("PresentationId", "UserName")
                        .IsUnique();

                    b.ToTable("UserConnections");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.Slide", b =>
                {
                    b.HasOne("CollaborativePresentation.Models.Presentation", "Presentation")
                        .WithMany("Slides")
                        .HasForeignKey("PresentationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Presentation");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.TextElement", b =>
                {
                    b.HasOne("CollaborativePresentation.Models.Slide", "Slide")
                        .WithMany("TextElements")
                        .HasForeignKey("SlideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slide");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.Presentation", b =>
                {
                    b.Navigation("Slides");
                });

            modelBuilder.Entity("CollaborativePresentation.Models.Slide", b =>
                {
                    b.Navigation("TextElements");
                });
#pragma warning restore 612, 618
        }
    }
}
