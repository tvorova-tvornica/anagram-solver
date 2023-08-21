using AnagramSolver.Controllers.Dto;
using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.Controllers;

[ApiController]
[Route("[controller]")]
public class CelebrityController : ControllerBase
{
    private readonly AnagramSolverContext _dbContext;

    public CelebrityController(AnagramSolverContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("create-celebrity")]
    public async Task CreateCelebrity([FromBody] CreateCelebrityDto celebrityDto)
    {
        var celebrity = new Celebrity(celebrityDto.FullName);
        _dbContext.Add(celebrity);
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet("resolve-anagram")]
    public async Task<List<string>> ResolveAnagram([FromQuery] ResolveAnagramDto anagramDto)
    {
        return await _dbContext.Celebrities
            .Where(c => c.SortedName == anagramDto.SortedAnagram)
            .Select(c => c.FullName)
            .ToListAsync();
    }
}
