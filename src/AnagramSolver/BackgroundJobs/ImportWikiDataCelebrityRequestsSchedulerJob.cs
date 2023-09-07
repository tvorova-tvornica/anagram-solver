using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.BackgroundJobs;

public class ImportWikiDataCelebrityRequestsSchedulerJob
{
    private readonly AnagramSolverContext _db;

    public ImportWikiDataCelebrityRequestsSchedulerJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task ScheduleAsync()
    {
        var requestedImports = await _db.ImportWikiDataCelebritiesRequests
            .Where(x => x.Status == ImportWikiDataCelebritiesRequestStatus.Requested)
            .ToListAsync();
        
        requestedImports.ForEach(x => {
            var jobId = BackgroundJob.Enqueue<ScheduleWikiDataCelebrityPagesImportJob>(y => y.RequestAsync(x.Id));
            BackgroundJob.ContinueJobWith<ProcessScheduledImportWikiDataCelebrityPagesJob>(jobId, y => y.ProcessAsync(x.Id));
            x.MarkScheduled();
        });

        await _db.SaveChangesAsync();
    }
}
