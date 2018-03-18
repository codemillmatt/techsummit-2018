// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using JokeForms.Core;
//
//    var jokeSearchResult = JokeSearchResult.FromJson(jsonString);

namespace JokeForms.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class JokeSearchResult
    {
        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("next_page")]
        public long NextPage { get; set; }

        [JsonProperty("previous_page")]
        public long PreviousPage { get; set; }

        [JsonProperty("results")]
        public JokeResults[] Results { get; set; }

        [JsonProperty("search_term")]
        public string SearchTerm { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("total_jokes")]
        public long TotalJokes { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }
    }

    public partial class JokeResults
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("joke")]
        public string Joke { get; set; }
    }

    public partial class JokeSearchResult
    {
        public static JokeSearchResult FromJson(string json) => JsonConvert.DeserializeObject<JokeSearchResult>(json);
    }
}
