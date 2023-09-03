using AnagramSolver.HttpClients.Dto;

namespace AnagramSolver.HttpClients;

public class WikiDataHttpClient
{
    private readonly HttpClient _httpClient;

    public WikiDataHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetTotalCelebrityCountAsync(string occupationId, string? nationalityId)
    {
        var query = nationalityId is null ? GetOccupationOnlyCountUrl(occupationId) : GetFullCountUrl(occupationId, nationalityId);

        return await _httpClient.GetFromJsonAsync<int>(query);
    }

    public async Task<List<WikiDataCelebrity>> GetCelebritiesPageAsync(string occupationId, string? nationalityId, int limit, int offset)
    {
        var query = nationalityId is null 
            ? GetOccupationOnlyCelebritiesPageUrl(occupationId, limit, offset) 
            : GetFullCelebritiesPageUrl(occupationId, nationalityId, limit, offset);

        return (await _httpClient.GetFromJsonAsync<List<WikiDataCelebrity>>(query))!;
    }

    private string GetOccupationOnlyCountUrl(string occupationId)
    {
        return $@"https://query.wikidata.org/sparql?query=SELECT DISTINCT (COUNT(?item) AS ?count) WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE]"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106) wd:{occupationId}.
                    }}
                }}
        }}";
    }

    private string GetFullCountUrl(string occupationId, string nationalityId)
    {
        return $@"https://query.wikidata.org/sparql?query=SELECT DISTINCT (COUNT(?item) AS ?count) WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE]"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106) wd:{occupationId}.
                        ?item p:P27 ?statement1.
                        ?statement1 (ps:P27) wd:{nationalityId}.
                    }}
                }}
        }}";
    }

    private string GetOccupationOnlyCelebritiesPageUrl(string occupationId, int limit, int offset)
    {
        return $@"https://query.wikidata.org/sparql?query=SELECT DISTINCT ?item ?itemLabel ?image ?genderLabel ?wikipediaLink WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE]"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106) wd:{occupationId}.
                    }}
                    OFFSET {offset}
                    LIMIT {limit}
                }}
                OPTIONAL {{
                    ?item wdt:P18 ?image.
                    ?item wdt:P21 ?gender.
                    ?item wdt:P31 wd:Q5.
                    OPTIONAL {{
                        ?wikipediaLink_en schema:about ?item;
                        schema:inLanguage ""en"";
                        schema:isPartOf <https://en.wikipedia.org/>.
                    }}
                    OPTIONAL {{
                        ?wikipediaLink schema:about ?item;
                        schema:inLanguage ?lang;
                        schema:isPartOf <https://www.wikipedia.org/>.
                        FILTER (LANGMATCHES(LANG(?lang), ""en"") = false)
                    }}
                }}
                BIND(COALESCE(?wikipediaLink_en, ?wikipediaLink) AS ?wikipediaLink)
        }}";
    }

    private string GetFullCelebritiesPageUrl(string occupationId, string nationalityId, int limit, int offset)
    {
        return $@"https://query.wikidata.org/sparql?query=SELECT DISTINCT ?item ?itemLabel ?image ?genderLabel ?wikipediaLink WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE]"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106) wd:{occupationId}.
                        ?item p:P27 ?statement1.
                        ?statement1 (ps:P27) wd:{nationalityId}.
                    }}
                    OFFSET {offset}
                    LIMIT {limit}
                }}
                OPTIONAL {{
                    ?item wdt:P18 ?image.
                    ?item wdt:P21 ?gender.
                    ?item wdt:P31 wd:Q5.
                    OPTIONAL {{
                        ?wikipediaLink_en schema:about ?item;
                        schema:inLanguage ""en"";
                        schema:isPartOf <https://en.wikipedia.org/>.
                    }}
                    OPTIONAL {{
                        ?wikipediaLink schema:about ?item;
                        schema:inLanguage ?lang;
                        schema:isPartOf <https://www.wikipedia.org/>.
                        FILTER (LANGMATCHES(LANG(?lang), ""en"") = false)
                    }}
                }}
                BIND(COALESCE(?wikipediaLink_en, ?wikipediaLink) AS ?wikipediaLink)
        }}";
    }
}
