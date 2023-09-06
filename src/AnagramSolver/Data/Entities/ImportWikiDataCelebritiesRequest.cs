using AnagramSolver.Exceptions;

namespace AnagramSolver.Data.Entities;

public class ImportWikiDataCelebritiesRequest
{
    public int Id { get; private set; }
    public string? WikiDataOccupationId { get; private set; }
    public string? WikiDataNationalityId { get; private set;}

    public ImportWikiDataCelebritiesRequestStatus Status { get; private set; } = ImportWikiDataCelebritiesRequestStatus.Requested;

    public ICollection<ImportWikiDataCelebritiesPageRequest> PageRequests { get; private set; } = new List<ImportWikiDataCelebritiesPageRequest>();

    public ImportWikiDataCelebritiesRequest(string? occupationId, string? nationalityId)
    {
        if (occupationId == null && nationalityId == null)
        {
            throw new BusinessRuleViolationException("Either occupationId or nationalityId are required");
        }
        WikiDataOccupationId = occupationId;
        WikiDataNationalityId = nationalityId;
    }

    
    public void AddPageRequests(int totalCount)
    {
        if (Status != ImportWikiDataCelebritiesRequestStatus.Scheduled)
        {
            throw new BusinessRuleViolationException("Only scheduled import requests can add page requests");
        }

        var pageSize = 200;
        var pageCount = (totalCount + pageSize - 1) / pageSize;

        for (int i = 0; i < pageCount; i++)
        {
            PageRequests.Add(new ImportWikiDataCelebritiesPageRequest 
            {
                Limit = pageSize,
                Offset = i * pageSize,
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
