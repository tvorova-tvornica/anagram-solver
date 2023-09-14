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
        public required WikiDataStringValue Item { get; set; }
        public required WikiDataStringValue ItemLabel { get; set; }
        public WikiDataStringValue? Image { get; set; }
        public WikiDataStringValue? EnDescription { get; set; }
        public WikiDataStringValue? HrDescription { get; set; }
        public WikiDataStringValue? EnWikipedia { get; set; }
        public WikiDataStringValue? HrWikipedia { get; set; }
    }
}
