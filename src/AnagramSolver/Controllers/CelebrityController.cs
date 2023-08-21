using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using AnagramSolver.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.Controllers;

public class CelebrityController : ControllerBase
{
    private readonly AnagramSolverContext _dbContext;

    public CelebrityController(AnagramSolverContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task CreateCelebrity(string fullName)
    {
        var celebrity = new Celebrity(fullName);
        _dbContext.Add(celebrity);
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet]
    public async Task<List<string>> ResolveAnagram(string anagram)
    {
        var sortedAnagram = anagram.ToTrimmedSorted();
        return await _dbContext.Celebrities
            .Where(c => c.SortedName == sortedAnagram)
            .Select(c => c.FullName)
            .ToListAsync();
    }
}
