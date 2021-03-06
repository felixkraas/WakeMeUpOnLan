﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WakeMeUpOnLan.Services;

namespace WakeMeUpOnLan.Migrations
{
    [DbContext(typeof(WolContext))]
    partial class WolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("ApiUserWolTarget", b =>
                {
                    b.Property<Guid>("AllowedTargetsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ApiUsersId")
                        .HasColumnType("TEXT");

                    b.HasKey("AllowedTargetsId", "ApiUsersId");

                    b.HasIndex("ApiUsersId");

                    b.ToTable("ApiUserWolTarget");
                });

            modelBuilder.Entity("WakeMeUpOnLan.Core.Models.ApiUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ApiKey")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ApiUsers");
                });

            modelBuilder.Entity("WakeMeUpOnLan.Core.Models.WolTarget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetMacAddress")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WolTargets");
                });

            modelBuilder.Entity("ApiUserWolTarget", b =>
                {
                    b.HasOne("WakeMeUpOnLan.Core.Models.WolTarget", null)
                        .WithMany()
                        .HasForeignKey("AllowedTargetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WakeMeUpOnLan.Core.Models.ApiUser", null)
                        .WithMany()
                        .HasForeignKey("ApiUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
