using AnagramSolver.Exceptions;
using static AnagramSolver.Data.Entities.ImportWikiDataCelebritiesPageRequest;

namespace AnagramSolver.Data.Entities;

public class ImportWikiDataCelebritiesRequest
{
    private const int ImportWikiDataCelebritiesPageSize = 800;
    public int Id { get; private set; }
    public string? WikiDataOccupationId { get; private set; }
    public string? WikiDataNationalityId { get; private set;}
    public DateTimeOffset CreatedAt { get; private init; }

    public ImportWikiDataCelebritiesRequestStatus Status { get; private set; } = ImportWikiDataCelebritiesRequestStatus.Requested;

    public ICollection<ImportWikiDataCelebritiesPageRequest> ImportPageRequests { get; private set; } = new List<ImportWikiDataCelebritiesPageRequest>();

    public ImportWikiDataCelebritiesRequest(string? wikiDataOccupationId, string? wikiDataNationalityId)
    {
        if (wikiDataOccupationId == null && wikiDataNationalityId == null)
        {
            throw new BusinessRuleViolationException("At least one of occupationId or nationalityId is required");
        }
        WikiDataOccupationId = wikiDataOccupationId;
        WikiDataNationalityId = wikiDataNationalityId;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void AddPageRequests(int totalCount)
    {
        if (Status != ImportWikiDataCelebritiesRequestStatus.Scheduled)
        {
            throw new BusinessRuleViolationException("Only scheduled import requests can add page requests");
        }
        var pageCount = (totalCount + ImportWikiDataCelebritiesPageSize - 1) / ImportWikiDataCelebritiesPageSize;

        for (int i = 0; i < pageCount; i++)
        {
            ImportPageRequests.Add(new ImportWikiDataCelebritiesPageRequest 
            {
                Limit = ImportWikiDataCelebritiesPageSize,
                Offset = i * ImportWikiDataCelebritiesPageSize,
            });
        }
        Status = ImportWikiDataCelebritiesRequestStatus.PageRequestsScheduled;
    }

    public void MarkScheduled()
    {
        if (Status != ImportWikiDataCelebritiesRequestStatus.Requested)
        {
            throw new BusinessRuleViolationException("Only requested import requests can transition to scheduled.");
        }

        Status = ImportWikiDataCelebritiesRequestStatus.Scheduled;
    }

    public void MarkProcessed()
    {
        var hasAnyUnprocessedPageRequests = Status != ImportWikiDataCelebritiesRequestStatus.PageRequestsScheduled ||
                                            ImportPageRequests.Any(x => x.Status != ImportWikiDataCelebritiesPageRequestStatus.Processed);

        if (hasAnyUnprocessedPageRequests)
        {
            throw new BusinessRuleViolationException("Request cannot transition to processed when some page requests are not processed.");
        }
        Status = ImportWikiDataCelebritiesRequestStatus.Processed;
    }

    public double CalculateCompletionPercentage()
    {
        if (Status == ImportWikiDataCelebritiesRequestStatus.Processed)
        {
            return 100;
        }

        if (ImportPageRequests.Count == 0)
        {
            return 0;
        }

        return ImportPageRequests.Count(x => x.Status == ImportWikiDataCelebritiesPageRequestStatus.Processed) * 100.0 / ImportPageRequests.Count;
    }

    public enum ImportWikiDataCelebritiesRequestStatus
    {
        Requested,
        Scheduled,
        PageRequestsScheduled,
        Processed,
    }
}
