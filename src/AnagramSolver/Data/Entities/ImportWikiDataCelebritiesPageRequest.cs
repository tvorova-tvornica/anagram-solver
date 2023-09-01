namespace AnagramSolver.Data.Entities;

public class ImportWikiDataCelebritiesPageRequest
{
    public int Id { get; private set; }
    public required int Offset { get; init; }
    public required int Limit { get; init; }

    public ImportWikiDataCelebritiesRequest ImportCelebritiesRequest { get; private set; }

    public ImportWikiDataCelebritiesPageRequestStatus Status { get; private set; } = ImportWikiDataCelebritiesPageRequestStatus.Requested;

    public void MarkScheduled()
    {
        Status = ImportWikiDataCelebritiesPageRequestStatus.Scheduled;
    }

    public void MarkProcessed()
    {
        Status = ImportWikiDataCelebritiesPageRequestStatus.Processed;
    }

    public enum ImportWikiDataCelebritiesPageRequestStatus
    {
        Requested,
        Scheduled,
        Processed,
    }
}
