using AnagramSolver.BackgroundJobs;

namespace AnagramSolver.HttpClients.Dto;

public class WikiDataCelebritiesResponse
{
    public WikiDataCelebritiesResults? Results { get; set; }

    public class WikiDataCelebritiesResults
    {
        public required List<WikiDataCelebrity> Bindings { get; set; }
    }

    public class WikiDataCelebrity
    {
        public required WikiDataStringValue ItemLabel { get; set; }
        public WikiDataStringValue? Image { get; set; }
        public WikiDataStringValue? GenderLabel { get; set; }
        public WikiDataStringValue? WikipediaLink { get; set; }
    }
}
