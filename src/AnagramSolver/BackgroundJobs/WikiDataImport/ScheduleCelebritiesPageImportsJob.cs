using AnagramSolver.Data;
using AnagramSolver.HttpClients;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.BackgroundJobs.WikiDataImport.ScheduleCelebritiesPageImportsJob;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

 public class ScheduleCelebritiesPageImportsJob : IJob<ScheduleCelebritiesPageImportsJobData>
 {
    private readonly AnagramSolverContext _db;
    private readonly WikiDataHttpClient _httpClient;
    private readonly ILogger<ScheduleCelebritiesPageImportsJob> _logger;

    public ScheduleCelebritiesPageImportsJob(
        AnagramSolverContext db, 
        WikiDataHttpClient httpClient,
        ILogger<ScheduleCelebritiesPageImportsJob> logger)
    {
        _db = db;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(ScheduleCelebritiesPageImportsJobData data)
    {
        var request = await _db.ImportWikiDataCelebritiesRequests.SingleOrDefaultAsync(x => x.Id == data.ImportCelebritiesRequestId && 
                                                                                            x.Status == ImportWikiDataCelebritiesRequestStatus.Scheduled);

        if (request is null)
        {
            throw new Exception($"Cannot find import request (requestId: {data.ImportCelebritiesRequestId}) with status 'Scheduled'");
        }

        var totalCelebrityCount = await _httpClient.GetTotalCelebrityCountAsync(request.WikiDataOccupationId, request.WikiDataNationalityId);

        if (totalCelebrityCount == 0)
        {
            _logger.LogError($@"There are no celebrities found for wikiDataOccupationId {request.WikiDataOccupationId} 
                                and wikiDataNationalityId {request.WikiDataNationalityId}");
        }

        request.AddPageRequests(totalCelebrityCount);

        await _db.SaveChangesAsync();
    }

    public record ScheduleCelebritiesPageImportsJobData(int ImportCelebritiesRequestId);
 }
 