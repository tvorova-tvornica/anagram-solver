using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

public class EnqueueScheduledCelebritiesPageImportsJob
{
    private readonly AnagramSolverContext _db;
    private const int DelayStepInSeconds = 8;

    public EnqueueScheduledCelebritiesPageImportsJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task EnqueueAsync(int importRequestId)
    {
        var importRequest = await _db.ImportWikiDataCelebritiesRequests
            .Include(x => x.ImportPageRequests)
            .SingleAsync(x => x.Id == importRequestId);
        
        var scheduledPageRequests = importRequest.ImportPageRequests
            .Where(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Scheduled)
            .ToList();
        
        var delayInSeconds = DelayStepInSeconds;
        scheduledPageRequests.ForEach(x => {
            BackgroundJob.Schedule<ImportCelebritiesPageJob>(y => y.ImportAsync(x.Id), TimeSpan.FromSeconds(delayInSeconds));
            delayInSeconds += DelayStepInSeconds;
        });
    }
}
