using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.BackgroundJobs.WikiDataImport.EnqueueScheduledCelebritiesPageImportsJob;
using static AnagramSolver.BackgroundJobs.WikiDataImport.ImportCelebrityRequestsSchedulerJob;
using static AnagramSolver.BackgroundJobs.WikiDataImport.ScheduleCelebritiesPageImportsJob;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.BackgroundJobs.WikiDataImport;

public class ImportCelebrityRequestsSchedulerJob : IJob<ImportCelebrityRequestsSchedulerJobData>
{
    private readonly AnagramSolverContext _db;

    public ImportCelebrityRequestsSchedulerJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task ExecuteAsync(ImportCelebrityRequestsSchedulerJobData _)
    {
        var isAnyRequestProcessing = await _db.ImportWikiDataCelebritiesRequests
            .AnyAsync(x => x.Status != ImportWikiDataCelebritiesRequestStatus.Requested && 
                           x.Status != ImportWikiDataCelebritiesRequestStatus.Processed);

        if (isAnyRequestProcessing)
        {
            return;
        }          

        var requestedImport = await _db.ImportWikiDataCelebritiesRequests
            .Where(x => x.Status == ImportWikiDataCelebritiesRequestStatus.Requested)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync();
        
        if (requestedImport is null)
        {
            return;
        }
        
        var jobId = BackgroundJob.Schedule<IJob<ScheduleCelebritiesPageImportsJobData>>(y => y.ExecuteAsync(new ScheduleCelebritiesPageImportsJobData(requestedImport.Id)), TimeSpan.FromSeconds(5));
        BackgroundJob.ContinueJobWith<IJob<EnqueueScheduledCelebritiesPageImportsJobData>>(jobId, y => y.ExecuteAsync(new EnqueueScheduledCelebritiesPageImportsJobData(requestedImport.Id)));
        requestedImport.MarkScheduled();

        await _db.SaveChangesAsync();
    }

    public record ImportCelebrityRequestsSchedulerJobData();
}
