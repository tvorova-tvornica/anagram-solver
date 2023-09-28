using AnagramSolver.Controllers.Dto;
using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

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
        var celebrity = new Celebrity(fullName: celebrityDto.FullName,
                                      photoUrl: celebrityDto.PhotoUrl,
                                      wikipediaUrl: celebrityDto.WikipediaUrl)
        {
            WikiDataPageId = celebrityDto.WikiDataPageId
        };

        _dbContext.Add(celebrity);
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet("resolve-anagram")]
    public Task<List<ResolveAnagramResult>> ResolveAnagram([FromQuery] ResolveAnagramDto anagramDto)
    {
        return _dbContext.Celebrities
            .Where(c => c.AnagramKey == anagramDto.AnagramKey || c.HrAnagramKey == anagramDto.AnagramKey)
            .Select(c => new ResolveAnagramResult
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
            .OrderByDescending(x => x.Status == ImportWikiDataCelebritiesRequestStatus.Scheduled || 
                                    x.Status == ImportWikiDataCelebritiesRequestStatus.PageRequestsScheduled)
            .ThenBy(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ImportCelebritiesRequestResult(x.Id, 
                x.WikiDataNationalityId, 
                x.WikiDataOccupationId,
                x.PageRequests.Count == 0 
                    ? x.Status == ImportWikiDataCelebritiesRequestStatus.Processed ? 100 : 0 
                    : x.PageRequests.Count(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Processed) * 100.0 / x.PageRequests.Count))
            .ToListAsync();
    }
}
