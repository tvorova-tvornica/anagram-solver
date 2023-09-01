namespace AnagramSolver.Data.Entities;

public class ImportWikiDataCelebritiesRequest
{
    public int Id { get; private set; }
    public required string WikiDataOccupationId { get; init; }
    public string? WikiDataNationalityId { get; init;}

    public ImportWikiDataCelebritiesRequestStatus Status { get; private set; } = ImportWikiDataCelebritiesRequestStatus.Requested;

    public ICollection<ImportWikiDataCelebritiesPageRequest> PageRequests { get; private set; } = new List<ImportWikiDataCelebritiesPageRequest>();

    // merge process and this one into single method
    public void AddPageRequests(int totalCount)
    {
        var pageSize = 1000;
        var pageCount = (totalCount + pageSize - 1) / pageSize;

        for (int i = 0; i < pageCount; i++)
        {
            PageRequests.Add(new ImportWikiDataCelebritiesPageRequest 
            {
                Limit = pageSize,
                Offset = i * pageSize,
            });
        }
    }

    public void MarkScheduled()
    {
        Status = ImportWikiDataCelebritiesRequestStatus.Scheduled;
    }

    public void MarkProcessed()
    {
        Status = ImportWikiDataCelebritiesRequestStatus.Processed;
    }

    public enum ImportWikiDataCelebritiesRequestStatus
    {
        Requested,
        Scheduled,
        Processed,
    }
}
