using AnagramSolver.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;

namespace AnagramSolver.BackgroundJobs;

public class ImportWikiDataCelebrityPagesSchedulerJob
{
    private readonly AnagramSolverContext _db;

    public ImportWikiDataCelebrityPagesSchedulerJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task ScheduleAsync()
    {
        var pageRequests = await _db.ImportWikiDataCelebritiesPageRequests
                .Where(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Requested)
                .ToListAsync();
        
        pageRequests.ForEach(x => {
            BackgroundJob.Enqueue<ImportWikiDataCelebritiesPageJob>(y => y.ImportAsync(x.Id));
            x.MarkScheduled();
        });
        
        await _db.SaveChangesAsync();
    }
}
