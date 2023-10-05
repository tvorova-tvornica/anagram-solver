namespace AnagramSolver.Controllers.Celebrities.Dto;

public record ImportCelebritiesRequestResult(int Id, DateTimeOffset CreatedAt, string? WikiDataNationalityId, string? WikiDataOccupationId, double CompletionPercentage);
