using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.BackgroundJobs.WikiDataImport.EnqueueScheduledCelebritiesPageImportsJob;
using static AnagramSolver.BackgroundJobs.WikiDataImport.ImportCelebritiesPageJob;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

public class EnqueueScheduledCelebritiesPageImportsJob : IJob<EnqueueScheduledCelebritiesPageImportsJobData>
{
    private readonly AnagramSolverContext _db;
    private const int DelayStepInSeconds = 8;

    public EnqueueScheduledCelebritiesPageImportsJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task ExecuteAsync(EnqueueScheduledCelebritiesPageImportsJobData data)
    {
        var importRequest = await _db.ImportWikiDataCelebritiesRequests
            .Include(x => x.ImportPageRequests)
            .SingleAsync(x => x.Id == data.ImportRequestId);
        
        var scheduledPageRequests = importRequest.ImportPageRequests
            .Where(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Scheduled)
            .ToList();
        
        var delayInSeconds = DelayStepInSeconds;
        scheduledPageRequests.ForEach(x => {
            BackgroundJob.Schedule<IJob<ImportCelebritiesPageJobData>>(y => y.ExecuteAsync(new ImportCelebritiesPageJobData(x.Id)), TimeSpan.FromSeconds(delayInSeconds));
            delayInSeconds += DelayStepInSeconds;
        });
    }

    public record EnqueueScheduledCelebritiesPageImportsJobData(int ImportRequestId);
}
