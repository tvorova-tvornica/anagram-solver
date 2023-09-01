using AnagramSolver.Controllers.Dto;
using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost("create-celebrity")]
    public async Task CreateCelebrity([FromBody] CreateCelebrityDto celebrityDto)
    {
        var celebrity = new Celebrity(celebrityDto.FullName)
        {
            PhotoUrl = celebrityDto.PhotoUrl,
            WikipediaUrl = celebrityDto.WikipediaUrl,
        };

        _dbContext.Add(celebrity);
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet("resolve-anagram")]
    public async Task<List<string>> ResolveAnagram([FromQuery] ResolveAnagramDto anagramDto)
    {
        return await _dbContext.Celebrities
            .Where(c => c.AnagramKey == anagramDto.AnagramKey)
            .Select(c => c.FullName)
            .ToListAsync();
    }

    [Authorize]
    [HttpPost("import-celebrities")]
    public async Task ImportCelebrities([FromBody] ImportCelebritiesRequestDto importCelebritiesRequestDto)
    {
        // TODO
        // create import request to be processed by hangfire
        // create request processor that polls for all requests and creates jobs for each, marking them processing scheduled
        // handle each separately -> create import page requests and its processor which does the same as first one -> mark principal as processed
        // each page job imports celebrities and marks itself as done
        // principal computes its completness based on dependants
    }
}
