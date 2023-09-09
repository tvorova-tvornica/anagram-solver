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
    public async Task<List<ResolveAnagramResult>> ResolveAnagram([FromQuery] ResolveAnagramDto anagramDto)
    {
        return await _dbContext.Celebrities
            .Where(c => c.AnagramKey == anagramDto.AnagramKey)
            .Select(c => new ResolveAnagramResult
            {
                FullName = c.FullName,
                PhotoUrl = c.PhotoUrl,
                WikipediaUrl = c.WikipediaUrl,
            })
            .ToListAsync();
    }

    [Authorize]
    [HttpPost("import-celebrities")]
    public async Task ImportCelebrities([FromBody] ImportCelebritiesRequestDto importCelebritiesRequestDto)
    {
        var request = new ImportWikiDataCelebritiesRequest(wikiDataOccupationId: importCelebritiesRequestDto.OccupationId,
                                                           wikiDataNationalityId: importCelebritiesRequestDto.NationalityId);
        _dbContext.Add(request);
        await _dbContext.SaveChangesAsync();
    }
}
