using AnagramSolver.Data;
using AnagramSolver.HttpClients;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

 public class ScheduleCelebrityPageImportsJob
 {
    private readonly AnagramSolverContext _db;
    private readonly WikiDataHttpClient _httpClient;

    public ScheduleCelebrityPageImportsJob(AnagramSolverContext db, WikiDataHttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task ScheduleAsync(int importCelebritiesRequestId)
    {
        var request = await _db.ImportWikiDataCelebritiesRequests.SingleOrDefaultAsync(x => x.Id == importCelebritiesRequestId && 
                                                                                            x.Status == ImportWikiDataCelebritiesRequestStatus.Scheduled);

        if (request is null)
        {
            throw new Exception($"Cannot find import request (requestId: {importCelebritiesRequestId}) with status 'Scheduled'");
        }

        var totalCelebrityCount = await _httpClient.GetTotalCelebrityCountAsync(request.WikiDataOccupationId, request.WikiDataNationalityId);

        request.AddPageRequests(totalCelebrityCount);

        await _db.SaveChangesAsync();
    }
 }
 