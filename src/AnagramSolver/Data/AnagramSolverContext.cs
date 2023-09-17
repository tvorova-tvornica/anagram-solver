using AnagramSolver.Data.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.Data;

public class AnagramSolverContext : DbContext
{
    public AnagramSolverContext(DbContextOptions<AnagramSolverContext> options)
        : base(options)
    {
    }

    public DbSet<Celebrity> Celebrities { get; set; }
    public DbSet<ImportWikiDataCelebritiesRequest> ImportWikiDataCelebritiesRequests { get; set; }
    public DbSet<ImportWikiDataCelebritiesPageRequest> ImportWikiDataCelebritiesPageRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Celebrity>().ToTable("Celebrities");
        
        modelBuilder.Entity<Celebrity>()
                    .Property(c => c.FullName)
                    .IsRequired();

        modelBuilder.Entity<Celebrity>()
                    .Property(c => c.AnagramKey)
                    .IsRequired();
        
        modelBuilder.Entity<Celebrity>()
                    .HasIndex(c => c.AnagramKey)
                    .HasMethod("hash");
        
        modelBuilder.Entity<Celebrity>()
                    .HasIndex(c => c.HrAnagramKey)
                    .HasMethod("hash");
        
        modelBuilder.Entity<Celebrity>()
                    .HasIndex(c => c.WikiDataPageId)
                    .IsUnique();
        
        modelBuilder.Entity<ImportWikiDataCelebritiesRequest>()
                    .Property(r => r.Status)
                    .IsConcurrencyToken();
        
        modelBuilder.Entity<ImportWikiDataCelebritiesRequest>()
                    .Property(r => r.Status)
                    .HasConversion(new EnumToStringConverter<ImportWikiDataCelebritiesRequestStatus>());
        
        modelBuilder.Entity<ImportWikiDataCelebritiesPageRequest>()
                    .Property(r => r.Status)
                    .IsConcurrencyToken();
        
        modelBuilder.Entity<ImportWikiDataCelebritiesPageRequest>()
                    .Property(r => r.Status)
                    .HasConversion(new EnumToStringConverter<ImportWikiDataCelebritiesPageRequestStatus>());
    }
}
