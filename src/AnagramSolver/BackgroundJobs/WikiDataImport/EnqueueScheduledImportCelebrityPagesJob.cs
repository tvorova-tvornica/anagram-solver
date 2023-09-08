using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

public class EnqueueScheduledImportCelebrityPagesJob
{
    private readonly AnagramSolverContext _db;

    public EnqueueScheduledImportCelebrityPagesJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task EnqueueAsync(int importRequestId)
    {
        var importRequest = await _db.ImportWikiDataCelebritiesRequests
            .Include(x => x.PageRequests)
            .SingleAsync(x => x.Id == importRequestId);
        
        var scheduledPageRequests = importRequest.PageRequests
            .Where(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Scheduled)
            .ToList();
        
        var delayInSeconds = 5;
        scheduledPageRequests.ForEach(x => {
            BackgroundJob.Schedule<ImportCelebritiesPageJob>(y => y.ImportAsync(x.Id), TimeSpan.FromSeconds(delayInSeconds));
            delayInSeconds += 5;
        });
    }
}
