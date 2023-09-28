namespace AnagramSolver.Controllers.Dto;

public record ImportCelebritiesRequestResult(int Id, string? WikiDataNationalityId, string? WikiDataOccupationId, double CompletionPercentage);
