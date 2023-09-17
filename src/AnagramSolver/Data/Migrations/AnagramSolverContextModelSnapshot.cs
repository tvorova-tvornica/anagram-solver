﻿// <auto-generated />
using AnagramSolver.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnagramSolver.Data.Migrations
{
    [DbContext(typeof(AnagramSolverContext))]
    partial class AnagramSolverContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HrAnagramKey")
                        .HasColumnType("text");

                    b.Property<string>("HrFullName")
                        .HasColumnType("text");

                    b.Property<bool>("OverrideOnNextWikiDataImport")
                        .HasColumnType("boolean");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("WikiDataPageId")
                        .HasColumnType("text");

                    b.Property<string>("WikipediaUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnagramKey");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("AnagramKey"), "hash");

                    b.HasIndex("WikiDataPageId")
                        .IsUnique();

                    b.ToTable("Celebrities", (string)null);
                });

            modelBuilder.Entity("AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ImportCelebritiesRequestId")
                        .HasColumnType("integer");

                    b.Property<int>("Limit")
                        .HasColumnType("integer");

                    b.Property<int>("Offset")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ImportCelebritiesRequestId");

                    b.ToTable("ImportWikiDataCelebritiesPageRequests");
                });

            modelBuilder.Entity("AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WikiDataNationalityId")
                        .HasColumnType("text");

                    b.Property<string>("WikiDataOccupationId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ImportWikiDataCelebritiesRequests");
                });

            modelBuilder.Entity("AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest", b =>
                {
                    b.HasOne("AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest", "ImportCelebritiesRequest")
                        .WithMany("PageRequests")
                        .HasForeignKey("ImportCelebritiesRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImportCelebritiesRequest");
                });

            modelBuilder.Entity("AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest", b =>
                {
                    b.Navigation("PageRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
