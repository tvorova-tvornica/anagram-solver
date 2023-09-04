using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using AnagramSolver.HttpClients;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BackgroundJobs;

public class ImportWikiDataCelebritiesPageJob
{
    private readonly AnagramSolverContext _db;
    private readonly WikiDataHttpClient _httpClient;

    public ImportWikiDataCelebritiesPageJob(AnagramSolverContext db, WikiDataHttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task ImportAsync(int importPageRequestId)
    {
        var pageRequest = await _db.ImportWikiDataCelebritiesPageRequests
            .Include(x => x.ImportCelebritiesRequest)
            .SingleAsync(x => x.Id == importPageRequestId);

        var occupationId = pageRequest.ImportCelebritiesRequest.WikiDataOccupationId;
        var nationalityId = pageRequest.ImportCelebritiesRequest.WikiDataNationalityId;
        var limit = pageRequest.Limit;
        var offset = pageRequest.Offset;

        var wikiDataCelebrities = await _httpClient.GetCelebritiesPageAsync(occupationId, nationalityId, limit, offset);

        var celebrities = wikiDataCelebrities!.Results!.Bindings.Select(x => new Celebrity(x.ItemLabel.Value)
        {
            PhotoUrl = x.Image?.Value,
            WikipediaUrl = x.WikipediaLink?.Value,
        })
        .DistinctBy(x => x.FullName.ToLower())
        .ToList();

        var celebrityNames = celebrities.Select(x => x.FullName.ToLower()).ToList();

        var existingCelebrityNames = await _db.Celebrities
            .Where(x => celebrityNames.Contains(x.FullName.ToLower()))
            .Select(x => x.FullName.ToLower())
            .ToListAsync();

        _db.Celebrities.AddRange(celebrities.Where(x => !existingCelebrityNames.Contains(x.FullName.ToLower())));

        pageRequest.MarkProcessed();

        await _db.SaveChangesAsync();
    }
}
