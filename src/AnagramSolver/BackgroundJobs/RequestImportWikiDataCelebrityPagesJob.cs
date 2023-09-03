using AnagramSolver.Data;
using AnagramSolver.HttpClients;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BackgroundJobs;

 public class RequestImportWikiDataCelebrityPagesJob
 {
    private readonly AnagramSolverContext _db;
    private readonly WikiDataHttpClient _httpClient;

    public RequestImportWikiDataCelebrityPagesJob(AnagramSolverContext db, WikiDataHttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task InitializeAsync(int importCelebritiesRequestId, string occupationId, string? nationalityId)
    {
        var totalCelebrityCount = await _httpClient.GetTotalCelebrityCountAsync(occupationId, nationalityId);

        if (totalCelebrityCount == 0)
        {
            return;
        }

        var request = await _db.ImportWikiDataCelebritiesRequests.SingleAsync(x => x.Id == importCelebritiesRequestId);
        request.AddPageRequests(totalCelebrityCount);

        await _db.SaveChangesAsync();
    }
 }
 