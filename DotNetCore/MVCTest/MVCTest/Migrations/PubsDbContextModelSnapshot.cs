﻿// <auto-generated />
using MVCTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace MVCTest.Migrations
{
    [DbContext(typeof(PubsDbContext))]
    partial class PubsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MVCTest.Models.Author", b =>
                {
                    b.Property<string>("quote")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("author")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("quote");

                    b.ToTable("AUTHOR");
                });
#pragma warning restore 612, 618
        }
    }
}
