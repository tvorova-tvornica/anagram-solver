using AnagramSolver.Data;
using AnagramSolver.Data.Entities;
using AnagramSolver.Extensions;
using AnagramSolver.HttpClients;
using AnagramSolver.HttpClients.Dto;
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
            throw new Exception($"Cannot find import page request (id:{importPageRequestId}) with 'Scheduled' status");
        }

        var wikiDataCelebritiesByPageId = await GetWikiDataCelebritiesByPageId(pageRequest);
        await UpsertCelebrities(wikiDataCelebritiesByPageId);
        pageRequest.MarkProcessed();

        await _db.SaveChangesAsync();
    }

    private async Task<Dictionary<string, WikiDataCelebritiesResponse.WikiDataCelebrity>> GetWikiDataCelebritiesByPageId(ImportWikiDataCelebritiesPageRequest pageRequest)
    {
        var occupationId = pageRequest.ImportCelebritiesRequest.WikiDataOccupationId;
        var nationalityId = pageRequest.ImportCelebritiesRequest.WikiDataNationalityId;
        var limit = pageRequest.Limit;
        var offset = pageRequest.Offset;

        return (await _httpClient.GetCelebritiesPageAsync(occupationId, nationalityId, limit, offset))!.Results!.Bindings
            .Where(x => !string.IsNullOrWhiteSpace(x.ItemLabel.Value.ToRemovedWhitespace().ToRemovedPunctuation()))
            .DistinctBy(x => x.Item.Value)
            .ToDictionary(k => k.Item.Value, v => v);
    }

    private async Task UpsertCelebrities(Dictionary<string, WikiDataCelebritiesResponse.WikiDataCelebrity> wikiDataCelebritiesByPageId)
    {
        var celebrityWikiDataPageIds = wikiDataCelebritiesByPageId.Keys;

        var existingCelebrities = await _db.Celebrities
            .Where(x => celebrityWikiDataPageIds.Contains(x.WikiDataPageId))
            .ToListAsync();

        existingCelebrities.ForEach(x =>
        {
            var wikiDataCelebrity = wikiDataCelebritiesByPageId.GetValueOrDefault(x.WikiDataPageId!);

            if (wikiDataCelebrity is null)
            {
                return;
            }

            x.Update(fullName: wikiDataCelebrity.ItemLabel.Value,
                     hrFullName: wikiDataCelebrity.HrItemLabel?.Value,
                     photoUrl: wikiDataCelebrity.Image?.Value,
                     description: wikiDataCelebrity.EnDescription?.Value ?? wikiDataCelebrity.HrDescription?.Value,
                     wikipediaUrl: wikiDataCelebrity.EnWikipedia?.Value ?? wikiDataCelebrity.HrWikipedia?.Value);
        });

        var celebritiesToInsert = wikiDataCelebritiesByPageId.Values
            .Where(x => !existingCelebrities.Select(y => y.WikiDataPageId).Contains(x.Item.Value))
            .Select(x => new Celebrity(fullName: x.ItemLabel.Value,
                                       hrFullName: x.HrItemLabel?.Value,
                                       photoUrl: x.Image?.Value,
                                       description: x.EnDescription?.Value ?? x.HrDescription?.Value,
                                       wikipediaUrl: x.EnWikipedia?.Value ?? x.HrWikipedia?.Value)
            {
                WikiDataPageId = x.Item.Value,
            });

        _db.Celebrities.AddRange(celebritiesToInsert);
    }
}
