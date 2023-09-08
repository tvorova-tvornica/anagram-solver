namespace AnagramSolver.HttpClients.Dto;

public class WikiDataCountResponse
{
    public required WikiDataCountResults Results { get; set; }

    public class WikiDataCountResults
    {
        public required List<WikiDataCount> Bindings { get; set;}
    }

    public class WikiDataCount 
    {
        public required WikiDataIntValue Count { get; set; }
    }
}
