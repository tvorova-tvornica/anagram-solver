using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using AnagramSolver.HttpClients;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

public class ImportCelebritiesPageJob
{
    private readonly AnagramSolverContext _db;
    private readonly WikiDataHttpClient _httpClient;

    public ImportCelebritiesPageJob(AnagramSolverContext db, WikiDataHttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task ImportAsync(int importPageRequestId)
    {
        var pageRequest = await _db.ImportWikiDataCelebritiesPageRequests
            .Include(x => x.ImportCelebritiesRequest)
            .Where(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Scheduled)
            .FirstOrDefaultAsync(x => x.Id == importPageRequestId);
        
        if (pageRequest == null)
        {
            return;
        }

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
        .ToList();

        var celebrityNames = celebrities.Select(x => x.FullName.ToUpper()).ToList();

        var existingCelebrityNames = await _db.Celebrities
            .Where(x => celebrityNames.Contains(x.FullName.ToUpper()))
            .Select(x => x.FullName)
            .ToListAsync();

        var celebritiesToInsert = celebrities
            .Where(x => !existingCelebrityNames.Any(name => string.Equals(name, x.FullName, StringComparison.OrdinalIgnoreCase)));

        _db.Celebrities.AddRange(celebritiesToInsert);

        pageRequest.MarkProcessed();

        await _db.SaveChangesAsync();
    }
}
