using AnagramSolver.Data;
using AnagramSolver.HttpClients;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

 public class ScheduleCelebrityPagesImportJob
 {
    private readonly AnagramSolverContext _db;
    private readonly WikiDataHttpClient _httpClient;

    public ScheduleCelebrityPagesImportJob(AnagramSolverContext db, WikiDataHttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task ScheduleAsync(int importCelebritiesRequestId)
    {
        var request = await _db.ImportWikiDataCelebritiesRequests.SingleAsync(x => x.Id == importCelebritiesRequestId);

        var totalCelebrityCount = await _httpClient.GetTotalCelebrityCountAsync(request.WikiDataOccupationId, request.WikiDataNationalityId);

        request.AddPageRequests(totalCelebrityCount);

        await _db.SaveChangesAsync();
    }
 }
 