using AnagramSolver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.Data;

public class AnagramSolverContext : DbContext
{
    public required DbSet<Celebrity> Celebrities { get; set; }
}