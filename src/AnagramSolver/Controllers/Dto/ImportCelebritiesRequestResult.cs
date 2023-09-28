namespace AnagramSolver.Controllers.Dto;

public record ImportCelebritiesRequestResult(int Id, DateTimeOffset CreatedAt, string? WikiDataNationalityId, string? WikiDataOccupationId, double CompletionPercentage);
