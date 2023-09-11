using System.Globalization;
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
            .Select(x => new Celebrity(x.ItemLabel.Value) 
            {
                PhotoUrl = x.Image?.Value,
                WikipediaUrl = x.WikipediaLink?.Value,
            })
            .DistinctBy(x => x.FullName.ToLower(new CultureInfo("en-US")))
            .ToList();

        var celebrityNames = celebrities.Select(x => x.FullName.ToLower(new CultureInfo("en-US"))).ToList();

        var existingCelebrityNames = await _db.Celebrities
            .Where(x => celebrityNames.Contains(x.FullName.ToLower()))
            .Select(x => x.FullName.ToLower())
            .ToListAsync();

        var celebritiesToInsert = celebrities
            .Where(x => !existingCelebrityNames.Contains(x.FullName.ToLower(new CultureInfo("en-US"))));

        _db.Celebrities.AddRange(celebritiesToInsert);

        pageRequest.MarkProcessed();

        await _db.SaveChangesAsync();
    }
}
