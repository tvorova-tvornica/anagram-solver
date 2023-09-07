using AnagramSolver.Exceptions;

namespace AnagramSolver.Data.Entities;

public class ImportWikiDataCelebritiesPageRequest
{
    public int Id { get; private set; }
    public required int Offset { get; init; }
    public required int Limit { get; init; }

    public ImportWikiDataCelebritiesRequest ImportCelebritiesRequest { get; private set; } = null!;

    public ImportWikiDataCelebritiesPageRequestStatus Status { get; private set; } = ImportWikiDataCelebritiesPageRequestStatus.Scheduled;

    public void MarkProcessed()
    {
        if (Status != ImportWikiDataCelebritiesPageRequestStatus.Scheduled)
        {
            throw new BusinessRuleViolationException("Only scheduled requested import page requests can transition to scheduled.");
        }
        Status = ImportWikiDataCelebritiesPageRequestStatus.Processed;
    }

    public enum ImportWikiDataCelebritiesPageRequestStatus
    {
        Scheduled,
        Processed,
    }
}
