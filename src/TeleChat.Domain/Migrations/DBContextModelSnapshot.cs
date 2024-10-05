﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeleChat.Domain;

#nullable disable

namespace TeleChat.Domain.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.GroupChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GroupChat", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2024, 10, 5, 20, 57, 40, 227, DateTimeKind.Utc).AddTicks(3916),
                            Guid = new Guid("8bd13829-9450-4d47-b054-2044a27eebff"),
                            Name = "Domyślna grupa"
                        });
                });

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("GroupChatId")
                        .HasColumnType("int");

                    b.Property<int>("MessageTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupChatId");

                    b.HasIndex("MessageTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Message", (string)null);
                });

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.MessageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DefaultStyle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MessageType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "PlainText"
                        },
                        new
                        {
                            Id = 2,
                            DefaultStyle = "max-width: 200px; max-height: 200px;",
                            Name = "GIF"
                        });
                });

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2024, 10, 5, 20, 57, 40, 226, DateTimeKind.Utc).AddTicks(8309),
                            Login = "demo1",
                            Name = "Konto Demo 1",
                            Password = "739136E95F37FEE4B526F9C20C3E9DA6-11FB2578105BEA2A05F32D9CA5DFD27C"
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2024, 10, 5, 20, 57, 40, 226, DateTimeKind.Utc).AddTicks(8331),
                            Login = "demo2",
                            Name = "Konto Demo 2",
                            Password = "739136E95F37FEE4B526F9C20C3E9DA6-11FB2578105BEA2A05F32D9CA5DFD27C"
                        });
                });

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.UserGroupChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("GroupChatId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupChatId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroupChat", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2024, 10, 5, 20, 57, 40, 227, DateTimeKind.Utc).AddTicks(6628),
                            GroupChatId = 1,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2024, 10, 5, 20, 57, 40, 227, DateTimeKind.Utc).AddTicks(6641),
                            GroupChatId = 1,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.Message", b =>
                {
                    b.HasOne("TeleChat.Domain.Models.Entities.GroupChat", "GroupChat")
                        .WithMany()
                        .HasForeignKey("GroupChatId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TeleChat.Domain.Models.Entities.MessageType", "MessageType")
                        .WithMany()
                        .HasForeignKey("MessageTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TeleChat.Domain.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GroupChat");

                    b.Navigation("MessageType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TeleChat.Domain.Models.Entities.UserGroupChat", b =>
                {
                    b.HasOne("TeleChat.Domain.Models.Entities.GroupChat", "GroupChat")
                        .WithMany()
                        .HasForeignKey("GroupChatId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TeleChat.Domain.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GroupChat");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
