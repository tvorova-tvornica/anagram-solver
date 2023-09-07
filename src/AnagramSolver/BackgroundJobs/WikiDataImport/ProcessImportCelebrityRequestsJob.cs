using AnagramSolver.Data;
using Microsoft.EntityFrameworkCore;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesRequest;

namespace AnagramSolver.BackgroundJobs;

public class ProcessImportCelebrityRequestsJob
{
    private readonly AnagramSolverContext _db;

    public ProcessImportCelebrityRequestsJob(AnagramSolverContext db)
    {
        _db = db;
    }

    public async Task ProcessAsync()
    {
        var processedRequests = await _db.ImportWikiDataCelebritiesRequests
            .Where(x => x.Status == ImportWikiDataCelebritiesRequestStatus.PageRequestsScheduled && 
                   x.PageRequests.All(y => y.Status == ImportWikiDataCelebritiesPageRequestStatus.Processed))
            .ToListAsync();

        processedRequests.ForEach(x => x.MarkProcessed());
        await _db.SaveChangesAsync();
    }
}
