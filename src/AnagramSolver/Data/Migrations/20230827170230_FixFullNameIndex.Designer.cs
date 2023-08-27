﻿// <auto-generated />
using AnagramSolver.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    [DbContext(typeof(AnagramSolverContext))]
    [Migration("20230827170230_FixFullNameIndex")]
    partial class FixFullNameIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AnagramSolver.Data.Entities.Celebrity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AnagramKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnagramKey");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("AnagramKey"), "hash");

                    b.HasIndex("FullName")
                        .IsUnique();

                    b.ToTable("Celebrities", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
