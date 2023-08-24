using AnagramSolver.Data.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.Data;

public class AnagramSolverContext : DbContext
{
    public AnagramSolverContext(DbContextOptions<AnagramSolverContext> options)
        : base(options)
    {
    }

    public DbSet<Celebrity> Celebrities { get; set; }

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
                    .HasIndex(c => c.FullName)
                    .IsUnique();

        modelBuilder.Entity<Celebrity>()
                    .Property(c => c.AnagramKey)
                    .IsRequired();
        
        modelBuilder.Entity<Celebrity>()
                    .HasIndex(c => c.AnagramKey)
                    .HasMethod("hash");
    }
}
