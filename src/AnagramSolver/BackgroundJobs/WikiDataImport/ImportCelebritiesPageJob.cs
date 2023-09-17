using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using AnagramSolver.Extensions;
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

        var celebrities = wikiDataCelebrities!.Results!.Bindings
            .Where(x => !string.IsNullOrWhiteSpace(x.ItemLabel.Value.ToRemovedWhitespace().ToRemovedPunctuation()))
            .Select(x => new Celebrity(x.ItemLabel.Value, x.HrItemLabel?.Value)
            {
                WikiDataPageId = x.Item.Value,
                PhotoUrl = x.Image?.Value,
                Description = x.EnDescription?.Value ?? x.HrDescription?.Value,
                WikipediaUrl = x.EnWikipedia?.Value ?? x.HrWikipedia?.Value,
            })
            .DistinctBy(x => x.WikiDataPageId)
            .ToList();
        
        var celebrityWikiDataPageIds = celebrities.Select(x => x.WikiDataPageId).ToList();

        var existingCelebrities = await _db.Celebrities
            .Where(x => celebrityWikiDataPageIds.Contains(x.WikiDataPageId))
            .ToListAsync();
        
        var celebritiesToRemove = existingCelebrities.Where(x => x.OverrideOnNextWikiDataImport);
        
        _db.Celebrities.RemoveRange(celebritiesToRemove);
        
        var celebritiesToInsert = celebrities.Where(x => !existingCelebrities.Where(y => !y.OverrideOnNextWikiDataImport)
                                                                             .Select(y => y.WikiDataPageId)
                                                                             .Contains(x.WikiDataPageId));

        _db.Celebrities.AddRange(celebritiesToInsert);
        
        pageRequest.MarkProcessed();
        await _db.SaveChangesAsync();
    }
}
