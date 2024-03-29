using AnagramSolver.Controllers.Celebrities.Dto;
using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.Controllers.Celebrities;

[ApiController]
[Route("[controller]")]
public class CelebritiesController : ControllerBase
{
    private readonly AnagramSolverContext _dbContext;

    public CelebritiesController(AnagramSolverContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [HttpPost("create-celebrity")]
    public async Task CreateCelebrity([FromBody] CreateCelebrityDto celebrityDto)
    {
        var celebrity = new Celebrity(fullName: celebrityDto.FullName,
                                      photoUrl: celebrityDto.PhotoUrl,
                                      wikipediaUrl: celebrityDto.WikipediaUrl)
        {
            WikiDataPageId = celebrityDto.WikiDataPageId
        };

        _dbContext.Add(celebrity);
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet("solve-anagram")]
    public Task<List<SolveAnagramResult>> SolveAnagram([FromQuery] SolveAnagramDto anagramDto)
    {
        return _dbContext.Celebrities
            .Where(c => c.AnagramKey == anagramDto.AnagramKey || c.HrAnagramKey == anagramDto.AnagramKey)
            .Select(c => new SolveAnagramResult
            {
                FullName = anagramDto.AnagramKey == c.AnagramKey ? c.FullName : c.HrFullName!,
                PhotoUrl = c.PhotoUrl,
                Description = c.Description,
                WikipediaUrl = c.WikipediaUrl,
            })
            .ToListAsync();
    }

    [Authorize]
    [HttpPost("request-celebrities-import")]
    public async Task RequestCelebritiesImport([FromBody] RequestCelebritiesImportDto importCelebritiesRequestDto)
    {
        var request = new ImportWikiDataCelebritiesRequest(wikiDataOccupationId: importCelebritiesRequestDto.OccupationId,
                                                           wikiDataNationalityId: importCelebritiesRequestDto.NationalityId);
        _dbContext.Add(request);
        await _dbContext.SaveChangesAsync();
    }

    [Authorize]
    [HttpGet("get-import-celebrities-requests")]
    public async Task<List<ImportCelebritiesRequestResult>> GetImportCelebritiesRequests(int page, int pageSize)
    {
        return await _dbContext.ImportWikiDataCelebritiesRequests
            .OrderByDescending(x => x.Status == ImportWikiDataCelebritiesRequestStatus.PageRequestsScheduled ||
                                    x.Status == ImportWikiDataCelebritiesRequestStatus.Scheduled)
            .ThenByDescending(x => x.Status == ImportWikiDataCelebritiesRequestStatus.Requested)
            .ThenByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.ImportPageRequests)
            .AsAsyncEnumerable()
            .Select(x => new ImportCelebritiesRequestResult(x.Id,
                x.CreatedAt,
                x.WikiDataNationalityId,
                x.WikiDataOccupationId,
                x.CalculateCompletionPercentage()))
            .ToListAsync();
    }
}
