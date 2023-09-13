using AnagramSolver.HttpClients.Dto;
using Microsoft.Net.Http.Headers;

namespace AnagramSolver.HttpClients;

public class WikiDataHttpClient
{
    private readonly HttpClient _httpClient;

    public WikiDataHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://query.wikidata.org");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/sparql-results+json");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, ".NET/6.0 ScraperBot/1.0 (+https://anagram-solver.fly.dev)");
    }

    public async Task<int> GetTotalCelebrityCountAsync(string? occupationId, string? nationalityId)
    {
        var query = GetCountQuery(occupationId, nationalityId);
        
        var response = (await _httpClient.GetFromJsonAsync<WikiDataCountResponse>(query))!;

        return response.Results.Bindings.First().Count.Value;
    }

    public async Task<WikiDataCelebritiesResponse> GetCelebritiesPageAsync(string? occupationId, string? nationalityId, int limit, int offset)
    {
        var query = GetCelebritiesQuery(occupationId, nationalityId, limit, offset);

        return (await _httpClient.GetFromJsonAsync<WikiDataCelebritiesResponse>(query))!;
    }

    private string GetCountQuery(string? occupationId, string? nationalityId)
    {
        if (occupationId is null)
        {
            return GetNationalityOnlyCountQuery(nationalityId!);
        }

        if (nationalityId is null)
        {
            return GetOccupationOnlyCountQuery(occupationId);
        }

        return GetFullCountQuery(occupationId, nationalityId);
    } 

    private string GetCelebritiesQuery(string? occupationId, string? nationalityId, int limit, int offset)
    {
        if (occupationId is null)
        {
            return GetNationalityOnlyCelebritiesPageQuery(nationalityId!, limit, offset);
        }
        
        if (nationalityId is null)
        {
            return GetOccupationOnlyCelebritiesPageQuery(occupationId, limit, offset);
        }

        return GetFullCelebritiesPageQuery(occupationId, nationalityId, limit, offset);
    }

    private string GetOccupationOnlyCountQuery(string occupationId)
    {
        return $@"sparql?query=SELECT DISTINCT (COUNT(?item) AS ?count) WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE], en, es, fr, hr"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106/(wdt:P279*)) wd:{occupationId}.
                    }}
                }}
        }}";
    }

    private string GetNationalityOnlyCountQuery(string nationalityId)
    {
        return $@"sparql?query=SELECT DISTINCT (COUNT(?item) AS ?count) WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE], en, es, fr, hr"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P27 ?statement1.
                        ?statement1 (ps:P27/(wdt:P279*)) wd:{nationalityId}.
                    }}
                }}
        }}";
    }

    private string GetFullCountQuery(string occupationId, string nationalityId)
    {
        return $@"sparql?query=SELECT DISTINCT (COUNT(?item) AS ?count) WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE], en, es, fr, hr"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106/(wdt:P279*)) wd:{occupationId}.
                        ?item p:P27 ?statement1.
                        ?statement1 (ps:P27/(wdt:P279*)) wd:{nationalityId}.
                    }}
                }}
        }}";
    }

    private string GetOccupationOnlyCelebritiesPageQuery(string occupationId, int limit, int offset)
    {
        return $@"sparql?query=SELECT DISTINCT ?item ?itemLabel ?image ?enDescription ?hrDescription ?hrwiki ?enwiki WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE], en, es, fr, hr"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106/(wdt:P279*)) wd:{occupationId}.
                    }}
                    ORDER BY ?itemLabel
                    OFFSET {offset}
                    LIMIT {limit}
                }}

                OPTIONAL {{
                    ?item wdt:P18 ?image.
                }}
  
                OPTIONAL {{
                    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"".
                                            ?item schema:description ?enDescription
                    }}
                }}
  
                OPTIONAL {{
                    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""hr"".
                                            ?item schema:description ?hrDescription
                    }}
                }}
  
                OPTIONAL {{
                    ?enwiki schema:about ?item;
                    schema:inLanguage ""en"";
                    schema:isPartOf <https://en.wikipedia.org/>.
                }}
  
                OPTIONAL {{
                    ?hrwiki schema:about ?item;
                        schema:inLanguage ""hr"";
                        schema:isPartOf <https://hr.wikipedia.org/>.
                }}
        }}";
    }

    private string GetNationalityOnlyCelebritiesPageQuery(string nationalityId, int limit, int offset)
    {
        return $@"sparql?query=SELECT DISTINCT ?item ?itemLabel ?image ?enDescription ?hrDescription ?hrwiki ?enwiki WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE], en, es, fr, hr"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P27 ?statement1.
                        ?statement1 (ps:P27/(wdt:P279*)) wd:{nationalityId}.
                    }}
                    ORDER BY ?itemLabel
                    OFFSET {offset}
                    LIMIT {limit}
                }}

                OPTIONAL {{
                    ?item wdt:P18 ?image.
                }}
  
                OPTIONAL {{
                    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"".
                                            ?item schema:description ?enDescription
                    }}
                }}
  
                OPTIONAL {{
                    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""hr"".
                                            ?item schema:description ?hrDescription
                    }}
                }}
  
                OPTIONAL {{
                    ?enwiki schema:about ?item;
                    schema:inLanguage ""en"";
                    schema:isPartOf <https://en.wikipedia.org/>.
                }}
  
                OPTIONAL {{
                    ?hrwiki schema:about ?item;
                        schema:inLanguage ""hr"";
                        schema:isPartOf <https://hr.wikipedia.org/>.
                }}
        }}";
    }

    private string GetFullCelebritiesPageQuery(string occupationId, string nationalityId, int limit, int offset)
    {
        return $@"sparql?query=SELECT DISTINCT ?item ?itemLabel ?image ?enDescription ?hrDescription ?hrWikipedia ?enWikipedia WHERE {{
                SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""[AUTO_LANGUAGE], en, es, fr, hr"". }}
                {{
                    SELECT DISTINCT ?item WHERE {{
                        ?item p:P106 ?statement0.
                        ?statement0 (ps:P106/(wdt:P279*)) wd:{occupationId}.
                        ?item p:P27 ?statement1.
                        ?statement1 (ps:P27/(wdt:P279*)) wd:{nationalityId}.
                    }}
                    ORDER BY ?itemLabel
                    OFFSET {offset}
                    LIMIT {limit}
                }}

                OPTIONAL {{
                    ?item wdt:P18 ?image.
                }}
  
                OPTIONAL {{
                    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""en"".
                                            ?item schema:description ?enDescription
                    }}
                }}
  
                OPTIONAL {{
                    SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""hr"".
                                            ?item schema:description ?hrDescription
                    }}
                }}
  
                OPTIONAL {{
                    ?enWikipedia schema:about ?item;
                    schema:inLanguage ""en"";
                    schema:isPartOf <https://en.wikipedia.org/>.
                }}
  
                OPTIONAL {{
                    ?hrWikipedia schema:about ?item;
                        schema:inLanguage ""hr"";
                        schema:isPartOf <https://hr.wikipedia.org/>.
                }}
        }}";
    }
}
