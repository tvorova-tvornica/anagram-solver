using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

public class ImportCelebrityRequestsSchedulerJob
{
    private readonly AnagramSolverContext _db;

    public ImportCelebrityRequestsSchedulerJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task ScheduleSingleAsync()
    {
        var isAnyRequestProcessing = await _db.ImportWikiDataCelebritiesRequests
            .AnyAsync(x => x.Status != ImportWikiDataCelebritiesRequestStatus.Requested && 
                           x.Status != ImportWikiDataCelebritiesRequestStatus.Processed);

        if (isAnyRequestProcessing)
        {
            return;
        }          

        var requestedImport = await _db.ImportWikiDataCelebritiesRequests
            .FirstOrDefaultAsync(x => x.Status == ImportWikiDataCelebritiesRequestStatus.Requested);
        
        if (requestedImport is null)
        {
            return;
        }
        
        var jobId = BackgroundJob.Schedule<ScheduleCelebritiesPageImportsJob>(y => y.ScheduleAsync(requestedImport.Id), TimeSpan.FromSeconds(5));
        BackgroundJob.ContinueJobWith<EnqueueScheduledCelebritiesPageImportsJob>(jobId, y => y.EnqueueAsync(requestedImport.Id));
        requestedImport.MarkScheduled();

        await _db.SaveChangesAsync();
    }
}
