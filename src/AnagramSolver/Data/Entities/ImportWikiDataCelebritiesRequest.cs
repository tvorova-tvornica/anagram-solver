using AnagramSolver.Exceptions;

namespace AnagramSolver.Data.Entities;

public class ImportWikiDataCelebritiesRequest
{
    private const int ImportWikiDataCelebritiesPageSize = 200;
    public int Id { get; private set; }
    public string? WikiDataOccupationId { get; private set; }
    public string? WikiDataNationalityId { get; private set;}

    public ImportWikiDataCelebritiesRequestStatus Status { get; private set; } = ImportWikiDataCelebritiesRequestStatus.Requested;

    public ICollection<ImportWikiDataCelebritiesPageRequest> PageRequests { get; private set; } = new List<ImportWikiDataCelebritiesPageRequest>();

    public ImportWikiDataCelebritiesRequest(string? wikiDataOccupationId, string? wikiDataNationalityId)
    {
        if (wikiDataOccupationId == null && wikiDataOccupationId == null)
        {
            throw new BusinessRuleViolationException("At least one of occupationId or nationalityId is required");
        }
        WikiDataOccupationId = wikiDataOccupationId;
        WikiDataNationalityId = wikiDataNationalityId;
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
            PageRequests.Add(new ImportWikiDataCelebritiesPageRequest 
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
                                            PageRequests.Any(x => x.Status != ImportWikiDataCelebritiesPageRequest.ImportWikiDataCelebritiesPageRequestStatus.Processed);

        if (hasAnyUnprocessedPageRequests)
        {
            throw new BusinessRuleViolationException("Request cannot transition to processed when some page requests are not processed.");
        }
        Status = ImportWikiDataCelebritiesRequestStatus.Processed;
    }

    public enum ImportWikiDataCelebritiesRequestStatus
    {
        Requested,
        Scheduled,
        PageRequestsScheduled,
        Processed,
    }
}
